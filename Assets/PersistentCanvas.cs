using UnityEngine;
using TMPro;

public class PersistentCanvas : MonoBehaviour
{
    public static PersistentCanvas Instance;

    public TextMeshProUGUI gemsCollectedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}