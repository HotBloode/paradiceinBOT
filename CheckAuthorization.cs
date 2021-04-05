using System.Security.Cryptography.X509Certificates;

namespace paradiceinBOT
{
    public class CheckAuthorization
    {
        private string token;
        private RequestBuilder requestBuilder;
        public CheckAuthorization()
        {

        }

        public bool Chek()
        {
            ReadTokenFile fileReader = new ReadTokenFile();
            token = fileReader.ReadToken();
            if (token == "" || token == " ")
            {
                return false;
            }
            RequestBuilder requestBuilder = new RequestBuilder(token);
            var x = requestBuilder.GetUserInfo();
            if (x.data.me != null)
            {
                //if token work
                return true;
            }
            else
            {
                //if token don`t work
                return false;
            }
        }

        public string Gettoken()
        {
            return token;
        }
    }
}