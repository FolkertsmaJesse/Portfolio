using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float waitTime;

    void Start()
    {
        Destroy(gameObject, waitTime);
    }
}
