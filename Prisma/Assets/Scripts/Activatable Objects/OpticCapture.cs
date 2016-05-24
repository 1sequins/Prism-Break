using UnityEngine;
using System.Collections;

[RequireComponent(typeof (ActivatableObjectSource))]
public class OpticCapture : ActivatableObject {

    public ColorEnum opticColor;

    private ActivatableObjectSource _source;
    private Spectra _activeSpectra;

    private Color activeTint;

	// Use this for initialization
	void Start () {
        _source = GetComponent<ActivatableObjectSource>();
        _activeSpectra = null;

        switch(opticColor)
        {
            case ColorEnum.RED: activeTint = new Color(1.0f, 0.0f, 0.0f, 0.5f); break;
            case ColorEnum.ORANGE: activeTint = new Color(1.0f, 0.5f, 0.0f, 0.5f); break;
            case ColorEnum.YELLOW: activeTint = new Color(1.0f, 1.0f, 0.0f, 0.5f); break;
            case ColorEnum.GREEN: activeTint = new Color(0.0f, 1.0f, 0.0f, 0.5f); break;
            case ColorEnum.BLUE: activeTint = new Color(0.0f, 0.0f, 1.0f, 0.5f); break;
            case ColorEnum.PURPLE: activeTint = new Color(1.0f, 0.0f, 1.0f, 0.5f); break;
            default: activeTint = new Color(1.0f, 1.0f, 1.0f, 0.5f); break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();
        GetComponent<SpriteRenderer>().color = activeTint;
        _source.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        _activeSpectra = null;
        _source.Deactivate();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Spectra>())
        {
            if(_activeSpectra == null)
            {
                _activeSpectra = collider.GetComponent<Spectra>();
                if (_activeSpectra.SpectraColor == opticColor)
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
