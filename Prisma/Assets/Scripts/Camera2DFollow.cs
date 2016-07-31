using System;
using System.Collections;
using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
    public float defaultCameraZoom = 8;
    public float zoomSpeed = 2.0f;
    public bool locked = false;

    private Camera _camera;
    private float _currentCameraZoom;
    private float _cameraZoom;
    private float _zoomVelocity;
    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    // Use this for initialization
    private void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographicSize = defaultCameraZoom;
        _cameraZoom = defaultCameraZoom;
        _currentCameraZoom = defaultCameraZoom;
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }


    // Update is called once per frame
    private void Update()
    {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        transform.position = newPos;

        m_LastTargetPosition = target.position;
    }

    public void ChangeTarget(Transform newTarget)
    {
        if (target == null) return;
        if (newTarget.gameObject.tag == "Player" || newTarget.gameObject.tag == "Spectra")
        {
            if (!newTarget.gameObject.GetComponent<PlayerController>().InCameraZone)
            {
                ResetCamera();
                target = newTarget;
            }
            else
            {
                SetCameraZone(newTarget.GetComponent<PlayerController>().CurrentCameraZone);
            }
        }
        else if (!locked)
        {
            target = newTarget;
        }
    }

    public void SetCameraZone(CameraZone zone)
    {
        ChangeTarget(zone.target);
        ChangeSize(zone.zoom);
    }

    public void ResetCamera()
    {
        locked = false;
        ChangeSize(defaultCameraZoom);
    }

    public void ChangeSize(float newSize)
    {
        if (!locked)
        {
            _cameraZoom = newSize;
            StopCoroutine("LerpToSize");
            StartCoroutine("LerpToSize");

        }
    }

    IEnumerator LerpToSize()
    {
        float currentTime = 0;
        float oldSize = _currentCameraZoom;
        while(_currentCameraZoom != _cameraZoom)
        {
            currentTime += zoomSpeed * Time.deltaTime;
            _currentCameraZoom = Mathf.Lerp(oldSize, _cameraZoom, currentTime);
            _camera.orthographicSize = _currentCameraZoom;
            yield return null;
        }

        _camera.orthographicSize = _cameraZoom;
    }
}
