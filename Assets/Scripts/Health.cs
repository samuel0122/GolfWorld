using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    private float _currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = MaxHealth;
    }

    // Takes damage to the health. Returns true if its not yet dead
    public bool takeDamage(float damage)
    {
        _currentHealth -= damage;
        return _currentHealth > 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
