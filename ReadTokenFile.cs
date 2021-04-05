using System.IO;

namespace paradiceinBOT
{
    public class ReadTokenFile
    {
        private string token = "";
        public ReadTokenFile(){ }

        public string ReadToken()
        {
            if (!File.Exists("token.txt"))
            {
                return "";
            }
            else
            {
                using (StreamReader sr = new StreamReader(@"token.txt"))
                {
                    token = sr.ReadToEnd();
                }
                return token;
                
            }
        }
    }
}