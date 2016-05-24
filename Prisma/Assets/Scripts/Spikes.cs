using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ow");
        PlayerController test = collision.gameObject.GetComponent<PlayerController>();
        

        //target.velocity = new Vector2(-300, 200);

    }
}
