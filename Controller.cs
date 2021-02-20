using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace paradiceinBOT
{
    class Controller
    {
        private Thread ThreadFoBot;
        private WriteReadFile dataWiter;
        private RequestWithStatistic requestWithStatistic;
       
        //Блок для вывода инфы
        private TextBlock frontStatusBlock;

        //Индекс который отвечает на что ставить (больше, меньше или чередовать)
        private int ab;

        private ClassBet botBet;

        //Значение шанса
        private double chance;

        private DataForBet data;

        public Controller(TextBlock frontStatusBlock, double baseBetD, string baseBetS, int ab, string сurrency, double chance, double multyProfit, bool flagMultyOnWin, double multyOnWin, bool flagMultyOnLose, double multyOnLose)
        {
            CheckAuthorization author = new CheckAuthorization();
            int flagauthor = author.Autorization();

            if (flagauthor==1)
            {
                data = new DataForBet();
                data.token = author.GetToken();
                data.baseBetD = baseBetD;
                data.baseBetS = baseBetS;
                data.сurrency = сurrency;
                data.multyProfit = multyProfit;
                data.chanceD = chance;
                data.flagMultyOnLose = flagMultyOnLose;
                data.flagMultyOnWin = flagMultyOnWin;
                data.multyOnLose = multyOnLose;
                data.multyOnWin = multyOnWin;

                this.chance = chance;

                this.frontStatusBlock = frontStatusBlock;

                this.ab = ab;

                dataWiter = new WriteReadFile();
                requestWithStatistic = new RequestWithStatistic(data, dataWiter);
            }
            else if (flagauthor == 0)
            {
                frontStatusBlock.Text = "Token don`t work. Please check it!";
            }
            else
            {
                frontStatusBlock.Text = "Something went wrong. Check the token file!";
            }

        }

        public void OnceSpam()
        {
            requestWithStatistic.RequestToSite();
        }

        public void Start()
        {

            if (ab == 1)
            {
                data.side = "ABOVE";
                data.chanceS = Convert.ToString(100.0 - chance);
                botBet = new ClassBet(frontStatusBlock, data, dataWiter);

                if (!data.flagMultyOnLose && !data.flagMultyOnWin)
                {
                    ThreadFoBot = new Thread(botBet.StartNormally);
                }
                else if(data.flagMultyOnWin&& data.flagMultyOnLose)
                {
                    ThreadFoBot = new Thread(botBet.StartWithMultOnWinLoseNormally);
                }
                else if (data.flagMultyOnLose)
                {
                    ThreadFoBot = new Thread(botBet.StartWithMultOnLoseNormally);
                }
                else if (data.flagMultyOnWin)
                {
                    ThreadFoBot = new Thread(botBet.StartWithMultOnWinNormally);
                }
            }
            else if (ab == 2)
            {
                data.side = "BELOW";
                data.chanceS = Convert.ToString(chance);
                botBet = new ClassBet(frontStatusBlock, data, dataWiter);

                if (!data.flagMultyOnLose && !data.flagMultyOnWin)
                {
                    ThreadFoBot = new Thread(botBet.StartNormally);
                }
                else if (data.flagMultyOnWin && data.flagMultyOnLose)
                {
                    ThreadFoBot = new Thread(botBet.StartWithMultOnWinLoseNormally);
                }
                else if (data.flagMultyOnLose)
                {
                    ThreadFoBot = new Thread(botBet.StartWithMultOnLoseNormally);
                }
                else if (data.flagMultyOnWin)
                {
                    ThreadFoBot = new Thread(botBet.StartWithMultOnWinNormally);
                }
            }
            else
            {
                data.side = "ABOVE";
                data.chanceS = Convert.ToString(100.0 - chance);
                botBet = new ClassBet(frontStatusBlock, data, dataWiter);

                if (!data.flagMultyOnLose && !data.flagMultyOnWin)
                {
                    ThreadFoBot = new Thread(botBet.StartNormallyWinthChangeSide);
                }
                else if (data.flagMultyOnWin && data.flagMultyOnLose)
                {

                    ThreadFoBot = new Thread(botBet.StartWithMultOnWinLoseWithChangeSide);
                }
                else if (data.flagMultyOnLose)
                {

                    ThreadFoBot = new Thread(botBet.StartWithMultOnLoseWinthChangeSide);
                }
                else if (data.flagMultyOnWin)
                {

                    ThreadFoBot = new Thread(botBet.StartWithMultOnWinWithChangeSide);
                }

            }

            ThreadFoBot.IsBackground = true;
            ThreadFoBot.Start();
        }

        public void OnOut()
        {
            ThreadFoBot.Suspend();
        }

        public void OnIn()
        {
            ThreadFoBot.Resume();
        }
    }
}