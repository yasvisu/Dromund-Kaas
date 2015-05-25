using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DromundKaas
{
    class SpaceShips
    {
        //COLLECTIONS

        private static Dictionary<string, EntityType> EntityTypes;
        private static List<Enemy> Enemies;
        private static bool[,] EnemyBullets;
        private static bool[,] PlayerBullets;
        private static int CycleCounter;

        private static char[,] ConsoleMap;

        private static Player PLAYER;

        //ASYNC
        private static HashSet<Task> HandlerSet;
        private static bool END;

        private static void Init()
        {
            Console.CursorVisible = false;

            Console.SetWindowSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);
            Console.SetBufferSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);

            EntityTypes = new Dictionary<string, EntityType>();
            EntityType.ExtractData(EntityTypes);

            Enemies = new List<Enemy>();

            EnemyBullets = new bool[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];
            PlayerBullets = new bool[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];
            ConsoleMap = new char[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];

            CycleCounter = 0;


            PLAYER = new Player(10, new Point(5, 5), EntityTypes["playerLuke"], ConsoleColor.Cyan);


            //ASYNC
            HandlerSet = new HashSet<Task>();

            //START HANDLERS
            HandlerSet.Add(Task.Run(() =>
                ConsoleKeypressDaemon()
                ));
        }

        static void Main(string[] args) //True Main
        {
            StarWarsIntro.Intro();
            Init();
            
            //1. INTRO
            //using functions from other files (IntroOutro.cs)
            //IntroOutro.Intro();
            
            //Load Player into Entities


            var ENEMY = new Enemy(10, new Point(5, 5), EntityTypes["enemyJabba"], ConsoleColor.Yellow);
            var ENEMY2 = new Enemy(10, new Point(50, 5), EntityTypes["enemySpaceship1"], ConsoleColor.Red);
            Enemies.Add(ENEMY);
            Enemies.Add(ENEMY2);

            //2. MAIN LOOP
            #region Main Loop
            while (!END)
            {

                Console.Clear();
                //1 - Increment CycleCounter
                CycleCounter++;

                //2 - Progress Bullets
                Utils.RollDown(EnemyBullets);
                Utils.RollUp(PlayerBullets);

                Utils.DrawBullets(PlayerBullets);
                Utils.DrawBullets(EnemyBullets);

                //3 - Match Bullets
                //SEE IF BULLETS COLLIDE


                //4 - Progress Entities
                //Progress player, based on last keypress
                for (int i = 0; i < Enemies.Count; i++)
                {
                    EnemyAction(Enemies[i]);
                }

                PrintEntity(PLAYER);
                foreach (var E in Enemies)
                    PrintEntity(E);
                Thread.Sleep(100);
            }
            #endregion

            //3. OUTRO
            // IntroOutro.Outro();

            Finit();
            Console.WriteLine("Successful Termination.");
        }

        /// <summary>
        /// Clear all resources.
        /// </summary>
        private static void Finit()
        {
            // do something
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

        private static void PrintEntity(Entity e)
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
                    if (AbsoluteX >= 0 && AbsoluteX < ConsoleMap.GetLength(1) && AbsoluteY >= 0 && AbsoluteY < ConsoleMap.GetLength(0))
                        Console.Write(e.Type.Sprite[i, j]);
                }
                temp.Y++;
                if (temp.X > 0 && temp.X < GlobalVar.CONSOLE_WIDTH && temp.Y > 0 && temp.Y < GlobalVar.CONSOLE_HEIGHT)
                    Console.SetCursorPosition(temp.X, temp.Y);
            }
            Console.SetCursorPosition(0, 0);
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


        /// <summary>
        /// Async ConsoleKey keypress listener.
        /// </summary>
        /// <param name="Target">Target to save the current keypress in.</param>
        public static void ConsoleKeypressDaemon()
        {
            ConsoleKeyInfo keyInfo;
            ConsoleKey Target;
            while (!END)
            {
                keyInfo = Console.ReadKey(true);
                Target = keyInfo.Key;
                switch (Target)
                {
                    case ConsoleKey.Escape:
                        END = true;
                        break;
                    case ConsoleKey.W:
                        Utils.MoveEntity(PLAYER, 'u');
                        break;
                    case ConsoleKey.A:
                        Utils.MoveEntity(PLAYER, 'l');
                        break;
                    case ConsoleKey.S:
                        Utils.MoveEntity(PLAYER, 'd');
                        break;
                    case ConsoleKey.D:
                        Utils.MoveEntity(PLAYER, 'r');
                        break;
                    case ConsoleKey.Spacebar:
                        Task.Run(() => PlayerShoot());
                        break;
                    default:
                        break;
                }
            }
        }

        public static void PlayerShoot()
        {
            Shoot(PLAYER, PlayerBullets);
        }

        public static void EnemyShoot(Enemy Shooter)
        {
            Shoot(Shooter, EnemyBullets);
        }

        private static void Shoot(Entity Shooter, bool[,] BulletMap)
        {
            foreach (var P in Shooter.Type.Blasters)
            {
                Point temp = new Point(P.X + Shooter.Location.X, P.Y + Shooter.Location.Y);
                if (temp.X > 0 && temp.X < GlobalVar.CONSOLE_WIDTH && temp.Y > 0 && temp.Y < GlobalVar.CONSOLE_HEIGHT)
                    BulletMap[temp.Y, temp.X] = true;
            }
        }

        public static void EnemyAction(Enemy Current)
        {
            if (Current.Step >= Current.Type.Movement.Length)
                Current.Step = 0;
            switch (Current.Type.Movement[Current.Step])
            {
                case '@':
                    EnemyShoot(Current);
                    break;
                default:
                    Utils.MoveEntity(Current, Current.Type.Movement[Current.Step]);
                    if (Current.Location.Y == GlobalVar.CONSOLE_HEIGHT - 1)
                        Enemies.Remove(Current);
                    break;
            }
            Current.Step++;
        }

    }
}
