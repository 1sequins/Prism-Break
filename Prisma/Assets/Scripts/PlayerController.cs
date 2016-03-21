using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    #region Public Variables

    public float health = 10.0f;
    public float maxSpeedWalk = 8.0f;
    public float maxSpeedCrouch = 1.0f;
    public float jumpForce = 80.0f;
    public float dashForce = 40.0f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    #endregion

    #region Private Radius

    private PlayerInput m_PlayerInput;

    private float _prevInputJump;

    private Rigidbody2D m_rigidbody;
    private Animator m_anim;

    private float _currentSpeed;

    private bool _facingRight = true;
    private bool _canControl = true;
    private bool _canJump = false;
    private bool _canDoubleJump = false;
    private bool _shielded = false;
    private bool _jumping = false;
    private bool _keepCrouch = false;
    private bool _crouching = false;
    private bool _grounded = false;
    private bool _invulnerable = false;
    private float _groundCheckRadius = 0.03f;
    private int _jumpTimer = 0;
    private int _jumpDelay = 1;
    private bool _hurt = false;
    private float _hurtTimer = 0.0f;
    private float _hurtDelay = 0.4f;

    private float distToGround;

    #endregion

    #region Properties

    public bool CanControl
    {
        get { return _canControl; }
        set { _canControl = value; }
    }

    public bool FacingRight
    {
        get { return _facingRight; }
        set { _facingRight = value; }
    }

    #endregion


    // Use this for initialization
	void Start () 
    {
        Debug.Log("Started");
        m_PlayerInput = GetComponent<PlayerInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        distToGround = GetComponent<BoxCollider2D>().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (_canControl)
        {
            ProcessInput();
        }
	}

    void FixedUpdate()
    {
        if (_hurt)
        {
            _hurtTimer -= Time.deltaTime;
            if (_hurtTimer <= 0)
            {
                _canControl = true;
                _hurt = false;
            }
        }
        m_anim.SetFloat("VSpeed", m_rigidbody.velocity.y);

        Land();

        _currentSpeed = _crouching ? maxSpeedCrouch : maxSpeedWalk;

        _crouching = false;
        m_anim.SetBool("Crouching", _crouching);
        

        if(_canControl)
        {
            float move = Input.GetAxis("Horizontal");
            m_anim.SetFloat("Speed", Mathf.Abs(move));
            if(move > 0)
            {
                if (!_facingRight) Flip();
            }
            else if(move < 0)
            {
                if (_facingRight) Flip();
            }

            m_rigidbody.velocity = new Vector2(move * _currentSpeed, m_rigidbody.velocity.y);
        }
    }

    private void ProcessInput()
    {
        if (m_PlayerInput.GetAxisRawPressed("Jump"))
        {
            Jump();
        }

        if (m_PlayerInput.GetAxisRaw("Dash") > 0.0f)
        {
            if(_grounded)
            {
                _crouching = true;
                m_anim.SetBool("Crouching", _crouching);
            }
            //transform.FindChild("Spectra_Dash").gameObject.GetComponent<DashSpectra>().Dash(_facingRight);
        }

        if (m_PlayerInput.GetAxisRawPressed("Fire"))
        {
            //transform.FindChild("Spectra_Shot").gameObject.GetComponent<ShotSpectra>().Fire(_facingRight);
        }

        if(m_PlayerInput.GetAxisRawPressed("Shield"))
        {
            _shielded = !_shielded;
            Debug.Log("Shielded");
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!GetClickedObject())
            {

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            GetClickedObject();
            
        }
    }

    private bool GetClickedObject()
    {
        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        return false;
    }

    private void Jump()
    {
        if (_canJump && _grounded)
        {
            _canJump = false;
            m_anim.SetBool("Grounded", false);
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
            m_rigidbody.AddForce(new Vector2(0, jumpForce));
        }
       else if(_canDoubleJump)
       {
            _canDoubleJump = false;
            _jumping = true;
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
            m_rigidbody.AddForce(new Vector2(0, jumpForce));
       }
    }

    private void Land()
    {
        if (!_canJump && _jumpTimer > 0)
        {
            _jumpTimer--;
            if (_jumpTimer <= 0)
            {
                _canJump = true;
                _canDoubleJump = true;
            }
        }

        bool lastGround = _grounded;
        _grounded = Physics2D.OverlapCircle(groundCheck.position, _groundCheckRadius, groundLayer);
        m_anim.SetBool("Grounded", _grounded);
        if (!lastGround && _grounded)
        {
            _canJump = false;
            _canDoubleJump = false;
            _jumpTimer = _jumpDelay;
            _jumping = false;

        }

        if (!_grounded) _canJump = false;
    }

    public void Hurt()
    {
        if(!_invulnerable)
        {
            _hurt = true;
            _canControl = false;
            //_invulnerable = true;
            m_rigidbody.velocity = new Vector2(0, 0);
            m_rigidbody.AddForce(new Vector2(-70, 70));
            _hurtTimer = _hurtDelay;
        }
    }

    public void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 flipScale = transform.localScale;
        flipScale.x *= -1;
        transform.localScale = flipScale;
    }
}
