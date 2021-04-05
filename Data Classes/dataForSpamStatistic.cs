namespace paradiceinBOT
{
    public class DataForSpamStatistic
    {
        public string сurrency;
        public string wagered;
        public string profit;
        public string win;
        public string lose;

        public void SetData(string сurrency, double wagered, double profit,int win, int lose)
        {
            this.сurrency = сurrency;
            this.wagered = ($"{wagered:f8}").Replace(",", ".");
            this.profit = ($"{profit:f8}").Replace(",", ".");
            this.win = win.ToString();
            this.lose = lose.ToString();
        }
    }
}