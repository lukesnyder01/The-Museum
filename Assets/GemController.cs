using UnityEngine;

public class GemController : MonoBehaviour
{
    private Vector3 gemPos;
    public GameObject particleEffect;
    private Vector3 particleOffset = new Vector3(0, 0.7f, 0);

    void Start()
    {
        gemPos = transform.position;

        if (GameManager.Instance.collectedGems.Contains(gemPos))
            gameObject.SetActive(false);
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameManager.Instance.CollectGem(gemPos);
            Instantiate(particleEffect, transform.position + particleOffset, transform.rotation);
            Destroy(gameObject);
        }
    }

}
