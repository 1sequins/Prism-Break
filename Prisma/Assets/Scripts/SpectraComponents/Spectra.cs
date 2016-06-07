using UnityEngine;
using System.Collections;

public class Spectra : MonoBehaviour
{

    #region Public Variables

    public ColorEnum SpectraColor;

    #endregion

    private Rigidbody2D m_rigidbody;

    private bool _active = true;

    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }
    

	// Use this for initialization
	void Start () {
        //Deactivate();
        m_rigidbody = GetComponent<Rigidbody2D>();
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
