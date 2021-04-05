namespace paradiceinBOT
{
    public class Controller
    {
        CheckAuthorization checkAuthorization = new CheckAuthorization();
        private string token;

        public Controller()
        {

        }

        public bool Chek()
        {
            bool res = checkAuthorization.Chek();
            if (res)
            {
                token = checkAuthorization.Gettoken();
            }
            return res;
        }

    }
}