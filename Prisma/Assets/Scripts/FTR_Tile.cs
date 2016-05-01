using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FTR_Tile : MonoBehaviour {

    private FTRTileTypeEnum e_type;
    private ColorEnum e_color;

    private List<ColorEnum> _colorSources;

    public FTRTileTypeEnum Type
    {
        get { return e_type; }
        set { e_type = value; }
    }

    public ColorEnum EColor
    {
        get { return e_color; }
        set { e_color = value; }
    }

	// Use this for initialization
	void Start () {
        _colorSources = new List<ColorEnum>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetLighting()
    {
        CalculateColor();
        LightTile();
    }

    private void CalculateColor()
    {
        bool r = false, y = false, b = false;

        foreach (ColorEnum color in _colorSources)
        {
            if (!r) r = (color == ColorEnum.RED);
            if (!y) y = (color == ColorEnum.YELLOW);
            if (!b) b = (color == ColorEnum.BLUE);
        }

        if (r && y)
        {
            e_color = ColorEnum.ORANGE;
            return;
        }
        if (y && b)
        {
            e_color = ColorEnum.GREEN;
            return;
        }
        if (r && b)
        {
            e_color = ColorEnum.PURPLE;
            return;
        }
        if (r && y && b)
        {
            e_color = ColorEnum.NONE;
            return;
        }

        if (r) e_color = ColorEnum.RED;
        if (y) e_color = ColorEnum.YELLOW;
        if (b) e_color = ColorEnum.BLUE;
    }

    private void LightTile()
    {
        switch (e_color)
        {
            case ColorEnum.RED:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case ColorEnum.ORANGE:
                GetComponent<SpriteRenderer>().color = new Color(255, 165, 0);
                break;
            case ColorEnum.YELLOW:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case ColorEnum.GREEN:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case ColorEnum.BLUE:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case ColorEnum.PURPLE:
                GetComponent<SpriteRenderer>().color = new Color(160, 32, 240);
                break;
            default:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }
    }

    public void SetCrystal(ColorEnum color)
    {
        e_type = FTRTileTypeEnum.CRYSTAL;
        e_color = color;
        LightTile();
    }

    public void RemoveCrystal()
    {
        e_type = FTRTileTypeEnum.CLEAR;
        e_color = ColorEnum.NONE;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Clear()
    {
        e_type = FTRTileTypeEnum.CLEAR;
        e_color = ColorEnum.NONE;
        _colorSources.Clear();
        LightTile();
    }

    public void AddSource(ColorEnum c)
    {
        _colorSources.Add(c);
    }

    public void RemoveSource(ColorEnum c)
    {
        foreach (ColorEnum color in _colorSources)
        {
            if (color == c)
            {
                _colorSources.Remove(color);
                break;
            }
        }
    }
}
