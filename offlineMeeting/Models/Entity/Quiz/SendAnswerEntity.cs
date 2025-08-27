using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Quiz
{
    public class SendAnswerEntity : ApiEntity
    {
        public ClassHashEntity ClassHash { get; set; }
        public DateHashEntity DateHash { get; set; }
        public DescriptionHashEntity DescriptionHash { get; set; }
        public ImageHashEntity ImageHash { get; set; }
        public SendAnswerEntity(
            string eventNumber,
            string userCd,
            string quizId,
            string base64Image
        )
        {
            ClassHash = new ClassHashEntity(
                eventNumber: eventNumber,
                userCd: userCd,
                quizId: quizId
            );
            DateHash = new DateHashEntity();
            DescriptionHash = new DescriptionHashEntity();
            ImageHash = new ImageHashEntity(base64Image: base64Image);
        }

        public class ClassHashEntity
        {
            public string ClassA { get; set; }
            public string ClassB { get; set; }
            public string ClassC { get; set; }
            public ClassHashEntity(
                string eventNumber,
                string userCd,
                string quizId
            )
            {
                ClassA = eventNumber;
                ClassB = userCd;
                ClassC = quizId;
            }
        }

        public class DateHashEntity
        {
            public DateTime DateA { get; set; }
            public DateHashEntity()
            {
                DateA = DateTime.UtcNow.AddHours(9);
            }
        }

        public class DescriptionHashEntity
        {
            public string DescriptionA { get; set; }
            public DescriptionHashEntity()
            {
                DescriptionA = "";
            }
        }

        public class ImageHashEntity
        {
            public Dictionary<string, object> DescriptionA { get; set; }
            public ImageHashEntity(
                string base64Image
            )
            {
                DescriptionA = new Dictionary<string, object>
                {
                    {"HeadNewLine", false },
                    {"EndNewLine", false },
                    {"Extension", ".png" },
                    {"Base64", base64Image }
                };
            }
        }
    }
}