using UnityEngine;
using System.Collections;

public class OpticIndicator : MonoBehaviour {

    public ColorEnum opticColor;
    public Color activeColor;

    private OpticCapture _opticCapture;

	// Use this for initialization
	void Start () {
        _opticCapture = transform.parent.GetComponent<OpticCapture>();
        SetActive();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetActive()
    {
        if(_opticCapture.opticColors.Contains(opticColor))
        {
            GetComponent<SpriteRenderer>().color = activeColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
