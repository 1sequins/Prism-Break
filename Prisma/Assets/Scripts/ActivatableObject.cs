using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObject : MonoBehaviour, IActivatable {

    public bool Active;
    public bool Unlocked;

    // Use this for initialization
    void Start () {
        
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

    public void UnlockSource()
    {
        //TODO: For if the object is both a target and source
        Unlocked = true;
    }

    public void LockSource()
    {
        //TODO: For if the object is both a target and source
        Unlocked = false;
    }
}
