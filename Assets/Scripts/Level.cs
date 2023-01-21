using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Transform spawnPoint;
    public Flag flag;
    public float maximumDropDistance = -10f;
    public LoadingManager loadingScreenManager;

    public Text infoText;
    public Text levelNameText;
    public bool hasLimitOfTime;
    public int hitsLimit = 15;
    public float timeLimitInSeconds = 60;
    public Enemy[] enemies;

    private Player _player;
    private int currentLevel;
    private float _timeLeft;
    private float _halfTotalTime;
    private float _halfTotalHits;
    private int _maxLevelReached;

    private void Start()
    {
        _maxLevelReached = PlayerPrefs.GetInt("levelReached", 1);
        _timeLeft = timeLimitInSeconds;
        _halfTotalTime = timeLimitInSeconds / 2f;
        _halfTotalHits = hitsLimit / 2;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("LEVEL: " + currentLevel);
        levelNameText.text = "Level" + currentLevel.ToString();

        if (_player)
        {
            _player.SpawnTo(spawnPoint.position);
        }
    }

    private void Update()
    {

        if (hasLimitOfTime)
        {
            if(_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                float minutes = Mathf.Floor(_timeLeft / 60);
                float seconds = Mathf.Floor(_timeLeft % 60);

                infoText.text = string.Format("Time left:\n{0:00} : {1:00}", minutes, seconds);

                if (_timeLeft > _halfTotalTime)
                {
                    // Si queda más de la mitad, incrementar el R
                    float timeProportion = 1 - ((_timeLeft - _halfTotalTime) / _halfTotalTime);
                    infoText.color = new Color(1f * timeProportion, 1f, 0f);
                }
                else
                {
                    // Si queda menos de la mitad, decrementar el G
                    float timeProportion = (_timeLeft) / _halfTotalTime;

                    infoText.color = new Color(1f, 1f * timeProportion, 0f, Mathf.Floor(_timeLeft%2f)/3f + 0.66f);
                }
            }
            else
            {
                infoText.enabled = false;
                loadingScreenManager.LoadLevel(currentLevel, "YOU RAN OUT OF TIME!\nRELOADING LEVEL");
            }
        }
        else
        {

            float hitsLeft = hitsLimit - _player.getNumberOfHits();
            if(hitsLeft > 0)
            {
                infoText.text = "Hits left:\n" + hitsLeft.ToString();

                if (hitsLeft > _halfTotalHits)
                {
                    // Si queda más de la mitad, incrementar el R
                    float timeProportion = 1 - ((hitsLeft - _halfTotalHits) / _halfTotalHits);
                    infoText.color = new Color(1f * timeProportion, 1f, 0f);
                }
                else
                {
                    // Si queda menos de la mitad, decrementar el G
                    float timeProportion = (hitsLeft) / _halfTotalHits;

                    infoText.color = new Color(1f, 1f * timeProportion, 0f);
                }
            }
            else
            {
                infoText.enabled = false;

                StartCoroutine(waitPlayerToStop());
                // yield return new WaitForSeconds(1);
                //if(!_player.isMoving())
                //    loadingScreenManager.LoadLevel(currentLevel, "YOU RAN OUT OF HITS!\nRELOADING LEVEL");

            }
        }
    }

    IEnumerator waitPlayerToStop()
    {
        yield return new WaitForSeconds(1);

        while (_player.isMoving())
            yield return new WaitForSeconds(1);

        loadingScreenManager.LoadLevel(currentLevel, "YOU RAN OUT OF HITS!\nRELOADING LEVEL");

    }

    public Vector3 getFlagPosition() { return flag.getPosition(); }

    public float getMaxDrop() { return transform.position.y + maximumDropDistance; }

    public void elevateFlagTo(float coordY)
    {
        Vector3 position = getFlagPosition();
        position.y = coordY;
        if(areEnemiesDead())
            flag.moveTo(position);
    }

    public bool areEnemiesDead()
    {
        // Search if there's an enemy that's not dead
        foreach (Enemy enemy in enemies)
            if (!enemy.isDead())
                return false;

        return true;
    }

    public void respawnEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.setMaxDrop(getMaxDrop());
            enemy.respawn();
        }
    }

    public void levelGoalReached(int currentLevel)
    {
        if (areEnemiesDead())
        {
            if(currentLevel < LevelScreenesManager.GetNumberOfLevels())
            {
                if(_maxLevelReached < currentLevel + 1)
                    PlayerPrefs.SetInt("levelReached", currentLevel+1);
    
                // If current level is by time, next will be by hits
                if(currentLevel % 2 == 0) 
                    loadingScreenManager.LoadLevel(currentLevel + 1, "Level"+(currentLevel+1).ToString()+"\nYOU MUST KILL ALL ENEMIES BEFORE YOU RUN OUT OF HITS.");
                else
                    loadingScreenManager.LoadLevel(currentLevel + 1, "Level"+(currentLevel+1).ToString()+ "\nYOU MUST KILL ALL ENEMIES BEFORE YOU RUN OUT OF TIME.");
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;

                if (_maxLevelReached < currentLevel)
                    PlayerPrefs.SetInt("levelReached", currentLevel);
                
                loadingScreenManager.LoadLevel(0, "GOING BACK TO MAIN MENU");

            }

        }
        else
        {
            loadingScreenManager.LoadLevel(currentLevel, "RESTARTING LEVEL\nYOU MUST KILL ALL ENEMIES");
        }
    }
}

