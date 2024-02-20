using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRtblavataritemdescToExcel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string file = "E:\\TRKR\\tr4\\script\\clientiteminfo\\tblavataritemdesc.txt";
            //DataProceed.TxtSlice(file);
            for (int i = 0; i < args.Length; i++)
            {

                DataProceed.TxtSlice(args[i]);
            }
        }
    }
}
