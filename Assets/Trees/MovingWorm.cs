using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;

[System.Serializable]
public class MovingWorm : ActionNode
{
    
    [SerializeField] GameObject posA;
    [SerializeField] GameObject posB;
    [SerializeField] GameObject worm;

    protected override void OnStart() {

        posA = GameObject.FindGameObjectWithTag("PosAWorm1");
        posB = GameObject.FindGameObjectWithTag("PosBWorm1");

        worm = GameObject.FindGameObjectWithTag("Worm1");
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        Vector3 pos = new Vector3();

        if (worm.transform.position.x == posA.transform.position.x && worm.transform.position.y == posA.transform.position.y)
        {
            pos.x = posA.transform.position.x;
            pos.y = posA.transform.position.y;
            blackboard.moveToPosition = pos;

        } else if (worm.transform.position.x == posB.transform.position.x && worm.transform.position.y == posB.transform.position.y) 
        {
            pos.x = posA.transform.position.x;
            pos.y = posA.transform.position.y;
            blackboard.moveToPosition = pos;
        }

        return State.Success;
    }
}
