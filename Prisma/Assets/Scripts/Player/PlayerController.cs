using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    #region Public Variables

    public float maxSpeedWalk = 8.0f;
    public float maxSpeedCrouch = 1.0f;
    public float jumpForce = 80.0f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    #endregion

    #region Private Variables

    private PlayerHealth m_PlayerHealth;

    private Rigidbody2D m_rigidbody;
    private Animator m_anim;

    private GameObject _interactableObj;

    private GameObject _movingPlatform;

    private float _currentSpeed;

    private bool _facingRight = true;
    private bool _canJump = false;
    private bool _crouching = false;
    private bool _grounded = false;
    private float _groundCheckRadius = 0.03f;
    private float _groundCheckOffset = 0.3f;
    private int _jumpTimer = 0;
    private int _jumpDelay = 1;
    private int _maxJumpFallBuffer = 5;
    private int _jumpFallBuffer = 0;

    private Vector3 _activeLocalPlatformPoint;
    private Vector3 _activeGlobalPlatformPoint;
    private Vector3 _platformVelocity;

    #endregion

    #region Properties

    public bool Active { get; set; }
    public bool CanControl { get; set; }
    public bool InCameraZone { get; set; }
    public CameraZone CurrentCameraZone { get; set; }

    public bool FacingRight
    {
        get { return _facingRight; }
        set { _facingRight = value; }
    }

    #endregion


    // Use this for initialization
	void Start () 
    {
        m_PlayerHealth = GetComponent<PlayerHealth>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        CanControl = true;
        Active = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (CanControl)
        {
            ProcessInput();
        }
	}

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;
        m_anim.SetFloat("VSpeed", m_rigidbody.velocity.y);

        Land();

        _currentSpeed = _crouching ? maxSpeedCrouch : maxSpeedWalk;

        _crouching = false;
        m_anim.SetBool("Crouching", _crouching);

        if(_movingPlatform != null)
        {
            Vector3 newGlobalPlatformPoint = _movingPlatform.transform.TransformPoint(_activeLocalPlatformPoint);
            Vector3 moveDist = newGlobalPlatformPoint - _activeGlobalPlatformPoint;
            if (moveDist != Vector3.zero)
            {
                transform.Translate(moveDist);
            }
        }
        else
        {
            _platformVelocity = Vector3.zero;
        }
        

        if(CanControl)
        {
            float move = Input.GetAxis("Horizontal");
            if(Mathf.Abs(move) > 0)
            {
                transform.parent = null;
            }
            m_anim.SetFloat("Speed", Mathf.Abs(move));
            if(move > 0)
            {
                if (!_facingRight) Flip();
            }
            else if(move < 0)
            {
                if (_facingRight) Flip();
            }
            velocity += new Vector2(move * _currentSpeed, 0);
        }

        velocity += new Vector2(0, m_rigidbody.velocity.y);
        m_rigidbody.velocity = velocity;

        if (_movingPlatform != null)
        {
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = _movingPlatform.transform.InverseTransformPoint(transform.position);
        }
        }

    private void ProcessInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetAxisRaw("Dash") > 0.0f)
        {
            if(_grounded)
            {
                _crouching = true;
                m_anim.SetBool("Crouching", _crouching);
            }
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
        //Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        return false;
    }

    private void Jump()
    {
        if (_canJump /*&& (_grounded || _jumpFallBuffer > 0)*/)
        {
            _canJump = false;
            m_anim.SetBool("Grounded", false);
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
                _jumpFallBuffer = _maxJumpFallBuffer;
            }
        }

        bool lastGround = _grounded;
        _grounded = Physics2D.OverlapCircle(groundCheck.position, _groundCheckRadius, groundLayer);
        m_anim.SetBool("Grounded", _grounded);
        if (!lastGround && _grounded)
        {
            _canJump = false;
            _jumpTimer = _jumpDelay;
        }

        if (!_grounded)
        {
            _movingPlatform = null;

            //Buffer so player can jump a short time after falling off a platform
            if (_canJump)
            {
                _jumpFallBuffer--;
                if (_jumpFallBuffer < 0) _canJump = false;
            }
        }
    }

    public void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 flipScale = transform.localScale;
        flipScale.x *= -1;
        transform.localScale = flipScale;
    }

    public void Activate()
    {
        Active = true;
        CanControl = true;
        if (m_PlayerHealth != null) m_PlayerHealth.stasis = false;
    }

    public void Deactivate()
    {
        Active = false;
        CanControl = false;
        if(m_PlayerHealth != null) m_PlayerHealth.stasis = true;
        _currentSpeed = 0;
        m_rigidbody.velocity = new Vector2(0, 0);
        m_anim.SetFloat("Speed", 0);
    }

    public void Unlock()
    {
        CanControl = true;
    }

    public void Lock()
    {
        CanControl = true;
    }

    public void EnablePhysics()
    {
        m_rigidbody.gravityScale = 1;
        GetComponent<BoxCollider2D>().isTrigger = false;
        GetComponent<CircleCollider2D>().isTrigger = false;
    }

    public void DisablePhysics()
    {
        m_rigidbody.gravityScale = 0;
        //GetComponent<BoxCollider2D>().isTrigger = true;
        //GetComponent<CircleCollider2D>().isTrigger = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Moving Platform")
        {
            //transform.parent = collision.gameObject.transform;
            _movingPlatform = collision.gameObject;
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = _movingPlatform.transform.InverseTransformPoint(transform.position);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Moving Platform")
        {
            _movingPlatform = null;
        }
    }


}
