using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelScreenesManager : MonoBehaviour {

    public GameObject loadingScreen;
    float loadingTimer;
    public LoadingManager loadingManager;

    [System.Serializable]
    public class LevelBut
    {
        public Sprite levelImage;
    }

    public Slider progressSlider;
    public GameObject LevelButtonPrefab;
    public Transform Holder;
    public List<LevelBut> LevelButtonsList;
    
    private int levelReached;

    // Use this for initialization
    void Start ()
    {
        loadingTimer = 0;

        levelReached = PlayerPrefs.GetInt("levelReached", 1);

        Delete();
        FillList();

        PlayerPrefs.SetInt("levelReached", levelReached);

        Debug.Log("Level reached: " + levelReached);
    }

    void FillList()
    {
        int iteration = 0;
        foreach(var level in LevelButtonsList)
        {
            ++iteration;

            GameObject newbutton = Instantiate(LevelButtonPrefab);
            levelButtons button = newbutton.GetComponent<levelButtons>();
            
            newbutton.GetComponent<Image>().sprite = level.levelImage;

            Debug.Log("Level unlocked: " + iteration.ToString() + " " + levelReached.ToString());

            // button.LevelText.text =  level.LevelText;
            if(iteration <= levelReached)
            {
                // LEVELS UNLOCKED
                button.levelName = "Level" + iteration.ToString();
                button.GetComponent<Button>().interactable = true;
                button.GetComponent<Button>().onClick.AddListener(() => loadingManager.LoadLevel(button.levelName));

            }
            else
            {
                // LEVELS LOCKED
                button.GetComponent<Button>().interactable = false;
                button.GetComponent<Button>().transition = Selectable.Transition.ColorTint;

            }


            newbutton.transform.SetParent(Holder);
        }
        // SAVE();
    }
    void SAVE()
    {
        {
            GameObject[] allbuttons = GameObject.FindGameObjectsWithTag("LevelButton");
            foreach (GameObject buttons in allbuttons)
            {
                levelButtons button = buttons.GetComponent<levelButtons>();
                // PlayerPrefs.SetInt("Level" + button.levelName, button.unlocked);
            }
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
    }




    public IEnumerator LoadSceneAsync(string sceneId)
    {
        progressSlider.value = 0;
        loadingScreen.SetActive(true);

        loadingTimer += Time.deltaTime;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId);
        asyncOperation.allowSceneActivation = false;

        float progressValue = 0;
        while (!asyncOperation.isDone || Time.timeSinceLevelLoad < 5)
        {
            // float progressValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressValue = Mathf.MoveTowards(progressValue, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progressValue;

            if(progressValue >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public IEnumerator LoadSceneAsync(int sceneId)
    {
        progressSlider.value = 0;
        loadingTimer += Time.deltaTime;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone )
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }


    void LoadLevel(string value)
    {
        StartCoroutine(LoadSceneAsync(value));
        Debug.Log("TIMER: " + loadingTimer);

        //SceneManager.LoadScene(value);
        //AsyncOperation operation = SceneManager.LoadSceneAsync(value);

    }

}
