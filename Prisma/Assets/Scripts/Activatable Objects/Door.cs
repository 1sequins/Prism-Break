using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ActivatableObjectTarget))]
public class Door : ActivatableObject {

    private BoxCollider2D _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

	// Use this for initialization
	void Start () {
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
