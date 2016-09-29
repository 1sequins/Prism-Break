using UnityEngine;
using System.Collections;

public class PhaseSpectra : MonoBehaviour {

    private PlayerController _controller;
    private GameObject _interactableObj;

    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.CanControl)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (_interactableObj != null)
                {
                    _interactableObj.GetComponent<MovableBlock>().Interact(gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.GetComponent<MovableBlock>() != null)
        {
            Debug.Log("Can phase");
            _interactableObj = obj;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (_interactableObj == collider.gameObject)
        {
            _interactableObj = null;
        }
    }
}
