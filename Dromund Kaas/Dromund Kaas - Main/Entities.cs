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
            for (int i = 1; i < types.Length - 1; i++)
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
                // Console.WriteLine("Name: {0}", match.Groups[1]);
                N = match.Groups["name"].Value;

                //Sprite
                if (match.Groups[1].ToString() == "LukeSkywalker")
                {
                    Console.WriteLine("Shape: {0}", typeFeatures[2]);
                    //S = ...
                }
                else
                {
                    Console.WriteLine("Shape: {0}", typeFeatures[3]);
                    //S = ...
                }

                //Life
                pattern = @"life=""(?<life>[\d]+)""";
                regex = new Regex(pattern);
                match = regex.Match(types[i]);
                // Console.WriteLine("Life: {0}", match.Groups[1]);
                M = int.Parse(match.Groups["life"].Value);

                //Movement
                pattern = @"movement=""(?<movement>[\w]+)""";
                regex = new Regex(pattern);
                match = regex.Match(types[i]);
                // Console.WriteLine("Move: {0}\n", match.Groups[1]);
                Mov = match.Groups["movement"].Value;

                EntityType temp = new EntityType(N, S, M, Mov);
                Target.Add(temp);
            }
        }
    }
}