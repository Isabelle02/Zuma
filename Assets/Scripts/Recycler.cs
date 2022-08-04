using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    protected static RecyclerConfig _recyclerConfig;
    
    private void Awake()
    {
        _recyclerConfig = Resources.Load<RecyclerConfig>("RecyclerConfig");
    }
}

public class Recycler<T> : Recycler where T : MonoBehaviour
{
    private static readonly Stack<T> _poolObjects = new Stack<T>();

    public static T GetObj()
    {
        var obj = default(T);
        if (_poolObjects.Count == 0)
            obj = CreateObject();
        else
            obj = _poolObjects.Pop();
        
        obj.gameObject.SetActive(true);
        return obj;
    }

    public static void ReleaseObj(T obj)
    {
        obj.gameObject.SetActive(false);
        _poolObjects.Push(obj);
    }
        
    private static T CreateObject()
    {
        foreach (var p in _recyclerConfig.Prefabs)
        {
            if (!p.TryGetComponent<T>(out var obj)) 
                continue;
            
            var newObject = Instantiate(obj);
            return newObject;
        }

        return default;
    }
}