using UnityEngine;
using System.Collections;

public class UnstableSpectra : MonoBehaviour {

    public LayerMask unstableMask;

    // Use this for initialization
    void Start()
    {
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
