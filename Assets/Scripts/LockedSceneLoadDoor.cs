using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedSceneLoadDoor : MonoBehaviour, IInteractable, IUnlockable
{
    public bool locked = true;

    public Vector3 targetPosition;
    public Vector3 targetRotation;

    [HideInInspector]
    public string targetScene;

    private bool hasInteractedWith = false;

    public string interactText;
    public string lockedInteractText = "This door is locked";

    public Vector3 pos;

#if UNITY_EDITOR

    // the scene in asset
    public UnityEditor.SceneAsset targetSceneAsset;

    // whenever you modify the scene in the project, this sets the new name in the
    // targetScene variable automatically.
    private void OnValidate()
    {
        targetScene = "";
        if (targetSceneAsset != null)
        {
            targetScene = targetSceneAsset.name;
        }
    }

#endif

    public void Start()
    {
        pos = transform.position;

        if (GameManager.Instance.unlockedDoors.Contains(pos))
            Unlock();
    }


    public void Interact()
    {
        if (locked)
        {
            AudioManager.Instance.PlayImmediate("Door Handle Jiggle");
        }
        else
        {
            if (!hasInteractedWith)
            {
                hasInteractedWith = true;
                SetPlayerSpawnTarget();
                AudioManager.Instance.PlayImmediate("Open Door");
                SceneFader.Instance.TransitionToScene(targetScene);
                GameManager.Instance.unlockedDoors.Add(pos);
            }
        }
    }


    public string GetInteractText()
    {
        if (locked)
            return lockedInteractText;
        else
            return interactText;
    }

    private void SetPlayerSpawnTarget()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.spawnInitialized = true;
        gameManager.lastSpawnPos = targetPosition;
        gameManager.lastSpawnRot = targetRotation;
        Debug.Log(targetRotation);
        Debug.Log(gameManager.lastSpawnRot);
    }

    public void Unlock()
    {
        locked = false;
    }

}
