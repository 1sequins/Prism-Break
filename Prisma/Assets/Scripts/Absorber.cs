using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Absorber : MonoBehaviour {

    public Material linkMaterial;
    public List<GameObject> linkedAbsorbers;

    private List<LineRenderer> _links;

	// Use this for initialization
	void Start () {
        _links = new List<LineRenderer>();
	    foreach(GameObject link in linkedAbsorbers)
        {
            CreateLink(link);
        }

        HideLinks();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateLink(GameObject other)
    {
        GameObject lineEmpty = new GameObject();
        LineRenderer line = lineEmpty.AddComponent<LineRenderer>();
        Debug.Log("Line added");
        line.SetVertexCount(2);
        line.material = linkMaterial;
        line.SetColors(Color.green, Color.green);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, other.transform.position);
        line.SetWidth(0.2f, 0.2f);
        lineEmpty.transform.parent = gameObject.transform;
        _links.Add(line);

    }

    public void DisplayLinks()
    {
        foreach(LineRenderer link in _links)
        {
            link.enabled = true;
        }
    }

    public void HideLinks()
    {
        Debug.Log("Hiding links");
        foreach (LineRenderer link in _links)
        {
            link.enabled = false;
        }
    }
}
