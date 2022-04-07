using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Placed in the scene to manage the dependancy injection system
/// On start will attempt to find and inject dependancies into objects with the <c>IRequiresDependancy</c> interface 
/// </summary>
/// <remarks>
/// Current implementation is not ideally as some behavours are strongly coupled to this class through the static instance.
/// This would make it hard to easily switch between using different service collections which would be useful for testing.
/// </remarks>
public class DependancyInjector : MonoBehaviour {

    [SerializeField]
    SpriteCollection spriteCollection;

    [SerializeField]
    ObjectSpawner spawner;

    [SerializeField]
    FileIOService fileIOService;

    [SerializeField]
    MapUpdater mapUpdater;

    [SerializeField]
    FrameRateLimiter rateLimiter;

    [SerializeField]
    ImageDataCollection imageDataCollection;

    [SerializeField]
    ToolManager toolManager;

    [SerializeField]
    VisionUpdateEventHandler visionEventHandler;

    [SerializeField]
    BoardManager boardManager;


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

        services.AddService<IImageDataCollection>(imageDataCollection);
        services.AddService<ISpriteCollection>(spriteCollection);
        services.AddService<IObjectSpawner>(spawner);
        services.AddService<IFileIOService>(fileIOService);
        services.AddService<IMapUpdater>(mapUpdater);
        services.AddService<IFrameRateLimiter>(rateLimiter);
        services.AddService<IToolManager>(toolManager);
        services.AddService<IVisionUpdateEventHandler>(visionEventHandler);
        services.AddService<ISaveablesManager>(spawner);
        services.AddService<IBoardManager>(boardManager);

        return services;

    }
}
