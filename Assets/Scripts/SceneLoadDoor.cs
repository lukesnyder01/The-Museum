using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadDoor : MonoBehaviour, IInteractable
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    // the scene in string
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
        SetPlayerSpawnTarget();
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void SetPlayerSpawnTarget()
    {
        GameManager.spawnPointSet = true;
        GameManager.targetPlayerPos = targetPosition;
        GameManager.targetPlayerRot = targetRotation;
    }

}
