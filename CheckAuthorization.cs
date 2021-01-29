using System.IO;
using RestSharp;

namespace paradiceinBOT
{
    public class CheckAuthorization
    {
        private RestClient client;
        private RestRequest request;

        private string token;

        public CheckAuthorization()
        {
            client = new RestClient("https://api.paradice.in/api.php");
            request = new RestRequest(Method.POST);
            token = "";
        }

        private void AddHeader()
        {
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            request.AddHeader("DNT", "1");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Authorization", token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "*/*");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Sec-Fetch-Site", "same-site");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Cookie", "__cfduid=da2abf5f89fc9f7f31d7bd151f7803ba91611183385");
            request.AddParameter("application/json", "{\"operationName\":null,\"variables\":{},\"query\":\"{\\n  me {\\n    ...FRAGMENT_COMPLETE_USER_DATA\\n    __typename\\n  }\\n}\\n\\nfragment FRAGMENT_COMPLETE_USER_DATA on User {\\n  id\\n  login\\n  email\\n  confirmed\\n  unconfirmedEmail\\n  protected\\n  userRoles\\n  clientSeed\\n  serverSeed\\n  serverSeedNonce\\n  messages\\n  serverSeedNext\\n  twoFactorResetDate\\n  twoFactorEnabled\\n  lastActivity\\n  isLongTimeInactive\\n  avatar\\n  likes\\n  token {\\n    token\\n    __typename\\n  }\\n  friendsIds\\n  ...FRAGMENT_USER_LOYALTY_LVL\\n  ...FRAGMENT_USER_PRIVACY_SETTINGS\\n  ...FRAGMENT_USER_WALLETS\\n  __typename\\n}\\n\\nfragment FRAGMENT_USER_PRIVACY_SETTINGS on User {\\n  privacySettings {\\n    isPMNotificationsEnabled\\n    isWageredHidden\\n    isAnonymous\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment FRAGMENT_USER_LOYALTY_LVL on User {\\n  loyaltyLevel {\\n    level {\\n      ...LOYALTY_LEVEL\\n      __typename\\n    }\\n    progress\\n    isTemporary\\n    endsIn\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment LOYALTY_LEVEL on LoyaltyLevel {\\n  category\\n  level\\n  id\\n  features {\\n    feature\\n    value\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment FRAGMENT_USER_WALLETS on User {\\n  wallets {\\n    currency\\n    balance\\n    address\\n    bonus\\n    rakeback\\n    safeAmount\\n    __typename\\n  }\\n  __typename\\n}\\n\"}", ParameterType.RequestBody);
        }

        //check token and file with token
        private bool CheckToken()
        {
            if (!File.Exists("token.txt"))
            {
                return false;
            }
            else
            {
                using (StreamReader sr = new StreamReader(@"token.txt"))
                {
                    token = sr.ReadToEnd();
                }

                if (token == "" || token == " ")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string GetToken()
        {
            return token;
        }
        public int Autorization()
        {
            if (CheckToken())
            {
                AddHeader();
                IRestResponse response = client.Execute(request);

                if (response.Content.Contains("login"))
                {
                    //if token work
                    return 1;
                }
                else
                {
                    //if token don`t work
                    return 0;
                }
            }
            else
            {
                //other errors
                return 3;
            }
        }
    }
}