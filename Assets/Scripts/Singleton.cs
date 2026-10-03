namespace Singleton
{
    using System;

    public sealed class Singleton<T> where T : class, new()
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

        private Singleton() { }

        public static T Instance { get { return _instance.Value; } }
    }
}