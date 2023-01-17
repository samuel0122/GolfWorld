using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHead : WormBehaviour
{

    // Update is called once per frame
    public void CollisionWithObjects(Collision collision)
    {

        if (collision.collider.tag == "Player")
        {
            /** Cómo actua cuando colisiona con el player */

            Vector3 speeds = p_rigidbody.velocity;
            speeds.y = 1f;
            p_rigidbody.velocity = speeds;

        }
    }
}
