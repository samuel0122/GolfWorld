using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public LoadingManager LoadingManager;
    public GameObject pauseMenuUI;
    /*
    public GameObject mainGame;
    public GameObject loadingScreen;
    public Slider progressSlider;
    private float loadingTimer;
    */
    private int currentLevel;

    private void Start()
    {
        // loadingTimer = 0;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

   public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPause = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPause = false;

    }

    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPause = false;

        LoadingManager.LoadLevel(currentLevel, "RESTARTING LEVEL");
        //SceneManager.LoadScene(currentLevel);
        // StartCoroutine(LoadSceneAsync(currentLevel));
        //StartCoroutine(Reload(currentLevel));
    }

 

    public void GoBackToMainMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPause = false;

        LoadingManager.LoadLevel(0, "GOING BACK TO MAIN MENU");


        //mainGame.SetActive(false);
        //SceneManager.LoadScene(0);
        //StartCoroutine(LoadSceneAsync(0));
    }

    /*
    private IEnumerator Reload(int sceneId)
    {
        yield return new WaitForSeconds(6);
        SceneManager.UnloadSceneAsync(sceneId);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(sceneId, LoadSceneMode.Single);
    }

    public IEnumerator LoadSceneAsync(int sceneId)
    {
        mainGame.SetActive(false);

        progressSlider.value = 0;
        loadingScreen.SetActive(true);

        loadingTimer += Time.deltaTime;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId);
        asyncOperation.allowSceneActivation = false;

        float progressValue = 0;
        while (!asyncOperation.isDone || loadingTimer < 5)
        {
            // float progressValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressValue = Mathf.MoveTowards(progressValue, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progressValue;

            if (progressValue >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    */
}
