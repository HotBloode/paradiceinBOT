namespace paradiceinBOT
{
    public class InfoAboutUser
    {

        public Data data { get; set; }


        public class Data
        {
            public Me me { get; set; }
        }

        public class Me
        {
            public string id { get; set; }
            public string login { get; set; }
            public string email { get; set; }
            public bool confirmed { get; set; }
            public object unconfirmedEmail { get; set; }
            public bool _protected { get; set; }
            public object[] userRoles { get; set; }
            public string clientSeed { get; set; }
            public string serverSeed { get; set; }
            public int serverSeedNonce { get; set; }
            public int messages { get; set; }
            public string serverSeedNext { get; set; }
            public object twoFactorResetDate { get; set; }
            public bool twoFactorEnabled { get; set; }
            public string lastActivity { get; set; }
            public bool isLongTimeInactive { get; set; }
            public string avatar { get; set; }
            public int likes { get; set; }
            public Token token { get; set; }
            public int[] friendsIds { get; set; }
            public Loyaltylevel loyaltyLevel { get; set; }
            public string __typename { get; set; }
            public Privacysettings privacySettings { get; set; }
            public Wallet[] wallets { get; set; }
        }

        public class Token
        {
            public string token { get; set; }
            public string __typename { get; set; }
        }

        public class Loyaltylevel
        {
            public Level level { get; set; }
            public float progress { get; set; }
            public bool isTemporary { get; set; }
            public object endsIn { get; set; }
            public string __typename { get; set; }
        }

        public class Level
        {
            public string category { get; set; }
            public int level { get; set; }
            public string id { get; set; }
            public Feature[] features { get; set; }
            public string __typename { get; set; }
        }

        public class Feature
        {
            public string feature { get; set; }
            public string value { get; set; }
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
            public string address { get; set; }
            public int bonus { get; set; }
            public double rakeback { get; set; }
            public float safeAmount { get; set; }
            public string __typename { get; set; }
        }

    }
}