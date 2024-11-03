using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float _fallDelay = 1f;
    private float _destroyDelay = 2f;
    [SerializeField] private Rigidbody2D platformBody;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(_fallDelay);
        platformBody.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, _destroyDelay);
    }
}
