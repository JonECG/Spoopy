using UnityEngine;
using System.Collections;

public class HealthyLiving : MonoBehaviour {

    public delegate void NoHealthHandler();
    public event NoHealthHandler NoHealthEvent;

    public float healthRegenDuration = 10;
    public float invincibilityTime = 1;
    public float invincibilityThreshold = 0.1f;
    public float health 
    { 
        get { return _health; } 
        set 
        {
            if (timeInvincible > invincibilityTime)
            {
                changed = value < _health;
                if (_health - value > invincibilityThreshold)
                {
                    timeInvincible = 0;
                    GetComponent<SoundStatePlayer>().PlaySoundFrom("HurtGoofy");
                }
                _health = Mathf.Clamp(value, 0, 1);
                if (_health == 0 && NoHealthEvent != null)
                    NoHealthEvent();
            }
        } 
    }
    private float _health;
    private bool changed;
    private float timeInvincible;
	void Start () 
	{
        timeInvincible = invincibilityTime;
        changed = false;
        _health = 1;
	}
	
	void Update () 
	{
        timeInvincible += Time.deltaTime;
        if (!changed)
            health += Time.deltaTime / healthRegenDuration;
        changed = false;
	}
}
