using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public List<Animal> animalsInScene;

    int eats = 0;
    public Text timesEaten;
    public Text toDo;

    public void Eat()
    {
        eats++;
        print(eats);
        timesEaten.text = "Times eaten: " + eats.ToString();
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public KeyValuePair<string,object>[] GetRealWorldData(Transform _agent)
    {
        List<KeyValuePair<string, object>> List = new List<KeyValuePair<string, object>>();

        //Check for fires
        bool fireExists = false;
        CampFire[] fires = FindObjectsOfType<CampFire>();
        foreach (CampFire cf in fires)
        {
            if (cf.isFound)
            {
                fireExists = true;
                break;
            }
        }
        List.Add(new KeyValuePair<string, object> ("FireExists", fireExists));

        //Check for carcasses
        bool carcassExists = false;
        Carcass[] carcasses = FindObjectsOfType<Carcass>();
        foreach (Carcass c in carcasses)
        {
            if (c.isFound)
            {
                carcassExists = true;
                break;
            }
        }
        List.Add(new KeyValuePair<string, object>("CarcassExists", carcassExists));

        //Check for pickups
        bool woodExists = false;
        bool stickExists = false;
        bool rockExists = false;
        Pickup[] pickups = FindObjectsOfType<Pickup>();
        for (int i = 0; i < pickups.Length; i++)
        {
            print(pickups[i].name + pickups[i].isFound);
            if (pickups[i].isFound)
            {
                switch (pickups[i].item)
                {
                    case Pickup.PickupType.Rock:
                        rockExists = true;
                        continue;
                    case Pickup.PickupType.Stick:
                        stickExists = true;
                        continue;
                    case Pickup.PickupType.Wood:
                        woodExists = true;
                        continue;
                }
                break;
            }
        }
        List.Add(new KeyValuePair<string, object>("WoodExists", woodExists));
        List.Add(new KeyValuePair<string, object>("StickExists", stickExists));
        List.Add(new KeyValuePair<string, object>("RockExists", rockExists));

        print("StickExist " + stickExists);
        print("WoodExists " + woodExists);
        print("RockExists " + rockExists);

        return List.ToArray();
    }
}
