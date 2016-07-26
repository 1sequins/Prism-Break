using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {

    public Transform laserOrigin;
    public LayerMask laserMask;

    private LineRenderer _laser;
    private Mirror _hitMirror;
    private bool _active;
    private Vector3 _laserDir;

    // Use this for initialization
    void Start () {
        _laser = laserOrigin.GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    private void ProcessNormal(ref Vector2 n)
    {
        float tolerance = 0.02f;

        if (n.x > 1 - tolerance) n.x = 1.0f;
        else if (n.x < -(1 - tolerance)) n.x = -1.0f;
        else if (n.x > tolerance) n.x = 0.5f;
        else if (n.x < -tolerance) n.x = -0.5f;
        else n.x = 0;

        if (n.y > 1 - tolerance) n.y = 1.0f;
        else if (n.y < -(1 - tolerance)) n.y = -1.0f;
        else if (n.y > tolerance) n.y = 0.5f;
        else if (n.y < -tolerance) n.y = -0.5f;
        else n.y = 0;

        n.Normalize();
    }

    private Vector3 ReflectLaser(Vector3 din, Vector3 n)
    {
        Vector3 dout = din - 2 * Vector2.Dot(din, n) * n;
        dout.Normalize();
        return dout;
    }

    IEnumerator BounceLaser()
    {
        _laser.enabled = true;
        while (_active)
        {

            Ray ray = new Ray(Vector2.zero, _laserDir);
            Vector3 direction = laserOrigin.right;
            RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position, _laserDir, 100, laserMask);
            _laser.SetPosition(0, ray.origin);
            if (hit.collider != null)
            {
                _laser.SetPosition(1, laserOrigin.InverseTransformPoint(hit.point));

                if (hit.collider.tag == "Mirror" && hit.collider.gameObject != transform.gameObject)
                {
                    Vector2 normal = hit.normal;

                    ProcessNormal(ref normal);

                    if (normal.x != 0 && normal.y != 0)
                    {
                        Vector3 newDirection = ReflectLaser(direction, normal);

                        _hitMirror = hit.collider.gameObject.GetComponent<Mirror>();
                        _hitMirror.Activate(newDirection);
                    }


                }
            }
            else
            {
                _laser.SetPosition(1, ray.origin);
            }
            


            yield return null;
        }
    }

    /*
    public void BounceLaser(Vector3 direction)
    {
        _laser.enabled = true;

        Debug.Log("Bouncing Laser");

        
        Ray ray = new Ray(Vector2.zero, laserOrigin.right);
        RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position, direction, 100, laserMask);
        _laser.SetPosition(0, ray.origin);
        if (hit.collider != null)
        {
            _laser.SetPosition(1, laserOrigin.InverseTransformPoint(hit.point));

            if (hit.collider.tag == "Mirror" && hit.collider.gameObject != transform.parent.gameObject)
            {
                    Vector2 normal = hit.normal;

                    ProcessNormal(ref normal);

                    if (normal.x != 0 && normal.y != 0)
                    {
                        Vector3 newDirection = ReflectLaser(direction, normal);
                        hit.collider.gameObject.GetComponent<Mirror>().Activate(newDirection);
                    }


                }
            }
            else
            {
                _laser.SetPosition(1, ray.origin);
            }

        _laser.enabled = false;
    }
    */

    public void Activate(Vector3 newDir)
    {
        if(!_active)
        {
            _active = true;
            _laserDir = newDir;
            StopCoroutine("BounceLaser");
            StartCoroutine("BounceLaser");
        }
    }

    public void Deactivate()
    {
        Debug.Log("Deactivating Mirror");
        _active = false;
        StopCoroutine("BounceLaser");
        _laser.enabled = false;
        if (_hitMirror) _hitMirror.Deactivate();
        _hitMirror = null;
    }
}
