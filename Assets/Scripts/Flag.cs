using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _initialPosition;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _initialPosition = _transform.position;
    }

    public Vector3 getPosition()
    {
        return _transform.position;
    }

    public void moveTo(Vector3 position)
    {
        position.y += _initialPosition.y;
        _transform.position = position;
    }

    public void resetFlagPosition()
    {
        _transform.position = _initialPosition;
    }


}
