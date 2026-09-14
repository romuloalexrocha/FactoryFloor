using UnityEngine;
using UnityEngine.SceneManagement;

public class ComportamentoPlayer : MonoBehaviour
{
    [Header("Configurações do Player")]
    public int vida = 5;
    private Vector2 posicaoInicial;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;

    private Color corOriginal;
    private bool estaMorto = false;

    [Header("Efeitos de Dano")]
    [SerializeField] private float tempoImunidade = 1.5f; // Tempo de imunidade após receber dano
    [SerializeField] private float velocidadePiscar = 15f; // Velocidade de piscar do sprite durante a imunidade
    [SerializeField] private Color corDano = Color.red; // Cor do sprite durante a imunidade

    
    private float tempoUltimoDano = -999f; // Inicializa com um valor negativo para permitir dano imediato

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        posicaoInicial = rb.position;

        if(sr != null) corOriginal = sr.color;
        
    }

    void Update()
    {
        RegularEfeitoImunidade();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Morte"))
    //    {
    //        rb.position = posicaoInicial;
    //    }
    //}

    public void HoraDeMorrer(int dano)
    {
        if (EstaImune()) return;

        vida = ReceberDano(dano);

        /* 
        // Atualização do HUD de texto
        VidaHUD vidaHud = FindAnyObjectByType<VidaHUD>();
        if (vidaHud != null)
        {
            vidaHud.AtualizarTextoVida();
        }
        Debug.Log($"Vida atual = {vida}");
        */

        // Atualização do HUD de corações
        CoracoesHUD coracoesHud = FindAnyObjectByType<CoracoesHUD>();
        if (coracoesHud != null)
        {
            coracoesHud.AtualizarCoracoes();
        }

        if (vida <= 0)
        {
            vida = 0; // Garante que a vida não fique negativa
            Debug.Log("MORREU!");
            ExecutarMorte();
        } else
        {
            tempoUltimoDano = Time.time; // Atualiza o tempo do último dano recebido
            if(sr != null) sr.color = corDano; // Muda a cor do sprite para indicar dano
        }
    }

    private bool EstaImune()
    {
        // Retorna true se o tempo atual ainda estiver dentro da janela de imunidade
        return Time.time < tempoUltimoDano + tempoImunidade;
    }

    private void RegularEfeitoImunidade()
    {
        if (sr == null || estaMorto) return;

        if (EstaImune())
        {
            bool visivel = Mathf.Sin(Time.time * velocidadePiscar) > 0;
            sr.enabled = visivel; // Pisca o sprite durante a imunidade
        } else
        {
            sr.enabled = true; // Garante que o sprite esteja visível quando não estiver imune
            sr.color = corOriginal; // Restaura a cor original do sprite
        }
    }

    private int ReceberDano(int dano)
    {
        return vida - dano;
    }

    private void ExecutarMorte()
    {
        estaMorto = true;

        // 1. Esconde a arte e desativa colisão/física
        col.enabled = false;
        rb.simulated = false;
        sr.enabled = false;

        // 2. O próprio Player agenda o recarregamento da cena (não depende de nenhum outro objeto)
        Invoke(nameof(ReiniciarCena), 3f);
    }

    private void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
