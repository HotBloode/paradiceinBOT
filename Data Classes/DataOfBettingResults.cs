using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paradiceinBOT
{
    class DataOfBettingResults
    {
        public Data data { get; set; }
        

        public class Data
        {
            public Rolldice rollDice { get; set; }
        }

        public class Rolldice
        {
            public string id { get; set; }
            public string number { get; set; }
            public string roll { get; set; }
            public string rollSide { get; set; }
            public bool win { get; set; }
            public float betAmount { get; set; }
            public double winAmount { get; set; }
            public string currency { get; set; }
            public float multiplier { get; set; }
            public double chance { get; set; }
            public string game { get; set; }
            public object bets { get; set; }
            public object winLines { get; set; }
            public User user { get; set; }
            public string __typename { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string login { get; set; }
            public string lastActivity { get; set; }
            public Wallet[] wallets { get; set; }
            public Loyaltylevel loyaltyLevel { get; set; }
            public Privacysettings privacySettings { get; set; }
            public string __typename { get; set; }
        }

        public class Loyaltylevel
        {
            public Level level { get; set; }
            public string __typename { get; set; }
        }

        public class Level
        {
            public string id { get; set; }
            public string category { get; set; }
            public int level { get; set; }
            public string __typename { get; set; }
        }

        public class Privacysettings
        {
            public bool isPMNotificationsEnabled { get; set; }
            public bool isWageredHidden { get; set; }
            public bool isAnonymous { get; set; }
            public string __typename { get; set; }
        }

        public class Wallet
        {
            public string currency { get; set; }
            public float balance { get; set; }
            public double safeAmount { get; set; }
            public string __typename { get; set; }
        }
    }
}
