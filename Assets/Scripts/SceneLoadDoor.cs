using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadDoor : MonoBehaviour, IInteractable
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    public PlayerSpawnData playerSpawnData;

    [HideInInspector]
    public string targetScene;

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
        AudioManager.Instance.PlayImmediate("Open Door");
        SetPlayerSpawnTarget();
        SceneFader.Instance.TransitionToScene(targetScene);
    }

    private void SetPlayerSpawnTarget()
    {
        playerSpawnData.spawnDataInitialized = true;
        playerSpawnData.position = targetPosition;
        playerSpawnData.rotation = targetRotation;
    }

}
