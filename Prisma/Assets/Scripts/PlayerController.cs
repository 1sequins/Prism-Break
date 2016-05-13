using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    #region Public Variables

    public float maxSpeedWalk = 8.0f;
    public float maxSpeedCrouch = 1.0f;
    public float jumpForce = 80.0f;
    public float dashForce = 40.0f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    #endregion

    #region Private Variables

    private PlayerHealth m_PlayerHealth;

    private float _prevInputJump;

    private Rigidbody2D m_rigidbody;
    private Animator m_anim;

    private GameObject _interactableObj;

    private float _currentSpeed;

    private bool _facingRight = true;
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

    public bool CanControl { get; set; }

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
        distToGround = GetComponent<BoxCollider2D>().bounds.extents.y;
        CanControl = true;
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
        if (_hurt)
        {
            _hurtTimer -= Time.deltaTime;
            if (_hurtTimer <= 0)
            {
                CanControl = true;
                _hurt = false;
            }
        }
        m_anim.SetFloat("VSpeed", m_rigidbody.velocity.y);

        Land();

        _currentSpeed = _crouching ? maxSpeedCrouch : maxSpeedWalk;

        _crouching = false;
        m_anim.SetBool("Crouching", _crouching);
        

        if(CanControl)
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
            //transform.FindChild("Spectra_Dash").gameObject.GetComponent<DashSpectra>().Dash(_facingRight);
        }

        if (Input.GetButtonDown("Fire"))
        {
            GetComponent<ShotSpectra>().Fire(_facingRight);
        }

        if(Input.GetButtonDown("Shield"))
        {
            if(_interactableObj != null)
            {
                _interactableObj.GetComponent<IInteractable>().Interact(gameObject);
            }
            //_shielded = !_shielded;
            //Debug.Log("Shielded");
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
            CanControl = false;
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

    public void Activate()
    {
        CanControl = true;
        if (m_PlayerHealth != null) m_PlayerHealth.stasis = false;
    }

    public void Deactivate()
    {
        CanControl = false;
        if(m_PlayerHealth != null) m_PlayerHealth.stasis = true;
        _currentSpeed = 0;
        m_rigidbody.velocity = new Vector2(0, 0);
        m_anim.SetFloat("Speed", 0);
    }

    #region Events

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if(obj.GetComponent<IInteractable>() != null)
        {
            Debug.Log("Interactable");
            _interactableObj = obj;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(_interactableObj == collider.gameObject)
        {
            _interactableObj = null;
        }
    }

    #endregion
}
