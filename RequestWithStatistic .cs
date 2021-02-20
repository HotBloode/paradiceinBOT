using RestSharp;

namespace paradiceinBOT
{
    public class RequestWithStatistic
    {
        private DataForRequest data;

        private RestClient client = new RestClient("https://api.paradice.in/api.php");
        private RestRequest request = new RestRequest(Method.POST);
        private WriteReadFile dataWriter;

        private string s1 = "{\r\n    \"operationName\": null,\r\n    \"variables\": {\r\n        \"message\": \"My statistics\",\r\n        \"channelId\": \"1\",\r\n        \"payload\": \"{\\\"currency\\\":\\\"";
        private string s2 = "\\\",\\\"betsAmount\\\":";
        private string s3 = ",\\\"totalProfit\\\":";
        private string s4 = ",\\\"wins\\\":";
        private string s5 = ",\\\"losses\\\":";
        private string s6 = ",\\\"chartData\\\":[";
        private string s7 = "]}\"\r\n    },\r\n    \"query\": \"mutation ($channelId: ID!, $message: String!, $payload: String) {\\n  chatAddMessage(channelId: $channelId, message: $message, payload: $payload) {\\n    ...FRAGMENT_MESSAGE\\n    __typename\\n  }\\n}\\n\\nfragment FRAGMENT_MESSAGE on ChatMessage {\\n  id\\n  body\\n  message {\\n    body\\n    balance {\\n      amount\\n      currency\\n      __typename\\n    }\\n    transaction {\\n      amount\\n      date\\n      currency\\n      status\\n      __typename\\n    }\\n    rain {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      users\\n      currency\\n      amount\\n      amounts\\n      loyaltyLevelIds\\n      __typename\\n    }\\n    tips {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      recipient {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      currency\\n      amount\\n      message\\n      isPrivate\\n      __typename\\n    }\\n    news {\\n      text\\n      imageUrl\\n      buttonUrl\\n      buttonText\\n      __typename\\n    }\\n    achievement {\\n      levelId\\n      data {\\n        key\\n        value\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  createdAt\\n  isDeleted\\n  payload\\n  likes {\\n    count\\n    likedBy\\n    __typename\\n  }\\n  user {\\n    id\\n    login\\n    avatar\\n    userRoles\\n    lastActivity\\n    loyaltyLevel {\\n      level {\\n        id\\n        category\\n        level\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  type\\n  __typename\\n}\\n\"\r\n}";

        private DataForBet dataForBet;

        public RequestWithStatistic(DataForBet data, WriteReadFile dataWriter)
        {
            this.dataForBet = data;
            this.dataWriter = dataWriter;
            AddParametersToRequest();
        }

        private void AddBetParameter()
        {
            request.Parameters[10].Value = s1 + dataForBet.сurrency + s2 +($"{dataForBet.wagered:f8}").Replace(",", ".") + s3+ ($"{dataForBet.profit:f8}").Replace(",", ".") + s4+ dataForBet.iw+s5+ dataForBet.il+ s6+ dataWriter.GetWageredList() + s7;
        }

        private void AddParametersToRequest()
        {
            client.Timeout = -1;
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            request.AddHeader("DNT", "1");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Authorization", dataForBet.token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "*/*");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Sec-Fetch-Site", "same-site");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Cookie", "__cfduid=d05ec5fcc4e574b8c77a5e81dc51caa411607638398; _ga=GA1.2.1474332697.1608739211");
            request.AddParameter("application/json", "tmp", ParameterType.RequestBody);
        }

        public void RequestToSite()
        {
            AddBetParameter();
            IRestResponse response = client.Execute(request);
        }
    }
}