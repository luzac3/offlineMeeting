using offlineMeeting.Models.Entity.Share;
using System;

namespace offlineMeeting.Models.Entity.Quiz
{
    public class SendQuizEntity : ApiEntity
    {
        public CheckHashEntity CheckHash { get; set; }
        public SendQuizEntity(
            bool active = true
        )
        {
            CheckHash = new CheckHashEntity(active);
        }

        public class CheckHashEntity
        {
            public bool CheckA { get; set; }
            public CheckHashEntity(
                bool active = true
            )
            {
                CheckA = active;
            }
        }
    }
}