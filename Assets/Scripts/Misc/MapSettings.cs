using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Component.Spawning;
using FishNet.Managing.Server;

public class MapSettings : MonoBehaviour
{
    public int timeLimit;
    public bool isFFA;

    public Transform spawnsParent;

    // Start is called before the first frame update
    void Start()
    {
        var ps = InstanceFinder.ServerManager.GetComponent<PlayerSpawner>();
        ps.Spawns = spawnsParent.GetComponentsInChildren<Transform>();
    }

}
