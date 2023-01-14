using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level[] levels;

    private Player _player;
    private int _currentLevel;

    private void Start()
    {
        _player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if (_player && levels.Length > 0)
        {
            _currentLevel = 0;
            _player.SpawnTo(levels[_currentLevel].spawnPoint.position);
        }
    }

    public void NextLevel()
    {
        // Before going to next track, reset flag
        levels[_currentLevel].flag.resetFlagPosition();

        // Calculate next track if all enemies are dead
        if (levels[_currentLevel].areEnemiesDead())
            _currentLevel = (_currentLevel + 1) % levels.Length;
        else
            levels[_currentLevel].respawnEnemies();

        // Spawn player to next flag
        _player.SpawnTo(levels[_currentLevel].spawnPoint.position);

    }

    public Level getCurrentLevel() { return levels[_currentLevel]; }

    public Vector3 getFlagPosition() { return levels[_currentLevel].flag.getPosition(); }

    public float getMaxDrop() { return levels[_currentLevel].maximumDropDistance; }

    public void elevateFlagTo(float coordY)
    {
        Vector3 position = getFlagPosition();
        position.y = coordY;
        levels[_currentLevel].flag.moveTo(position);
    }
}

