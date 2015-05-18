using System;
using System.Collections.Generic;
using System.Threading;

namespace DromundKaas
{
    class SpaceShips
    {

        private static HashSet<EntityType> EntityTypes;
        private static List<Enemy> Enemies;
        private static bool[,] EnemyBullets;
        private static bool[,] PlayerBullets;
        private static int CycleCounter;

        private static void Init()
        {
            Console.SetBufferSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);

            EntityTypes = new HashSet<EntityType>();
            LoadEntityTypes(EntityTypes);

            Enemies = new List<Enemy>();

            EnemyBullets = new bool[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];
            PlayerBullets = new bool[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];

            CycleCounter = 0;
        }

        static void Main(string[] args)
        {
            // Init();

            //1. INTRO
            //using functions from other files (IntroOutro.cs)
            IntroOutro.Intro();
            IntroOutro.PrintSomething("lo6 lo6i lo6 lo6i lo6 lo6i");

            //Load Player into Entities

            var PLAYER = new Player();

            //2. MAIN LOOP
            bool end = false;
            while (!end)
            {
                //Increment CycleCounter
                CycleCounter++;

                //Progress Bullets
                RollDown(EnemyBullets);
                RollUp(PlayerBullets);

                //Match Bullets
                //SEE IF BULLETS COLLIDE


                //Progress Entities
                //Progress player, based on last keypress
                for (int i = 1; i < Enemies.Count; i++)
                {
                    if (Enemies[i].Step > Enemies[i].Type.Movement.Length)
                        Enemies[i].Step = 0;
                    MoveEntity(Enemies[i], Enemies[i].Type.Movement[Enemies[i].Step]);
                }
                end = true;
                // Thread.Sleep(1000);
            }


            //3. OUTRO
            // IntroOutro.Outro();

            Console.ReadKey();
        }

        private static void LoadEntityTypes(HashSet<EntityType> target)
        {

        }

        private static void RollUp(bool[,] target)
        {
            for (int j = 0; j < target.GetLength(1); j++)
            {
                target[0, j] = false;
            }
            for (int i = 1; i < target.GetLength(0); i++)
            {
                for (int j = 0; j < target.GetLength(1); j++)
                {
                    if (target[i, j])
                    {
                        target[i, j] = false;
                        target[i - 1, j] = true;
                    }
                }
            }
        }

        private static void RollDown(bool[,] target)
        {
            for (int j = 0; j < target.GetLength(1); j++)
            {
                int temp = target.GetLength(1) - 1;
                target[temp, j] = false;
            }
            for (int i = 0; i < target.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < target.GetLength(1); j++)
                {
                    if (target[i, j])
                    {
                        target[i, j] = false;
                        target[i + 1, j] = true;
                    }
                }
            }
        }

        private static void MoveEntity(Entity e, char direction)
        {
            int x = 0, y = 0;
            switch (direction)
            {
                case 'u':
                    y = -1;
                    break;
                case 'd':
                    y = 1;
                    break;
                case 'l':
                    x = -1;
                    break;
                case 'r':
                    x = 1;
                    break;
                default:
                    break;
            }
            e.Location.X += x;
            e.Location.Y += y;
        }
    }
}
