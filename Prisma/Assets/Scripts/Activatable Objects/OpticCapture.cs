﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (ActivatableObjectSource))]
public class OpticCapture : ActivatableObject {

    public List<ColorEnum> opticColors = new List<ColorEnum>();

    private ActivatableObjectSource[] _sources;
    private Spectra _activeSpectra;

    private Color activeTint;

	// Use this for initialization
	void Start () {
        _sources = GetComponents<ActivatableObjectSource>();
        _activeSpectra = null;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Active && _activeSpectra == null)
        {
            Deactivate();
        }
	}

    public override void Activate()
    {
        base.Activate();
        switch (_activeSpectra.SpectraColor)
        {
            case ColorEnum.RED: activeTint = new Color(1.0f, 0.0f, 0.0f, 0.5f); break;
            case ColorEnum.ORANGE: activeTint = new Color(1.0f, 0.5f, 0.0f, 0.5f); break;
            case ColorEnum.YELLOW: activeTint = new Color(1.0f, 1.0f, 0.0f, 0.5f); break;
            case ColorEnum.GREEN: activeTint = new Color(0.0f, 1.0f, 0.0f, 0.5f); break;
            case ColorEnum.BLUE: activeTint = new Color(0.0f, 0.0f, 1.0f, 0.5f); break;
            case ColorEnum.PURPLE: activeTint = new Color(1.0f, 0.0f, 1.0f, 0.5f); break;
            default: activeTint = new Color(1.0f, 1.0f, 1.0f, 0.5f); break;
        }
        GetComponent<SpriteRenderer>().color = activeTint;
        foreach(ActivatableObjectSource source in _sources)
        {
            source.Activate();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        _activeSpectra = null;
        foreach (ActivatableObjectSource source in _sources)
        {
            source.Deactivate();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Spectra>())
        {
            if(_activeSpectra == null)
            {
                _activeSpectra = collider.GetComponent<Spectra>();
                if (opticColors.Contains(_activeSpectra.SpectraColor))
                {
                    Activate();
                }
                else
                {
                    _activeSpectra = null;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.GetComponent<Spectra>())
        {
            if(_activeSpectra != null && collider.gameObject.GetComponent<Spectra>() == _activeSpectra)
            {
                Deactivate();
            }
        }
    }
}
