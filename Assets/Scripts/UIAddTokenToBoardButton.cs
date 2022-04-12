using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavour that goes on each UI element that adds tokens to the board.
/// Each behavour instance has a specific sprite that will be used for the 
/// token that it spawns assigned.
/// </summary>
public class UIAddTokenToBoardButton : MonoBehaviour, IRequiresDependancy {

    /// <summary>
    /// Hash of the sprite that will be used for the token when it is spawned
    /// </summary>
    public string tokenHash;

    IObjectSpawner spawner;
    
    [SerializeField]
    bool dontAutoDependancies;

    private void Start() {
        if (dontAutoDependancies == false) SetUpDependancies(DependancyInjector.instance.Services);
    }

    /// <summary>
    /// Spawn a token in the centre of the board. Give it the sprite 
    /// assigned to this behavour instance.
    /// </summary>
    public void OnClick() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            spawner.SpawnObject(IObjectSpawner.SpawnType.playerToken, tokenHash);
        }
        else if (Input.GetKey(KeyCode.LeftControl)) {
            spawner.SpawnObject(IObjectSpawner.SpawnType.map, tokenHash);
        }
        else {
            spawner.SpawnObject(IObjectSpawner.SpawnType.token, tokenHash);
        }
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spawner = serviceCollection.GetService<IObjectSpawner>();
    }
}
