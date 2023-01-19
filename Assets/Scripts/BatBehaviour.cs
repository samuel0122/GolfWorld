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

    protected override void behaviourWhenPlayerNotVisible()
    {


        float newY = Mathf.Sin(Time.time * maxSpeed) * flyHeight + initialPosY;

        //get the objects current position and put it in a variable so we can access it later with less code
        //calculate what the new Y position will be
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

    }

}
