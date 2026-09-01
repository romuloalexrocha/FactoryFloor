using UnityEngine;

public class ComportamentoPlayer : MonoBehaviour
{
    private Vector2 posicaoInicial;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicaoInicial = rb.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Morte"))
        {
            rb.position = posicaoInicial;
        }
    }
}
