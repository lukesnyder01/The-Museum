using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool spawnInitialized;
    public Vector3 lastSpawnPos;
    public Vector3 lastSpawnRot;

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

    private void Initialize()
    {
        spawnInitialized = false;
    }
}
