using UnityEngine;
using System.Collections;

public class Spectra : MonoBehaviour
{

    #region Public Variables

    public float maxSpeed = 4.0f;
    public float jumpForce = 300.0f;
    public float cooldown;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Color primaryColor = Color.white;
    public Color secondaryColor = Color.black;

    #endregion

    private Rigidbody2D m_rigidbody;

    private bool _active = true;
    private bool _canActivate = true;
    private bool _primaryForm = true;

    private Vector2 _startLoc;
    private Vector2 _targetLoc;
    private bool _traveling = false;
    private float _travelDuration = 0.0f;
    private float _travelCounter = 0.0f;
    private float _activeCount = 0.0f;
    private float _rechargeCount = 0.0f;

    private bool _canControl = true;
    private bool _canJump = false;
    public bool _jumping = false;
    public bool _grounded = false;
    private float _groundCheckRadius = 0.07f;
    private int _jumpTimer = 0;
    private int _jumpDelay = 10;

    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }

    public bool CanActivate
    {
        get { return _canActivate; }
        set { _canActivate = value; }
    }
    

	// Use this for initialization
	void Start () {
        //Deactivate();
        _canActivate = true;
        m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {
        if(_active)
        {
            if (!_grounded) _canJump = false;
            Land();
        }
        else if(!_canActivate)
        {
            _rechargeCount += Time.fixedDeltaTime;
            if(_rechargeCount >= cooldown)
            {
                _canActivate = true;
            }
        }
        if (_traveling)
        {
            //Move();
        }
    }

    public void Jump()
    {
        if (_grounded)
        {
            _jumping = true;
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
            m_rigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }

    private void Land()
    {
        bool lastGround = _grounded;
        _grounded = Physics2D.OverlapCircle(groundCheck.position, _groundCheckRadius, groundLayer);
        if (!lastGround && _grounded)
        {
            _canJump = false;
            _jumpTimer = _jumpDelay;
            _jumping = false;

        }
    }

    public void Move(float move)
    {
        m_rigidbody.velocity = new Vector2(move * maxSpeed, m_rigidbody.velocity.y);
    }

    public void Activate()
    {
        _active = true;
        _activeCount = 0.0f;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = true;
    }

    public void Deactivate()
    {
        _active = false;
        _canActivate = false;
        _rechargeCount = 0.0f;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }
}
