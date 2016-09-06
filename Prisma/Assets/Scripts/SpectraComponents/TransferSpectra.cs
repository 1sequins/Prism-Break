using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransferSpectra : MonoBehaviour {

    public float transferSpeed;
    
    //TODO: Add animator state for when in absorber

    private List<Transform> _absorbers;
    private Transform _activeAbsorber;
    private Absorber _activeAbsorberScript;

    private PlayerController _controller;

	// Use this for initialization
	void Start () {
        _absorbers = new List<Transform>();
        _controller = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {

        if(_controller.Active && _controller.CanControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GetClickedObject())
                {
                    _controller.DisablePhysics();
                    StopAllCoroutines();
                    StartCoroutine("MoveToAbsorber");
                }
            }
            if (Input.GetAxisRaw("Dash") > 0.0f)
            {
                StopAllCoroutines();
                ExitAbsorber();
                _controller.EnablePhysics();
                _controller.Activate();
            }
        }
    }

    private bool GetClickedObject()
    {
        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if(hit && hit.tag == "Absorber")
        {
            if(_activeAbsorber)
            {
                if(_activeAbsorberScript.linkedAbsorbers.Contains(hit.gameObject))
                {
                    Absorber absorberScript = hit.transform.GetComponent<Absorber>();
                    if (!absorberScript.Active) return false;
                    _activeAbsorberScript.HideLinks();
                    _activeAbsorber = hit.transform;
                    _activeAbsorberScript = absorberScript;
                    return true;
                }
            }
            if (_absorbers.Count > 0 && _absorbers.Contains(hit.transform))
            {
                Absorber absorberScript = hit.transform.GetComponent<Absorber>();
                if (!absorberScript.Active) return false;
                _activeAbsorber = hit.transform;
                _activeAbsorberScript = absorberScript;
                return true;
            }
        }

        return false;
    }

    private void ExitAbsorber()
    {
        if(_activeAbsorber != null)
            _activeAbsorberScript.HideLinks();
        _activeAbsorber = null;
        _activeAbsorberScript = null;
        _controller.Unlock();
    }

    IEnumerator MoveToAbsorber()
    {
        _controller.Lock();

        Vector2 originalPosition = transform.position;

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * transferSpeed;

            transform.position = Vector2.Lerp(originalPosition, _activeAbsorber.position, t);
            yield return 0;
        }

        _activeAbsorberScript.DisplayLinks();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Absorber")
        {
            if(!_absorbers.Contains(collider.transform.parent))
            {
                _absorbers.Add(collider.transform.parent);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Absorber")
        {
            if (_absorbers.Contains(collider.transform.parent))
            {
                _absorbers.Remove(collider.transform.parent);
            }
        }
    }
}
