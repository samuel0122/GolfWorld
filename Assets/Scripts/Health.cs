using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private float _maxHealth;
    private float _currentHealth;

    public Health(float health)
    {
        _maxHealth = health;
        _currentHealth = health;
    }

    // Takes damage to the health. Returns true if its not yet dead
    public bool takeDamage(float damage)
    {
        _currentHealth -= damage;
        return _currentHealth > 0f;
    }

    // Resets the current health
    public void resetCurrentHealth()
    {
        _currentHealth = _maxHealth;
    }
}
