using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {

    private Dictionary<string, float> _prevInput;
    string[] keys;

	// Use this for initialization
	void Start () {
        _prevInput = new Dictionary<string, float>();
        _prevInput.Add("Jump", 0.0f);
        _prevInput.Add("Dash", 0.0f);
        _prevInput.Add("Fire", 0.0f);
        _prevInput.Add("Interact", 0.0f);

        keys = new string[_prevInput.Count];
        _prevInput.Keys.CopyTo(keys, 0);
	}
	
	// Late update to avoid missing input
	void LateUpdate () {
        CacheInput();
	}

    private void CacheInput()
    {
        foreach (string key in keys)
        {
            _prevInput[key] = Input.GetAxis(key);
        }
    }

    private float GetPrevAxisRaw(string axis)
    {
        return (_prevInput[axis] > 0) ? 1.0f : 0.0f;
    }

    public float GetAxisDown(string axis)
    {
        return Input.GetAxis(axis);
    }

    public float GetAxisRaw(string axis)
    {
        return Input.GetAxisRaw(axis);
    }

    public bool GetAxisRawPressed(string axis)
    {
        float currentAxis = Input.GetAxisRaw(axis);
        return (currentAxis > 0) && (currentAxis != GetPrevAxisRaw(axis));
    }
}
