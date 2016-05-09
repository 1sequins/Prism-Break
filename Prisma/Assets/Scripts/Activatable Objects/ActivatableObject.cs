using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObject : MonoBehaviour, IActivatable {

    public bool Active;
    public bool Unlocked;

    // Use this for initialization
    void Start () {
        //Set objects to be default on or off at runtime
        if (Active) Activate();
        else Deactivate();
        Debug.Log("Setting active state");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Activate()
    {
        Active = true;
    }

    public virtual void Deactivate()
    {
        Active = false;
    }

    public virtual void UnlockSource()
    {
        //TODO: For if the object is both a target and source
        Unlocked = true;
    }

    public virtual void LockSource()
    {
        //TODO: For if the object is both a target and source
        Unlocked = false;
    }
}
