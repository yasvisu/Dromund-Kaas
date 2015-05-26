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

        private static Player PLAYER;
        private static List<Enemy> Enemies;
        private static List<Bullet> Bullets;

        private static Entity[,] EntityMap;

        private static int CycleCounter;
        private static uint IDCounter;

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

            PLAYER = new Player(IDCounter++, 10, new Point(GlobalVar.CONSOLE_WIDTH / 2, GlobalVar.CONSOLE_HEIGHT / 2), EntityTypes["playerMouse"], ConsoleColor.Green);
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();

            EntityMap = new Entity[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];

            CycleCounter = 0;
            IDCounter = 0;

            //ASYNC
            HandlerSet = new HashSet<Task>();

            //START HANDLERS
            HandlerSet.Add(Task.Run(() =>
                ConsoleKeypressDaemon()
                ));
        }

        static void Main(string[] args) //True Main
        {


            //1. INTRO
            IntroOutro.Intro();
            Console.Clear();
            Init();

            var ENEMY = new Enemy(IDCounter++, 10, new Point(5, 0), EntityTypes["enemyJabba"], ConsoleColor.Yellow);
            var ENEMY2 = new Enemy(IDCounter++, 10, new Point(50, 0), EntityTypes["enemySpaceship1"], ConsoleColor.Red);
            Enemies.Add(ENEMY);
            Enemies.Add(ENEMY2);

            //2. MAIN LOOP
            #region Main Loop
            while (!END)
            {

                //1 - Increment CycleCounter
                CycleCounter++;

                //2 - Progress Bullets


                //3 - Match Bullets
                //SEE IF BULLETS COLLIDE
                for (int i = 0; i < Bullets.Count; i++)
                {

                }

                //4 - Progress Entities

                for (int i = 0; i < Enemies.Count; i++)
                {
                    EnemyAction(Enemies[i]);
                }


                for (int i = 0; i < Bullets.Count; i++)
                {
                    BulletAction(Bullets[i]);
                }

                Console.Clear();
                Utils.PrintEntity(PLAYER);
                foreach (var E in Enemies)
                {
                    Utils.PrintEntity(E);
                }
                lock (Bullets)
                {
                    foreach (var B in Bullets)
                    {
                        Utils.PrintEntity(B);
                    }
                }
                Thread.Sleep(100);
            }
            #endregion

            //3. OUTRO
            // IntroOutro.Outro();

            Finit();
        }
        #region Non-Main

        /// <summary>
        /// Clear all resources.
        /// </summary>
        private static void Finit()
        {
            // do something
            Console.WriteLine("Successful termination.");
            Console.ReadKey(true);
        }


        /// <summary>
        /// Move given entity in given direction. Modifies the Entity Location parameter.
        /// </summary>
        /// <param name="ToMove"> Entity to move.</param>
        /// <param name="Direction">Direction in which to move the entity. Accepted directions: 'u' (up), 'd' (down), 'l' (left), 'r' (right).</param>
        public static void MoveEntity(Entity ToMove, char Direction)
        {
            int x = 0, y = 0;
            switch (Direction)
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

            var temp = new Point(ToMove.Location.X + x, ToMove.Location.Y + y);
            if (Utils.IsValidPoint(temp) && !Collides(temp))
            {
                //LayerEntity(ToMove, temp);
                ToMove.Location = temp;
            }
        }

        private static bool Collides(Point Current)
        {
            return false;
        }

        private static void LayerEntity(Entity Current, Point Destination)
        {
            Point temp = Current.Location;
            for (int i = temp.Y; i < temp.Y + Current.Type.Sprite.GetLength(0); i++)
            {
                for (int j = temp.X; j < temp.X + Current.Type.Sprite.GetLength(1); j++)
                {
                    if (Utils.IsValidPoint(new Point(i, j)))
                        EntityMap[i, j] = null;
                }
            }
            temp = Destination;

            for (int i = temp.Y; i < temp.Y + Current.Type.Sprite.GetLength(0); i++)
            {
                for (int j = temp.X; j < temp.X + Current.Type.Sprite.GetLength(1); j++)
                {
                    if (Utils.IsValidPoint(new Point(i, j)))
                        EntityMap[i, j] = Current;
                }
            }

        }

        /// <summary>
        /// Async ConsoleKey keypress listener.
        /// </summary>
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
                        MoveEntity(PLAYER, 'u');
                        break;
                    case ConsoleKey.A:
                        MoveEntity(PLAYER, 'l');
                        break;
                    case ConsoleKey.S:
                        MoveEntity(PLAYER, 'd');
                        break;
                    case ConsoleKey.D:
                        MoveEntity(PLAYER, 'r');
                        break;
                    case ConsoleKey.Spacebar:
                        lock (Bullets)
                        {
                            Task.Run(() => PlayerShoot());
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static void PlayerShoot()
        {
            Shoot(PLAYER, true);
        }

        public static void EnemyShoot(Enemy Shooter)
        {
            Shoot(Shooter, false);
        }

        private static void Shoot(Entity Shooter, bool Friendly)
        {
            foreach (var P in Shooter.Type.Blasters)
            {
                Point temp = new Point(P.X + Shooter.Location.X, P.Y + Shooter.Location.Y);
                if (temp.X > 0 && temp.X < GlobalVar.CONSOLE_WIDTH && temp.Y > 0 && temp.Y < GlobalVar.CONSOLE_HEIGHT)
                {
                    EntityType bulletTemp = EntityTypes["bulletRegular"];
                    Bullets.Add(new Bullet(IDCounter++,
                        bulletTemp.MaxLife,
                        temp,
                        bulletTemp,
                        (Friendly ? GlobalVar.PLAYER_BULLET_COLOR : GlobalVar.ENEMY_BULLET_COLOR),
                        Friendly));
                }
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
                    MoveEntity(Current, Current.Type.Movement[Current.Step]);
                    if (Current.Location.Y == GlobalVar.CONSOLE_HEIGHT - 1)
                        Enemies.Remove(Current);
                    break;
            }
            Current.Step++;
        }

        public static void BulletAction(Bullet Current)
        {
            if (Current.Step >= Current.Type.Movement.Length)
                Current.Step = 0;
            switch (Current.Type.Movement[Current.Step])
            {
                case '@':
                    EnemyShoot(Current);
                    break;
                default:
                    char temp = Current.Type.Movement[Current.Step];
                    if (Current.Friendly)
                    {
                        if (temp == 'd')
                        {
                            temp = 'u';
                        }
                        else if (temp == 'u')
                        {
                            temp = 'd';
                        }
                    }
                    MoveEntity(Current, temp);
                    if (Current.Location.Y == 0 || Current.Location.Y == GlobalVar.CONSOLE_HEIGHT - 1)
                        Bullets.Remove(Current);
                    break;
            }
            Current.Step++;
        }

        #endregion
    }
}
