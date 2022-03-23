using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DependancyInjector : MonoBehaviour {

    [SerializeField]
    ImageSender sender;

    [SerializeField]
    ImageReciever reciever;

    [SerializeField]
    SpriteCollection spriteCollection;

    public static DependancyInjector instance;
    public ServiceCollection Services { get; set; }

    private void Start() {

        if (instance != null) throw new Exception("A dependancy injector already exists");
        instance = this;

        Services = MakeServiceProvider();

        var objects = FindObjectsOfType<MonoBehaviour>().OfType<IRequiresDependancy>();

        foreach (var obj in objects) {
            obj.SetUpDependancies(Services);
        }

    }

    private ServiceCollection MakeServiceProvider() {
        ServiceCollection services = new ServiceCollection();

        services.AddService<IImageReciever>(reciever);
        services.AddService<IImageSender>(sender);
        services.AddService<ISpriteCollection>(spriteCollection);

        return services;

    }
}
