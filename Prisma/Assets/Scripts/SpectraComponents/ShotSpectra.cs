using UnityEngine;
using System.Collections;

public class ShotSpectra : MonoBehaviour {

    public GameObject bullet;

    private PlayerController _controller;
    private GameObject _shotSpawn;



	// Use this for initialization
	void Start () {
        _controller = GetComponent<PlayerController>();
        _shotSpawn = transform.FindChild("ShotSpawn").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if(_controller.CanControl)
        {
            if (Input.GetButtonDown("Fire"))
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        GameObject firedBullet = (GameObject)Instantiate(bullet, _shotSpawn.transform.position, Quaternion.identity);
        firedBullet.GetComponent<Bullet>().Fire(_controller.FacingRight);
    }
}
