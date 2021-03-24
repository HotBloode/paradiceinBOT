namespace paradiceinBOT
{
    public class ChatInfo
    {
        public Data data { get; set; }
        public class Data
        {
            public Chatchannel chatChannel { get; set; }
        }

        public class Chatchannel
        {
            public string id { get; set; }
            public string key { get; set; }
            public int unreadCount { get; set; }
            public bool isPublic { get; set; }
            public object user { get; set; }
            public Message[] messages { get; set; }
            public string __typename { get; set; }
        }

        public class Message
        {
            public int id { get; set; }
            public string body { get; set; }
            public Message1 message { get; set; }
            public string createdAt { get; set; }
            public object isDeleted { get; set; }
            public string payload { get; set; }
            public Likes likes { get; set; }
            public User user { get; set; }
            public string type { get; set; }
            public string __typename { get; set; }
        }

        public class Message1
        {
            public string body { get; set; }
            public object balance { get; set; }
            public object transaction { get; set; }
            public object rain { get; set; }
            public Tips tips { get; set; }
            public object news { get; set; }
            public object achievement { get; set; }
            public string __typename { get; set; }
        }

        public class Tips
        {
            public Author author { get; set; }
            public Recipient recipient { get; set; }
            public string currency { get; set; }
            public float amount { get; set; }
            public object message { get; set; }
            public bool isPrivate { get; set; }
            public string __typename { get; set; }
        }

        public class Author
        {
            public string id { get; set; }
            public string login { get; set; }
            public string lastActivity { get; set; }
            public object avatar { get; set; }
            public string __typename { get; set; }
        }

        public class Recipient
        {
            public string id { get; set; }
            public string login { get; set; }
            public string lastActivity { get; set; }
            public string avatar { get; set; }
            public string __typename { get; set; }
        }

        public class Likes
        {
            public int count { get; set; }
            public string[] likedBy { get; set; }
            public string __typename { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string login { get; set; }
            public string avatar { get; set; }
            public string[] userRoles { get; set; }
            public string lastActivity { get; set; }
            public Loyaltylevel loyaltyLevel { get; set; }
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
    }
}