using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using RestSharp;

namespace paradiceinBOT
{
    public class RequestBuilder
    {
        private RequestWithData request;
        

        private string s = "";

        private string sDataForBet1 = "{\r\n    \"operationName\": \"rollDice\",\r\n    \"variables\": {\r\n        \"betAmount\": \"";
        private string sDataForBet2 = "\",\r\n        \"number\": \"";
        private string sDataForBet3 = "\",\r\n        \"side\": \"";
        private string sDataForBet4 = "\",\r\n        \"currency\": \"";
        private string sDataForBet5 = "\"\r\n    },\r\n    \"query\": \"mutation rollDice($number: Float!, $betAmount: Float!, $side: RollSideEnum!, $currency: CurrencyEnum!) {\\n  rollDice(number: $number, betAmount: $betAmount, side: $side, currency: $currency) {\\n    id\\n    number\\n    roll\\n    rollSide\\n    win\\n    betAmount\\n    winAmount\\n    currency\\n    multiplier\\n    chance\\n    game\\n    bets {\\n      pocket\\n      payout\\n      win\\n      bet\\n      __typename\\n    }\\n    winLines {\\n      id\\n      __typename\\n    }\\n    user {\\n      id\\n      login\\n      lastActivity\\n      wallets {\\n        currency\\n        balance\\n        safeAmount\\n        __typename\\n      }\\n      loyaltyLevel {\\n        level {\\n          id\\n          category\\n          level\\n          __typename\\n        }\\n        __typename\\n      }\\n      privacySettings {\\n        isPMNotificationsEnabled\\n        isWageredHidden\\n        isAnonymous\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"\r\n}";


        private string sDataForStatistick1 = "{\r\n    \"operationName\": null,\r\n    \"variables\": {\r\n        \"message\": \"My statistics\",\r\n        \"channelId\": \"1\",\r\n        \"payload\": \"{\\\"currency\\\":\\\"";
        private string sDataForStatistick2 = "\\\",\\\"betsAmount\\\":";
        private string sDataForStatistick3 = ",\\\"totalProfit\\\":";
        private string sDataForStatistick4 = ",\\\"wins\\\":";
        private string sDataForStatistick5 = ",\\\"losses\\\":";
        private string sDataForStatistick6 = ",\\\"chartData\\\":[";
        private string sDataForStatistick7 = "]}\"\r\n    },\r\n    \"query\": \"mutation ($channelId: ID!, $message: String!, $payload: String) {\\n  chatAddMessage(channelId: $channelId, message: $message, payload: $payload) {\\n    ...FRAGMENT_MESSAGE\\n    __typename\\n  }\\n}\\n\\nfragment FRAGMENT_MESSAGE on ChatMessage {\\n  id\\n  body\\n  message {\\n    body\\n    balance {\\n      amount\\n      currency\\n      __typename\\n    }\\n    transaction {\\n      amount\\n      date\\n      currency\\n      status\\n      __typename\\n    }\\n    rain {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      users\\n      currency\\n      amount\\n      amounts\\n      loyaltyLevelIds\\n      __typename\\n    }\\n    tips {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      recipient {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      currency\\n      amount\\n      message\\n      isPrivate\\n      __typename\\n    }\\n    news {\\n      text\\n      imageUrl\\n      buttonUrl\\n      buttonText\\n      __typename\\n    }\\n    achievement {\\n      levelId\\n      data {\\n        key\\n        value\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  createdAt\\n  isDeleted\\n  payload\\n  likes {\\n    count\\n    likedBy\\n    __typename\\n  }\\n  user {\\n    id\\n    login\\n    avatar\\n    userRoles\\n    lastActivity\\n    loyaltyLevel {\\n      level {\\n        id\\n        category\\n        level\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  type\\n  __typename\\n}\\n\"\r\n}";

        private string sDataForGetUserInfo = "{\"operationName\":null,\"variables\":{},\"query\":\"{\\n  me {\\n    ...FRAGMENT_COMPLETE_USER_DATA\\n    __typename\\n  }\\n}\\n\\nfragment FRAGMENT_COMPLETE_USER_DATA on User {\\n  id\\n  login\\n  email\\n  confirmed\\n  unconfirmedEmail\\n  protected\\n  userRoles\\n  clientSeed\\n  serverSeed\\n  serverSeedNonce\\n  messages\\n  serverSeedNext\\n  twoFactorResetDate\\n  twoFactorEnabled\\n  lastActivity\\n  isLongTimeInactive\\n  avatar\\n  likes\\n  token {\\n    token\\n    __typename\\n  }\\n  friendsIds\\n  ...FRAGMENT_USER_LOYALTY_LVL\\n  ...FRAGMENT_USER_PRIVACY_SETTINGS\\n  ...FRAGMENT_USER_WALLETS\\n  __typename\\n}\\n\\nfragment FRAGMENT_USER_PRIVACY_SETTINGS on User {\\n  privacySettings {\\n    isPMNotificationsEnabled\\n    isWageredHidden\\n    isAnonymous\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment FRAGMENT_USER_LOYALTY_LVL on User {\\n  loyaltyLevel {\\n    level {\\n      ...LOYALTY_LEVEL\\n      __typename\\n    }\\n    progress\\n    isTemporary\\n    endsIn\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment LOYALTY_LEVEL on LoyaltyLevel {\\n  category\\n  level\\n  id\\n  features {\\n    feature\\n    value\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment FRAGMENT_USER_WALLETS on User {\\n  wallets {\\n    currency\\n    balance\\n    address\\n    bonus\\n    rakeback\\n    safeAmount\\n    __typename\\n  }\\n  __typename\\n}\\n\"}";

        private string sDataForGetChatInfo =  "{\"operationName\":\"fetchChat\",\"variables\":{\"channelId\":\"1\",\"user\":null},\"query\":\"query fetchChat($channelId: ID, $user: ID) {\\n  chatChannel(channelId: $channelId, user: $user) {\\n    ...FRAGMENT_CHAT_CHANNEL\\n    __typename\\n  }\\n}\\n\\nfragment FRAGMENT_CHAT_CHANNEL on ChatChannel {\\n  id\\n  key\\n  unreadCount\\n  isPublic\\n  user {\\n    id\\n    login\\n    avatar\\n    lastActivity\\n    loyaltyLevel {\\n      level {\\n        id\\n        category\\n        level\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  messages {\\n    ...FRAGMENT_MESSAGE\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment FRAGMENT_MESSAGE on ChatMessage {\\n  id\\n  body\\n  message {\\n    body\\n    balance {\\n      amount\\n      currency\\n      __typename\\n    }\\n    transaction {\\n      amount\\n      date\\n      currency\\n      status\\n      __typename\\n    }\\n    rain {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      users\\n      currency\\n      amount\\n      amounts\\n      loyaltyLevelIds\\n      __typename\\n    }\\n    tips {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      recipient {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      currency\\n      amount\\n      message\\n      isPrivate\\n      __typename\\n    }\\n    news {\\n      text\\n      imageUrl\\n      buttonUrl\\n      buttonText\\n      __typename\\n    }\\n    achievement {\\n      levelId\\n      data {\\n        key\\n        value\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  createdAt\\n  isDeleted\\n  payload\\n  likes {\\n    count\\n    likedBy\\n    __typename\\n  }\\n  user {\\n    id\\n    login\\n    avatar\\n    userRoles\\n    lastActivity\\n    loyaltyLevel {\\n      level {\\n        id\\n        category\\n        level\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  type\\n  __typename\\n}\\n\"}";

        private string sDataForLike1 = "{\"operationName\":\"likeMessage\",\"variables\":{\"messageId\":";
        private string sDataForLike2 = "},\"query\":\"mutation likeMessage($messageId: ID!) {\\n  likeMessage(messageId: $messageId) {\\n    id\\n    user {\\n      id\\n      login\\n      likes\\n      __typename\\n    }\\n    likes {\\n      count\\n      likedBy\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}";

        private string sDataForMess1 = "{\r\n    \"operationName\": null,\r\n    \"variables\": {\r\n        \"message\": \"";
        private string sDataForMess2 = "\",\r\n        \"channelId\": \"1\"\r\n    },\r\n    \"query\": \"mutation ($channelId: ID!, $message: String!, $payload: ChatMessagePayloadInput) {\\n  chatAddMessageV2(channelId: $channelId, message: $message, payload: $payload) {\\n    ...FRAGMENT_MESSAGE\\n    __typename\\n  }\\n}\\n\\nfragment FRAGMENT_MESSAGE on ChatMessage {\\n  id\\n  body\\n  message {\\n    body\\n    balance {\\n      amount\\n      currency\\n      __typename\\n    }\\n    transaction {\\n      amount\\n      date\\n      currency\\n      status\\n      __typename\\n    }\\n    rain {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      users\\n      currency\\n      amount\\n      amounts\\n      loyaltyLevelIds\\n      __typename\\n    }\\n    tips {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      recipient {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      currency\\n      amount\\n      message\\n      isPrivate\\n      __typename\\n    }\\n    news {\\n      text\\n      imageUrl\\n      buttonUrl\\n      buttonText\\n      __typename\\n    }\\n    achievement {\\n      type\\n      levelId\\n      data {\\n        key\\n        value\\n        __typename\\n      }\\n      __typename\\n    }\\n    shareGift {\\n      recipient {\\n        id\\n        login\\n        avatar\\n        __typename\\n      }\\n      user {\\n        id\\n        login\\n        avatar\\n        __typename\\n      }\\n      gift {\\n        __typename\\n        ... on TemporaryStatus {\\n          id\\n          level {\\n            category\\n            level\\n            id\\n            __typename\\n          }\\n          duration\\n          __typename\\n        }\\n        ... on CoinsPack {\\n          id\\n          alias\\n          coinsAmount\\n          coinsCurrency\\n          __typename\\n        }\\n      }\\n      __typename\\n    }\\n    chatBotContest {\\n      ...FRAGMENT_CHAT_BOT_CONTEST\\n      __typename\\n    }\\n    chatBotContestShared {\\n      ...FRAGMENT_CHAT_BOT_CONTEST\\n      __typename\\n    }\\n    lightSpeedStrategy {\\n      strategy {\\n        id\\n        ...LIGHTSPEED_STRATEGY\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  createdAt\\n  isDeleted\\n  payload\\n  likes {\\n    count\\n    likedBy\\n    __typename\\n  }\\n  user {\\n    id\\n    login\\n    avatar\\n    userRoles\\n    lastActivity\\n    loyaltyLevel {\\n      level {\\n        id\\n        category\\n        level\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  type\\n  __typename\\n}\\n\\nfragment FRAGMENT_CHAT_BOT_CONTEST on ChatBotContest {\\n  id\\n  dateEnd\\n  dateFinished\\n  game\\n  prizeFund\\n  prizeCurrency\\n  minBetAmountUsd\\n  isFinished\\n  task {\\n    ... on ChatBotContestMinesTask {\\n      bombCount\\n      diamondsIndexes\\n      __typename\\n    }\\n    ... on ChatBotContestRouletteTask {\\n      rouletteRoll: roll\\n      __typename\\n    }\\n    ... on ChatBotContestDiceTask {\\n      diceRoll: roll\\n      __typename\\n    }\\n    ... on ChatBotContestSlotTask {\\n      wilds\\n      __typename\\n    }\\n    __typename\\n  }\\n  winningBet {\\n    game\\n    slotGame {\\n      name\\n      __typename\\n    }\\n    offsets\\n    user {\\n      id\\n      login\\n      avatar\\n      __typename\\n    }\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment LIGHTSPEED_STRATEGY on LightspeedStrategy {\\n  name\\n  betAmount\\n  chance\\n  onLossAction\\n  onLossValue\\n  onWinAction\\n  onWinValue\\n  side\\n  betSpeed\\n  stopOnLoss\\n  stopOnWin\\n  __typename\\n}\\n\"\r\n}";

        public RequestBuilder(string token)
        {
            request = new RequestWithData(token);
        }

        public int TakeBet(string bet, string chanse, string side, string сurrency)
        {
            IRestResponse resOfReq=request.GetData(sDataForBet1 + bet + sDataForBet2 + chanse.Replace(",", ".") + sDataForBet3 + side +
                                              sDataForBet4 + сurrency + sDataForBet5);

            var results = JsonConvert.DeserializeObject<DataOfBettingResults>(resOfReq.Content);

            if (results.data.rollDice.win)
            {
                return 1;
            }
            else if (!results.data.rollDice.win)
            {
                return 0;
            }

            // тут нужна проверка на точ то сервер упал

            return 3;
        }

        private void SpamStatistick(string сurrency,double wagered, double profit, int iw, int il)
        {
            IRestResponse resOfReq = request.GetData(sDataForStatistick1 + сurrency + sDataForStatistick2 + ($"{wagered:f8}").Replace(",", ".") + sDataForStatistick3 + ($"{profit:f8}").Replace(",", ".") + sDataForStatistick4 + iw + sDataForStatistick5 + il + sDataForStatistick6 + GetWageredList() + sDataForStatistick7);
        }

        private string GetWageredList()
        {
            bool flagEx = true;
            string wageredList = "";

            do
            {
                try
                {

                    using (StreamReader sr = new StreamReader("wagered.json"))
                    {
                        wageredList = sr.ReadToEnd();
                        flagEx = false;
                    }
                }
                catch (System.IO.IOException e)
                {

                }

            } while (flagEx);


            wageredList = wageredList.Remove(wageredList.Length - 1);

            return wageredList;
        }

        private InfoAboutUser GetUserInfo()
        {
            IRestResponse resOfReq = request.GetData(sDataForGetUserInfo);
            InfoAboutUser infUser = JsonConvert.DeserializeObject<InfoAboutUser>(resOfReq.Content);
            return infUser;
        }

        private ChatInfo GetInfoAboutChat()
        {
            IRestResponse resOfReq = request.GetData(sDataForGetChatInfo);
            ChatInfo infChat = JsonConvert.DeserializeObject<ChatInfo>(resOfReq.Content);
            return infChat;
        }

        private void LikeMess(int idUser)
        {
            IRestResponse resOfReq = request.GetData(sDataForLike1+idUser+ sDataForLike2);
        }

        private void SendMess(string mess)
        {
            IRestResponse resOfReq = request.GetData(sDataForMess1+mess+ sDataForMess2);
        }
    }
}