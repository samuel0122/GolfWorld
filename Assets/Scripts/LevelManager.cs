using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    /** 
     * NOT USED
     * NOT USED
     */
    public Level[] levels;

    private Player _player;
    private int _currentLevel;

    private void Start()
    {
        _player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if (_player && levels.Length > 0)
        {
            _currentLevel = 0;

            levels[_currentLevel].respawnEnemies();
            _player.SpawnTo(levels[_currentLevel].spawnPoint.position);
        }
    }

    public void NextLevel()
    {
        // Before going to next track, reset flag
        levels[_currentLevel].flag.resetFlagPosition();

        // Calculate next track if all enemies are dead
        if (levels[_currentLevel].areEnemiesDead())
        {
            // Si pasa al siguiente nivel, respawnea los enemigos del siguiente nivel
            Debug.Log("Level compleated");
            _currentLevel = (_currentLevel + 1) % levels.Length;
            levels[_currentLevel].respawnEnemies();
        }
        else
            levels[_currentLevel].respawnEnemies();

        // Spawn player to next flag
        _player.SpawnTo(levels[_currentLevel].spawnPoint.position);

    }

    public Level getCurrentLevel() { return levels[_currentLevel]; }

    public Vector3 getFlagPosition() { return levels[_currentLevel].flag.getPosition(); }

    public float getMaxDrop() { return levels[_currentLevel].getMaxDrop(); }

    public void elevateFlagTo(float coordY)
    {
        Vector3 position = getFlagPosition();
        position.y = coordY;
        levels[_currentLevel].flag.moveTo(position);
    }
}

