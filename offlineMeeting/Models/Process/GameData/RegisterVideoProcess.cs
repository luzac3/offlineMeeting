using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.DBProcess;


namespace offlineMeeting.Models.Process.GameData
{

    public class RegisterVideoProcess
    {
        public RegisterVideoProcess() { }

        public bool Set(RegisterVideoPostEntity registerVideoPostEntity)
        {
            if(
                registerVideoPostEntity.DeviceId == null ||
                registerVideoPostEntity.Label == null
            ) { 
                throw new ArgumentNullException(nameof(registerVideoPostEntity));
            }

            VideoProcess videoProcess = new VideoProcess();
            VideoEntity videoEntity = new VideoEntity(
                    deviceId: registerVideoPostEntity.DeviceId,
                    label: registerVideoPostEntity.Label,
                    cameraName: registerVideoPostEntity.CameraName
            );
            return videoProcess.Upsert(videoEntity);
        }
    }
}