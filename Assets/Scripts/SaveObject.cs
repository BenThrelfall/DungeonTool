using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveObject : MonoBehaviour {

    [SerializeField]
    IObjectSpawner.SpawnType spawnType;

    public ObjectSaveData Save() {

        List<CompSaveData> componentData = gameObject.GetInterfacesInChildren<ISaveComp>().Select(x => x.Save()).ToList();

        ObjectSaveData output = new ObjectSaveData() {
            spawnType = spawnType,
            position = transform.position,
            rotation = transform.rotation.eulerAngles,
            scale = transform.localScale,
            componentData = componentData
        };

        return output;
    }

    public void Load(ObjectSaveData data) {

        List<ISaveComp> comps = gameObject.GetInterfacesInChildren<ISaveComp>().ToList();

        foreach (var item in data.componentData) {
            var comp = comps.First(x => x.ComponentType == item.compType);
            comp.Load(item);
            comps.Remove(comp);
        }
    }


}
