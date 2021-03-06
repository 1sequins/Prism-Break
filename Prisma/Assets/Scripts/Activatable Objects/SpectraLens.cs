﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpectraLens : MonoBehaviour, IInteractable {

    #region Public Variables

    public List<ColorEnum> SpectraColors;

    #endregion

    #region Private Variables

    

    #endregion
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact(GameObject obj)
    {
        if(obj.tag == "Player")
        {
            Debug.Log("Entering Lens");
            GameObject.Find("Main_GO").GetComponentInChildren<LensController>().ActivateLens(gameObject, obj);
            obj.GetComponent<PlayerController>().Deactivate();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            
        }
    }
}
