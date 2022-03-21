using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceCollection {

    Dictionary<Type, object> internalCollection = new Dictionary<Type, object>();

    public void AddService<T>(T service) {
        internalCollection.Add(typeof(T), service); 
    }

    public T GetService<T>() {
        return (T)internalCollection[typeof(T)];
    }

}

