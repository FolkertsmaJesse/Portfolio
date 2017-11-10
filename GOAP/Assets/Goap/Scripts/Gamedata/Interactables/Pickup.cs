using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable {
    public enum PickupType
    {
        Stick,
        Rock,
        Wood,
    }

    public PickupType item;

    private void Update()
    {
        IUpdate();
    }

    public void PickupItem(Caveman caveman)
    {
        switch (item)
        {
            case PickupType.Wood:
                caveman.backpack.wood++;
                break;
            case PickupType.Stick:
                caveman.backpack.sticks++;
                break;
            case PickupType.Rock:
                caveman.backpack.rocks++;
                break;
        }
        Destroy(gameObject);
    }
}
