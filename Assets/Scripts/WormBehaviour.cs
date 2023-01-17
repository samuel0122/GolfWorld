using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WormBehaviour : Enemy
{

    // More variables for slime
    static float timeUntilExplosion = 0.5f;

    [SerializeField]
    protected GameObject explosion;

    protected float _counterUntilExplosion;
    protected float _timeRandomWalking;

    private Mesh OriginalMesh, MeshClone;
    private new MeshRenderer renderer;
    private Vector3[] vertexArray;
    private Vector3 target;

    WormHead headWorm;
    WormTail tailWorm;

    public int beginsize = 2;

    // Start is called before the first frame update
    // Overriding function
    protected override void Awake()
    {
        headWorm = new WormHead();
        tailWorm = new WormTail();

        // Obtiene los componentes asignados al objeto
        p_rigidbody = GetComponent<Rigidbody>();
        p_meshCollider = GetComponent<MeshCollider>();

        p_health = new Health(maxHealth);


        p_isDead = false;

        p_player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if (!_respawnPointIsSet)
        {
            Debug.Log("Setting RESPAWN");
            _respawnPoint = transform.position;
            _initialRotation = transform.rotation;
            _respawnPointIsSet = true;
        }
        Debug.Log("Starting at " + _respawnPoint);


        maxSpeed = 0.8f;
        maxHealth = 8f;
        detectionRange = 0f;
    }

    protected void FixedUpdate()
    {
        if(isDead() == true)
        {
            behaviourOnDead();
        }

    }

    protected override void behaviourOnDead()
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

    protected override void OnCollisionEnter(Collision collision)
    {
        if (p_isDead) return;

        if (collision.collider.tag == "Player")
        {
            headWorm.CollisionWithObjects(collision);
            tailWorm.CollisionWithObjects(collision);
        }
    }
}
