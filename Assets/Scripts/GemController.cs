using UnityEngine;

public class GemController : MonoBehaviour
{
    private Vector3 pos;
    public GameObject particleEffect;
    private Vector3 particleOffset = new Vector3(0, 0.7f, 0);

    void Start()
    {
        pos = transform.position;

        if (GameManager.Instance.collectedGems.Contains(pos))
            gameObject.SetActive(false);
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameManager.Instance.CollectGem(pos);
            Instantiate(particleEffect, transform.position + particleOffset, transform.rotation);
            Destroy(gameObject);
        }
    }

}
