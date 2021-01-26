using RestSharp;
using System;
using System.IO;
using System.Windows.Controls;

namespace paradiceinBOT
{
    public class ClassBet
    {
        //Токен авторизации
        private string token;

        //Счётчики: Всего ставок, Текущий стак побед, Текущий стак поражений
        private long i = 0;
        private int maxL, maxW;

        //Счётчки побед и поражений
        private int iw = 0, il=0;

        //Профит (на основе игры)
        private double profit = 0;

        //Базовая ставка и текущая в дабле(для расчётов) и стринге (для запроса)
        private double baseBetD;
        private double betD;
        private string baseBetS;
        private string betS;

        private string side;
        private string chance;

        private double percent;
        private double ch;

        //Валюта игры
        private string сurrency;

        //Ответ от сервера
        private string result;

        //Максимум стак побед/поражений за сессию игры
        private int sesMaxLose = 0;
        private int sesMaxWin = 0;

        //Блок инфы
        private TextBlock frontStatusBlock;

        //Клиент для запросов
        private RestClient client = new RestClient("https://api.paradice.in/api.php");
        RestRequest request = new RestRequest(Method.POST);

        private string s1 = "{\r\n    \"operationName\": \"rollDice\",\r\n    \"variables\": {\r\n        \"betAmount\": \"";
        private string s2 = "\",\r\n        \"number\": \"";
        private string s3 = "\",\r\n        \"side\": \"";
        private string s4 = "\",\r\n        \"currency\": \"";
        private string s5 = "\"\r\n    },\r\n    \"query\": \"mutation rollDice($number: Float!, $betAmount: Float!, $side: RollSideEnum!, $currency: CurrencyEnum!) {\\n  rollDice(number: $number, betAmount: $betAmount, side: $side, currency: $currency) {\\n    id\\n    number\\n    roll\\n    rollSide\\n    win\\n    betAmount\\n    winAmount\\n    currency\\n    multiplier\\n    chance\\n    game\\n    bets {\\n      pocket\\n      payout\\n      win\\n      bet\\n      __typename\\n    }\\n    winLines {\\n      id\\n      __typename\\n    }\\n    user {\\n      id\\n      login\\n      lastActivity\\n      wallets {\\n        currency\\n        balance\\n        safeAmount\\n        __typename\\n      }\\n      loyaltyLevel {\\n        level {\\n          id\\n          category\\n          level\\n          __typename\\n        }\\n        __typename\\n      }\\n      privacySettings {\\n        isPMNotificationsEnabled\\n        isWageredHidden\\n        isAnonymous\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"\r\n}";


        public ClassBet(TextBlock frontStatusBlock, double baseBetD, string baseBetS, string сurrency, string side, string token, double ch, double percent)
        {
            this.frontStatusBlock = frontStatusBlock;
            this.baseBetD = baseBetD;
            this.baseBetS = baseBetS;
            this.сurrency = сurrency;
            this.percent = percent;

            this.side = side;

            betD = this.baseBetD;
            betS = this.baseBetS;

            maxL = 0;
            maxW = 0;

            this.token = token;

            this.ch = ch;

            chance = ch.ToString();
            chance = chance.Replace(",", ".");

            AddParametersToRequest();
        }

        private void AddParametersToRequest()
        {
            client.Timeout = -1;
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
            request.AddHeader("Cookie", "__cfduid=d05ec5fcc4e574b8c77a5e81dc51caa411607638398; _ga=GA1.2.1474332697.1608739211");
            request.AddParameter("application/json", s1 + betS + s2 + chance + s3 + side + s4 + сurrency + s5, ParameterType.RequestBody);
        }
        private void InfOut()
        {
            frontStatusBlock.Dispatcher.Invoke(new Action(() => frontStatusBlock.Text =
                "Ставки: " + i +
                "\nПобеды " + iw +
                "\nПоражения " + il +
                "\nMaxLose " + sesMaxLose +
                "\nMaxWin " + sesMaxWin +
                "\nПрофит " + $"{profit:f8}"));
        }

        public void Start()
        {
            do
            {
                int fl;

                result = Bet();

                if (result.Contains("win\":true"))
                {
                    profit = profit + ((betD * percent) - betD);

                    iw++;
                    fl = 1;
                    
                }
                else if (result.Contains("win\":false"))
                {
                    profit = profit - betD;

                    il++;
                    fl = 0;
                }
                else
                {
                    fl = 2;
                }
                

                if (fl == 0)
                {
                    betD = betD * 10;
                    betS = $"{betD:f8}";

                    betS = betS.Replace(",", ".");

                    maxL++;
                    maxW = 0;

                    CalculateOutputInformation();
                }
                else if (fl == 1)
                {
                    betS = baseBetS;
                    betD = baseBetD;

                    maxL = 0;
                    maxW++;

                    CalculateOutputInformation();
                }
                else
                {
                    //error
                }

            } while (true);
        }

        private bool ChangeSide(bool flag)
        {
            if (flag)
            {
                side = "BELOW";
                chance = (100 - ch).ToString();
                chance = chance.Replace(",", ".");
                return false;
            }
            else
            {
                side = "ABOVE";
                chance = ch.ToString();
                chance = chance.Replace(",", ".");
                return true;
            }
        }

        public void Start1()
        {
            bool flagSide = true;
            do
            {
                int fl;

                result = Bet();

                if (result.Contains("win\":true"))
                {
                    profit = profit + ((betD * percent)- betD);

                    iw++;
                    fl = 1;

                    flagSide = ChangeSide(flagSide);
                }
                else if (result.Contains("win\":false"))
                {
                    profit = profit - betD;

                    il++;
                    fl = 0;

                    flagSide = ChangeSide(flagSide);
                }
                else
                {
                    fl = 2;
                }

                if (fl == 0)
                {
                    betD = betD * 2;
                    betS = $"{betD:f8}";

                    betS = betS.Replace(",", ".");

                    maxL++;
                    maxW = 0;

                    CalculateOutputInformation();
                }
                else if (fl == 1)
                {
                    betS = baseBetS;
                    betD = baseBetD;

                    maxL = 0;
                    maxW++;

                    CalculateOutputInformation();
                }
                else
                {
                    //error
                }

            } while (true);
        }

        private void CalculateOutputInformation()
        {
            sesMaxLose = maxL > sesMaxLose ? maxL : sesMaxLose;
            sesMaxWin = maxW > sesMaxWin ? maxW : sesMaxWin;

            i++;
            InfOut();
        }
        private void AddBetParameter()
        {
            request.Parameters[10].Value = s1 + betS + s2 + chance + s3 + side + s4 + сurrency + s5;
        }
        private string Bet()
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
