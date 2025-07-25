using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.ViewModel;

namespace offlineMeeting.Models.Process.GameData
{

    public class GetCameraProcess
    {
        public GetCameraProcess() { }

        public GameDataViewModel Set(GameDataViewModel GameDataViewModel, GetCameraPostEntity getCameraPostEntity)
        {
            VideoProcess videoProcess = new VideoProcess();

            // deviceId label
            List<VideoEntity> videoList = videoProcess.Get();
            List<Dictionary<string, string>> cameraList = getCameraPostEntity.CameraContainer ??
                throw new NullReferenceException("cameraContainer is invalid");
            //List<RetVideoEntity> videoEntityList = new List<RetVideoEntity>();
            List<UnionVideo> videoEntityList = new List<UnionVideo>();

            foreach (Dictionary<string, string> camera in cameraList)
            {
                videoEntityList.Add(
                    new UnionVideo(
                        deviceId: camera["deviceId"],
                        label: camera["label"],
                        cameraName: $"(未登録){camera["label"]}",
                        isReadOnly: false
                    )
                );
            }

            // 登録済カメラリストにJoinして、登録されているが接続されていないカメラのreadOnlyフラグをオンに
            List<RetVideoEntity> retVideoEntityList = videoList.GroupJoin(
                videoEntityList,
                x => x.DeviceId,
                y => y.DeviceId,
                (vl, rvel) => new {
                    vl,
                    rvel
                }
            ).SelectMany(
                x=>x.rvel.DefaultIfEmpty(),
                (x, r)=> new UnionVideo
                (
                    x.vl.DeviceId,
                    x.vl.Label,
                    cameraName: String.IsNullOrEmpty(r?.Label) ? $"(未接続){ x.vl.CameraName }" : x.vl.CameraName,
                    isReadOnly: String.IsNullOrEmpty(r?.Label)
                )
            ).Union(videoEntityList)
                .Select(
                x=>
                new RetVideoEntity(
                    x.DeviceId,
                    x.Label,
                    x.CameraName,
                    x.IsReadOnly
                )
            ).ToList();

            GameDataViewModel.setRetVideoEntityList(retVideoEntityList);

            return GameDataViewModel;
        }
    }
    file class UnionVideo: RetVideoEntity
    {
        public UnionVideo(
            string deviceId,
            string label,
            string? cameraName,
            bool? isReadOnly = true
        ): base(
            deviceId,
            label,
            cameraName,
            isReadOnly
        )
        {
            DeviceId = deviceId;
            Label = label;
            CameraName = cameraName;
            IsReadOnly = isReadOnly;
        }

        public override string ToString()
              => $"{DeviceId}";

        public override bool Equals(object? obj)
        {
            var p = obj as UnionVideo;
            if (p == null)
                return false;
            return (this.DeviceId == p.DeviceId);
        }
        public override int GetHashCode()
        {
            if (DeviceId != null)
            {
                return DeviceId.GetHashCode();
            }
            throw new Exception("invalid deviceId");
        }
    }
}