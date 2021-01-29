using RestSharp;

namespace paradiceinBOT
{
    public class RequestWithData
    {
        private DataForRequest data;

        private RestClient client = new RestClient("https://api.paradice.in/api.php");
        RestRequest request = new RestRequest(Method.POST);

        private string s1 = "{\r\n    \"operationName\": \"rollDice\",\r\n    \"variables\": {\r\n        \"betAmount\": \"";
        private string s2 = "\",\r\n        \"number\": \"";
        private string s3 = "\",\r\n        \"side\": \"";
        private string s4 = "\",\r\n        \"currency\": \"";
        private string s5 = "\"\r\n    },\r\n    \"query\": \"mutation rollDice($number: Float!, $betAmount: Float!, $side: RollSideEnum!, $currency: CurrencyEnum!) {\\n  rollDice(number: $number, betAmount: $betAmount, side: $side, currency: $currency) {\\n    id\\n    number\\n    roll\\n    rollSide\\n    win\\n    betAmount\\n    winAmount\\n    currency\\n    multiplier\\n    chance\\n    game\\n    bets {\\n      pocket\\n      payout\\n      win\\n      bet\\n      __typename\\n    }\\n    winLines {\\n      id\\n      __typename\\n    }\\n    user {\\n      id\\n      login\\n      lastActivity\\n      wallets {\\n        currency\\n        balance\\n        safeAmount\\n        __typename\\n      }\\n      loyaltyLevel {\\n        level {\\n          id\\n          category\\n          level\\n          __typename\\n        }\\n        __typename\\n      }\\n      privacySettings {\\n        isPMNotificationsEnabled\\n        isWageredHidden\\n        isAnonymous\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"\r\n}";


        public RequestWithData(DataForRequest data)
        {
            this.data = data;

            AddParametersToRequest();
        }
        private void AddParametersToRequest()
        {
            client.Timeout = -1;
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            request.AddHeader("DNT", "1");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Authorization", data.token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "*/*");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Sec-Fetch-Site", "same-site");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Cookie", "__cfduid=d05ec5fcc4e574b8c77a5e81dc51caa411607638398; _ga=GA1.2.1474332697.1608739211");
            request.AddParameter("application/json", s1 + "tmp" + s2 + "tmp" + s3 + "tmp" + s4 + "tmp" + s5, ParameterType.RequestBody);
        }
        private void AddBetParameter()
        {
            request.Parameters[10].Value = s1 + data.bet + s2 + data.chance.Replace(",", ".") + s3 + data.side + s4 + data.сurrency + s5;
        }
        public string TakeBet()
        {
            AddBetParameter();
            IRestResponse response = client.Execute(request);

            //Для вытягивания инфы из ответа
            //var weather = JsonConvert.DeserializeObject<Rootobject>(response.Content);
            // File.WriteAllText("123.json", JsonConvert.SerializeObject(response.Content));

            return response.Content;
        }
    }
}