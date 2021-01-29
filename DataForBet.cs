namespace paradiceinBOT
{
    public class DataForBet
    {
        //User token
       public string token;

       //base bet from UI
       public double baseBetD;
       public string baseBetS;

       //bet
       public double betD;
       public string betS;

        //currency (DOGE, PRDC, BTC etc)
        public string сurrency;

        //gain factor for current chance
        public double multyProfit;

        public string chanceS;
        public double chanceD;


        //multiply on win
        public bool flagMultyOnWin;
        public double multyOnWin;

        //multiply on lose
        public bool flagMultyOnLose;
        public double multyOnLose;

        public string side;

        public double profit;

        public DataForBet()
        {
            profit = 0.0;
        }

    }
}