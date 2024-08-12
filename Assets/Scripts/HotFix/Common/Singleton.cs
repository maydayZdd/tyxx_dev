using System;
/// <summary>
/// 单例模板
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (typeof(T))
                {
                    if (_instance == null)
                    {
                        _instance = (T)Activator.CreateInstance(typeof(T), true);
                    }
                }
            }
            return _instance;
        }
    }

    protected Singleton() { }

    // 防止子类被实例化
    protected Singleton(bool dummy) { }
}
