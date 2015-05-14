using System;

namespace DromundKaas
{
    class SpaceShips
    {
        static void Main(string[] args)
        {

            //1. INTRO
            //using functions from other files (IntroOutro.cs)
            IntroOutro.Intro();
            IntroOutro.PrintSomething("lo6 lo6i lo6 lo6i lo6 lo6i");


            //2. MAIN LOOP
            bool end = false;
            while(!end)
            {
                //...
                end = true;
            }


            //3. OUTRO
            IntroOutro.Outro();

            Console.ReadKey();
        }
    }
}
