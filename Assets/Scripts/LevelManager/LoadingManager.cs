using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public Slider progressSlider;
    public Text loadingTextInfo;
    public GameObject loadingScreen;
    public GameObject mainMenu;

    private void Start()
    {
    }


    private IEnumerator LoadSceneAsync(string sceneId)
    {
        progressSlider.value = 0;
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);

        yield return new WaitForSeconds(2);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId);
        asyncOperation.allowSceneActivation = false;

        float progressValue = 0;
        float inverseProgressValue = 1; ;
        while (!asyncOperation.isDone || Time.timeSinceLevelLoad < 50f)
        {
            // float progressValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressValue = Mathf.MoveTowards(progressValue, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progressValue;
            inverseProgressValue = 1 - progressValue;
            progressSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1f * inverseProgressValue, 1f, 1f * inverseProgressValue);

            if (progressValue >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
                progressSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0f, 1f, 0.1f);
            }

            yield return null;
        }
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        progressSlider.value = 0;
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId);
        asyncOperation.allowSceneActivation = false;

        if(sceneId != 0)
            yield return new WaitForSeconds(2);
    
        float progressValue = 0;
        float inverseProgressValue = 1; ;
        while (!asyncOperation.isDone || Time.timeSinceLevelLoad < 50f)
        {
            // float progressValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressValue = Mathf.MoveTowards(progressValue, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progressValue;
            inverseProgressValue = 1 - progressValue;
            progressSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1f * inverseProgressValue, 1f, 1f * inverseProgressValue);

            if (progressValue >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
                progressSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0f, 1f, 0.1f);
            }

            yield return null;
        }
    }


    public void LoadLevel(int value, string loadingInfo)
    {
        loadingTextInfo.text = loadingInfo;

        StartCoroutine(LoadSceneAsync(value));

        //SceneManager.LoadScene(value);
        //AsyncOperation operation = SceneManager.LoadSceneAsync(value);

    }

    public void LoadLevel(string value, string loadingInfo)
    {
        
        loadingTextInfo.text = loadingInfo;
       
        StartCoroutine(LoadSceneAsync(value));

        //SceneManager.LoadScene(value);
        //AsyncOperation operation = SceneManager.LoadSceneAsync(value);

    }
}
