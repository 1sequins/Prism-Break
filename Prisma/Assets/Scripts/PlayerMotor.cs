using UnityEngine;
using System;
using System.Collections;

/*
    Based on https://github.com/cjddmut/Unity-2D-Platformer-Controller
*/
public class PlayerMotor : MonoBehaviour {

    //Layer Masks for the environment
    public LayerMask staticEnviromentLayer;
    public LayerMask movingPlatformLayer;

    //Distances to check for environment masks
    public float envCheckDistance = 0.04f;
    public float minDistFromEnv = 0.02f;

    //Number of iterations the motor can make during FixedUpdate
    public int numIterations = 2;
    public int additionalRaycastsPerSide = 2;

    public bool enableOneWayPlatforms = true;
    public bool oneWayPlatformsAreSolid = true;

    public float groundSpeed = 8.0f;
    public float groundAcceleration = 0.01f;
    public float groundStopDistance = 0.333f;

    public float airSpeed = 5.0f;
    public float airAcceleration = 0.2f;
    public float airStopDistance = 2.0f;
    public float fallSpeed = 16.0f;
    public float gravityMultiplier = 4.0f;
    public bool changeDirectionInAir = true;

    public float jumpHeight = 1.5f;
    public float extraJumpHeight = 1.5f;
    public int numOfAirJumps = 0;
    public float jumpWindowWhenFalling = 0.2f;
    public float jumpWindowWhenActivated = 0.2f;

    public bool enableSlopes = true;
    public float slopeTolerance = 50.0f;
    public float minSpeedToClimbSteepSlope = 7.5f;
    public bool changeSpeedOnSlopes = true;
    [Range(0.0f, 1.0f)]
    public float speedMultiplierOnSlope = 0.75f;
    public bool stickOnGround = true;

    //Delegates for actions
    public Action onJump;
    public Action onAirJump;
    public Action onLanded;
    public Action onSlipping;
    public Action onSlippingEnd;

    public bool movingPlatformDebug;
    public bool iterationDebug;

    //Motor States
    public enum MotorState
    {
        OnGround,
        Jumping,
        Falling,
        Frozen,
        Slipping,
        FreeState
    }

    //Surfaces the motor may be colliding against
    [Flags]
    public enum CollidedSurface
    {
        None = 0x0,
        Ground = 0x1,
        LeftWall = 0x2,
        RightWall = 0x4,
        Ceiling = 0x8,
        LeftSlope = 0x10,
        RightSlope = 0x20
    }

    //The surfaces the motor may be colliding against
    [Flags]
    public enum CollidedArea
    {
        None = 0x0,
        Restricted = 0x01,
        FreeArea = 0x02,
        Ladder = 0x04
    }

    //Zones in the ladder
    public enum LadderZone
    {
        Top,
        Middle,
        Bottom
    }

    public MotorState motorState { get; private set; }
    public CollidedSurface collidingAgainst { get; private set; }
    public CollidedArea inArea { get; private set; }
    public LadderZone ladderZone { get; private set; }

    //Movement directions multiplied by max speed [-1, 1]
    public float NormalizedXMovement { get; set; }
    public float NormalizedYMovement { get; set; }

    public float TimeScale
    {
        get
        {
            return _timeScale;
        }
        set
        {
            if(value > 0)
            {
                if(_timeScale != 0)
                {
                    //ReadjustTimers(_timeScale / value);
                }
                else
                {
                    //ReadjustTimers(_savedTimeScale / value);
                }
            }

            _savedTimeScale = _timeScale;
            _timeScale = value;

            if(_timeScale < 0)
            {
                _timeScale = 0;
            }
        }
    }

    public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }
    public bool FacingRight { get; set; }
    public bool JumpingHeld
    {
        get
        {
            return _jumping.held;
        }
        set
        {
            //Only set jumping held to false here to prevent it from being set after a release
            if(!value)
            {
                _jumping.held = false;
            }
        }
    }

    public bool Frozen
    {
        get
        {
            return _frozen;
        }
        set
        {
            if(_frozen != value)
            {
                _frozen = value;

                //Do not use ChangeState, because delegates would be called
                if(_frozen)
                {
                    _prevState = motorState;
                    motorState = MotorState.Frozen;
                }
                else
                {
                    motorState = _prevState;
                }
            }
        }
    }

    //TODO: Make own class
    //public MovingPlatformMotor2D connectedPlatform { get { return _movingPlatformState.platform; } }
    public bool OnSlope { get; private set; }
    public Vector2 SlopeNormal { get; private set; }

    public void Jump()
    {

    }

    private const float NEAR_ZERO = 0.0001f;

    private const float DISTANCE_TO_END_ITERATION = 0.001f;
    private const float CHECK_TOUCHING_TRIM = 0.01f;

    private const int STARTING_ARRAY_SIZE = 4;
    private const float INCREASE_ARRAY_SIZE_MULTIPLIER = 2;

    private const int DIRECTIONS_CHECKED = 4;
    private const int DIRECTION_DOWN = 0;
    private const int DIRECTION_UP = 1;
    private const int DIRECTION_LEFT = 2;
    private const int DIRECTION_RIGHT = 3;

    private static RaycastHit2D[] _hits = new RaycastHit2D[STARTING_ARRAY_SIZE];
    private static Collider2D[] _overlappingColliders = new Collider2D[STARTING_ARRAY_SIZE];

    private LayerMask _collisionMask;

    private Vector2 _restrictedAreaTR;
    private Bounds _restrictedArea;
    private Vector2 _restrictedAreaBL;
    private float _ignorerMovementFrames;
    private bool _frozen;
    private bool _originalKinematic;
    private float _timeScale = 1;
    private Vector2 _prevLoc;
    private Collider2D[] _collidersUpAgainst = new Collider2D[DIRECTIONS_CHECKED];
    private Vector2[] _collidedNormals = new Vector2[DIRECTIONS_CHECKED];
    private MotorState _prevState;
    private Bounds _prevColliderBounds;
    private float _dotAllowedForSlopes;
    private float _cornerDistanceCheck;
    private float _distanceFromEnvCorner;
    private Vector2 _bottomRight;
    private Vector2 _toTransform;
    private float _currentDeltaTime;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    private float _distanceToBoundsCorner;
    private float _savedTimeScale;
    private Vector2 _disallowedSlopeNormal;
    private Vector2 _prevMoveDir;
    private bool _isValidWallInteraction;

    //Unconverted motor velocity
    private Vector2 _velocity;

    //Catch public field changes during runtime
    private float _currentSlopeDegreeAllowed;

    //Moving platform debug
    private Vector2 _point;
    private Vector2 _point2;
    private Bounds _prevPosPlat;
    private Bounds _startPosMotor;
    private Bounds _movedPosMotor;

    //Iteration debug
    private int _iterationsUsed;
    private Bounds[] _iterationBounds;

    private Bounds _ladderArea;
    private Bounds _ladderBottomArea;
    private Bounds _ladderTopArea;

    //Contains jump variables
    private struct JumpState
    {
        public bool pressed;
        public bool held;
        public int numAirJumps;

        public int timeToldFrames;
        public int allowExtraFrames;

        public bool force;
        public float height;

        public float jumpGraceFrames;
        public bool jumpTypeChanged;

        public JumpType LastValidJump
        {
            get { return _lastValidJump; }
            set
            {
                if(value != JumpType.None)
                {
                    jumpTypeChanged = true;
                }
                else
                {
                    jumpGraceFrames = -1;
                }

                _lastValidJump = value;
            }
        }

        public enum JumpType
        {
            None,
            Normal
        }

        private JumpType _lastValidJump;
    }
    private JumpState _jumping = new JumpState();

    private bool _ignoreGravity;

    private struct MovingPlatformState
    {
        //public MovingPlatformMotor2D platform;

        public Vector2 PrevPosition;
        //public bool IsOnPlatform { get { return platform != null; } }
    }
    private MovingPlatformState _movingPlatformState = new MovingPlatformState();

    void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if(iterationDebug)
        {
            _iterationBounds = new Bounds[2 + numIterations];
        }
    }

	// Use this for initialization
	void Start ()
    {
        _prevLoc = _collider2D.bounds.center;

        //Initial set, do not use ChangeState
        motorState = MotorState.Falling;
        _cornerDistanceCheck = Mathf.Sqrt(2 * envCheckDistance * envCheckDistance);
        _distanceFromEnvCorner = Mathf.Sqrt(2 * minDistFromEnv * minDistFromEnv);

        _distanceToBoundsCorner = (_collider2D.bounds.max - _collider2D.bounds.center).magnitude;

        _bottomRight = new Vector2(1, -1).normalized;
	}

    
	
	// Update is called once per frame
	void Update () {
	
	}
}
