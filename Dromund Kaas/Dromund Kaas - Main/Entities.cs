﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DromundKaas
{
    #region Entities

    /// <summary>
    /// Abstract Entity class to be extended by both Players and Enemies.
    /// </summary>
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

        /// <summary>
        /// Default Entity builder for all in-class parameters.
        /// </summary>
        /// <param name="ID">ID of the Entity.</param>
        /// <param name="Life">Current life of the Entity.</param>
        /// <param name="Location">Current location of the Entity.</param>
        /// <param name="Type">Type of the Entity.</param>
        /// <param name="Color">Console color of the Entity.</param>
        public Entity(int Life, Point Location, EntityType Type, ConsoleColor Color)
        {
            this.Life = Life;
            this.Location = Location;
            this.Type = Type;
            this.Color = Color;
        }

        /// <summary>
        /// Modify life of current Entity.
        /// </summary>
        /// <param name="Count">Amount by which to modify the life.</param>
        public void ModifyLife(int Count)
        {
            this.Life += Count;
        }

        /// <summary>
        /// Get the bottom right corner of the Entity's Type Sprite.
        /// </summary>
        /// <returns>The Bottom Right Corner of the Entity's Type Sprite.</returns>
        public Point GetBottomRightCorner()
        {
            return new Point(this.Location.X + this.Type.Sprite.GetLength(1) - 1, this.Location.Y + this.Type.Sprite.GetLength(0) - 1);
        }
    }

    /// <summary>
    /// Enemy class to hold additional parameter of Step (int).
    /// </summary>
    public class Enemy : Entity
    {
        /// <summary>
        /// Current step in the Enemy's movement path.
        /// </summary>
        public int Step;


        /// <summary>
        /// Default Entity builder for all in-class parameters.
        /// </summary>
        /// <param name="ID">ID of the Entity.</param>
        /// <param name="Life">Current life of the Entity.</param>
        /// <param name="Location">Current location of the Entity.</param>
        /// <param name="Type">Type of the Entity.</param>
        /// <param name="Color">Console color of the Entity.</param>
        public Enemy(int Life, Point Location, EntityType Type, ConsoleColor Color)
            : base(Life, Location, Type, Color)
        {

        }
    }

    /// <summary>
    /// Player class to extend Entity.
    /// </summary>
    public class Player : Entity
    {
        /// <summary>
        /// Default Entity builder for all in-class parameters.
        /// </summary>
        /// <param name="ID">ID of the Entity.</param>
        /// <param name="Life">Current life of the Entity.</param>
        /// <param name="Location">Current location of the Entity.</param>
        /// <param name="Type">Type of the Entity.</param>
        /// <param name="Color">Console color of the Entity.</param>
        public Player(int Life, Point Location, EntityType Type, ConsoleColor Color)
            : base(Life, Location, Type, Color)
        {

        }
    }


    /// <summary>
    /// Bullet class to hold Bullet entity instances. Has an additional field for Friendliness.
    /// </summary>
    public class Bullet : Enemy
    {
        /// <summary>
        /// True if it is a bullet friendly to the player. False if it is an enemy bullet.
        /// </summary>
        public bool Friendly;

        /// <summary>
        /// Default Entity builder for all in-class parameters.
        /// </summary>
        /// <param name="ID">ID of the Entity.</param>
        /// <param name="Life">Current life of the Entity.</param>
        /// <param name="Location">Current location of the Entity.</param>
        /// <param name="Type">Type of the Entity.</param>
        /// <param name="Color">Console color of the Entity.</param>
        /// <param name="Friendly">Friendliness of the Bullet.</param>
        public Bullet(int Life, Point Location, EntityType Type, ConsoleColor Color, bool Friendly)
            : base(Life, Location, Type, Color)
        {
            this.Friendly = Friendly;
        }
    }

    #endregion

    /// <summary>
    /// Type of Entity. To be stored as unique values in the Main module.
    /// </summary>
    public class EntityType : IComparable
    {
        /// <summary>
        /// Default Entity name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Default Entity image, as a character matrix.
        /// </summary>
        public char[,] Sprite;

        /// <summary>
        /// Maximum life for Entity type by default.
        /// </summary>
        public int MaxLife;

        /// <summary>
        /// Default movement instructions of Entity type - udlr_@
        /// </summary>
        public string Movement;

        /// <summary>
        /// The locations of the blasters, relative to the main Location of the Entity.
        /// </summary>
        public Point[] Blasters;

        /// <summary>
        /// Default EntityType constructor for all in-class parameters.
        /// </summary>
        /// <param name="Name">The Name of the Entity Type.</param>
        /// <param name="Sprite">The character image of the Entity Type.</param>
        /// <param name="MaxLife">The default maximum life of the Entity Type.</param>
        /// <param name="Movement">The default movement pattern of the Entity Type.</param>
        /// <param name="Blasters">Array of blasters.</param> 
        public EntityType(string Name, char[,] Sprite, int MaxLife, string Movement, Point[] Blasters)
        {
            this.Name = Name;
            this.Sprite = Sprite;
            this.MaxLife = MaxLife;
            this.Movement = Movement;
            this.Blasters = Blasters;
        }

        /// <summary>
        /// Compares two EntityTypes by name.
        /// </summary>
        /// <param name="obj">The other EntityType.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            EntityType other = obj as EntityType;
            if (other != null)
                return this.Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Object is not an EntityType");
        }

        /// <summary>
        /// Extracts all data from the EntityTypes.dk file into a HashSet of EntityTypes.
        /// </summary>
        /// <param name="Target">The target to hold all the EntityTypes.</param>
        public static void ExtractData(Dictionary<string, EntityType> Target)
        {
            string path = GlobalVar.FILE_ENTITY_TYPES_DK;
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
                if (match.Groups["name"].ToString().Contains("player"))
                {
                    spriteLines = typeFeatures[2].Split('\n');
                }
                else
                {
                    spriteLines = typeFeatures[3].Split('\n');
                }
                S = new char[spriteLines.Length - 2, spriteLines[1].Length];
                for (int line = 1; line < spriteLines.Length - 1; line++)
                {
                    for (int elem = 0; elem < spriteLines[line].Length; elem++)
                    {
                        S[line - 1, elem] = spriteLines[line][elem];
                    }
                }

                //Life
                pattern = @"life=""(?<life>[\d]+)""";
                regex = new Regex(pattern);
                match = regex.Match(types[i]);
                M = int.Parse(match.Groups["life"].Value);

                //Movement
                pattern = @"movement=""(?<movement>[udlr@_]*)""";
                regex = new Regex(pattern);
                match = regex.Match(types[i]);
                Mov = match.Groups["movement"].Value;

                List<Point> Blasters = new List<Point>();

                for (int m = 0; m < S.GetLength(0); m++)
                {
                    for (int n = 0; n < S.GetLength(1); n++)
                    {
                        if (S[m, n] == '$' || S[m, n] == '@')
                        {
                            Blasters.Add(new Point(n, m));
                        }
                    }
                }


                EntityType temp = new EntityType(N, S, M, Mov, Blasters.ToArray());
                Target[temp.Name] = temp;
            }
        }
    }
}
