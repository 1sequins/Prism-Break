using UnityEngine;
using System.Collections;

public class PlayerPoweredInteract : MonoBehaviour
{
    public int maxPowerStore = 1;
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
                    PoweredSwitch pSwitch = _interactableObj.GetComponent<PoweredSwitch>();
                    if(pSwitch.Active && _powerStore < maxPowerStore)
                    {
                        pSwitch.Interact(gameObject);
                        IncreasePowerStore();
                    }
                    else if(!pSwitch.Active && _powerStore > 0)
                    {
                        pSwitch.Interact(gameObject);
                        DecreasePowerStore();
                   }
                }
            }
        }
    }

    private void IncreasePowerStore()
    {
        _powerStore++;
    }
    
    private void DecreasePowerStore()
    {
        _powerStore--;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.GetComponent<PoweredSwitch>() != null)
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