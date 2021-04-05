using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paradiceinBOT.Data_Classes
{
    class ReadFileWIthBets
    {
        public string GetWageredList()
        {
            bool flagEx = true;
            string wageredList = "";

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

            return wageredList;
        }
    }
}
