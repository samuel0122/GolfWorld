using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    public float MaxSpeed;
    private float Speed;

    private Collider[] hitColliders;
    private RaycastHit Hit;

    private Health _health;

    public float DetectionRange;

    public GameObject Target;
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private float _timeRandomWalking;

    private bool seePlayer;
    private bool dead;
    private float _timeUntilExplosion;

    [SerializeField]
    private GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        // Obtiene los componentes asignados al objeto
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _health = GetComponent<Health>();

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
            if(_timeRandomWalking < 0f)
            {
                float speedX = Random.Range(0.2f, 0.35f) * Random.Range(0, 2) * 2 - 1;
                float speedZ = Random.Range(0.2f, 0.35f) * Random.Range(0, 2) * 2 - 1;
                
                _rigidbody.velocity = new Vector3(speedX, _rigidbody.velocity.y, speedZ);

                _timeRandomWalking = 3f;
            }
        }
    }
    /**/
    


    private void OnCollisionEnter(Collision collision)
    {
        if (dead) return;

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
