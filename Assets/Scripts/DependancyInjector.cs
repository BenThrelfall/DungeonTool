using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DependancyInjector : MonoBehaviour {

    [SerializeField]
    ImageSender sender;

    [SerializeField]
    ImageReciever reciever;

    private void Start() {
        var serviceProvider = MakeServiceProvider();

        var objects = FindObjectsOfType<MonoBehaviour>().OfType<IRequiresDependancy>();

        foreach (var obj in objects) {
            obj.SetUpDependancies(serviceProvider);
        }

    }

    private ServiceCollection MakeServiceProvider() {
        ServiceCollection services = new ServiceCollection();

        services.AddService<IImageReciever>(reciever);
        services.AddService<IImageSender>(sender);

        return services;

    }
}
