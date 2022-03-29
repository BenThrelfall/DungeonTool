using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Indicates a class relies on dependancies from the 
/// dependacy injection system
/// </summary>
public interface IRequiresDependancy {

    /// <summary>
    /// Set up object dependancies from the provided <c>serviceCollection</c>
    /// </summary>
    /// <param name="serviceCollection">Collection of services that can used</param>
    void SetUpDependancies(ServiceCollection serviceCollection);

}
