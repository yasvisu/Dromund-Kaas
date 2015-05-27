using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

namespace DromundKaas
{
    class SpaceShips
    {
        #region STATIC VARIABLES
        //COLLECTIONS
        private static Dictionary<string, EntityType> EntityTypes;
        private static Level[] Levels;
        private static List<EntityType> EnemyTypes;
        private static List<EntityType> BossTypes;
        private static List<EntityType> PlayerTypes;
        private static List<EntityType> BulletTypes;


        //ENTITIES
        private static Player PLAYER;
        private static List<Enemy> Enemies;
        private static List<Bullet> Bullets;

        private static Entity[,] EntityMap;

        //COUNTERS
        private static ulong CycleCounter;
        private static long SCORE;
        private static uint EnemySpawnCount;
        private static uint EnemyKillCount;

        //ASYNC
        private static HashSet<Task> HandlerSet;

        //GAME STATE
        private static bool END;
        private static bool LEVEL_END;
        private static bool WINNER;

        //VOICEOVER
        private static VoiceOver R2DTA;
        #endregion

        private static void Init()
        {
            //COLLECTIONS
            EntityTypes = new Dictionary<string, EntityType>();
            EntityType.ExtractData(EntityTypes);
            Levels = Level.ExtractLevels();

            //KEYRINGS
            EnemyTypes = new List<EntityType>();
            BossTypes = new List<EntityType>();
            PlayerTypes = new List<EntityType>();
            BulletTypes = new List<EntityType>();
            foreach (var KVP in EntityTypes)
            {
                if (KVP.Key.Contains("enemy"))
                {
                    EnemyTypes.Add(KVP.Value);
                }
                else if (KVP.Key.Contains("boss"))
                {
                    BossTypes.Add(KVP.Value);
                }
                else if (KVP.Key.Contains("player"))
                {
                    PlayerTypes.Add(KVP.Value);
                }
                else if (KVP.Key.Contains("bullet"))
                {
                    PlayerTypes.Add(KVP.Value);
                }
            }

            //ENTITIES
            SelectCommanderShip();
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();

            EntityMap = new Entity[GlobalVar.CONSOLE_HEIGHT, GlobalVar.CONSOLE_WIDTH];

            //COUNTERS
            CycleCounter = 0;
            GlobalVar.IDCounter = 0;

            //ASYNC
            HandlerSet = new HashSet<Task>();

            //VOICEOVER
            R2DTA = new VoiceOver("R-2-D-TA");

            //START HANDLERS
            HandlerSet.Add(Task.Run(() =>
                    ConsoleKeypressDaemon()
                ));
        }

        private static void ConsoleInit()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetWindowSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);
            Console.SetBufferSize(GlobalVar.CONSOLE_WIDTH, GlobalVar.CONSOLE_HEIGHT);
        }

        static void Main() //True Main
        {
            //1. INTRO
            Task Initask = Task.Run(() => Init());
            //IntroOutro.Intro();
            ConsoleInit();
            Console.SetCursorPosition(0, 0);
            Console.Write("Loading assets...");
            Initask.Wait();
            Console.Clear();

            R2DTA.UtterAsync(string.Format("Greetings, Commander. I am your adjutant. Welcome to the SS {0} {1}. Rescue the Galaxy!", PLAYER.Color.ToString(), PLAYER.Type.Name.Substring(6)));

            #region LEVEL LOOP
            for (int l = 0; l < Levels.Length && !END; l++)
            {
                //RESET VARIABLES
                PLAYER.Life = PLAYER.Type.MaxLife;
                LEVEL_END = false;
                if (Levels[l].Name.Contains("Boss"))
                    Levels[l].EnemyKeyring = BossTypes;
                else
                    Levels[l].EnemyKeyring = EnemyTypes;
                List<Enemy> temp = Levels[l].GetNextWave();
                if (temp != null)
                {
                    int length = temp.Sum((x) => x.Type.Sprite.GetLength(1)) + temp.Count;
                    int XCounter = (GlobalVar.CONSOLE_WIDTH - length) / 2;
                    Console.WriteLine(Levels[l].Name + " " + Levels[l].SpawnCount);
                    Console.ReadKey();
                    foreach (var E in temp)
                    {
                        E.Location = new Point(XCounter, 2);
                        XCounter += E.Type.Sprite.GetLength(1) + 1;
                        Enemies.Add(E);
                    }
                }
                while (temp != null)
                {
                    #region 2. MAIN LOOP
                    while (!(END || LEVEL_END))
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
                        if (Enemies.Count == 0)
                            break;
                    }
                    #endregion
                    temp = Levels[l].GetNextWave();
                }


            }
            #endregion

            #region 3. GAME FINISH
            if (!END)
                WINNER = true;
            if (WINNER)
            {
                GameWin();
            }
            else
            {
                GameOver();
            }
            #endregion

            //4. OUTRO
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
            Console.SetCursorPosition(0, GlobalVar.CONSOLE_HEIGHT - 1);
            Console.Write("Press any key to quit...");
            Console.ReadKey();
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

        #region 2. MAIN LOOP FUNC

        /// <summary>
        /// Iterate through all Bullets, colide them with enemies, and delete entities upon collision.
        /// </summary>
        private static void CollideBullets()
        {
            Stack<Bullet> BulletCollisionStack = new Stack<Bullet>();
            Stack<Enemy> EnemyCorpseStack = new Stack<Enemy>();
            lock (Bullets)
            {

                for (int i = 0; i < Bullets.Count; i++)
                {
                    var tempBullet = Bullets[i];
                    if (Bullets[i].Friendly)
                    {
                        lock (Enemies)
                        {
                            for (int j = 0; j < Enemies.Count; j++)
                            {
                                var temp = Enemies[j];
                                if (tempBullet.Location.IsWithin(temp.Location, temp.GetBottomRightCorner()) &&
                                        temp.Type.Sprite[tempBullet.Location.Y - temp.Location.Y,
                                                        tempBullet.Location.X - temp.Location.X] != ' ')
                                {
                                    temp.ModifyLife(-tempBullet.Life);
                                    if (temp.Life <= 0)
                                    {
                                        EnemyKillCount++;
                                        AwardScoreEnemyKill(temp);
                                        EnemyCorpseStack.Push(temp);
                                    }

                                    BulletCollisionStack.Push(tempBullet);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (tempBullet.Location.IsWithin(PLAYER.Location, PLAYER.GetBottomRightCorner()) &&
                                        PLAYER.Type.Sprite[tempBullet.Location.Y - PLAYER.Location.Y,
                                                        tempBullet.Location.X - PLAYER.Location.X] != ' ')
                        {
                            PLAYER.ModifyLife(-tempBullet.Life);
                            if (PLAYER.Life <= 0)
                            {
                                END = true;
                                WINNER = false;
                            }

                            BulletCollisionStack.Push(tempBullet);
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

        /// <summary>
        /// Award Player SCORE based on the type of Enemy killed.
        /// </summary>
        /// <param name="Killed">The enemy killed.</param>
        private static void AwardScoreEnemyKill(Enemy Killed)
        {
            SCORE += 2 * Killed.Type.MaxLife;
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
            DisplayHUD();
        }

        /// <summary>
        /// Display the HUD, featuring the player Life and SCORE.
        /// </summary>
        private static void DisplayHUD()
        {
            var tempF = Console.ForegroundColor;
            var tempB = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, GlobalVar.CONSOLE_HEIGHT - 1);
            Console.Write(" HULL INTEGRITY: ");
            if (PLAYER.Life < PLAYER.Type.MaxLife / 2)
            {
                if (PLAYER.Life < PLAYER.Type.MaxLife / 4)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
                Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new String('@', Math.Max(PLAYER.Life, 0)).PadLeft(PLAYER.Type.MaxLife, '.'));
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\tSCORE: {0} ", SCORE);
            Console.ForegroundColor = tempF;
            Console.BackgroundColor = tempB;
        }

        /// <summary>
        /// Progress entities according to their current step and movement pattern.
        /// </summary>
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

        #endregion

        // Functions called at the Game end.
        #region 3. GAME FINISH FUNC
        private static void GameOver()
        {
            Console.Clear();
            PrintCentered("GAME OVER! YOU TRIED!", GlobalVar.CONSOLE_HEIGHT / 2, ConsoleColor.Red);
            R2DTA.Utter("All Systems Critical!");
            PrintCentered("Your Score:", GlobalVar.CONSOLE_HEIGHT / 2 + 1, ConsoleColor.White);
            PrintCentered(string.Format(" {0} ", SCORE.ToString()), GlobalVar.CONSOLE_HEIGHT / 2 + 3, ConsoleColor.Black, ConsoleColor.White);

            PLAYER.Location = new Point((GlobalVar.CONSOLE_WIDTH - PLAYER.Type.Sprite.GetLength(1)) / 2, GlobalVar.CONSOLE_HEIGHT / 2 + 5);
            Utils.PrintEntity(PLAYER);

        }

        private static void GameWin()
        {
            Console.Clear();
            SCORE += PLAYER.Life * 2;
            PrintCentered("YOU WIN!", GlobalVar.CONSOLE_HEIGHT / 2, ConsoleColor.Green);
            R2DTA.Utter("You Saved the Galaxy!");
            PrintCentered("Your Score:", GlobalVar.CONSOLE_HEIGHT / 2 + 1, ConsoleColor.White);
            PrintCentered(string.Format(" {0} ", SCORE.ToString()), GlobalVar.CONSOLE_HEIGHT / 2 + 3, ConsoleColor.Black, ConsoleColor.White);

            PLAYER.Location = new Point((GlobalVar.CONSOLE_WIDTH - PLAYER.Type.Sprite.GetLength(1)) / 2, GlobalVar.CONSOLE_HEIGHT / 2 + 5);
            Utils.PrintEntity(PLAYER);
        }

        private static void PrintCentered(string Text, int Height, ConsoleColor ForegroundColor = ConsoleColor.Gray, ConsoleColor BackgroundColor = ConsoleColor.Black)
        {
            var previousF = Console.ForegroundColor;
            var previousB = Console.BackgroundColor;

            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
            Console.SetCursorPosition((GlobalVar.CONSOLE_WIDTH - Text.Length) / 2, Height);
            Console.WriteLine(Text);

            Console.ForegroundColor = previousF;
            Console.BackgroundColor = previousB;
        }
        #endregion


        // Various functions called throughout the engine.
        #region MISC

        /// <summary>
        /// Generate Player bullets and reduce SCORE.
        /// </summary>
        public static void PlayerShoot()
        {
            SCORE--;
            Shoot(PLAYER, true);
        }

        /// <summary>
        /// Generate Enemy bullets.
        /// </summary>
        /// <param name="Shooter">The enemy to generate bullets for.</param>
        public static void EnemyShoot(Enemy Shooter)
        {
            Shoot(Shooter, false);
        }

        /// <summary>
        /// Generate player or enemy bullets for given Entity.
        /// </summary>
        /// <param name="Shooter">The shooter to generate bullets for.</param>
        /// <param name="Friendly">Whether the bullets are friendly to the player.</param>
        private static void Shoot(Entity Shooter, bool Friendly)
        {
            foreach (var P in Shooter.Type.Blasters)
            {
                Point temp = new Point(P.X + Shooter.Location.X, P.Y + Shooter.Location.Y);
                if (temp.X > 0 && temp.X < GlobalVar.CONSOLE_WIDTH && temp.Y > 0 && temp.Y < GlobalVar.CONSOLE_HEIGHT)
                {
                    EntityType bulletTemp = EntityTypes["bulletRegular"];
                    Bullets.Add(new Bullet(
                        bulletTemp.MaxLife,
                        temp,
                        bulletTemp,
                        (Friendly ? GlobalVar.PLAYER_BULLET_COLOR : GlobalVar.ENEMY_BULLET_COLOR),
                        Friendly));
                }
            }
        }

        /// <summary>
        /// Progress Enemy according to its Movement and Step.
        /// </summary>
        /// <param name="Current">The current enemy to progress.</param>
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

        /// <summary>
        /// Progress Bullet according to its current Movement and Step.
        /// </summary>
        /// <param name="Current">The current bullet to be progressed.</param>
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

        /// <summary>
        /// Draw option menu and let player pick ship.
        /// </summary>
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
                        var temp = new Player(10, new Point(xaxis, yaxis), ET.Value, Utils.SwitchColor(ship));
                        ship++;
                        Utils.PrintEntity(temp);
                        xaxis += temp.Type.Sprite.GetLength(1) + 2;
                    }
                }
                var KeyInfo = Console.ReadKey(false);
                int choice = int.Parse(KeyInfo.KeyChar.ToString());
                PLAYER = new Player(Entities[choice - 1].MaxLife, new Point(GlobalVar.CONSOLE_WIDTH / 2, GlobalVar.CONSOLE_HEIGHT - 5), Entities[choice - 1], Utils.SwitchColor(choice));
            }
            catch (Exception e)
            {
                PLAYER = new Player(Entities[0].MaxLife, new Point(GlobalVar.CONSOLE_WIDTH / 2, GlobalVar.CONSOLE_HEIGHT - 5), Entities[0], Utils.SwitchColor(1));
            }
        }

        #endregion

        #endregion

    }
}
