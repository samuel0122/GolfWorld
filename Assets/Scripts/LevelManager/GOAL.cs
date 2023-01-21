using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GOAL : MonoBehaviour {

    //public GameObject level_complete;

    private int currentLevel;
    private int LevelAmount = 50;

    public Level LevelController;

    void Start ()
    {
        Time.timeScale = 1;
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //level_complete.SetActive(false);


    }

    void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Player"))
        {
            Debug.Log("###PLAYER REACTED");
            
            //level_complete.SetActive(true);

            LevelController.levelGoalReached(currentLevel);
            //CheckLevel();
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
