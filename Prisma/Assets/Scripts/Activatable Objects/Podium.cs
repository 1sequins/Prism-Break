using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ActivatableObjectSource))]
public class Podium : ActivatableObject, IInteractable {

    public GameObject FTRPuzzle;

    private ActivatableObjectSource _source;

	// Use this for initialization
	void Start () {
        _source = GetComponent<ActivatableObjectSource>();
        LockSource();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public override void Activate()
    {
        base.Activate();
        _source.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _source.Deactivate();
    }

    public override void UnlockSource()
    {
        base.UnlockSource();
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    public override void LockSource()
    {
        base.LockSource();
        GetComponent<SpriteRenderer>().color = Color.grey;
    }

    public void Interact(GameObject obj)
    {
        if(Unlocked)
        {
            FTRPuzzle.GetComponent<FTRController>().Activate(obj);
        }
    }
}
