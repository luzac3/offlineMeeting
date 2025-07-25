using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.DBEntity
{
    [PrimaryKey(nameof(DeviceId))]
    public class VideoEntity
    {
        public string DeviceId {  get; set; }
        public string Label { get; set;}
        public string? CameraName { get; set; }
        public VideoEntity(
            string deviceId,
            string label,
            string? cameraName
        )
        {
            DeviceId = deviceId;
            Label = label;
            CameraName = cameraName;
        }
    }
}