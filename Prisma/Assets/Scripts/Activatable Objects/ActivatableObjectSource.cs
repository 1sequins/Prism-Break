using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObjectSource : MonoBehaviour {

    public int AO_ID;

    public bool Active { get; set; }

    private List<GameObject> _targets;

	// Use this for initialization
	void Start () {
        _targets = new List<GameObject>();

        GameObject[] targetArr = GameObject.FindGameObjectsWithTag("AO");

        if (AO_ID <= 0) return;

        //Get all the targets that have the same ID
        foreach (GameObject target in targetArr)
        {
            Debug.Log(target.name);
            if (target.GetComponent<ActivatableObjectTarget>() != null && target.GetComponent<ActivatableObjectTarget>().AO_ID == AO_ID) _targets.Add(target);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate()
    {
        Active = true;
        Debug.Log("Source Activated");
        foreach (GameObject target in _targets)
        {
            if (target && target.GetComponent<ActivatableObjectTarget>()) target.GetComponent<ActivatableObjectTarget>().ActivateFromSource();
        }
    }

    public void Deactivate()
    {
        Active = false;
        foreach (GameObject target in _targets)
        {
            if (target && target.GetComponent<ActivatableObjectTarget>()) target.GetComponent<ActivatableObjectTarget>().Deactivate();
        }
    }

}
