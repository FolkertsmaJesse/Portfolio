using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : Interactable {

    public TextMesh claimText;
    public Caveman claimer;

	void Update () {
        IUpdate();
	}

    public void Claim(Caveman _claimer)
    {
        claimText.gameObject.SetActive(true);
        claimer = _claimer;
        claimText.text = claimer.gameObject.name + "'s Cave";
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.root.tag == "Caveman")
        {
            col.transform.root.GetComponent<Caveman>().sheltered = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.root.tag == "Caveman")
        {
            col.transform.root.GetComponent<Caveman>().sheltered = false;
        }
    }
}
