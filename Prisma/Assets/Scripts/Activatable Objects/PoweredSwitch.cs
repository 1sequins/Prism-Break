using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ActivatableObjectSource))]
public class PoweredSwitch : ActivatableObject
{
    private ActivatableObjectSource[] _sources;

    // Use this for initialization
    void Start()
    {
        _sources = GetComponents<ActivatableObjectSource>();
        //_sources.AddRange(sourceList);
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
        foreach(ActivatableObjectSource source in _sources)
        {
            Debug.Log("Source [" + source.AO_ID + "] Activated");
            source.Activate();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GetComponent<SpriteRenderer>().color = new Color(0.69f, 1.0f, 0.54f);

        foreach (ActivatableObjectSource source in _sources)
        {
            source.Deactivate();
        }
    }

    public void Interact(GameObject obj)
    {
        Active = !Active;
        if (Active) Activate();
        else Deactivate();
    }
}

