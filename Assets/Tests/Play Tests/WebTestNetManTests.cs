using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.TestTools;
using UnityEditor;
using System.IO;

public class WebTestNetManTests {

    GameObject netObj;
    NetworkManager network;

    [OneTimeSetUp]
    public void SetUp() {
        netObj = new GameObject();
        network = netObj.AddComponent<WebTestNetworkManager>();
    }

    [Test]
    public void CanBeCreated() {
        Assert.IsNotNull(network);
    }

    [UnityTest]
    public IEnumerator CanStartNetworkAsHost() {
        network.StartHost();
        yield return new WaitForSecondsRealtime(1f);
        network.StopHost();

    }

    [UnityTest]
    public IEnumerator CanStartNetworkAsServer() {
        network.StartServer();
        yield return new WaitForSecondsRealtime(1f);
        network.StopServer();

    }

    [OneTimeTearDown]
    public void TearDown() {
        GameObject.Destroy(netObj);
    }



}
