using System;

namespace DromundKaas
{
    //Entity
    public abstract class Entity : IComparable
    {
        public string Name;
        public int Power;
        public char[,] Image;

        public Entity() { }

        public Entity(string N, int P, char[,] I)
        {
            this.Name = N;
            this.Power = P;
            this.Image = I;
        }

        public int CompareTo(Object a)
        {
            if (a == null)
                return 1;

            Entity other = a as Entity;
            if (other != null)
                return this.Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Object is not a Temperature");
        }
    }

    public class Enemy : Entity
    {

    }

    public class Player : Entity
    {

    }

    public class EntityType { }
}