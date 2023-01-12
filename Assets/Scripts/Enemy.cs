using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float MaxSpeed;
    protected float Speed;
    public float MaxHealth;
    protected Collider[] hitColliders;
    protected RaycastHit Hit;

    protected Health _health;

    public float DetectionRange;

    public GameObject Target;
    protected Rigidbody _rigidbody;
    protected BoxCollider _boxCollider;
    protected float _timeRandomWalking;

    protected bool seePlayer;
    protected bool dead;
    protected float _timeUntilExplosion;

    [SerializeField]
    protected GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        // Obtiene los componentes asignados al objeto
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        _health = new Health(MaxHealth) ;

        // Asigna la velocidad de movimiento
        Speed = MaxSpeed;
        seePlayer = true;

        dead = false;

        _timeRandomWalking = 0f;

        // Evita que rote
        _rigidbody.freezeRotation = true;

    }
    /**/

    private bool playerIsVisible(Vector3 direction)
    {
        // Traza una recta hacia el player
        if (Physics.Raycast(transform.position, direction, out Hit, DetectionRange))
            // Si esa recta no colisiona con el player, el player ya no está a la vista
            if (Hit.collider.tag == "Player")
                return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {

        // Si está muerto, cuenta atrás para explotar
        if (dead)
        {
            _timeUntilExplosion -= Time.deltaTime;

            if (_timeUntilExplosion < 0f)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            return;
        }

        // Busca el player
        var Heading = (Target.transform.position - _rigidbody.transform.position);
        var Distance = Heading.magnitude;
        var Direction = Heading / Distance;

        if (Distance < 4 && playerIsVisible(Direction))
        {
            // Si tiene al player a la vista, huye
            _timeRandomWalking = 0f;

            Vector3 Move = new Vector3(-Direction.x * Speed, _rigidbody.velocity.y, -Direction.z * Speed);
            _rigidbody.velocity = Move;
            //transform.forward = Move;
        }
        else
        {
            // Cuenta cuanto tiempo lleva caminando en una dirección aleatoria
            _timeRandomWalking -= Time.deltaTime;

            // Si termina el contador, cambia de dirección
            if (_timeRandomWalking < 0f)
            {
                float speedX = Random.Range(0.2f, 0.35f) * Random.Range(0, 2) * 2 - 1;
                float speedZ = Random.Range(0.2f, 0.35f) * Random.Range(0, 2) * 2 - 1;

                _rigidbody.velocity = new Vector3(speedX, _rigidbody.velocity.y, speedZ);

                _timeRandomWalking = 3f;
            }
        }
    }
    /**/

    

    public abstract void onHitActuation(Collision collision);

    void OnCollisionEnter(Collision collision)
    {
        if (dead) return;

        onHitActuation(collision);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        // detect if player in range

        if (!seePlayer)
        {
            hitColliders = Physics.OverlapSphere(_rigidbody.transform.position, DetectionRange);
            foreach(var collider in hitColliders)
            {
                if(collider.tag == "Player")
                {
                    Target = collider.gameObject;
                    seePlayer = true;
                }
            }
        }
        else
        {
            var Heading = (Target.transform.position - _rigidbody.transform.position);
            var Distance = Heading.magnitude;
            var Direction = Heading / Distance;

            // Traza una recta hacia el enemigo
            if (Physics.Raycast(transform.position, Direction, out Hit, SightRange))
            {
                // Si esa recta no colisiona con el player, el player ya no está a la vista
                if(Hit.collider.tag != "Player")
                {
                    seePlayer = false;
                } 
                else
                {
                    // Si tiene al player a la vista, huye
                    

                    Vector3 Move = new Vector3(-Direction.x * Speed, 0, -Direction.z * Speed);
                    _rigidbody.velocity = Move;
                    transform.forward = Move;
                }
            }
        }
    }*/
}
