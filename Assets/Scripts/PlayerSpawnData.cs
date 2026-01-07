using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpawnData", menuName = "Scriptable Object/Player Spawn Data")]
public class PlayerSpawnData : ScriptableObject
{
    public bool spawnDataInitialized;
    public Vector3 position;
    public Vector3 rotation;

    private void OnEnable()
    {
        spawnDataInitialized = false;
        position = Vector3.zero;
        rotation = Vector3.zero;
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}