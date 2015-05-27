using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DromundKaas
{
    /// <summary>
    /// Level class containing everything a Level object needs.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// The verbose name of the level.
        /// </summary>
        public string Name;

        /// <summary>
        /// The number of enemies to be spawned.
        /// </summary>
        public int SpawnCount;

        /// <summary>
        /// The spawn pattern of the level. See Levels.dk for examples.
        /// </summary>
        public string[] Pattern;

        /// <summary>
        /// Keyring of Entities for the level to pick from. Either Enemies or Bosses, set by the Main function.
        /// </summary>
        public List<EntityType> EnemyKeyring;

        private int CurrentWave;
        private int EnemyCounter;
        private Dictionary<char, EntityType> TokenTypes;

        /// <summary>
        /// Default Level constructor. Warning! Does not include EnemyKeyring initialization.
        /// </summary>
        /// <param name="Name">The name of the level.</param>
        /// <param name="SpawnCount">The number of Spawns in the level.</param>
        /// <param name="Pattern">The spawn pattern of the level.</param>
        public Level(string Name, int SpawnCount, string[] Pattern)
        {
            this.SpawnCount = SpawnCount;
            this.Name = Name;
            this.Pattern = Pattern;
            this.CurrentWave = 0;
            this.EnemyCounter = 0;
            this.TokenTypes = new Dictionary<char, EntityType>();
        }

        /// <summary>
        /// Get the next wave of Enemies in the Level, as a List.
        /// </summary>
        /// <returns>A List of Enemies representing the next wave.</returns>
        public List<Enemy> GetNextWave()
        {
            if (EnemyKeyring == null)
                throw new Exception("Null Enemy Keyring while trying to GetNextWave!");

            if (CurrentWave >= Pattern.Length)
                CurrentWave = 0;
            if (EnemyCounter == SpawnCount)
                return null;

            List<Enemy> result = new List<Enemy>();
            for (int i = 0; i < Pattern[CurrentWave].Length; i++)
            {
                char c = Pattern[CurrentWave][i];
                switch (c)
                {
                    case ' ':
                        break;
                    case '\n':
                        break;
                    case '\r':
                        break;
                    default:
                        EntityType temp;
                        if (!TokenTypes.ContainsKey(c))
                        {
                            temp = EnemyKeyring[i];
                            TokenTypes[c] = temp;
                        }
                        temp = TokenTypes[c];
                        GlobalVar.IDCounter++;
                        result.Add(new Enemy(temp.MaxLife, new Point(0, 0), temp, Utils.SwitchColor((int)GlobalVar.IDCounter % 10)));
                        EnemyCounter++;
                        break;
                }
                if (EnemyCounter == SpawnCount)
                    break;
            }
            CurrentWave++;
            return result;
        }

        /// <summary>
        /// Extracts all data from the Levels.dk file into an Array of Levels.
        /// </summary>
        public static Level[] ExtractLevels()
        {
            string path = GlobalVar.FILE_LEVELS_DK;
            string text = File.ReadAllText(path);
            string[] levelsInText = text.Split(new string[] { ">>>>>" }, StringSplitOptions.RemoveEmptyEntries);

            Level[] result = new Level[levelsInText.Length];

            for (int i = 0; i < result.Length; i++)
            {
                text = levelsInText[i];
                int SpawnCount;
                string Name;
                string[] Pattern;

                //Name
                string pattern = @"name *= *""(?<name>.*?)""";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(levelsInText[i]);
                Name = match.Groups["name"].Value;

                //SpawnCount
                pattern = @"spawncount *= *""(?<spawncount>.*?)""";
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                match = regex.Match(levelsInText[i]);
                SpawnCount = int.Parse(match.Groups["spawncount"].Value);

                //Pattern
                string[] tokens = text.Split(';');
                string wholePattern = tokens[tokens.Length - 1].Trim(new char[] { '\n', '\r' });
                Pattern = wholePattern.Split('\n');

                result[i] = new Level(Name, SpawnCount, Pattern);
            }

            return result;
        }

    }
}