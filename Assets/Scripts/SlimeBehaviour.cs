using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : Enemy
{
    
    protected float _timeRandomWalking;

    // Slime Mesh
    private float Intensity = 1f;
    private float Mass = 1f;
    private float stiffness = 0.5f;
    private float damping = 0.8f;

    private Mesh OriginalMesh, MeshClone;
    private new MeshRenderer renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;


    // Overriding function
    protected override void Awake()
    {
        base.Awake();
        _timeRandomWalking = 0f;

        Debug.Log("AWAKING");

        // Evita que rote (buscar alternativa para permitir que rote de izquierda a derecha)
        //_rigidbody.freezeRotation = true;

        // Slime Mesh
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = GetComponent<MeshRenderer>();
        jv = new JellyVertex[MeshClone.vertices.Length];

        for (int iteration = 0; iteration < MeshClone.vertices.Length; iteration++)
            jv[iteration] = new JellyVertex(iteration, transform.TransformPoint(MeshClone.vertices[iteration]));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vertexArray = OriginalMesh.vertices;

        for (int iteration = 0; iteration < jv.Length; iteration++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[iteration].ID]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;
            jv[iteration].Shake(target, Mass, stiffness, damping);
            target = transform.InverseTransformPoint(jv[iteration].Position);
            vertexArray[jv[iteration].ID] = Vector3.Lerp(vertexArray[jv[iteration].ID], target, intensity);

        }

        MeshClone.SetVertices(vertexArray);
    }



    protected override void behaviourWhenPlayerVisible()
    {
        /** Cómo actua cuando se encuentra al player */
        transform.LookAt(p_player.transform.position);
        //transform.rotation = Quaternion.LookRotation(- transform.position + p_player.transform.position);

        // Si tiene al player a la vista, huye
        _timeRandomWalking = 0.5f;

        Vector3 Move = new Vector3(-p_playerDirection.x * maxSpeed, p_rigidbody.velocity.y, -p_playerDirection.z * maxSpeed);
        p_rigidbody.velocity = Move;
        //transform.forward = Move;
    }

    protected override void behaviourWhenPlayerNotVisible()
    {
        /** Cómo actua cuando se está por libre */
        transform.LookAt(2* transform.position  - p_player.transform.position);
        //transform.rotation = Quaternion.LookRotation(transform.position - p_player.transform.position);

        // Cuenta cuanto tiempo lleva caminando en una dirección aleatoria
        _timeRandomWalking -= Time.deltaTime;

        // Si termina el contador, cambia de dirección
        if (_timeRandomWalking <= 0f)
        {
            float speedX = Random.Range(0.2f, 0.35f) * (Random.Range(0, 2) * 2 - 1);
            float speedZ = Random.Range(0.2f, 0.35f) * (Random.Range(0, 2) * 2 - 1);

            p_rigidbody.velocity = new Vector3(speedX, p_rigidbody.velocity.y, speedZ);
            //_rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, Random.Range(0.2f, 0.35f) * (Random.Range(0, 2) * 2 - 1));

            _timeRandomWalking = 3f;
        }
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        if (p_isDead) return;

        if (collision.collider.tag == "Player")
        {
            /** Cómo actua cuando colisiona con el player */

            // Si fue golpeado por el player, recibe el daño sin condiciones
            if (recieveDamageFromPlayer())
                p_meshCollider.material.bounciness = 0;

        }
        else if (collision.collider.tag == "Wall")
        {
            /** Cómo actua cuando toca la pared */
            // Si colisiona con algo y no es el suelo ni el player, entonces será la pared. 
            // Reinicia el contador de rangomWalking para cambiar su dirección aleatoria.
            _timeRandomWalking = 0f;

        }
        else if (collision.collider.tag == "Floor" || collision.collider.tag == "Hole")
        {
            // Si colisiona con algo y no es el suelo ni el player, entonces será la pared. 
            // Reinicia el contador de rangomWalking para cambiar su dirección aleatoria.
            /** Cómo actua cuando toca el suelo*/

            // Cuando toca el suelo, vuelve a saltar
            Vector3 speeds = p_rigidbody.velocity;
            speeds.y = 1.5f;
            p_rigidbody.velocity = speeds;
        }
    }



    private class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;

        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;

            if ((velocity + Force + Force / m).magnitude < 0.001f)
            {
                Position = target;
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
