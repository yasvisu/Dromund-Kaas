using System;
using System.Collections.Generic;
using System.Threading;

namespace DromundKaas
{
    class SpaceShips
    {   
        private static Dictionary<string, EntityType> EntityTypes;
        private static List<Enemy> Enemies;
        private static bool[,] EnemyBullets;
        private static bool[,] PlayerBullets;
        private static int CycleCounter;

        private static char[,] ConsoleMap;

        /// <summary>
        /// Struct to hold a pair of console colors.
        /// </summary>
        private struct ConsoleColorPair
        {
            ConsoleColor Foreground;
            ConsoleColor Background;

            public ConsoleColorPair(ConsoleColor Foreground, ConsoleColor Background)
            {
                this.Foreground = Foreground;
                this.Background = Background;
            }
        }

        private static void Init()
        {
            Console.SetWindowSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);
            Console.SetBufferSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);

            EntityTypes = new Dictionary<string, EntityType>();
            EntityType.ExtractData(EntityTypes);

            Enemies = new List<Enemy>();

            EnemyBullets = new bool[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];
            PlayerBullets = new bool[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];
            ConsoleMap = new char[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];

            CycleCounter = 0;
        }

        static void Main(string[] args) //True Main
        {
            StarWarsIntro.Intro();
            Init();
            
            //1. INTRO
            //using functions from other files (IntroOutro.cs)
            //IntroOutro.Intro();
            
            //Load Player into Entities

            //var PLAYER = new Player(10, new Point(0,0), )

            var ENEMY = new Enemy(10, new Point(5, 5), EntityTypes["playerJabba"], ConsoleColor.Cyan);
            LayerEntity(ENEMY);
            var ENEMY2 = new Enemy(10, new Point(12, 12), EntityTypes["playerSpaceship1"], ConsoleColor.Red);
            LayerEntity(ENEMY2);
            PrintConsoleMap();

            //2. MAIN LOOP
            bool end = false;
            while (!end)
            {
                //1 - Increment CycleCounter
                CycleCounter++;

                //2 - Progress Bullets
                Utils.RollDown(EnemyBullets);
                Utils.RollUp(PlayerBullets);

                //3 - Match Bullets
                //SEE IF BULLETS COLLIDE


                //4 - Progress Entities
                //Progress player, based on last keypress
                for (int i = 1; i < Enemies.Count; i++)
                {
                    if (Enemies[i].Step >= Enemies[i].Type.Movement.Length)
                        Enemies[i].Step = 0;
                    Utils.MoveEntity(Enemies[i], Enemies[i].Type.Movement[Enemies[i].Step]);
                    Enemies[i].Step++;
                }
                end = true;
                // Thread.Sleep(1000);
            }


            //3. OUTRO
            // IntroOutro.Outro();

            Console.WriteLine("Successful Termination.");
            Console.ReadKey();
        }



        private static void LayerEntity(Entity e)
        {
            //Set text color.
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = e.Color;

            Point temp = e.Location;
            Console.SetCursorPosition(temp.X, temp.Y);
            for (int i = 0; i < e.Type.Sprite.GetLength(0); i++)
            {
                for (int j = 0; j < e.Type.Sprite.GetLength(1); j++)
                {
                    int AbsoluteX = temp.X + j,
                        AbsoluteY = temp.Y + i;
                    if (AbsoluteX >= 0 && AbsoluteX < ConsoleMap.GetLength(0) && AbsoluteY >= 0 && AbsoluteY < ConsoleMap.GetLength(1))
                        ConsoleMap[AbsoluteY, AbsoluteX] = e.Type.Sprite[i, j];
                }
                temp.Y++;
                Console.SetCursorPosition(temp.X, temp.Y);
            }
            Console.ForegroundColor = previous;
        }

        private static void PrintConsoleMap()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < ConsoleMap.GetLength(0); i++)
            {
                for (int j = 0; j < ConsoleMap.GetLength(1) - 1; j++)
                    Console.Write(ConsoleMap[i, j] != '\0' ? ConsoleMap[i, j] : ' ');
                Console.WriteLine(ConsoleMap[i, ConsoleMap.GetLength(1) - 1]);
            }
        }
    }
}
