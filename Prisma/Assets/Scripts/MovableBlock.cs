using UnityEngine;
using System.Collections;
using System;

public class MovableBlock : MonoBehaviour {

    private PhasingBlock _phasingScript;

	// Use this for initialization
	void Start () {
        _phasingScript = transform.parent.GetComponent<PhasingBlock>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void PhaseBlock()
    {
        _phasingScript.MoveToPosition();
    }

    public void Interact(GameObject obj)
    {
        PhaseBlock();
    }
}
