using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMTest : MonoBehaviour
{
    public GameObject LoadingScene;

    [System.Serializable]
    public class Level
    {
        public string LevelText;
        public int Unlock;
        public bool isInteractible;
        public Button.ButtonClickedEvent OnClick;
    }
    public GameObject LEVELButton;
    public Transform Spacer;
    public List<Level> LevelList;

    void Start()
    {
        Delete();
        FillList();
    }

    void FillList() 
    {
        foreach (var level in LevelList) 
        {
            GameObject newButton = Instantiate(LEVELButton) as GameObject;
            LevelButtonNew button = newButton.GetComponent<LevelButtonNew>();

            button.LevelText.text = level.LevelText;

            if (PlayerPrefs.GetInt("Level" + button.LevelText.text) == 1)
            {
                level.Unlock = 1;
                level.isInteractible = true;
            }

            button.unlocked = level.Unlock;
            button.GetComponent<Button>().interactable = level.isInteractible;
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel("Level" + button.LevelText.text));

            newButton.transform.SetParent(Spacer);
        }
        Save();
    }

    void Save()
    {
        {
            GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");

            foreach (GameObject buttons in allButtons)
            {
                LevelButtonNew button = buttons.GetComponent<LevelButtonNew>();
                PlayerPrefs.SetInt("Level" + button.LevelText.text, button.unlocked);
            }
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
    }

    void LoadLevel(string value)
    {
        SceneManager.LoadScene(value);
        LoadingScene.SetActive(true);
    }
}
