using System;
using System.IO;
using System.Text;

namespace paradiceinBOT
{
    public class WriteReadFile
    {
        public WriteReadFile()
        {
            FileStream fs;
            if (!File.Exists("wagered.json"))
            {
                fs = File.Create("wagered.json");
            }
            else
            {
                File.Delete("wagered.json");
                fs = File.Create("wagered.json");
            }

            fs.Close();
        }

        public void AddProfitToFile(double prof)
        {
            bool flagEx = true;
            do
            {
                try
                {
                    if (!File.Exists("wagered.json"))
                    {
                        FileStream fs = File.Create("wagered.json");
                        fs.Close();
                    }

                    using (StreamWriter sw = new StreamWriter("wagered.json", true))
                    {
                        string tmpStr = ($"{prof:f8}").Replace(",", ".");
                        sw.Write(tmpStr + ",");
                    }

                    flagEx = false;
                }
                catch (Exception e)
                {
                }
            } while (flagEx);
        }
    

    public void ReCreateFile()
        {
            File.Delete("wagered.json");
            FileStream fs = File.Create("wagered.json");
            fs.Close();
        }

        public string GetWageredList()
        {
            bool flagEx = true;
            string wageredList ="" ;
            
            do
            {
                try
                {

                    using (StreamReader sr = new StreamReader("wagered.json"))
                    {
                        wageredList = sr.ReadToEnd();
                        flagEx = false;
                    }
                }
                catch (System.IO.IOException e)
                {

                }

            } while (flagEx);

            
            wageredList = wageredList.Remove(wageredList.Length - 1);
            //wageredList = wageredList.Replace(",", ".");

            return wageredList;
        }
    }
}