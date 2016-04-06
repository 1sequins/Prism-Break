using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SpectraDictionary : MonoBehaviour {

    [Serializable]
    public struct SpectraLink
    {
        public ColorEnum Color;
        public GameObject Spectra;
    }
    [SerializeField]
    public List<SpectraLink> SpectraMap;

    void Start()
    {

    }

    void Update()
    {

    }

    public GameObject GetSpectra(ColorEnum color)
    {
        foreach(SpectraLink link in SpectraMap)
        {
            if (link.Color == color) return link.Spectra;
        }

        return null;
    }
}
