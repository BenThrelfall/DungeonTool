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

public class ImageUploaderTests {

    [UnityTest]
    public IEnumerator ImageUploaderAndRecieverTransfer() {

        ServiceCollection serviceCollection = new ServiceCollection();
        var fileTester = new TestFileIO(1024 * 1024 * 7);
        serviceCollection.AddService<IImageFileIO>(fileTester);

        GameObject network = new GameObject();
        var manager = network.AddComponent<WebTestNetworkManager>();

        yield return new WaitForEndOfFrame();

        GameObject upObject = new GameObject();
        var uploader = upObject.AddComponent<ImageUploader>();

        GameObject recieverObject = new GameObject();
        var reciever = recieverObject.AddComponent<ImageReciever>();

        uploader.SetUpDependancies(serviceCollection);
        reciever.SetUpDependancies(serviceCollection);

        yield return new WaitForEndOfFrame();

        manager.StartHost();

        yield return new WaitForSecondsRealtime(2f);

        uploader.OpenAndSendImageFromFile("test");

        yield return new WaitForSecondsRealtime(10f);

        Assert.IsTrue(fileTester.recieved);

    }

    class TestFileIO : IImageFileIO {

        int testSize;
        public bool recieved = false;

        public TestFileIO(int testSize) {
            this.testSize = testSize;
        }

        public byte[] ReadAllImageBytes(string filePath) {
            byte[] output = new byte[testSize];

            for (int i = 0; i < testSize; i++) {
                output[i] = (byte)(i % 128);
            }

            return output;

        }

        public void SaveGameImage(string hash, byte[] data) {

            if (data.Length != testSize) throw new System.Exception();

            for (int i = 0; i < testSize; i++) {
                if (data[i] != (byte)(i % 128)) throw new System.Exception();
            }

            recieved = true;

        }
    }

}
