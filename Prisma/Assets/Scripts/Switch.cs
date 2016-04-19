using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof (ActivatableObjectSource))]
public class Switch : ActivatableObject {

    private ActivatableObjectSource _source;

    // Use this for initialization
    void Start () {
        _source = GetComponent<ActivatableObjectSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        _source.Activate();
    }

    public override void Deactivate()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        _source.Deactivate();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<Bullet>())
        {
            Debug.Log("Switch Hit");
            Activate();
        }
    }
}
