using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    

    // Protected variables to modify
    [SerializeField] protected float maxSpeed = 0.5f;       // Si queremos que esté quieto, dejar a 0
    [SerializeField] protected float maxHealth = 5f;        // Si queremos que al mínimo golpe se muera, dejar a 0
    [SerializeField] protected float detectionRange = 3f;   // Si no queremos que vea al player, dejar a 0

    // Explosion variables for simulating dead
    protected float timeUntilExplosion = 0.5f;
    [SerializeField]
    protected GameObject explosion;
    protected float _counterUntilExplosion;

    // Protected objects
    protected Health p_health;
    protected Player p_player;
    protected Rigidbody p_rigidbody;
    protected MeshCollider p_meshCollider;

    // Protected variables
    protected bool p_isDead;

    protected Vector3 p_playerHeading;
    protected Vector3 p_playerDirection;
    protected float p_playerDistance;

    // Private "backup" variables
    [SerializeField]  protected Vector3 _respawnPoint;
    protected bool _respawnPointIsSet = false;
    protected Quaternion _initialRotation;

    protected float maxDrop;

    // Start is called before the first frame update
    protected virtual void Awake()
    {

        // Obtiene los componentes asignados al objeto
        p_rigidbody = GetComponent<Rigidbody>();
        p_meshCollider = GetComponent<MeshCollider>();

        p_health = new Health(maxHealth);


        p_isDead = false;

        p_player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if(!_respawnPointIsSet)
        {
            Debug.Log("Setting RESPAWN");
            _respawnPoint = transform.position;
            _initialRotation = transform.rotation;
            _respawnPointIsSet = true;
        }
        Debug.Log("Starting at " + _respawnPoint);
    }
    /**/

    public void setMaxDrop(float y) { maxDrop = y;  }

    public void respawn()
    {
         Debug.Log("RESPAWNING");
        //transform.position = _respawnPoint;
        transform.SetPositionAndRotation(_respawnPoint, _initialRotation);// = _initialRotation;
        gameObject.SetActive(true);
        Awake();
    }

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

        float playerDmg = p_player.getHitDamage();
        Debug.Log("Player speed: " + p_player.GetComponent<Rigidbody>().velocity.magnitude);
        Debug.Log("Player dmg: " + playerDmg);

        if (p_health.takeDamage(playerDmg) == false)
            // Its dead
            p_isDead = true;

        return p_isDead;
    }


    public bool isDead() { return p_isDead; }


    /// <summary>
    /// Looks for if player is between detection range and checks that there's no obstacles between.
    /// <para> Updates playerDistance and playerDirection variables from Enemy class. </para>
    /// </summary>
    /// <returns>True if the player is directly visible.</returns>
    private bool isPlayerVisible()
    { 
        RaycastHit playerHit;
        
        p_playerHeading = (p_player.transform.position - p_rigidbody.transform.position);
        p_playerDistance = p_playerHeading.magnitude;
        p_playerDirection = p_playerHeading / p_playerDistance;

        if (p_playerDistance < detectionRange)

            // Traza una recta hacia el player
            if (Physics.Raycast(transform.position, p_playerDirection, out playerHit, detectionRange))
                // Si esa recta no colisiona con el player, el player ya no está a la vista
                if (playerHit.collider.tag == "Player")
                    return true;

        return false;
    }

    /** Qué ocurre si está muerto. Si no ocurre nada, dejar la función vacía. */
    protected virtual void behaviourOnDead()
    {
        /** Qué ocurre si está muerto */

        _counterUntilExplosion += Time.deltaTime;

        if (_counterUntilExplosion < timeUntilExplosion)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    /** Cómo actua cuando se encuentra al player. Si no ocurre nada, dejar la función vacía. */
    protected virtual void behaviourWhenPlayerVisible()
    {

    }

    protected virtual void behaviourWhenPlayerNotVisible()
    {

    }


    protected virtual void Update()
    {
        /** Si el enemigo se ha caído, se muere. */
        if (transform.position.y < maxDrop)
        {
            Debug.Log("FUERA DE PISTA");
            p_isDead = true;
            gameObject.SetActive(false);
        }

        /** Si el enemigo sigue en la pista, llama a una función de actuación. */
        if (p_isDead) behaviourOnDead();
        else if (isPlayerVisible()) behaviourWhenPlayerVisible();
        else behaviourWhenPlayerNotVisible();

    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (p_isDead) return;

        if (collision.collider.tag == "Player")
        {
            /** Cómo actua cuando colisiona con el player */

            // Si fue golpeado por el player, recibe el daño sin condiciones
            if (recieveDamageFromPlayer())
                _counterUntilExplosion = 0;

        }

    }

}
