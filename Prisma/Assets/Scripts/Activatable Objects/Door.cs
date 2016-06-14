﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ActivatableObjectTarget))]
public class Door : ActivatableObject {

    private BoxCollider2D _collider;

	// Use this for initialization
	void Start () {
        _collider = GetComponent<BoxCollider2D>();

        SetInitialState();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();
        _collider.enabled = false;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _collider.enabled = true;
    }
}
