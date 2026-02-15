using UnityEngine;

public class GemController : MonoBehaviour
{
    private Vector3 gemPos;

    void Start()
    {
        gemPos = transform.position;
        if (GameManager.Instance.collectedGems.Contains(gemPos))
            gameObject.SetActive(false);
        Debug.Log(gemPos);
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameManager.Instance.CollectGem(gemPos);
            Destroy(gameObject);
        }
    }

}
