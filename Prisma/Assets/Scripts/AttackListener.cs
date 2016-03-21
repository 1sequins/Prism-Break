using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackListener : MonoBehaviour {

    private Queue<string> inputQueue;

    private Attack currentAttack;

	// Use this for initialization
	void Start () {
        inputQueue = new Queue<string>();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentAttack != null)
        {

        }
	}
}
