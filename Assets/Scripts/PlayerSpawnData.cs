using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpawnData", menuName = "Scriptable Object/Player Spawn Data")]
public class PlayerSpawnData : ScriptableObject
{
    public bool spawnDataInitialized = false;
    public Vector3 position;
    public Vector3 rotation;
}