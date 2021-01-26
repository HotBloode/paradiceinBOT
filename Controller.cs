using System;
using System.Collections.Generic;
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
        private double percent;

        //Блок для вывода инфы
        private TextBlock frontStatusBlock;

        //Базовая ставка с тринге и в дабле
        private double baseBetD;
        private string baseBetS;

        //Индекс который отвечает на что ставить (больше, меньше или чередовать)
        private int ab;

        //Валюта на которую играем
        private string сurrency;

        private ClassBet botBet;

        private string token;

        //Значение шанса
        private double chance;

        public Controller(TextBlock frontStatusBlock, double baseBetD, string baseBetS, int ab, string сurrency, double chance, double percent)
        {
            using (StreamReader sr = new StreamReader(@"token.txt"))
            {
                token = sr.ReadToEnd();
            }

            //Проихнуть проверку авторизации

            this.percent = percent;
            this.frontStatusBlock = frontStatusBlock;
            this.baseBetD = baseBetD;
            this.baseBetS = baseBetS;
            this.ab = ab;
            this.сurrency = сurrency;
            this.chance = chance;
        }

        public void Start()
        {

            if (ab == 1)
            {
                botBet = new ClassBet(frontStatusBlock, baseBetD, baseBetS, сurrency, "ABOVE", token, 100.0- chance, percent);
                ThreadFoBot = new Thread(new ThreadStart(botBet.Start));
                ThreadFoBot.Start();
            }
            else if(ab==2)
            {
                botBet = new ClassBet(frontStatusBlock, baseBetD, baseBetS, сurrency, "BELOW", token, chance, percent);
                ThreadFoBot = new Thread(new ThreadStart(botBet.Start));
                ThreadFoBot.Start();
            }
            else
            {
                botBet = new ClassBet(frontStatusBlock, baseBetD, baseBetS, сurrency, "ABOVE", token, 100.0 - chance, percent);
                ThreadFoBot = new Thread(new ThreadStart(botBet.Start1));
                ThreadFoBot.Start();
            }
           
        }


    }
}