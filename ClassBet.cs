﻿using RestSharp;
using System;
using System.IO;
using System.Windows.Controls;

namespace paradiceinBOT
{
    public class ClassBet
    {
        private DataForBet dataForBet;
        private DataForRequest dataForRequest;
        private RequestWithData requestWithData;

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

     
        public ClassBet(TextBlock frontStatusBlock, DataForBet data)
        {
            this.dataForBet = data;
            this.frontStatusBlock = frontStatusBlock;

            dataForRequest = new DataForRequest();
            dataForRequest.token = dataForBet.token;
            dataForRequest.сurrency = dataForBet.сurrency;

            requestWithData = new RequestWithData(dataForRequest);

            
            maxL = 0;
            maxW = 0;

            chance = ch.ToString();
            chance = chance.Replace(",", ".");

            dataForBet.betS = $"{dataForBet.baseBetD:f8}";
            dataForBet.betS = dataForBet.betS.Replace(",", ".");
            dataForBet.betD = dataForBet.baseBetD;
        }

        private void СopyData()
        {
            dataForRequest.bet = dataForBet.betS;
            dataForRequest.side = dataForBet.side;
            dataForRequest.chance = dataForBet.chanceS;
        }


        public void StartNormally()
        {
            do
            {
                СopyData();
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.baseBetD * dataForBet.multyProfit) - dataForBet.baseBetD);

                    iw++;

                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -= dataForBet.baseBetD;

                    il++;

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
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.baseBetD * dataForBet.multyProfit) - dataForBet.baseBetD);

                    iw++;

                    maxL = 0;
                    maxW++;

                    flagSide = ChangeSide(flagSide);

                    CalculateOutputInformation();
                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -=  dataForBet.baseBetD;

                    il++;

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
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    iw++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");
                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -= dataForBet.betD;
                    il++;
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
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    iw++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");

                    flagSide = ChangeSide(flagSide);
                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -= dataForBet.betD;
                    il++;
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
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    iw++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.baseBetD;
                    dataForBet.betS = dataForBet.baseBetS;
                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -= dataForBet.betD;
                    il++;
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
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    iw++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.baseBetD;
                    dataForBet.betS = dataForBet.baseBetS;

                    flagSide = ChangeSide(flagSide);
                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -= dataForBet.betD;
                    il++;
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
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    iw++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");
                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -= dataForBet.betD;
                    il++;
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
                result = requestWithData.TakeBet();

                if (result.Contains("win\":true"))
                {
                    dataForBet.profit += ((dataForBet.betD * dataForBet.multyProfit) - dataForBet.betD);
                    iw++;
                    maxL = 0;
                    maxW++;
                    CalculateOutputInformation();

                    dataForBet.betD = dataForBet.betD * dataForBet.multyOnWin;
                    dataForBet.betS = $"{dataForBet.betD:f8}";
                    dataForBet.betS = dataForBet.betS.Replace(",", ".");

                    flagSide = ChangeSide(flagSide);
                }
                else if (result.Contains("win\":false"))
                {
                    dataForBet.profit -= dataForBet.baseBetD;
                    il++;
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
       

        private void InfOut()
        {
            frontStatusBlock.Dispatcher.Invoke(new Action(() => frontStatusBlock.Text =
                "Ставки: " + i +
                "\nПобеды " + iw +
                "\nПоражения " + il +
                "\nMaxLose " + sesMaxLose +
                "\nMaxWin " + sesMaxWin +
                "\nПрофит " + $"{dataForBet.profit:f8}"));
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
                dataForBet.chanceS =  dataForBet.chanceD.ToString();
                dataForBet.chanceS = dataForBet.chanceS.Replace(",", ".");

                return true;
            }
        }

        private void CalculateOutputInformation()
        {
            sesMaxLose = maxL > sesMaxLose ? maxL : sesMaxLose;
            sesMaxWin = maxW > sesMaxWin ? maxW : sesMaxWin;
            i++;
            InfOut();
        }
       
    }
}
