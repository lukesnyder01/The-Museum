using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool spawnInitialized;
    public Vector3 lastSpawnPos;
    public Vector3 lastSpawnRot;

    public List<Vector3> collectedGems = new List<Vector3>();
    public List<Vector3> unlockedDoors = new List<Vector3>();

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
    }


    public void CollectGem(Vector3 gemPos)
    {
        collectedGems.Add(gemPos);
        AudioManager.Instance.PlayImmediate("Crystal Chime");
        PersistentCanvas.Instance.gemsCollectedText.text = collectedGems.Count.ToString();
    }


    private void Initialize()
    {
        spawnInitialized = false;
    }

}
