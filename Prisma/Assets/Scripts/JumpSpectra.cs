using UnityEngine;
using System.Collections;

public class JumpSpectra : MonoBehaviour {

    public float jumpForce = 80.0f;

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Jump()
    {

    }
}
