using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : Enemy
{
    public override void onHitActuation(Collision collision)
    {

        if (collision.collider.tag == "Player")
        {
            // Gets player script
            Player player = collision.gameObject.GetComponent<Player>();

            // Gets player's speed
            //float playerDmg = player.GetComponent<Rigidbody>().velocity.magnitude;
            float playerDmg = player.getHitDamage();
            Debug.Log("Player speed: " + player.GetComponent<Rigidbody>().velocity.magnitude);
            Debug.Log("Player dmg: " + playerDmg);

            if (_health.takeDamage(playerDmg) == false)
            {
                // Its dead
                dead = true;
                _boxCollider.material.bounciness = 0;
                _timeUntilExplosion = 0.5f;
            }

        }
        // Cuando toca el suelo, vuelve a saltar
        else if (collision.collider.tag == "Suelo")
        {
            Vector3 speeds = _rigidbody.velocity;
            speeds.y = 1.5f;
            _rigidbody.velocity = speeds;
        }
        else
        {
            _timeRandomWalking = 0f;
        }
    }
}
