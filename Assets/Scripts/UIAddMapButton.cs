using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIAddMapButton : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    MapsCollection mapCollection;

    IFileIOService fileIOService;

    public void OnClick() {

        fileIOService.ReadAllBytes((x) => mapCollection.AddMap(x));

    }


    public void SetUpDependancies(ServiceCollection serviceCollection) {
        fileIOService = serviceCollection.GetService<IFileIOService>();
    }

}
