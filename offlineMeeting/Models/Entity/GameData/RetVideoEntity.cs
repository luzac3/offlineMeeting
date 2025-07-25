namespace offlineMeeting.Models.Entity.GameData
{
    public class RetVideoEntity
    {
        public string DeviceId {  get; set; }
        public string Label { get; set;}
        public string? CameraName { get; set; }
        public bool? IsReadOnly { get; set; }
        public RetVideoEntity(
            string deviceId,
            string label,
            string? cameraName,
            bool? isReadOnly = true
        )
        {
            DeviceId = deviceId;
            Label = label;
            CameraName = cameraName;
            IsReadOnly = isReadOnly;
        }
    }
}