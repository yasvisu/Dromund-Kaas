using System;
using System.Collections.Generic;
using System.Threading;

namespace DromundKaas
{
    class SpaceShips
    {
        //MAP STATE
        private static HashSet<EntityType> EntityTypes;

        private static void Init()
        {
            Console.SetBufferSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);
            game = new GameState();
        }

        static void Main(string[] args)
        {
            Init();

            //1. INTRO
            //using functions from other files (IntroOutro.cs)
            IntroOutro.Intro();
            IntroOutro.PrintSomething("lo6 lo6i lo6 lo6i lo6 lo6i");

            //2. MAIN LOOP
            bool end = false;
            while (!end)
            {
                end = true;
                // Thread.Sleep(1000);
            }


            //3. OUTRO
            IntroOutro.Outro();

            Console.ReadKey();
        }
    }
}
