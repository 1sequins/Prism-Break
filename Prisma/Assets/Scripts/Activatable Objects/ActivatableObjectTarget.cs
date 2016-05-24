using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatableObjectTarget : MonoBehaviour {

    public int AO_ID;

    public bool Active { get; set; }

    private bool _sourceAndTarget;

    private List<ActivatableObjectSource> _sources;

    private ActivatableObject _activatableObject;

	// Use this for initialization
	void Start () {

        _sourceAndTarget = GetComponent<ActivatableObjectSource>() != null;

        _sources = new List<ActivatableObjectSource>();

        _activatableObject = GetComponent<ActivatableObject>();

        GameObject[] sourceArr = GameObject.FindGameObjectsWithTag("AO");

        if (AO_ID <= 0) return;

        //Get all the targets that have the same ID
        foreach (GameObject source in sourceArr)
        {
            ActivatableObjectSource aosource = source.GetComponent<ActivatableObjectSource>();
            if (aosource != null)
            {
                if (aosource.AO_ID == AO_ID) _sources.Add(aosource);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateFromSource(ActivatableObjectSource source)
    {
        bool activate = (_sources.Count > 0);
        if (source.GetType() == typeof(AO_CompondSource))
        {
            activate = ActivateFromCompoundSource((AO_CompondSource)source);
        }
        else if(source.GetType() == typeof(AO_ToggleSource))
        {
            activate = ToggleFromSource((AO_ToggleSource)source);
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
    public bool ActivateFromCompoundSource(AO_CompondSource source)
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
    public bool ToggleFromSource(AO_ToggleSource source)
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
        if (source.GetType() == typeof(AO_CompondSource))
        {
            deactivate = DeactivateFromCompoundSource((AO_CompondSource)source);
        }
        else if (source.GetType() == typeof(AO_ToggleSource))
        {
            deactivate = !ToggleFromSource((AO_ToggleSource)source);
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

    public bool DeactivateFromCompoundSource(AO_CompondSource source)
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

    public void Activate()
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

    public void Deactivate()
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
