using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObjectTarget : MonoBehaviour {

    public int AO_ID;

    public bool Active { get; set; }

    private bool _sourceAndTarget;

    private List<ActivatableObjectSource> _sources;

    private ActivatableObject _activatableObject;

	// Use this for initialization
	void Start () {

        _sourceAndTarget = GetComponent<ActivatableObjectSource>() != null;

        _sources = new List<ActivatableObjectSource>();

        _activatableObject = GetComponent<ActivatableObject>();

        GameObject[] sourceArr = GameObject.FindGameObjectsWithTag("AO");

        if (AO_ID <= 0) return;

        //Get all the targets that have the same ID
        foreach (GameObject source in sourceArr)
        {
            ActivatableObjectSource aosource = source.GetComponent<ActivatableObjectSource>();
            if (aosource != null)
            {
                if (aosource.AO_ID == AO_ID) _sources.Add(aosource);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateFromSource()
    {
        Debug.Log("Activating [" + gameObject.name + "] from source");
        bool activate = (_sources.Count > 0);
        foreach (ActivatableObjectSource source in _sources)
        {
            if (!source.GetComponent<ActivatableObjectSource>().Active)
            {
                Debug.Log("Source inactive");
                activate = false;
                break;
            }
        }

        if (activate) Activate();
        else Deactivate();
    }

    public void Activate()
    {
        Debug.Log("Target Activated");
        Active = true;
        if (_sourceAndTarget)
        {
            _activatableObject.UnlockSource();
        }
        else
        {
            _activatableObject.Activate();
        }
    }

    public void Deactivate()
    {
        Active = false;
        if(_sourceAndTarget)
        {
            _activatableObject.UnlockSource();
        }
        else
        {
            _activatableObject.Deactivate();
        }
    }
}
