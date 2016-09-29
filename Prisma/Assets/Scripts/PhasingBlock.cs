using UnityEngine;
using System.Collections;

public class PhasingBlock : MonoBehaviour {

    public Transform block;
    public Transform startPosition;
    public Transform endPosition;

    private bool _atStart;

    private bool _movable;

    // Use this for initialization
    void Start()
    {

        _atStart = false;
        MoveToPosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToPosition()
    {
        block.position = _atStart ? endPosition.position : startPosition.position;

        _atStart = !_atStart;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.8f, 0);
        Gizmos.DrawLine(startPosition.position, endPosition.position);
    }
}
