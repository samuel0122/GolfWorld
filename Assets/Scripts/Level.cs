using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform spawnPoint;
    public Flag flag;
    public float maximumDropDistance = -10f;
    public Enemy[] enemies;

    private Player _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if (_player)
        {
            _player.SpawnTo(spawnPoint.position);
        }
    }

    public Vector3 getFlagPosition() { return flag.getPosition(); }

    public float getMaxDrop() { return transform.position.y + maximumDropDistance; }

    public void elevateFlagTo(float coordY)
    {
        Vector3 position = getFlagPosition();
        position.y = coordY;
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
}

