using System;
using System.Collections.Generic;
using System.Threading;

namespace DromundKaas
{
    class SpaceShips
    {

        private static HashSet<EntityType> EntityTypes;
        private static List<Entity> Entities;
        private static Queue<bool[]> EnemyBullets;
        private static Queue<bool[]> PlayerBullets;
        private static int CycleCounter;

        private static void Init()
        {
            Console.SetBufferSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);

            EntityTypes = new HashSet<EntityType>();
            LoadEntityTypes(EntityTypes);

            Entities = new List<Entity>();

            EnemyBullets = new Queue<bool[]>(GlobalVar.CONSOLE_HEIGHT);
            PlayerBullets = new Queue<bool[]>(GlobalVar.CONSOLE_HEIGHT);
            for (int i = 0; i < GlobalVar.CONSOLE_HEIGHT; i++)
            {
                EnemyBullets.Enqueue(new bool[GlobalVar.CONSOLE_WIDTH]);
                EnemyBullets.Enqueue(new bool[GlobalVar.CONSOLE_WIDTH]);
            }

            CycleCounter = 0;
        }

        static void Main(string[] args)
        {
            // Init();

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
    
        private static void LoadEntityTypes(HashSet<EntityType> target)
        {

        }
    }
}
