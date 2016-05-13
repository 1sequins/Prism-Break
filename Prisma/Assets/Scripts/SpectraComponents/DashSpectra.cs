using UnityEngine;
using System.Collections;

public class DashSpectra : MonoBehaviour {

    private GameObject player;

    private Animator anim;
    private bool _active;

    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }

	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Dash(bool right)
    {
        anim.SetBool("Right", right);
        anim.SetTrigger("Dash");
    }

    public void Activate()
    {
        _active = true;
        GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<PlayerController>().CanControl = false;

    }

    public void Deactivate()
    {
        _active = false;
        GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<PlayerController>().CanControl = true;
        transform.position = player.transform.position;
    }

    public void PullPlayer()
    {
        player.transform.position = transform.position;
    }
}
