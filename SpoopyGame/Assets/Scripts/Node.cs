using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public List<GameObject> connections = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addConnection(GameObject newConnection)
    {
        connections.Add(newConnection);
    }

    public void removeConnection(GameObject targetConnection)
    {
        if (connections.Contains(targetConnection))
        {
            connections.Remove(targetConnection);
        }
    }

    public GameObject[] getConnections()
    {
        return connections.ToArray();
    }

    public int getNumConnections()
    {
        return connections.Count;
    }

    public bool isConnected(GameObject target)
    {
        return connections.Contains(target);
    }

}
