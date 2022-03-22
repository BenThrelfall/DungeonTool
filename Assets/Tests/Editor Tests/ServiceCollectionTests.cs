using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.TestTools;
using UnityEditor;
using System.IO;
using System;

public class ServiceCollectionTests {

    [Test]
    public void AddAndRetrieveServices() {

        ServiceCollection services = new ServiceCollection();

        services.AddService<IImageFileIO>(new ImageFileIO());
        services.AddService(new List<int>());

        Assert.IsTrue(services.GetService<IImageFileIO>() is ImageFileIO);
        Assert.IsTrue(services.GetService<List<int>>() is List<int>);

    }

    [Test]
    public void ThrowsErrorOnBadGet() {

        ServiceCollection services = new ServiceCollection();

        Assert.Throws<KeyNotFoundException>(() => services.GetService<IImageFileIO>());
        Assert.Throws<KeyNotFoundException>(() => services.GetService<List<int>>());

    }

    [Test]
    public void DoesntConfuseTypes() {

        ServiceCollection services = new ServiceCollection();

        services.AddService<IImageFileIO>(new ImageFileIO());
        services.AddService(new List<int>());

        Assert.Throws<KeyNotFoundException>(() => services.GetService<ImageFileIO>());
        Assert.Throws<KeyNotFoundException>(() => services.GetService<IList<int>>());

    }

    interface IImageFileIO { }
    class ImageFileIO : IImageFileIO { }

}
