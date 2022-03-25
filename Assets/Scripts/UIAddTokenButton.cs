using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class UIAddTokenButton : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    TokenCollection tokenCollection;

    IFileIOService fileIOService;

    public void OnClick() {

        fileIOService.ReadAllBytes((x) => tokenCollection.AddToken(x));

    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        fileIOService = serviceCollection.GetService<IFileIOService>();
    }
}
