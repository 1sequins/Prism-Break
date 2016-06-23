using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransferSpectra : MonoBehaviour {

    public float transferSpeed;

    private List<Transform> _absorbers;
    private Transform _activeAbsorber;

    private PlayerController _controller;

	// Use this for initialization
	void Start () {
        _absorbers = new List<Transform>();
        _controller = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if(GetClickedObject())
            {
                _controller.DisablePhysics();
                StopAllCoroutines();
                StartCoroutine("MoveToAbsorber");
            }
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
            _controller.EnablePhysics();
            _controller.Activate();
        }
    }

    private bool GetClickedObject()
    {
        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if(hit && hit.tag == "Absorber")
        {
            if (_absorbers.Count > 0 && _absorbers.Contains(hit.transform))
            {
                _activeAbsorber = hit.transform;
                Debug.Log("Can jump to absorber");
                return true;
            }
        }

        return false;
    }

    IEnumerator MoveToAbsorber()
    {
        _controller.Deactivate();

        Vector2 originalPosition = transform.position;

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * transferSpeed;

            transform.position = Vector2.Lerp(originalPosition, _activeAbsorber.position, t);
            yield return 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Absorber")
        {
            if(!_absorbers.Contains(collider.transform.parent))
            {
                _absorbers.Add(collider.transform.parent);
                Debug.Log("Absorber added");
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
                Debug.Log("Absorber removed");
            }
        }
    }
}
