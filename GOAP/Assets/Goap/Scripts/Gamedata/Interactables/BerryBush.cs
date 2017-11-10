using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBush : Interactable {

    int foodAmount = 1;
    float endTime = 0;
    public float regrowTime = 30f;
    bool hasBerries = true;

    [SerializeField]
    GameObject berries;

    public bool CanHarvest
    {
        get
        {
            if (foodAmount > 0 && hasBerries)
                return true;
            else
                return false;
        }
    }

    void Update()
    {
        IUpdate();
        if (!hasBerries)
        {
            if(Time.time > endTime)
            {
                hasBerries = true;
                berries.SetActive(true);
            }
        }
    }

    public void Harvest()
    {
        endTime = Time.time + regrowTime;
        hasBerries = false;
        berries.SetActive(false);
    }
}
