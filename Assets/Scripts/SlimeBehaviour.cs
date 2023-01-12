using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : Enemy
{
    // More variables for slime
    static float timeUntilExplosion = 0.5f;

    [SerializeField]
    protected GameObject explosion;

    protected float _counterUntilExplosion;
    protected float _timeRandomWalking;

    // Overriding function
    protected override void Start()
    {
        base.Start();
        _timeRandomWalking = 0f;
        
        // Evita que rote (buscar alternativa para permitir que rote de izquierda a derecha)
        _rigidbody.freezeRotation = true;

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (dead) return;

        if (collision.collider.tag == "Player")
        {
            /** Cómo actua cuando colisiona con el player */

            // Si fue golpeado por el player, recibe el daño sin condiciones
            if (recieveDamageFromPlayer())
                _boxCollider.material.bounciness = 0;

        }
        else if (collision.collider.tag == "Suelo")
        {
            /** Cómo actua cuando toca el suelo*/

            // Cuando toca el suelo, vuelve a saltar
            Vector3 speeds = _rigidbody.velocity;
            speeds.y = 1.5f;
            _rigidbody.velocity = speeds;
        }
        else
        {
            // Si colisiona con algo y no es el suelo ni el player, entonces será la pared. 
            // Reinicia el contador de rangomWalking para cambiar su dirección aleatoria.
            _timeRandomWalking = 0f;
        }
    }

    protected override void Update()
    {
        // Si está muerto, cuenta atrás para explotar
        if (dead)
        {
            /** Qué ocurre si está muerto */
            _counterUntilExplosion += Time.deltaTime;

            if (_counterUntilExplosion < timeUntilExplosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            return;
        }

        
        if (isPlayerVisible())
        {
            /** Cómo actua cuando se encuentra al player */
            // Si tiene al player a la vista, huye
            _timeRandomWalking = 0f;

            Vector3 Move = new Vector3(-playerDirection.x * Speed, _rigidbody.velocity.y, -playerDirection.z * Speed);
            _rigidbody.velocity = Move;
            //transform.forward = Move;
        }
        else
        {
            /** Cómo actua cuando se está por libre */

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
