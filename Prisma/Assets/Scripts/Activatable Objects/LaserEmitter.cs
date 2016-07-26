using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(ActivatableObjectTarget))]
public class LaserEmitter : ActivatableObject {

    public Transform laserOrigin;
    public LayerMask laserMask;

    private LineRenderer _laser;
    private Mirror _hitMirror;

    private List<Vector3> _laserPoints;

    // Use this for initialization
    void Start()
    {
        _laser = laserOrigin.GetComponent<LineRenderer>();
        _laserPoints = new List<Vector3>();

        SetInitialState();
    }

    // Update is called once per frame
    void Update()
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

    /*
    private void BounceLaser(Vector3 origin, Vector3 direction)
    {
        Debug.Log("Origin: " + origin);
        Debug.Log("Direction: " + direction);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, 100, laserMask);
        Debug.Log("Hit: " + hit.collider);
        if (hit.collider != null)
        {
            Vector3 localHitPoint = laserOrigin.InverseTransformPoint(hit.point);
            _laserPoints.Add(localHitPoint);

            if (hit.collider.tag == "Mirror")
            {
                Vector2 normal = hit.normal;

                ProcessNormal(ref normal);

                Vector3 newDirection = ReflectLaser(direction, normal);
                Debug.Log("Bouncing");
                Debug.Log(_laserPoints.Count);
                BounceLaser(hit.point, newDirection);
            }
        }
        else
        {
            _laserPoints.Add(origin);
        }
    }
    */

    IEnumerator FireLaser()
    {
        _laser.enabled = true;
        while (Active)
        {
            _laserPoints.Clear();
            _laserPoints.Add(Vector3.zero);
            //BounceLaser(laserOrigin.position, laserOrigin.right);

            _laser.SetPositions(_laserPoints.ToArray());
            
            Ray ray = new Ray(Vector2.zero, laserOrigin.right);
            Vector3 direction = laserOrigin.right;
            RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position, direction, 100, laserMask);
            _laser.SetPosition(0, ray.origin);
            if (hit.collider != null)
            {
                _laser.SetPosition(1, laserOrigin.InverseTransformPoint(hit.point));

                if(hit.collider.tag == "Mirror")
                {
                    Vector2 normal = hit.normal;

                    ProcessNormal(ref normal);

                    if(normal.x != 0 && normal.y != 0)
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

    public override void Activate()
    {
        Debug.Log("Laser Activated");
        base.Activate();
        StopCoroutine("FireLaser");
        StartCoroutine("FireLaser");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        StopCoroutine("FireLaser");
        _laser.enabled = false;
        if (_hitMirror) _hitMirror.Deactivate();
        _hitMirror = null;
    }
}
