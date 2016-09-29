using UnityEngine;
using System.Collections;

public class PlayerSwitchInteract : MonoBehaviour
{

    private PlayerController _controller;
    private GameObject _interactableObj;

    private int _powerStore;

    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<PlayerController>();

        _powerStore = 0;
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
                    Switch pSwitch = _interactableObj.GetComponent<Switch>();
                    pSwitch.Interact(gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.GetComponent<Switch>() != null)
        {
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