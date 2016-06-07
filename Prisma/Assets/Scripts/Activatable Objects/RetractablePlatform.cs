using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActivatableObjectTarget))]
public class RetractablePlatform : ActivatableObject {

    private ActivatableObjectTarget _target;
    private LayerMask _originalLayer;
    private BoxCollider2D _collider;

	// Use this for initialization
	void Start () {
        _target = GetComponent<ActivatableObjectTarget>();
        _originalLayer = gameObject.layer;
        _collider = GetComponent<BoxCollider2D>();

        SetInitialState();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();
        _collider.enabled = true;
        gameObject.layer = _originalLayer;
        Color color = GetComponent<SpriteRenderer>().color;
        color = new Color(color.r, color.g, color.b, 1.0f);
        GetComponent<SpriteRenderer>().color = color;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Background");
        Color color = GetComponent<SpriteRenderer>().color;
        color = new Color(color.r, color.g, color.b, 0.5f);
        GetComponent<SpriteRenderer>().color = color;
    }
}
