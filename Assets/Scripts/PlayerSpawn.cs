using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public PlayerSpawnData playerSpawnData;

    void Awake()
    {
        if (!playerSpawnData.spawnDataInitialized)
        {
            var transform = GetComponent<Transform>();

            transform.position = playerSpawnData.position;
            transform.rotation = Quaternion.Euler(playerSpawnData.rotation);
        }
    }
}
