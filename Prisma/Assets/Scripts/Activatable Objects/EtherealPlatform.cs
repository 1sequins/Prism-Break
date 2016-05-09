using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActivatableObjectTarget))]
public class EtherealPlatform : ActivatableObject {

    private ActivatableObjectTarget _target;

    private BoxCollider2D _collider;

	// Use this for initialization
	void Start () {

        _target = GetComponent<ActivatableObjectTarget>();
        _collider = GetComponent<BoxCollider2D>();

        //Set objects to be default on or off at runtime
        if (Active) Activate();
        else Deactivate();
        Debug.Log("Setting active state");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();
        _collider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Etherial Platform");
        GetComponent<SpriteRenderer>().color = new Color(0.93f, 1.0f, 0.27f, 1.0f);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Foreground");
        GetComponent<SpriteRenderer>().color = new Color(0.93f, 1.0f, 0.27f, 0.5f);
    }
}
