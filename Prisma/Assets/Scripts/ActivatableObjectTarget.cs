using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObjectTarget : MonoBehaviour {

    public int AO_ID;

    public bool Active { get; set; }

    private bool _sourceAndTarget;

    private List<GameObject> _sources;

    private ActivatableObject _activatableObject;

	// Use this for initialization
	void Start () {

        _sourceAndTarget = GetComponent<ActivatableObjectSource>() != null;

        _sources = new List<GameObject>();

        _activatableObject = GetComponent<ActivatableObject>();

        GameObject[] sourceArr = GameObject.FindGameObjectsWithTag("AO");

        if (AO_ID <= 0) return;

        //Get all the targets that have the same ID
        foreach (GameObject source in sourceArr)
        {
            if (source.GetComponent<ActivatableObjectSource>() != null && source.GetComponent<ActivatableObjectSource>().AO_ID == AO_ID) _sources.Add(source);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateFromSource()
    {
        Debug.Log("Activating from source");
        bool activate = (_sources.Count > 0);
        foreach (GameObject source in _sources)
        {
            if (source && source.GetComponent<ActivatableObjectSource>())
            {
                if (!source.GetComponent<ActivatableObjectSource>().Active)
                {
                    Debug.Log("Source inactive");
                    activate = false;
                    break;
                }
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
