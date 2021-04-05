using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using paradiceinBOT.Data_Classes;

namespace paradiceinBOT
{
    public class Game
    {
        private static DataForBet dataForBet;
        private DataForBetRequest dataForBetRequest;
        private static DataForSpamStatistic dataForSpamStatistic = new DataForSpamStatistic();
        private static WriteFile writer = new WriteFile();
        private static RequestBuilder builder =  new RequestBuilder("");
        private  ReadFileWIthBets readFileWIthBets = new ReadFileWIthBets();

        //Токен авторизации
        private string token;

        //Счётчики: Всего ставок, Текущий стак побед, Текущий стак поражений
        private static long i = 0;
        private int maxL, maxW;



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
        private int result;


        //Блок инфы
        private static TextBlock frontStatusBlock = new TextBlock();

        private static int sesMaxLose = 0;
        private static int sesMaxWin = 0;

        //flag of Spam on bets
        private static bool flagOfSpam = false;

        //count of bets for Spam
        private static int countBetsForSpam = 10000;


        public Game(TextBlock frontStatusBlockFromUI, DataForBet data, int countBetsForSpamIn, bool flagForSpamOnBets, RequestBuilder builder1)
        {
            builder = builder1;
            dataForBetRequest = new DataForBetRequest();
            dataForBetRequest.сurrency = dataForBet.сurrency;

            dataForBet = data;
            frontStatusBlock = frontStatusBlockFromUI;


            maxL = 0;
            maxW = 0;

            chance = ch.ToString();
            chance = chance.Replace(",", ".");

            dataForBet.betS = $"{dataForBet.baseBetD:f8}";
            dataForBet.betS = dataForBet.betS.Replace(",", ".");
            dataForBet.betD = dataForBet.baseBetD;
            dataForBet.wagered += dataForBet.baseBetD;

            i = 0;
            sesMaxLose = 0;
            sesMaxWin = 0;

            flagOfSpam = flagForSpamOnBets;
            countBetsForSpam = countBetsForSpamIn;
        }

        private void СopyData()
        {
            dataForBetRequest.bet = dataForBet.betS;
            dataForBetRequest.side = dataForBet.side;
            dataForBetRequest.chance = dataForBet.chanceS;
        }

        #region LogicOfStrategy
        public void StartNormally()
        {
            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result == 1)
                {
                    dataForBet.profit += ((dataForBet.baseBetD * dataForBet.multyProfit) - dataForBet.baseBetD);

                    dataForBet.countOfAllWins++;

                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.baseBetD;

                    dataForBet.countOfAllLose++;

                    maxL++;
                    maxW = 0;
                    CalculateOutputInformation();
                }
                else
                {
                    //error
                }


            } while (true);
        }
        public void StartNormallyWinthChangeSide()
        {
            bool flagSide = true;

            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result == 1)
                {
                    dataForBet.profit += ((dataForBet.baseBetD * dataForBet.multyProfit) - dataForBet.baseBetD);

                    dataForBet.countOfAllWins++;

                    maxL = 0;
                    maxW++;

                    flagSide = ChangeSide(flagSide);

                    CalculateOutputInformation();
                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.baseBetD;

                    dataForBet.countOfAllLose++;

                    maxL++;
                    maxW = 0;

                    flagSide = ChangeSide(flagSide);

                    CalculateOutputInformation();
                }
                else
                {
                    //error
                }


            } while (true);
        }
        public void StartWithMultOnWinNormally()
        {
            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result==1)
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    dataForBet.countOfAllWins++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");
                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.betD;
                    dataForBet.countOfAllLose++;
                    maxL++;
                    maxW = 0;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.baseBetD;
                    dataForBet.betS = dataForBet.baseBetS;
                }
                else
                {
                    //error
                }
            } while (true);
        }
        public void StartWithMultOnWinWithChangeSide()
        {

            bool flagSide = true;
            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result == 1)
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    dataForBet.countOfAllWins++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");

                    flagSide = ChangeSide(flagSide);
                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.betD;
                    dataForBet.countOfAllLose++;
                    maxL++;
                    maxW = 0;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.baseBetD;
                    dataForBet.betS = dataForBet.baseBetS;

                    flagSide = ChangeSide(flagSide);
                }
                else
                {
                    //error
                }
            } while (true);
        }
        public void StartWithMultOnLoseNormally()
        {
            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result == 1)
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    dataForBet.countOfAllWins++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.baseBetD;
                    dataForBet.betS = dataForBet.baseBetS;
                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.betD;
                    dataForBet.countOfAllLose++;
                    maxL++;
                    maxW = 0;
                    CalculateOutputInformation();


                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnLose;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");
                }
                else
                {
                    //error
                }
            } while (true);
        }
        public void StartWithMultOnLoseWinthChangeSide()
        {
            bool flagSide = true;
            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result == 1)
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    dataForBet.countOfAllWins++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.baseBetD;
                    dataForBet.betS = dataForBet.baseBetS;

                    flagSide = ChangeSide(flagSide);
                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.betD;
                    dataForBet.countOfAllLose++;
                    maxL++;
                    maxW = 0;
                    CalculateOutputInformation();


                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnLose;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");

                    flagSide = ChangeSide(flagSide);
                }
                else
                {
                    //error
                }
            } while (true);
        }
        public void StartWithMultOnWinLoseNormally()
        {
            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result == 1)
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    dataForBet.countOfAllWins++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");
                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.betD;
                    dataForBet.countOfAllLose++;
                    maxL++;
                    maxW = 0;
                    CalculateOutputInformation();


                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnLose;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");
                }
                else
                {
                    //error
                }
            } while (true);
        }
        public void StartWithMultOnWinLoseWithChangeSide()
        {
            bool flagSide = true;
            do
            {
                СopyData();
                result = builder.TakeBet(dataForBetRequest);

                if (result == 1)
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    dataForBet.countOfAllWins++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");

                    flagSide = ChangeSide(flagSide);
                }
                else if (result == 0)
                {
                    dataForBet.profit -= dataForBet.baseBetD;
                    dataForBet.countOfAllLose++;
                    maxL++;
                    maxW = 0;
                    CalculateOutputInformation();


                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnLose;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");

                    flagSide = ChangeSide(flagSide);
                }
                else
                {
                    //error
                }
            } while (true);
        }
        #endregion

        

        private static void InfOut()
        {
            if (i % 5500 == 0)
            {
                writer.ReCreateFile();
            }
            writer.AddProfitToFile(dataForBet.profit);

            dataForSpamStatistic.SetData(dataForBet.сurrency, dataForBet.wagered, dataForBet.profit,
                dataForBet.countOfAllWins, dataForBet.countOfAllLose);
            writer.AddDataForSpamStatiscToFile(dataForSpamStatistic);
            

            frontStatusBlock.Dispatcher.Invoke(new Action(() => frontStatusBlock.Text =
                "AllBets: " + i +
                "\nWins " + dataForBet.countOfAllWins +
                "\nLosses " + dataForBet.countOfAllLose +
                "\nMaxLosses " + sesMaxLose +
                "\nMaxWins " + sesMaxWin +
                "\nProfit " + $"{dataForBet.profit:f8}"));
            if (flagOfSpam)
            {
                if (i % countBetsForSpam == 0)
                {
                    builder.SpamStatistick();
                }
            }
        }
        private static async void InfOutAsync()
        {
            Task.Run(() => InfOut());
        }
        private bool ChangeSide(bool flag)
        {
            if (flag)
            {
                dataForBet.side = "BELOW";
                dataForBet.chanceS = (100 - dataForBet.chanceD).ToString();
                dataForBet.chanceS = dataForBet.chanceS.Replace(",", ".");
                return false;
            }
            else
            {
                dataForBet.side = "ABOVE";
                dataForBet.chanceS = dataForBet.chanceD.ToString();
                dataForBet.chanceS = dataForBet.chanceS.Replace(",", ".");

                return true;
            }
        }

        private void CalculateOutputInformation()
        {
            sesMaxLose = maxL > sesMaxLose ? maxL : sesMaxLose;
            sesMaxWin = maxW > sesMaxWin ? maxW : sesMaxWin;
            dataForBet.wagered += dataForBet.betD;
            i++;
            InfOutAsync();
        }
    }
}
