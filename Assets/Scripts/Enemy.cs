using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float MaxSpeed;
    protected float Speed;

    public float MaxHealth;

    protected Collider[] hitColliders;
    

    protected Health _health;

    public float DetectionRange;

    protected GameObject _playerTarget;
    private Player player;
    protected Rigidbody _rigidbody;
    protected BoxCollider _boxCollider;

    protected bool seePlayer;
    protected bool dead;

    protected Vector3 playerDirection;
    protected float playerDistance;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Obtiene los componentes asignados al objeto
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        _health = new Health(MaxHealth);

        // Asigna la velocidad de movimiento
        Speed = MaxSpeed;
        seePlayer = false;

        dead = false;

        _playerTarget = GameObject.FindGameObjectsWithTag("Player")[0];
        player = _playerTarget.gameObject.GetComponent<Player>();
    }
    /**/


    /// <summary>
    /// Recieves the player's hit, lowering the life points.
    /// </summary>
    /// <returns> True if enemy is dead </returns>
    protected bool recieveDamageFromPlayer()
    {
        // Gets player script
        //Player player = collision.gameObject.GetComponent<Player>();

        // Gets player's speed
        //float playerDmg = player.GetComponent<Rigidbody>().velocity.magnitude;

        float playerDmg = player.getHitDamage();
        Debug.Log("Player speed: " + player.GetComponent<Rigidbody>().velocity.magnitude);
        Debug.Log("Player dmg: " + playerDmg);

        if (_health.takeDamage(playerDmg) == false)
            // Its dead
            dead = true;

        return dead;
    }


    /// <summary>
    /// Looks for if player is between detection range and checks that there's no obstacles between.
    /// <para> Updates playerDistance and playerDirection variables from Enemy class. </para>
    /// </summary>
    /// <returns>True if the player is directly visible.</returns>
    protected bool isPlayerVisible()
    { 
        RaycastHit playerHit;
        
        Vector3 Heading = (_playerTarget.transform.position - _rigidbody.transform.position);
        playerDistance = Heading.magnitude;
        playerDirection = Heading / playerDistance;

        if (playerDistance < DetectionRange)

            // Traza una recta hacia el player
            if (Physics.Raycast(transform.position, playerDirection, out playerHit, DetectionRange))
                // Si esa recta no colisiona con el player, el player ya no está a la vista
                if (playerHit.collider.tag == "Player")
                    return true;

        return false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        
    }
    /**/

    

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (dead) return;

    }

}
