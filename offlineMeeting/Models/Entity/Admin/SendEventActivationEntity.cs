using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Admin
{
    public class SendEventActivationEntity : ApiEntity
    {
        public CheckHashEntity CheckHash { get; set; }
        public ClassHashEntity ClassHash { get; set; }

        public SendEventActivationEntity(
            bool active,
            string sessionId
        )
        {
            CheckHash = new CheckHashEntity(active);
            ClassHash = new ClassHashEntity(sessionId);
        }

        public class CheckHashEntity
        {
            public bool CheckB { get; set; } // イベント有効化記録
            public CheckHashEntity(bool active)
            {
                CheckB = active;
            }
        }

        public class ClassHashEntity
        {
            public string ClassB { get; set; } // SessionId
            public ClassHashEntity(string sessionId)
            {
                ClassB = sessionId;
            }
        }
    }
}
