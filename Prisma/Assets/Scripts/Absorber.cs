using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Absorber : ActivatableObject {

    public Material linkMaterial;
    public bool rotatingConnection;
    public List<GameObject> linkedAbsorbers;

    private Dictionary<GameObject, LineRenderer> _links;

	// Use this for initialization
	void Start () {
        _links = new Dictionary<GameObject, LineRenderer>();
	    foreach(GameObject link in linkedAbsorbers)
        {
            CreateLink(link);
        }

        HideLinks();
        SetInitialState();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateLink(GameObject other)
    {
        GameObject lineEmpty = new GameObject();
        LineRenderer line = lineEmpty.AddComponent<LineRenderer>();
        line.SetVertexCount(2);
        line.material = linkMaterial;
        line.SetColors(Color.blue, Color.blue);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, other.transform.position);
        line.SetWidth(0.2f, 0.2f);
        lineEmpty.transform.parent = gameObject.transform;
        _links.Add(other, line);

    }

    public void DisplayLinks()
    {
        foreach(KeyValuePair<GameObject, LineRenderer> pair in _links)
        {
            Absorber other = pair.Key.GetComponent<Absorber>();
            LineRenderer link = pair.Value;

            if (other.Active) link.enabled = true;
        }
    }

    public void HideLinks()
    {
        foreach (KeyValuePair<GameObject, LineRenderer> pair in _links)
        {
            LineRenderer link = pair.Value;

            link.enabled = false;
        }
    }

    public override void Activate()
    {
        base.Activate();
        GetComponent<SpriteRenderer>().color = new Color(0.06f, 0f, 0.79f, 1.0f);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GetComponent<SpriteRenderer>().color = new Color(0.06f, 0f, 0.79f, 0.5f);
    }
}
