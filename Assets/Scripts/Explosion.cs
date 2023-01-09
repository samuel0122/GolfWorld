using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Destruye el objeto en 0.5 segundos
        Destroy(gameObject, 5f);
    }
}
