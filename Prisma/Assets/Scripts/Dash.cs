using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour
{

    #region Public Variables

    public float dashSpeed;
    public float dashDistance;

    #endregion

    private Vector2 _startLoc;
    private Vector2 _endLoc;

    //private bool _dashing = false;

    private float _dashCounter;
    private float _dashDuration;

    public Vector2 StartLoc
    {
        set { _startLoc = value; }
    }

    public Vector2 EndLoc
    {
        set { _startLoc = value; }
    }

    public void SetDash(Vector2 start)
    {
        _startLoc = start;

        float dist = Input.GetAxis("Horizontal") >= 0.0f ? dashDistance : -dashDistance;

        //Calculate how far to travel
        _endLoc = new Vector2(_startLoc.x + dist, _startLoc.y);

        //And how fast to travel
        _dashDuration = Mathf.Abs(dist) / dashSpeed;
    }

    public void SetDash(Vector2 start, bool right)
    {
        _startLoc = start;

        float dist = right ? dashDistance : -dashDistance;

        //Calculate how far to travel
        _endLoc = new Vector2(_startLoc.x + dist, _startLoc.y);

        //And how fast to travel
        _dashDuration = dist / dashSpeed;
    }

    public void SetDash(Vector2 start, Vector2 end)
    {
        _startLoc = start;
        _endLoc = end;

        //And how fast to travel
        _dashDuration = Vector2.Distance(_startLoc, _endLoc) / dashSpeed;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Co_Dash()
    {
        SetDash(transform.position);
        _dashCounter = 0.0f;
        //_dashing = true;

        while (_dashCounter < _dashDuration)
        {
            transform.position = Vector2.Lerp(_startLoc, _endLoc, _dashCounter / _dashDuration);
            _dashCounter += Time.deltaTime;
            yield return null;
        }

        transform.position = _endLoc;

        //_dashing = false;
    }
}
