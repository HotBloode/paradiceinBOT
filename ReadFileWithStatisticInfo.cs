using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace paradiceinBOT.Data_Classes
{
    class ReadFileWithStatisticInfo
    {
        public DataForSpamStatistic GetStatisticInfo()
        {
            bool flagEx = true;
            DataForSpamStatistic data = new DataForSpamStatistic();

            do
            {
                try
                {
                        data = JsonConvert.DeserializeObject<DataForSpamStatistic>(File.ReadAllText("dataForSpam.json"));
                        flagEx = false;
                }
                catch (System.IO.IOException e)
                {

                }

            } while (flagEx);

            return data;
        }
    }
}
