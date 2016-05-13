using UnityEngine;
using System.Collections;

public class EtherialSpectra : MonoBehaviour {

    public LayerMask etherialMask;

    private PlayerController _controller;

	// Use this for initialization
	void Start () {
        _controller = GetComponent<PlayerController>();
        MakeEtherial();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void MakeEtherial()
    {
        gameObject.layer = LayerMask.NameToLayer("Etherial Spectra");
        _controller.groundLayer = etherialMask;
    }
}
