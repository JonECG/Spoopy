using UnityEngine;
using System.Collections;

public class PrefabSpawner : MonoBehaviour {

    public GameObject toSpawn;

    void Awake()
    {
        GameObject go = Instantiate(toSpawn) as GameObject;
        go.transform.parent = transform.parent;
        go.transform.position = transform.position;
        Destroy(gameObject);
    }
}
