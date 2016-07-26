using UnityEngine;
using System.Collections;

public class BouncingLaser : MonoBehaviour {

    public enum LaserDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public LaserDirection laserDirection;

    private ActivatableObjectTarget _target;

	// Use this for initialization
	void Start () {
        _target = transform.parent.GetComponent<ActivatableObjectTarget>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateLaser()
    {

    }
}
