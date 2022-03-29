using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collection of services made availiable through dependancy injection
/// </summary>
public class ServiceCollection {

    Dictionary<Type, object> internalCollection = new Dictionary<Type, object>();

    /// <summary>
    /// Adds a service to the available services
    /// </summary>
    /// <typeparam name="T">Type of the service</typeparam>
    /// <param name="service">Service instance</param>
    public void AddService<T>(T service) {
        internalCollection.Add(typeof(T), service); 
    }

    /// <summary>
    /// Retrieves a service from the collection based on its type
    /// </summary>
    /// <typeparam name="T">Type of the service to retrieve</typeparam>
    /// <returns>Instance of the service of type <c>T</c></returns>
    public T GetService<T>() {
        return (T)internalCollection[typeof(T)];
    }

}

