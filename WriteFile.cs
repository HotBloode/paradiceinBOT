using System;
using System.IO;
using Newtonsoft.Json;

namespace paradiceinBOT
{
    public class WriteFile
    {
        public WriteFile()
        {
            ChekFile("wagered.json");
            ChekFile("dataForSpam.json");
        }

        private void ChekFile(string nameFile)
        {
            FileStream fs;
            if (!File.Exists(nameFile))
            {
                fs = File.Create(nameFile);
            }
            else
            {
                File.Delete(nameFile);
                fs = File.Create(nameFile);
            }

            fs.Close();
        }

        public void ReCreateFile()
        {
            File.Delete("wagered.json");
            FileStream fs = File.Create("wagered.json");
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

        public void AddDataForSpamStatiscToFile(DataForSpamStatistic data)
        {
            bool flagEx = true;
            do
            {
                try
                {
                    File.WriteAllText("dataForSpam.json", JsonConvert.SerializeObject(data, Formatting.Indented));

                    flagEx = false;
                }
                catch (Exception e)
                {

                }
            } while (flagEx);
        }

    }
}