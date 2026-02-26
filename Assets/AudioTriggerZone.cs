using UnityEngine;

public class AudioTriggerZone : MonoBehaviour
{
    public string audioName;



    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            AudioManager.Instance.PlayImmediate(audioName);
            Destroy(gameObject);
        }
    }
}
