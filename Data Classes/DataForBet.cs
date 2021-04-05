namespace paradiceinBOT
{
    public class DataForBet
    {
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

        public double wagered;

        public int iw;
        public int il;

        public DataForBet()
        {
            profit = 0.0;
            wagered = 0.0;
            iw = 0;
            il = 0;
        }

        //Stop flags
        public bool flagStopOnCountOfBet;
        public bool flagStopOnCountOfLose;
        public bool flagStopOnCountOfWin;
        public bool flagStopOnCountProfit;

        public int StopOnCountOfBet;
        public int StopOnCountOfLose;
        public int StopOnCountOfWin;
        public double StopOnCountProfit;


    }
}