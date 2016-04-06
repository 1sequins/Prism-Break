using UnityEngine;
using System.Collections;
using System;

public class Switch : MonoBehaviour, IActivateable {
    public bool Active { get; set; }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate()
    {
        Active = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Deactivate()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<Bullet>())
        {
            Debug.Log("Switch Hit");
            Activate();
        }
    }
}
