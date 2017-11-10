using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopableTree : Interactable {

    public GameObject log;
    public Transform[] logSpawns;

    private void Update()
    {
        IUpdate();
    }

    public void ChopDown()
    {
        foreach(Transform t in logSpawns)
        {
            Instantiate(log, t.position, t.rotation);
        }
        Destroy(gameObject);
    }
}
