using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActivatableObjectTarget))]
public class MovablePlatform : ActivatableObject {

    public Transform platform;

    public Transform startPosition;
    public Transform endPosition;

    public float speed;

    public Vector2 PreviousPosition { get { return _prevPosition; } }
    public Vector2 Position { get { return _currentPosition; } }

    //private Rigidbody2D _rigidbody;
    private Vector2 _originalPosition;
    private Vector2 _prevPosition;
    private Vector2 _currentPosition;

	// Use this for initialization
	void Start () {
        //_rigidbody = platform.GetComponent<Rigidbody2D>();
        _originalPosition = platform.position;
        _currentPosition = _originalPosition;
        _prevPosition = _currentPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        //_rigidbody.MovePosition(Vector2.Lerp(platform.position, endPosition.position, speed * Time.deltaTime));
    }

    IEnumerator MoveToEnd()
    {
        float dist = Vector2.Distance(_currentPosition, endPosition.position);
        float rate = speed / dist;

        _originalPosition = platform.position;

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;

            _prevPosition = platform.position;

            platform.position = Vector2.Lerp(_originalPosition, endPosition.position, t);
            //_rigidbody.MovePosition(Vector2.Lerp(_originalPosition, endPosition.position, t));
            _currentPosition = platform.position;
            yield return 0;
        }

        //Zero out position
        _prevPosition = _currentPosition;
    }

    IEnumerator MoveToStart()
    {
        float dist = Vector2.Distance(_currentPosition, startPosition.position);
        float rate = speed / dist;

        _originalPosition = platform.position;

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;

            _prevPosition = _currentPosition;

            platform.position = Vector2.Lerp(_originalPosition, startPosition.position, t);
            //_rigidbody.MovePosition(Vector2.Lerp(_originalPosition, startPosition.position, t));
            _currentPosition = platform.position;
            yield return 0;
        }

        //Zero out position
        _prevPosition = _currentPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPosition.position, endPosition.position);
    }

    public override void Activate()
    {
        base.Activate();
        StopAllCoroutines();
        StartCoroutine("MoveToEnd");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        StopAllCoroutines();
        StartCoroutine("MoveToStart");
    }
}
