using UnityEngine;
using System.Collections;

public class ShotSpectra : MonoBehaviour {

    public GameObject bullet;

    private GameObject player;
    private GameObject shotSpawn;



	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject;
        shotSpawn = transform.FindChild("ShotSpawn").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Fire(bool right)
    {
        Debug.Log("Firing Bullet");
        GameObject firedBullet = (GameObject)Instantiate(bullet, shotSpawn.transform.position, Quaternion.identity);
        firedBullet.GetComponent<Bullet>().Fire(right);
    }
}
