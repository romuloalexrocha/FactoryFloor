using UnityEngine;
using UnityEngine.SceneManagement;

public class ComportamentoPlayer : MonoBehaviour
{
    [Header("Configurações do Player")]
    public int vida = 5;
    //private Vector2 posicaoInicial;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;
    private Color corOriginal; // Cor original do sprite para restaurar após o efeito de dano
    private bool estaMorto = false; // Flag para indicar se o player está morto

    [Header("Efeitos de Dano")]
    [SerializeField] private float tempoImunidade = 1.5f; // Tempo de imunidade após receber dano
    [SerializeField] private float velocidadePiscar = 15f; // Velocidade de piscar do sprite durante a imunidade
    [SerializeField] private Color corDano = Color.red; // Cor do sprite durante a imunidade
    private float tempoUltimoDano = -999f; // Inicializa com um valor negativo para permitir dano imediato

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicializa os componentes do player
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        // posicaoInicial = rb.position;

        // Armazena a cor original do sprite para restaurar após o efeito de dano
        if (sr != null) corOriginal = sr.color;
        
    }

    void Update()
    {
        RegularEfeitoImunidade(); // Chama o método para regular o efeito de imunidade a cada frame
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
        if (EstaImune()) return; // Se estiver imune, não recebe dano

        vida = ReceberDano(dano);

        // Atualização do HUD de texto
        VidaHUD vidaHud = FindAnyObjectByType<VidaHUD>();
        if (vidaHud != null)
        {
            vidaHud.AtualizarTextoVida();
        }
        // Debug.Log($"VIDAS: {vida}");

        // Atualização do HUD de corações
        CoracoesHUD coracoesHud = FindAnyObjectByType<CoracoesHUD>();
        if (coracoesHud != null)
        {
            coracoesHud.AtualizarCoracoes();
        }

        if (vida <= 0) // Jogador morreu
        {
            vida = 0; // Garante que a vida não fique negativa
            // Debug.Log("MORREU!");
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
        if (sr == null || estaMorto) return; // Se o SpriteRenderer não estiver definido ou o player estiver morto, não faz nada

        // Se o player estiver imune, faz o sprite piscar; caso contrário, garante que o sprite esteja visível e com a cor original
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
        estaMorto = true; // Marca o player como morto para evitar efeitos de dano adicionais

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
