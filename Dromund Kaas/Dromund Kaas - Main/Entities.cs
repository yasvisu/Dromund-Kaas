using System;

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
        /// Type of Entity.
        /// </summary>
        public EntityType Type;

        private Entity() { }

        public Entity(int L, Point Loc, EntityType T)
        {
            this.Life = L;
            this.Location = Loc;
            this.Type = T;
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

    /// <summary>
    /// Type of Entity. To be stored as unique values in the Main module.
    /// </summary>
    public class EntityType : IComparable
    {
        public string   Name;
        public char[,]  Sprite;
        public int      MaxLife;
        public string   Movement;

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
    }
}