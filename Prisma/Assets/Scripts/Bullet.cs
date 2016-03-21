using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;

    private float timeout = 2.0f;
    private float life = 0.0f;
    private Rigidbody2D m_rigidbody;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.gravityScale = 0;
        m_rigidbody.drag = 0;
    }
	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.gravityScale = 0;
        m_rigidbody.drag = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        life += Time.deltaTime;
        if (life >= timeout)
        {
            Destroy(gameObject);
        }
	}

    void FixedUpdate()
    {

    }

    void OnTrigger2DEnter(Collider2D collider)
    {
        Debug.Log("Bullet Collision");
        Destroy(transform.gameObject);
    }

    public void Fire(bool right)
    {
        float dirSpeed = right ? speed : -speed;

        m_rigidbody.AddForce(new Vector2(dirSpeed, 0));
    }
}
