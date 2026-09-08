using UnityEngine;
using UnityEngine.SceneManagement;

public enum TipoObstaculo { Serra, Laser }

public class GerenciadorArmadilhas : MonoBehaviour
{
    [Header("Configuração Geral")]
    [SerializeField] private TipoObstaculo tipo = TipoObstaculo.Serra;

    [Header("⚙️ Configurações da Serra")]
    [SerializeField] private float distancia = 4f;
    [SerializeField] private float velocidade = 3f;

    [Header("⚡ Configurações do Laser")]
    [SerializeField] private float tempoAtivo = 2f;     // Duração do laser ligado
    [SerializeField] private float tempoInativo = 1.5f; // Duração do laser desligado
    [SerializeField] private float atrasoInicial = 0f;  // ⏱️ Tempo de espera ANTES de iniciar o ritmo
    [SerializeField] private Collider2D colisorLaser;

    private Vector3 posInicial;
    private float cronometro;
    private bool emAtraso = false;
    private Animator anim;

    private void Start()
    {
        posInicial = transform.position;

        // Pega o Animator do próprio objeto
        anim = GetComponent<Animator>();

        // Busca o colisor se não for atribuído no Inspector
        if (colisorLaser == null) colisorLaser = GetComponent<Collider2D>();

        // Se houver atraso inicial configurado, começa o laser desligado
        if (tipo == TipoObstaculo.Laser && atrasoInicial > 0)
        {
            emAtraso = true;
            DesligarLaser();
        }
    }

    private void Update()
    {
        switch (tipo)
        {
            case TipoObstaculo.Serra:
                AtualizarSerra();
                break;

            case TipoObstaculo.Laser:
                AtualizarLaser();
                break;
        }
    }

    private void AtualizarSerra()
    {
        float deslocamento = Mathf.PingPong(Time.time * velocidade, distancia);
        transform.position = posInicial + new Vector3(deslocamento, 0, 0);
    }

    private void AtualizarLaser()
    {
        // 1. Fase de Atraso Inicial (Dispara apenas uma vez no começo do jogo)
        if (emAtraso)
        {
            cronometro += Time.deltaTime;
            if (cronometro >= atrasoInicial)
            {
                emAtraso = false;
                cronometro = 0f; // Zera o cronômetro para iniciar o ciclo normal
            }
            return; // Interrompe o Update enquanto o atraso estiver valendo
        }

        // 2. Ciclo Normal de Funcionamento
        cronometro += Time.deltaTime;
        float cicloTotal = tempoAtivo + tempoInativo;

        if (cronometro < tempoAtivo)
        {
            LigarLaser();
        }
        else if (cronometro < cicloTotal)
        {
            DesligarLaser();
        }
        else
        {
            cronometro = 0f; // Reinicia o ciclo
        }
    }

    private void LigarLaser()
    {
        if (colisorLaser != null) colisorLaser.enabled = true;
        if (anim != null) anim.SetBool("Ativado", true);
    }

    private void DesligarLaser()
    {
        if (colisorLaser != null) colisorLaser.enabled = false;
        if (anim != null) anim.SetBool("Ativado", false);
    }

    // --- SISTEMA DE DANO E MORTE ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Invoke("ReiniciarCena", 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(collider.gameObject);
            Invoke("ReiniciarCena", 2f);
        }
    }

    private void ProcessarMorte(GameObject objetoAtingido)
    {
        if (objetoAtingido.CompareTag("Player"))
        {
            Destroy(objetoAtingido);
            Invoke(nameof(ReiniciarCena), 2f);
        }
    }

    private void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}