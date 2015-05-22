using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DromundKaas
{
    /// <summary>
    /// Point structure to hold 2D coordinates.
    /// </summary>
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    #region Entities
    public abstract class Entity
    {
        /// <summary>
        /// Remaining life to the entity.
        /// </summary>
        public int Life;

        /// <summary>
        /// Location of the entity (top left corner of the sprite).
        /// </summary>
        public Point Location;

        /// <summary>
        /// Color of the entity.
        /// </summary>
        public ConsoleColor Color;

        /// <summary>
        /// Type of Entity.
        /// </summary>
        public EntityType Type;

        private Entity() { }

        public Entity(int L, Point Loc, EntityType T, ConsoleColor C)
        {
            this.Life = L;
            this.Location = Loc;
            this.Type = T;
            this.Color = C;
        }
    }

    public class Enemy : Entity
    {
        /// <summary>
        /// Current step in the Enemy's movement path.
        /// </summary>
        public int Step;
    }

    public class Player : Entity
    {

    }

    #endregion

    /// <summary>
    /// Type of Entity. To be stored as unique values in the Main module.
    /// </summary>
    public class EntityType : IComparable
    {
        public string Name;
        public char[,] Sprite;
        public int MaxLife;
        public string Movement;

        private EntityType() { }

        public EntityType(string N, char[,] S, int M, string Mov)
        {
            this.Name = N;
            this.Sprite = S;
            this.MaxLife = M;
            this.Movement = Mov;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            EntityType other = obj as EntityType;
            if (other != null)
                return this.Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Object is not an EntityType");
        }

        static void ExtractData(HashSet<EntityType> Target)
        {
            string path = @"../../EntityTypes.dk";
            string text = File.ReadAllText(path);
            string[] types = text.Split(new string[] { ">>>>>" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < types.Length; i++)
            {
                string[] typeFeatures = types[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                string N;
                char[,] S;
                int M;
                string Mov;

                //Name
                string pattern = @"name=""(?<name>[\w]+)""";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(types[i]);
                N = match.Groups["name"].Value;

                //Sprite
                string[] spriteLines;
                if (match.Groups["name"].ToString() == "playerLuke")
                {
                    spriteLines = typeFeatures[2].Split('\n');
                }
                else
                {
                    spriteLines = typeFeatures[3].Split('\n');
                }
                S = new char[spriteLines.Length - 1, spriteLines[1].Length];
                for (int line = 0; line < spriteLines.Length - 1; line++)
                {
                    for (int elem = 0; elem < spriteLines[line].Length; elem++)
                    {
                        S[line, elem] = spriteLines[line][elem];
                    }
                }

                //Life
                pattern = @"life=""(?<life>[\d]+)""";
                regex = new Regex(pattern);
                match = regex.Match(types[i]);
                M = int.Parse(match.Groups["life"].Value);

                //Movement
                pattern = @"movement=""(?<movement>[\w]+)""";
                regex = new Regex(pattern);
                match = regex.Match(types[i]);
                Mov = match.Groups["movement"].Value;

                EntityType temp = new EntityType(N, S, M, Mov);
                Target.Add(temp);
            }
        }
    }
}