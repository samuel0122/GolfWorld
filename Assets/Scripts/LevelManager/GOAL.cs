using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GOAL : MonoBehaviour {

    //public GameObject level_complete;

    public int unlock;
    int LevelAmount = 50;
    private int currentLevel;

    void Start ()
    {
        Time.timeScale = 1;
        unlock = SceneManager.GetActiveScene().buildIndex + 1;

        //level_complete.SetActive(false);
        Debug.Log("STARTING LEVEL @@@");


    }

    void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Player"))
        {
            Debug.Log("###PLAYER REACTED");
            
            PlayerPrefs.SetInt("levelReached", unlock);
            //level_complete.SetActive(true);

            CheckLevel();
        }
    }

    void CheckLevel()
    {
        for (int i = 1; i <= LevelAmount; i++)
        {
            if (SceneManager.GetActiveScene().name == "Level" + i)
            {
                currentLevel = i;
                SaveMyGame();
            }
        }
    }

    void SaveMyGame()
    {
        int NextLevel = currentLevel + 1;
        if (NextLevel <= LevelAmount)
        {
            Debug.Log("Save Level" + NextLevel.ToString());
            PlayerPrefs.SetInt("Level" + NextLevel.ToString(), 1);
        }

        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        // AdManager.Instance.bannerad.Destroy();
        //LevelScreenesManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
