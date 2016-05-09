using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObjectSource : MonoBehaviour {

    public int AO_ID;

    public bool Active { get; set; }

    private List<ActivatableObjectTarget> _targets;

	// Use this for initialization
	void Start () {
        _targets = new List<ActivatableObjectTarget>();

        GameObject[] targetArr = GameObject.FindGameObjectsWithTag("AO");

        if (AO_ID <= 0) return;

        //Get all the targets that have the same ID
        foreach (GameObject target in targetArr)
        {
            Debug.Log(target.name);

            ActivatableObjectTarget aotarget = target.GetComponent<ActivatableObjectTarget>();
            if (aotarget != null)
            {
                if (aotarget.AO_ID == AO_ID) _targets.Add(aotarget);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate()
    {
        Active = true;
        Debug.Log("Source [" + gameObject.name + "] Activated");
        foreach (ActivatableObjectTarget target in _targets)
        {
            target.ActivateFromSource();
        }
    }

    public void Deactivate()
    {
        Active = false;
        foreach (ActivatableObjectTarget target in _targets)
        {
            target.Deactivate();
        }
    }

}
