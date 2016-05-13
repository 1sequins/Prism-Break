using UnityEngine;
using System.Collections;

public class ShotSpectraLaser : MonoBehaviour {

    public Transform laserOrigin;
    private PlayerController _controller;
    private LineRenderer _laser;

	// Use this for initialization
	void Start () {
        _controller = GetComponent<PlayerController>();
        _laser = laserOrigin.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Fire"))
        {
            Debug.Log("Fire pressed");
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
	}

    IEnumerator FireLaser()
    {
        Debug.Log(laserOrigin.right);
        _laser.enabled = true;
        while(Input.GetButton("Fire"))
        {
            Ray ray = new Ray(Vector2.zero, laserOrigin.right);
            Vector3 direction = _controller.FacingRight ? laserOrigin.right : -laserOrigin.right;
            RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position,  direction, 100);
            _laser.SetPosition(0, ray.origin);
            if (hit.collider != null)
            _laser.SetPosition(1, laserOrigin.InverseTransformPoint(hit.point));
            else
                _laser.SetPosition(1, ray.origin);

            yield return null;
        }

        _laser.enabled = false;
    }
}
