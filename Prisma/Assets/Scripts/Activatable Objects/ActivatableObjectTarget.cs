using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObjectTarget : MonoBehaviour {

    public int AO_ID;

    public bool inverse;

    public bool Active { get; set; }

    private bool _sourceAndTarget;

    private List<ActivatableObjectSource> _sources;

    private ActivatableObject _activatableObject;

    public delegate void ActivateDel();
    public ActivateDel Activate;
    public ActivateDel Deactivate;

    void Awake()
    {
        _sourceAndTarget = GetComponent<ActivatableObjectSource>() != null;

        _sources = new List<ActivatableObjectSource>();

        _activatableObject = GetComponent<ActivatableObject>();

        GameObject[] sourceArr = GameObject.FindGameObjectsWithTag("AO");

        if (AO_ID <= 0) return;

        //Get all the targets that have the same ID
        foreach (GameObject source in sourceArr)
        {
            ActivatableObjectSource[] aosource = source.GetComponents<ActivatableObjectSource>();

            foreach(ActivatableObjectSource aos in aosource)
            {
                if (aos.AO_ID == AO_ID)
                {
                    _sources.Add(aos);
                }
            }
        }

        //Flip delegate calls if inverse 
        if (inverse) Activate = DeactivateObject;
        else Activate = ActivateObject;

        if (inverse) Deactivate = ActivateObject;
        else Deactivate = DeactivateObject;
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateFromSource(ActivatableObjectSource source)
    {
        bool activate = (_sources.Count > 0);
        if (source.sourceType == SourceType.Compound)
        {
            activate = ActivateFromCompoundSource();
        }
        else if(source.sourceType == SourceType.Toggle)
        {
            activate = ToggleFromSource();
            if (activate) Activate();
            else Deactivate();
            return;
        }
        else
        {
            activate = ActivateFromSerialSource();
        }

        if (activate) Activate();
    }

    /// <summary>
    /// Only activates if all compound sources are active
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool ActivateFromCompoundSource()
    {
        bool activate = (_sources.Count > 0);
        foreach (ActivatableObjectSource s in _sources)
        {
            if (!s.GetComponent<ActivatableObjectSource>().Active)
            {
                activate = false;
                break;
            }
        }

        return activate;
    }

    /// <summary>
    /// Toggles the active state, regardless of other source's states
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool ToggleFromSource()
    {
        Debug.Log("Toggling Activation");
        Debug.Log(!Active);
        return !Active;
    }

    /// <summary>
    /// Activates if any of the sources are active
    /// </summary>
    /// <returns></returns>
    public bool ActivateFromSerialSource()
    {
        bool activate = false;
        foreach (ActivatableObjectSource s in _sources)
        {
            if (s.Active)
            {
                activate = true;
                break;
            }
        }

        return activate;
    }

    public void DeactivateFromSource(ActivatableObjectSource source)
    {
        bool deactivate = (_sources.Count == 0);
        if (source.sourceType == SourceType.Compound)
        {
            deactivate = DeactivateFromCompoundSource();
        }
        else if (source.sourceType == SourceType.Toggle)
        {
            deactivate = !ToggleFromSource();
            if (deactivate) Deactivate();
            else Activate();
            return;
        }
        else
        {
            deactivate = DeactivateFromSerialSource();
        }

        if (deactivate) Deactivate();
    }

    public bool DeactivateFromCompoundSource()
    {
        bool deactivate = false;
        foreach (ActivatableObjectSource s in _sources)
        {
            if (!s.Active)
            {
                deactivate = true;
                break;
            }
        }

        return deactivate;
    }

    public bool DeactivateFromSerialSource()
    {
        bool deactivate = true;
        foreach (ActivatableObjectSource s in _sources)
        {
            if (s.Active)
            {
                deactivate = false;
                break;
            }
        }

        return deactivate;
    }

    public void ActivateObject()
    {
        Active = true;
        if (_sourceAndTarget)
        {
            _activatableObject.UnlockSource();
        }
        else
        {
            _activatableObject.Activate();
        }
    }

    public void DeactivateObject()
    {
        Active = false;
        if(_sourceAndTarget)
        {
            _activatableObject.LockSource();
        }
        else
        {
            _activatableObject.Deactivate();
        }
    }
}
