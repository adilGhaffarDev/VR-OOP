using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerContainer
{
    protected Dictionary<Type, object> _managers = new Dictionary<Type, object>();

    public virtual void Bind<T>(object implementation)
    {
        Type key = typeof(T);
        if (_managers.ContainsKey(key))
        {
            _managers[key] = implementation;
        }
        else
        {
            _managers.Add(key, implementation);
        }
    }

    public virtual T Resolve<T>()
    {
        return (T)_managers[typeof(T)];
    }

    public virtual void UnBind<T>()
    {
        _managers.Remove(typeof(T));
    }
}
