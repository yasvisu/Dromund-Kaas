using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace DromundKaas
{
    public class Level
    {
        public string Name;
        public int SpawnCount;
        public string[] Pattern;
        private int CurrentWave;
        private int EnemyCounter;
        private Dictionary<char, EntityType> TokenTypes;
        public List<EntityType> EnemyKeyring;

        public Level(string Name, int SpawnCount, string[] Pattern)
        {
            this.SpawnCount = SpawnCount;
            this.Name = Name;
            this.Pattern = Pattern;
            this.CurrentWave = 0;
            this.EnemyCounter = 0;
            this.TokenTypes = new Dictionary<char, EntityType>();
        }

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