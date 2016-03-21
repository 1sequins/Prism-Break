using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {

    private bool _grounded;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Grounded");
    }
}
