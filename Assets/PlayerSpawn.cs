using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Awake()
    {
        if (GameManager.spawnPointSet)
        {
            var transform = GetComponent<Transform>();
            transform.position = GameManager.targetPlayerPos;
            transform.rotation = Quaternion.Euler(GameManager.targetPlayerRot);
        }
    }
}
