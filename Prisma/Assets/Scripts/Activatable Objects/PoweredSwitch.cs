using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ActivatableObjectSource))]
public class PoweredSwitch : ActivatableObject
{
    private ActivatableObjectSource _source;

    // Use this for initialization
    void Start()
    {
        _source = GetComponent<ActivatableObjectSource>();
        SetInitialState();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate()
    {
        base.Activate();
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
        _source.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GetComponent<SpriteRenderer>().color = new Color(0.69f, 1.0f, 0.54f);
        _source.Deactivate();
    }

    public void Interact(GameObject obj)
    {
        Active = !Active;
        if (Active) Activate();
        else Deactivate();
    }
}

