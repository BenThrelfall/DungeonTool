using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Behavour that goes on the UI element that prompts the user to add a token 
/// from the file system. 
/// </summary>
public class UIAddTokenButton : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    TokenCollection tokenCollection;

    IFileIOService fileIOService;

    /// <summary>
    /// Prompt a user to choose a file from the file system then send the
    /// bytes of that file to the token collection to be added as a new token
    /// </summary>
    public void OnClick() {

        fileIOService.ReadAllBytes((x) => tokenCollection.AddToken(x));

    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        fileIOService = serviceCollection.GetService<IFileIOService>();
    }
}
