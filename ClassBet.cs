using RestSharp;
using System;
using System.IO;
using System.Windows.Controls;

namespace paradiceinBOT
{
    public class ClassBet
    {
        private string path = @"token.txt";
        private string token;

        private long i = 0;
        private long iw = 0;
        private long il = 0;

        private double profit = 0;


        private double baseBetD;
        private double betD;

        private string baseBetS;
        private string betS;


        private string side = "ABOVE";
        private string chan = "52";
        private string сurrency;

        private string result;

        private int sesMaxLose = 0;
        private int sesMaxWin = 0;

        private int maxL, maxW;

        private TextBlock frontStatusBlock;

        private RestClient client = new RestClient("https://api.paradice.in/api.php");
        RestRequest request = new RestRequest(Method.POST);

        private string s1 = "{\r\n    \"operationName\": \"rollDice\",\r\n    \"variables\": {\r\n        \"betAmount\": \"";
        private string s2 = "\",\r\n        \"number\": \"";
        private string s3 = "\",\r\n        \"side\": \"";
        private string s4 = "\",\r\n        \"currency\": \"";
        private string s5 = "\"\r\n    },\r\n    \"query\": \"mutation rollDice($number: Float!, $betAmount: Float!, $side: RollSideEnum!, $currency: CurrencyEnum!) {\\n  rollDice(number: $number, betAmount: $betAmount, side: $side, currency: $currency) {\\n    id\\n    number\\n    roll\\n    rollSide\\n    win\\n    betAmount\\n    winAmount\\n    currency\\n    multiplier\\n    chance\\n    game\\n    bets {\\n      pocket\\n      payout\\n      win\\n      bet\\n      __typename\\n    }\\n    winLines {\\n      id\\n      __typename\\n    }\\n    user {\\n      id\\n      login\\n      lastActivity\\n      wallets {\\n        currency\\n        balance\\n        safeAmount\\n        __typename\\n      }\\n      loyaltyLevel {\\n        level {\\n          id\\n          category\\n          level\\n          __typename\\n        }\\n        __typename\\n      }\\n      privacySettings {\\n        isPMNotificationsEnabled\\n        isWageredHidden\\n        isAnonymous\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"\r\n}";


        public ClassBet(TextBlock frontStatusBlock, double baseBetD, string baseBetS, string сurrency)
        {
            this.frontStatusBlock = frontStatusBlock;
            this.baseBetD = baseBetD;
            this.baseBetS = baseBetS;
            this.сurrency = сurrency;

            betD = this.baseBetD;
            betS = this.baseBetS;

            maxL = 0;
            maxW = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                token = sr.ReadToEnd();
            }

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
            request.AddParameter("application/json", s1 + betS + s2 + chan + s3 + side + s4 + сurrency + s5, ParameterType.RequestBody);
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
                    profit = profit + betD;

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

                #region Мартингейл

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

                #endregion

                #region Даламбер 

                //lose
                //if (fl == 0)
                //{

                //    betD = betD + baseBetD;
                //    betS = $"{betD:f8}";
                //    betS = betS.Replace(",", ".");
                //    maxL++;
                //}
                ////win
                //else if (fl == 1)
                //{
                //    betD = betD <= baseBetD ? baseBetD : betD - baseBetD;
                //    betS = $"{betD:f8}";
                //    betS = betS.Replace(",", ".");
                //    maxL = 0;
                //}
                #endregion

                #region При победе x2, при проигрыше /2

                ////lose
                //if (fl == 0)
                //{

                //    betD = betD * 2;
                //    betS = $"{betD:f8}";
                //    betS = betS.Replace(",", ".");
                //    maxL++;
                //}
                ////win
                //else if (fl == 1)
                //{
                //    betD = betD < (betD / 2) ? baseBetD : betD / 2;
                //    betS = $"{betD:f8}";
                //    betS = betS.Replace(",", ".");
                //    maxL = 0;
                //}

                #endregion

                #region Фибоначи
                /* Это впихнуть в начало функции
                int ii = 0;
                List<double> listD = new List<double>() { 0.00000001, 0.00000001, 0.00000002, 0.00000003, 0.00000005, 0.00000008, 0.00000013, 0.00000021, 0.00000034, 0.00000055, 0.00000089, 0.00000144, 0.00000223, 0.00000377, 0.00000610, 0.00000987, 0.00001597, 0.00002584, 0.00004181, 0.00006765, 0.00010946, 0.00017711, 0.00028657, 0.00046468, 0.00075025, 0.00121393, 0.00196418, 0.00317811 };
                List<string> listS = new List<string>() { "0.00000001", "0.00000001", "0.00000002", "0.00000003", "0.00000005", "0.00000008", "0.00000013", "0.00000021", "0.00000034", "0.00000055", "0.00000089", "0.00000144", "0.00000223", "0.00000377", "0.00000610", "0.00000987", "0.00001597", "0.00002584", "0.00004181", "0.00006765", "0.00010946", "0.00017711", "0.00028657", "0.00046468", "0.00075025", "0.00121393", "0.00196418", "0.00317811" };
                */
                //if (fl == 0)
                //{
                //    ii++;
                //    betD =listD[ii];
                //    betS = listS[ii];
                //    maxL++;
                //}
                ////win
                //else if (fl == 1)
                //{

                //    ii = ii - 2 < 0 ? 0 : ii - 2;
                //    betD = listD[ii];
                //    betS = listS[ii];

                //    maxL = 0;
                //}
                #endregion

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
            request.Parameters[10].Value = s1 + betS + s2 + chan + s3 + side + s4 + сurrency + s5;
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
