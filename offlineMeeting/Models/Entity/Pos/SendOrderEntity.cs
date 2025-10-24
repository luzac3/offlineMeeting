namespace offlineMeeting.Models.Entity.Pos.SendOrderEntity
{
    public class SendOrderEntity
    {
        public ClassHashEntity ClassHash { get; set; }
        public NumHashEntity NumHash { get; set; }
        public CheckHashEntity CheckHash { get; set; }

        public SendOrderEntity(
            string eventNumber,
            string userCd,
            int orderCoins,
            bool isAlcohol
        )
        {
            ClassHash = new ClassHashEntity(
                eventNumber: eventNumber,
                userCd: userCd
            );
            NumHash = new NumHashEntity(
                orderCoins: orderCoins
            );
            CheckHash = new CheckHashEntity(isAlcohol: isAlcohol);
        }
    }

    public class ClassHashEntity
    {
        public string ClassA { get; set; } // EventNumber
        public string ClassD { get; set; } // UserCd
        public ClassHashEntity(
            string eventNumber,
            string userCd
        )
        {
            ClassA = eventNumber;
            ClassD = userCd;
        }
    }

    public class NumHashEntity
    {
        public int NumA { get; set; } // OrderCoins
        public NumHashEntity(
            int orderCoins
        )
        {
            NumA = orderCoins;
        }
    }

    public class CheckHashEntity
    {
        public bool CheckA { get; set; } // InProgress
        public bool CheckB { get; set; } // IsAlcohol
        public CheckHashEntity(
            bool isAlcohol
        )
        {
            CheckA = true;
            CheckB = isAlcohol;
        }
    }
}