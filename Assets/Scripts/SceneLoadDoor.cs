using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadDoor : MonoBehaviour, IInteractable
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    [HideInInspector]
    public string targetScene;

    private bool hasInteractedWith = false;

    public string interactText;

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

    public void Interact()
    {
        if (!hasInteractedWith)
        {
            hasInteractedWith = true;
            SetPlayerSpawnTarget();
            AudioManager.Instance.PlayImmediate("Open Door");
            SceneFader.Instance.TransitionToScene(targetScene);
        }

    }

    public string GetInteractText()
    {
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

}
