using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GOAL : MonoBehaviour {

    public GameObject level_complete;

    public int unlock;

    void Start ()
    {
        Time.timeScale = 1;

        unlock = SceneManager.GetActiveScene().buildIndex + 1;

        level_complete.SetActive(false);


    }

    void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Player"))
        {

            
            PlayerPrefs.SetInt("levelReached", unlock);
            level_complete.SetActive(true);


        }
    }
}
