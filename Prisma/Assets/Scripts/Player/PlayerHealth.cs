using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public Text healthText;

    public float maxHealth;
    public float healthDecay;
    public float healthRegen;
    public bool stasis;
    public bool healing;

    public float _currentHealth;

	// Use this for initialization
	void Start () {
        _currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (!stasis) DecayHealth();
        if (healing) RegenerateHealth();

        healthText.text = Mathf.Floor(_currentHealth).ToString();
	}

    private void DecayHealth()
    {
        if(_currentHealth > 0)
            _currentHealth -= healthDecay * Time.deltaTime;
        if (_currentHealth < 0) _currentHealth = 0;
    }

    private void RegenerateHealth()
    {
        if(_currentHealth < maxHealth)
            _currentHealth += healthRegen * Time.deltaTime;
        if (_currentHealth > maxHealth) _currentHealth = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Light")
        {
            stasis = true;
            healing = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Light")
        {
            stasis = false;
            healing = false;
        }
    }

}
