using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;

public class FTRController : MonoBehaviour {

    public ColorEnum currentColor;

    private bool _active;
    private bool _solved;
    private GameObject _player;

    private List<Vector2> position;
    private List<GameObject> tiles;
    private List<GameObject> crystals;

	// Use this for initialization
	void Start () {
        tiles = new List<GameObject>();
        tiles.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
        crystals = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if(_active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Deactivate();
            }

            if (Input.GetMouseButtonDown(0))
            {
                GetTileClick();

            }
        }
    }

    private void PlaceCrystal(FTR_Tile tile)
    {
        if(tile.Type == FTRTileTypeEnum.CLEAR)
        {
            tile.SetCrystal(currentColor);
            crystals.Add(tile.gameObject);
        }
        else if(tile.Type == FTRTileTypeEnum.CRYSTAL)
        {
            tile.RemoveCrystal();
            crystals.Remove(tile.gameObject);
        }

        UpdateLighting();
    }

    private void UpdateLighting()
    {
        Debug.Log("Updating Lighting");
        //Clear lit tiles
        foreach(GameObject t in tiles)
        {
            FTR_Tile tile = t.GetComponent<FTR_Tile>();
            if (tile.Type == FTRTileTypeEnum.LIT)
                tile.Clear();
        }

        //Calculate potential colors for tiles
        foreach(GameObject c in crystals)
        {
            Debug.Log("Calculating Crystal");
            PropagateSource(c, c.GetComponent<FTR_Tile>().EColor, "up");
            PropagateSource(c, c.GetComponent<FTR_Tile>().EColor, "down");
            PropagateSource(c, c.GetComponent<FTR_Tile>().EColor, "left");
            PropagateSource(c, c.GetComponent<FTR_Tile>().EColor, "right");
            //TODO: Propagate other sides
        }

        foreach(GameObject t in tiles)
        {
            t.GetComponent<FTR_Tile>().SetLighting();
        }
    }

    private void PropagateSource(GameObject tile,  ColorEnum color, string direction)
    {
        Vector2 start = tile.transform.position;
        Vector2 dir = new Vector2();
        switch(direction.ToLower())
        {
            case "up":
                dir.y = 0.9f;
                break;
            case "down":
                dir.y = -0.9f;
                break;
            case "left":
                dir.x = -0.9f;
                break;
            case "right":
                dir.x = 0.9f;
                break;
        }

        Vector2 end = start + dir;
        
        //Disable so raycast doesn't hit origin tile
        tile.GetComponent<BoxCollider2D>().enabled = false;

        RaycastHit2D hit = Physics2D.Linecast(start, end);
        
        tile.GetComponent<BoxCollider2D>().enabled = true;

        if (hit.transform == null) return;
        FTR_Tile s_tile = hit.transform.gameObject.GetComponent<FTR_Tile>();
        if (s_tile == null) return;
        if (s_tile.Type == FTRTileTypeEnum.WALL || s_tile.Type == FTRTileTypeEnum.CRYSTAL) return;

        GameObject nextTile = hit.transform.gameObject;

        s_tile.Type = FTRTileTypeEnum.LIT;
        s_tile.AddSource(color);
        //TODO: Recurse
        PropagateSource(nextTile, color, direction);
    }

    private void CleanSource(GameObject tile, ColorEnum color, string direction, bool clean)
    {
        Vector2 start = tile.transform.position;
        Vector2 dir = new Vector2();
        switch (direction.ToLower())
        {
            case "up":
                dir.y = 0.9f;
                break;
            case "down":
                dir.y = -0.9f;
                break;
            case "left":
                dir.x = -0.9f;
                break;
            case "right":
                dir.x = 0.9f;
                break;
        }

        Vector2 end = start + dir;

        //Disable so raycast doesn't hit origin tile
        tile.GetComponent<BoxCollider2D>().enabled = false;

        RaycastHit2D hit = Physics2D.Linecast(start, end);

        tile.GetComponent<BoxCollider2D>().enabled = true;

        if (hit.transform == null) return;
        FTR_Tile s_tile = hit.transform.gameObject.GetComponent<FTR_Tile>();
        if (s_tile == null) return;
        if (s_tile.Type == FTRTileTypeEnum.WALL || s_tile.Type == FTRTileTypeEnum.CRYSTAL) return;

        GameObject nextTile = hit.transform.gameObject;

        s_tile.Type = FTRTileTypeEnum.LIT;
        s_tile.AddSource(color);
        //TODO: Recurse
        PropagateSource(nextTile, color, direction);
    }

    public void GetTileClick()
    {
        Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit == null) return;
        FTR_Tile tile = hit.gameObject.GetComponent<FTR_Tile>();
        if (tile == null) return;

        PlaceCrystal(tile);
    }

    public void Activate(GameObject player)
    {
        _player = player;
        player.GetComponent<PlayerController>().Deactivate();
        _active = true;
        Camera.main.GetComponent<Camera2DFollow>().target = transform.FindChild("FocalPoint").transform;
    }

    public void Deactivate()
    {
        _player.GetComponent<PlayerController>().Activate();
        Camera.main.GetComponent<Camera2DFollow>().target = _player.transform;
        _active = false;
    }
}
