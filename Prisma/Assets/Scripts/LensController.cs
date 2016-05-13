using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;

public class LensController : MonoBehaviour
{
    public struct LensMapping
    {
        public bool Active { get; set; }
        public bool Locked { get; set; }
        public GameObject Spectra { get; set; }
    }

    #region Public Variables

    public List<GameObject> _spectraList;

    #endregion

    #region Private Variables

    private Dictionary<ColorEnum, LensMapping> d_ColorInput;

    private SpectraDictionary m_SpectraDictionary;

    private bool _playerFocused;
    private GameObject _lens;
    private GameObject _currentSpectra;
    private ColorEnum _focusedColor;

    private bool _active;
    private bool _switchFocus;

    #endregion

    #region Properties

    

    #endregion


    // Use this for initialization
    void Start()
    {
        LoadColorDictionary();
        m_SpectraDictionary = GameObject.Find("SpectraDictionary").GetComponent<SpectraDictionary>();
        _spectraList = new List<GameObject>();
        _spectraList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        _active = false;
        _playerFocused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_active)
            ProcessInput();
    }

    #region Private Methods

    private void LoadColorDictionary()
    {
        d_ColorInput = new Dictionary<ColorEnum, LensMapping>();
        d_ColorInput.Add(ColorEnum.RED, new LensMapping { Active = false, Locked = false, Spectra = null });
        d_ColorInput.Add(ColorEnum.ORANGE, new LensMapping { Active = false, Locked = false, Spectra = null });
        d_ColorInput.Add(ColorEnum.YELLOW, new LensMapping { Active = false, Locked = false, Spectra = null });
        d_ColorInput.Add(ColorEnum.GREEN, new LensMapping { Active = false, Locked = false, Spectra = null });
        d_ColorInput.Add(ColorEnum.BLUE, new LensMapping { Active = false, Locked = false, Spectra = null });
        d_ColorInput.Add(ColorEnum.PURPLE, new LensMapping { Active = false, Locked = false, Spectra = null });
    }

    private void ResetColorDictionary()
    {
        d_ColorInput[ColorEnum.RED] = new LensMapping { Active = false, Locked = false, Spectra = null };
        d_ColorInput[ColorEnum.ORANGE] = new LensMapping { Active = false, Locked = false, Spectra = null };
        d_ColorInput[ColorEnum.YELLOW] = new LensMapping { Active = false, Locked = false, Spectra = null };
        d_ColorInput[ColorEnum.GREEN] = new LensMapping { Active = false, Locked = false, Spectra = null };
        d_ColorInput[ColorEnum.BLUE] = new LensMapping { Active = false, Locked = false, Spectra = null };
        d_ColorInput[ColorEnum.PURPLE] = new LensMapping { Active = false, Locked = false, Spectra = null };
    }

    private void MapSpectraColors()
    {
        Debug.Log("Mapping Colors");
        List<ColorEnum> lensColors = _lens.GetComponent<SpectraLens>().SpectraColors;
        ResetColorDictionary();

        foreach (ColorEnum ce in lensColors)
        {
            Debug.Log(string.Format("Color [{0}] active in lens", ce));
            LensMapping map = d_ColorInput[ce];
            map.Locked = true;
        }
    }

    private void ProcessInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToPlayer();
        }
        if(_switchFocus)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchSpectra(ColorEnum.RED);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchSpectra(ColorEnum.ORANGE);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchSpectra(ColorEnum.YELLOW);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchSpectra(ColorEnum.GREEN);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SwitchSpectra(ColorEnum.BLUE);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SwitchSpectra(ColorEnum.PURPLE);
            }
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            _switchFocus = !_switchFocus;

            if(_switchFocus)
            {
                CycleSpectra();
            }
            else
            {
                SelectSpectra();
            }
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

    private void CycleSpectra()
    {
        Debug.Log("Switching Spectra");
        Camera.main.orthographicSize = 10;
        if (_currentSpectra != null)
        {
            _currentSpectra.GetComponent<PlayerController>().Deactivate();
            Debug.Log(string.Format("Focusing on Spectra Color [{0}]", _focusedColor));
            SwitchSpectra(_currentSpectra.GetComponent<Spectra>().SpectraColor);
        }
        else
        {
            Debug.Log("Focusing on Player");
            SwitchToPlayer();
        }
    }

    private void SelectSpectra()
    {
        Camera.main.orthographicSize = 7;

        if (_playerFocused)
        {
            DeactivateLens();
            return;
        }

        LensMapping map = d_ColorInput[_focusedColor];

        if(map.Spectra != null)
        {
            _currentSpectra = map.Spectra;
        }
        else
        {
            GameObject newSpectra = m_SpectraDictionary.GetSpectra(_focusedColor);
            map.Spectra = (GameObject)Instantiate(newSpectra, _lens.transform.position, Quaternion.identity);
            _currentSpectra = map.Spectra;
            d_ColorInput[_focusedColor] = map;
            Debug.Log("Spectra created");
            Debug.Log(d_ColorInput[_focusedColor].Spectra);
        }

        _currentSpectra.GetComponent<PlayerController>().Activate();
        Camera.main.GetComponent<Camera2DFollow>().target = _currentSpectra.transform;
    }

    private void SwitchSpectra(ColorEnum color)
    {
        _focusedColor = color;
        LensMapping map = d_ColorInput[color];
        if(map.Equals(null))
        {
            Debug.Log(string.Format("ERROR: Color[{0}] does not exist in mapping", color));
            return;
        }
        if (map.Locked) return;
        if(map.Spectra != null)
        {
            GameObject focusSpectra = m_SpectraDictionary.GetSpectra(color);
            string spectraName = string.Format(focusSpectra.name + "(Clone)");
            Debug.Log(string.Format("Looking for Spectra with name [{0}]", spectraName));
            Camera.main.GetComponent<Camera2DFollow>().target = GameObject.Find(spectraName).transform;
        }
        else
        {
            Debug.Log(string.Format("No active spectra for color [{0}] found", color));
            Camera.main.GetComponent<Camera2DFollow>().target = _lens.transform;
        }
        _playerFocused = false;
    }

    private void SwitchSpectra(string name)
    {
        if(_currentSpectra != null)
            _currentSpectra.GetComponent<PlayerController>().Deactivate();

        foreach(GameObject go in _spectraList)
        {
            if(go.name == name)
            {
                _currentSpectra = go;
                break;
            }
        }

        _currentSpectra.GetComponent<PlayerController>().Activate();
        Camera.main.GetComponent<Camera2DFollow>().target = _currentSpectra.transform;
    }

    private void SwitchToPlayer()
    {
        if (_currentSpectra != null)
            _currentSpectra.GetComponent<PlayerController>().Deactivate();

        _playerFocused = true;

        _currentSpectra = GameObject.FindGameObjectWithTag("Player");
        Camera.main.GetComponent<Camera2DFollow>().target = _currentSpectra.transform;
    }

    private bool GetClickedObject()
    {
        //Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        return false;
    }

    private void DestroySpectra()
    {
        GameObject[] spectraToDestroy = GameObject.FindGameObjectsWithTag("Spectra");
        foreach(GameObject s in spectraToDestroy)
        {
            Destroy(s);
        }
    }

    #endregion

    #region Public Methods

    public void ActivateLens(GameObject lens)
    {
        Debug.Log("Activating Lens");
        _active = true;
        _lens = lens;
        _currentSpectra = null;
        MapSpectraColors();
        _switchFocus = true;
        CycleSpectra();
    }

    public void DeactivateLens()
    {
        Debug.Log("Deactivating Lens");
        ResetColorDictionary();
        DestroySpectra();
        _currentSpectra.GetComponent<PlayerController>().Activate();
        Camera.main.GetComponent<Camera2DFollow>().target = _currentSpectra.transform;
        _active = false;
        _lens = null;
        _currentSpectra = null;

    }

    #endregion
}
