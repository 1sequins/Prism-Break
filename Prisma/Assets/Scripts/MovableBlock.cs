using UnityEngine;
using System.Collections;

public class MovableBlock : MonoBehaviour {

    public int movingMass = 5;
    public int immobileMass = 100;

    private bool _movable;

    private Rigidbody2D _rigidbody;

	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody2D>();

        Release();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Grab()
    {
        _movable = true;
        _rigidbody.mass = movingMass;
    }

    public void Release()
    {
        _movable = false;
        _rigidbody.mass = immobileMass;
    }
}
