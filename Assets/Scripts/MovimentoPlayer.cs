using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{

    public float velocidade = 2f;
    public float forcaImpulso = 2.5f;
    public float escalaGravidade = 0.5f;
    public int pulosRestantes, maxPulos = 2;
    private Vector2 posicaoInicial;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pulosRestantes = maxPulos;
        posicaoInicial = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * velocidade * Time.deltaTime);
        }

        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(Vector2.down * velocidade * Time.deltaTime);
        //}

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * velocidade * Time.deltaTime);
        }

        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(Vector2.up * velocidade * Time.deltaTime);
        //}

        if (Input.GetKeyDown(KeyCode.Space) && pulosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * forcaImpulso, ForceMode2D.Impulse);
            pulosRestantes--;
        }

    }

    void FixedUpdate()
    {
        rb.AddForce(Physics2D.gravity * (escalaGravidade - 1) * rb.mass, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            pulosRestantes = maxPulos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Morte"))
        {
            rb.position = posicaoInicial;
            pulosRestantes = maxPulos;
        }
    }
}
