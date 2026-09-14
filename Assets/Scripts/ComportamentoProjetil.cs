using UnityEngine;

public class ComportamentoProjetil : MonoBehaviour
{
    private int danoProjetil = 1;
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void ConfigurarDano(int valorDano)
    {
        danoProjetil = valorDano;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessarMorte(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ProcessarMorte(collider.gameObject);
    }
    private void ProcessarMorte(GameObject objetoAtingido)
    {
        if (objetoAtingido.CompareTag("Player"))
        {
            // Tenta chamar o método de morte no próprio Player
            objetoAtingido.SendMessage("HoraDeMorrer", danoProjetil, SendMessageOptions.DontRequireReceiver);

            // Destrói o projétil imediatamente no impacto
            Destroy(gameObject);
        }
    }
}
