using UnityEngine;
using System.Collections;

public class CameraZone : MonoBehaviour {

    public int zoom;
    public Transform target;

    private Camera _camera;
    private Camera2DFollow _cameraFollow;
    private Transform _originalTarget;
    private float _originalZoom;

    // Use this for initialization
    void Start() {
        _camera = Camera.main;
        _cameraFollow = _camera.GetComponent<Camera2DFollow>();
        _originalZoom = _cameraFollow.defaultCameraZoom;
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Spectra")
        {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            player.InCameraZone = true;
            player.CurrentCameraZone = this;
            _originalTarget = collider.transform;
            _originalZoom = _cameraFollow.defaultCameraZoom;
            if (target != null && player.Active)
            {
                _cameraFollow.SetCameraZone(this);
                _cameraFollow.locked = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player" || collider.gameObject.tag == "Spectra")
        {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            player.InCameraZone = false;
            player.CurrentCameraZone = null;
            if(player.Active) _cameraFollow.ChangeTarget(player.transform);
        }
    }
}
