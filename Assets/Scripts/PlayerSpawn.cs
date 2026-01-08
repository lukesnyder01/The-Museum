using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Awake()
    {
        if (GameManager.Instance.spawnInitialized)
        {
            CharacterController controller = GetComponent<CharacterController>();

            if (controller != null)
            {
                // CRITICAL: Disable the CharacterController before moving
                controller.enabled = false;
                transform.position = GameManager.Instance.lastSpawnPos;
                transform.rotation = Quaternion.Euler(GameManager.Instance.lastSpawnRot);
                Debug.Log("Set player rotation to " + GameManager.Instance.lastSpawnRot);
                Debug.Log("Player rotation " + transform.rotation.eulerAngles);
                controller.enabled = true;
            }
            else
            {
                // Fallback if no CharacterController
                transform.position = GameManager.Instance.lastSpawnPos;
                transform.rotation = Quaternion.Euler(GameManager.Instance.lastSpawnRot);
            }
        }
    }
}
