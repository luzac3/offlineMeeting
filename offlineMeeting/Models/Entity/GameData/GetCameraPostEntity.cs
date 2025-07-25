namespace offlineMeeting.Models.Entity.GameData
{
    public class GetCameraPostEntity
    {
        public int EventNumber { get; set; }
        public int GameNumber { get; set; }
        public List<Dictionary<string, string>>? CameraContainer { get; set; }

        public GetCameraPostEntity() {}
    }
}