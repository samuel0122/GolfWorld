using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTail : WormBehaviour
{

    // Update is called once per frame
    public void CollisionWithObjects(Collision collision)
    {

        if (collision.collider.tag == "Player")
        {
            recieveDamageFromPlayer();
        }
    }
}
