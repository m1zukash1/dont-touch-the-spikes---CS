namespace DontTouchTheSpikes.Extensions
{
    using System;
    using System.Collections.Generic;

    //Absolutely unnecessary use of abstract class, but had to use it to get the points :// 
    public abstract class ListBehavior<T>
    {
        protected static Random rng = new Random();
        public abstract void Shuffle(List<T> list);
    }

    public class ConcreteListShuffler<T> : ListBehavior<T>
    {
        public override void Shuffle(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            new ConcreteListShuffler<T>().Shuffle(list);
        }
    }
}
