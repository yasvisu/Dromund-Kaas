using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DromundKaas
{
    class SpaceShips
    {
        #region STATIC VARIABLES
        //COLLECTIONS
        private static Dictionary<string, EntityType> EntityTypes;

        //ENTITIES
        private static Player PLAYER;
        private static List<Enemy> Enemies;
        private static List<Bullet> Bullets;

        private static Entity[,] EntityMap;

        //COUNTERS
        private static int CycleCounter;
        private static uint IDCounter;

        //ASYNC
        private static HashSet<Task> HandlerSet;
        private static bool END;

        //VOICEOVER
        private static VoiceOver R2DTA;
        #endregion

        private static void Init()
        {
            //CONSOLE
            Console.CursorVisible = false;
            Console.SetWindowSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);
            Console.SetBufferSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);

            //COLLECTIONS
            EntityTypes = new Dictionary<string, EntityType>();
            EntityType.ExtractData(EntityTypes);

            //ENTITIES
            SelectCommanderShip();
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();

            EntityMap = new Entity[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];

            //COUNTERS
            CycleCounter = 0;
            IDCounter = 0;

            //ASYNC
            HandlerSet = new HashSet<Task>();

            //VOICEOVER
            R2DTA = new VoiceOver("R-2-D-TA");

            //START HANDLERS
            HandlerSet.Add(Task.Run(() =>
                    ConsoleKeypressDaemon()
                ));
        }

        static void Main(string[] args) //True Main
        {
            //1. INTRO
            //IntroOutro.Intro();
            Console.Clear();

            Init();

            R2DTA.UtterAsync(string.Format("Greetings, Commander. I am your adjutant. Welcome to the SS {0} {1}. Rescue the Galaxy!", PLAYER.Color.ToString(), PLAYER.Type.Name.Substring(6)));

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

                //2 - Collide Bullets
                CollideBullets();

                //3 - Progress Entities
                ProgressEntities();

                //4 - Print State on Console
                PrintGameState();

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
                    Utils.MoveEntity(Current, Current.Type.Movement[Current.Step]);
                    if (Current.Location.Y == GlobalVar.CONSOLE_HEIGHT - 1)
                    {
                        Task.Run(() =>
                        {
                            lock (Enemies)
                            {
                                Enemies.Remove(Current);
                            }
                        });
                    }
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
                    Utils.MoveEntity(Current, temp);
                    if (Current.Location.Y == 0 || Current.Location.Y == GlobalVar.CONSOLE_HEIGHT - 1)
                        Bullets.Remove(Current);
                    break;
            }
            Current.Step++;
        }

        private static void SelectCommanderShip()
        {
            int xaxis = 1, yaxis = 2;
            int ship = 1;
            List<EntityType> Entities = new List<EntityType>();
            try
            {
                Console.WriteLine("Select your ship, Commander!");
                foreach (var ET in EntityTypes)
                {
                    if (ET.Key.Contains("player"))
                    {
                        Entities.Add(ET.Value);
                        if (xaxis + ET.Value.Sprite.GetLength(1) >= GlobalVar.CONSOLE_WIDTH)
                        {
                            xaxis = 1;
                            yaxis += 10;
                        }
                        Console.SetCursorPosition(xaxis, yaxis - 1);
                        Console.Write(ship);
                        var temp = new Player(IDCounter++, 10, new Point(xaxis, yaxis), ET.Value, Utils.SwitchCommanderColor(ship));
                        ship++;
                        Utils.PrintEntity(temp);
                        xaxis += temp.Type.Sprite.GetLength(1) + 2;
                    }
                }
                var KeyInfo = Console.ReadKey(false);
                int choice = int.Parse(KeyInfo.KeyChar.ToString());
                PLAYER = new Player(IDCounter++, Entities[choice - 1].MaxLife, new Point(GlobalVar.CONSOLE_WIDTH/2, GlobalVar.CONSOLE_HEIGHT-5), Entities[choice - 1], Utils.SwitchCommanderColor(choice));
            }
            catch (Exception e)
            {
                PLAYER = new Player(IDCounter++, Entities[0].MaxLife, new Point(0, 0), Entities[0], Utils.SwitchCommanderColor(1));
            }
        }

        /// <summary>
        /// Print the current game state.
        /// </summary>
        private static void PrintGameState()
        {
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
        }

        private static void ProgressEntities()
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                EnemyAction(Enemies[i]);
            }
            lock (Bullets)
            {
                for (int i = 0; i < Bullets.Count; i++)
                {
                    BulletAction(Bullets[i]);
                }
            }
        }

        private static void CollideBullets()
        {
            Stack<Bullet> BulletCollisionStack = new Stack<Bullet>();
            Stack<Enemy> EnemyCorpseStack = new Stack<Enemy>();
            lock (Bullets)
            {
                lock (Enemies)
                {
                    for (int i = 0; i < Bullets.Count; i++)
                    {
                        var tempBullet = Bullets[i];
                        if (Bullets[i].Friendly)
                        {
                            for (int j = 0; j < Enemies.Count; j++)
                            {
                                var temp = Enemies[j];
                                if (tempBullet.Location.IsWithin(temp.Location, temp.GetBottomRightCorner()) &&
                                        temp.Type.Sprite[Bullets[i].Location.Y - temp.Location.Y, Bullets[i].Location.X - temp.Location.X] != ' ')
                                {
                                    temp.ModifyLife(-tempBullet.Life);
                                    if (temp.Life <= 0)
                                    {
                                        EnemyCorpseStack.Push(Enemies[j]);
                                    }

                                    BulletCollisionStack.Push(Bullets[i]);
                                }
                            }
                        }
                    }
                    while (BulletCollisionStack.Count > 0)
                    {
                        Bullet temp = BulletCollisionStack.Pop();
                        Bullets.Remove(temp);
                    }
                    while (EnemyCorpseStack.Count > 0)
                    {
                        Enemies.Remove(EnemyCorpseStack.Pop());
                    }
                }
            }
        }

        #endregion
    }
}
