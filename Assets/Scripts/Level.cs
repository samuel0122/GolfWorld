using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform spawnPoint;
    public Flag flag;
    public float maximumDropDistance = -10f;
    public Enemy[] enemies;


    public float getMaxDrop() { return transform.position.y + maximumDropDistance; }


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

