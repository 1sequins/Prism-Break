using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof (ActivatableObjectSource))]
public class Switch : ActivatableObject, IInteractable {

    private ActivatableObjectSource[] _sources;

    // Use this for initialization
    void Start () {
        _sources = GetComponents<ActivatableObjectSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
        foreach(ActivatableObjectSource source in _sources)
        {
            source.Activate();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.5f);
        foreach(ActivatableObjectSource source in _sources)
        {
            source.Deactivate();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Bullet>())
        {
            Debug.Log("Switch Hit");
            Activate();
        }
    }

    public void Interact(GameObject obj)
    {
        Active = !Active;
        if (Active) Activate();
        else Deactivate();
    }
}
