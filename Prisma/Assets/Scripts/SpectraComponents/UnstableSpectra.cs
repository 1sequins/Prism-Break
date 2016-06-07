using UnityEngine;
using System.Collections;

public class UnstableSpectra : MonoBehaviour {

    public LayerMask unstableMask;

    private PlayerController _controller;

    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        MakeUnstable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakeUnstable()
    {
        gameObject.layer = LayerMask.NameToLayer("Unstable Spectra");
    }
}
