using UnityEngine;
using System.Collections;

public class Spectra : MonoBehaviour
{

    #region Public Variables

    public ColorEnum SpectraColor;

    #endregion

    private bool _active = true;

    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }
    

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Activate()
    {
        _active = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = true;
    }

    public void Deactivate()
    {
        _active = false;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }
}
