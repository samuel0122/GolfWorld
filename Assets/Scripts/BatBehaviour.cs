using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : Enemy
{


    public float flyHeight;

    private float initialPosY;

    protected override void Awake()
    {
        base.Awake();
        initialPosY = transform.position.y;
    }

    private void goUpDown()
    {

        float newY = Mathf.Sin(Time.time * maxSpeed) * flyHeight + initialPosY;

        //get the objects current position and put it in a variable so we can access it later with less code
        //calculate what the new Y position will be
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    
    protected override void behaviourWhenPlayerVisible()
    {
        transform.LookAt(p_player.transform.position);
        transform.Rotate(0, 90, 0);

        goUpDown();
    }

    protected override void behaviourWhenPlayerNotVisible()
    {

        goUpDown();

    }

}
