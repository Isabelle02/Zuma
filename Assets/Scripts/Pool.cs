using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    private const int StartCount = 30;

    private readonly Stack<T> _poolObjects = new Stack<T>();
    private static Pool<T> _instance;

    private void Awake()
    {
        _instance = this;
        for (var i = 0; i < StartCount; i++)
            PutObject(CreateObject());
    }

    protected static T TakeObject()
    {
        var obj = default(T);
        if (_instance._poolObjects.Count == 0)
            obj = _instance.CreateObject();
        else
            obj = _instance._poolObjects.Pop();
        
        obj.gameObject.SetActive(true);
        return obj;
    }

    protected static void PutObject(T obj)
    {
        obj.transform.SetParent(_instance.transform);
        obj.gameObject.SetActive(false);
        _instance._poolObjects.Push(obj);
    }
        
    private T CreateObject()
    {
        var newObject = Instantiate(_prefab, transform);
        return newObject;
    }
}