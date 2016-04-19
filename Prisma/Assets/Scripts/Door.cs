using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ActivatableObjectTarget))]
public class Door : ActivatableObject {

    private ActivatableObjectTarget _target;

    private BoxCollider2D _collider;

	// Use this for initialization
	void Start () {
        _collider = GetComponent<BoxCollider2D>();
        
        Debug.Log("Activated object Start");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        _collider.enabled = false;
    }

    public override void Deactivate()
    {
        _collider.enabled = true;
    }
}
