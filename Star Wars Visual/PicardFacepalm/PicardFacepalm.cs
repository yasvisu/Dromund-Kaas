using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicardFacepalm
{
    class Picard_Facepalm
    {
        static void Main()
        {
            PicardFacepalm();
        }
        static void PicardFacepalm()
        {
            string text = System.IO.File.ReadAllText(@"..\..\PicardPacepalm.txt");
            Console.WriteLine(text);
        }
    }
}
