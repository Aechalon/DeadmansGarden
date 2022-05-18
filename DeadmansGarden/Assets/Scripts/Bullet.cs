using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 2.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DestroyOnContact());
    }

    IEnumerator DestroyOnContact()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
