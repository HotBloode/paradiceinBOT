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
        private Thread ThreadOfSpamMins;
        private WriteReadFile dataWiter;
        private RequestWithStatistic requestWithStatistic;
       
        //Блок для вывода инфы
        private TextBlock frontStatusBlock;

        //Индекс который отвечает на что ставить (больше, меньше или чередовать)
        private int ab;

        private ClassBet botBet;

        //chance of win 
        private double chance;

        private DataForBet data;

        //flags of spam
        private bool flagOfSpamOnBets;
        private bool flagOfSpamOnMins;

        private int mins;
        private int bets;

        public Controller(TextBlock frontStatusBlock,
            double baseBetD,
            string baseBetS,
            int ab,
            string сurrency,
            double chance,
            double multyProfit,
            bool flagMultyOnWin,
            double multyOnWin,
            bool flagMultyOnLose,
            double multyOnLose,
            bool flagOfSpamOnBets,
            bool flagOfSpamOnMins,
            int mins,
            int bets)
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

                this.flagOfSpamOnBets = flagOfSpamOnBets;
                this.flagOfSpamOnMins = flagOfSpamOnMins;
                this.mins = mins;
                this.bets = bets;

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

        public void Stop()
        {
            ThreadFoBot.Abort();
            ThreadOfSpamMins.Abort();
        }

        public void Start()
        {

            if (ab == 1)
            {
                data.side = "ABOVE";
                data.chanceS = Convert.ToString(100.0 - chance);
                botBet = new ClassBet(frontStatusBlock, data, dataWiter, bets, flagOfSpamOnBets, requestWithStatistic);

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
                botBet = new ClassBet(frontStatusBlock, data, dataWiter, bets, flagOfSpamOnBets, requestWithStatistic);

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
                botBet = new ClassBet(frontStatusBlock, data, dataWiter, bets, flagOfSpamOnBets, requestWithStatistic);

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

            if (flagOfSpamOnMins)
            {
                ThreadOfSpamMins = new Thread(SpamMins);
                ThreadOfSpamMins.Start();
            }
        }

        private void SpamMins()
        {
            while (true)
            {
                Thread.Sleep(mins * 60000);
                OnceSpam();
            }
        }

        public void OnOut()
        {
            ThreadFoBot.Suspend();
            ThreadOfSpamMins.Suspend();
        }

        public void OnIn()
        {
            ThreadFoBot.Resume();
            ThreadOfSpamMins.Resume();
        }
    }
}