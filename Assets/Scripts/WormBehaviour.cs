using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBehaviour : Enemy
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

        dead = false;

        _timeRandomWalking = 0f;

        // Evita que rote
        _rigidbody.freezeRotation = true;


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
        // En caso de no estar muerto sigue su recorrido predefinido
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
}
