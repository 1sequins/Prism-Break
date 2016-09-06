using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SourceType
{
    Serial,
    Compound,
    Toggle
}

public class ActivatableObjectSource : MonoBehaviour {

    public int AO_ID;

    public SourceType sourceType;

    public bool Active { get; set; }

    private List<ActivatableObjectTarget> _targets;

    void Awake()
    {
        _targets = new List<ActivatableObjectTarget>();

        GameObject[] targetArr = GameObject.FindGameObjectsWithTag("AO");
        GameObject[] absorberArr = GameObject.FindGameObjectsWithTag("Absorber");

        if (AO_ID <= 0) return;

        //Get all the targets that have the same ID
        foreach (GameObject target in targetArr)
        {
            ActivatableObjectTarget aotarget = target.GetComponent<ActivatableObjectTarget>();
            if (aotarget != null)
            {
                if (aotarget.AO_ID == AO_ID) _targets.Add(aotarget);
            }
        }

        //Get all the targets that have the same ID
        foreach (GameObject target in absorberArr)
        {
            ActivatableObjectTarget aotarget = target.GetComponent<ActivatableObjectTarget>();
            if (aotarget != null)
            {
                if (aotarget.AO_ID == AO_ID) _targets.Add(aotarget);
            }
        }
    }

	// Use this for initialization
	void Start () {

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
            target.ActivateFromSource(this);
        }
    }

    public void Deactivate()
    {
        Active = false;
        foreach (ActivatableObjectTarget target in _targets)
        {
            target.DeactivateFromSource(this);
        }
    }

}
