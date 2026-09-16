using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ControleJogador2D : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Efeitos Sonoros do Jogador")]
    [SerializeField] private AudioClip somPulo;
    [SerializeField] private AudioClip somPassos;
    [SerializeField] private float intervaloPassos = 0.35f; // Intervalo entre os sons de passos

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private float moveInputX;
    private bool estaNoChao;
    private float timerPassos;
    // public int vida = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Método disparado pelo Player Input (Action: Move)
    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        moveInputX = inputVector.x;
    }

    // Método disparado pelo Player Input (Action: Jump)
    public void OnJump(InputValue value)
    {
        // Pula apenas se estiver no chão
        if (value.isPressed && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            TocarSomPulo();
            estaNoChao = false; // Força a saída do chão imediatamente no frame do pulo
        }
    }

    private void Update()
    {
        // Considera no chão se a velocidade vertical estiver próxima de zero
        estaNoChao = Mathf.Abs(rb.linearVelocity.y) < 0.1f;

        // Atualiza as variáveis do Animator (garanta que o nome seja exatamente idêntico)
        animator.SetBool("isGrounded", estaNoChao);
        animator.SetBool("isWalking", Mathf.Abs(moveInputX) > 0.1f);

        // Inverte a direção do sprite
        InverterSprite();

        // Lógica dos passos: toca o som de passos apenas se estiver andando e no chão
        if(Mathf.Abs(moveInputX) > 0.1f && estaNoChao)
        {
            timerPassos += Time.deltaTime;
            if (timerPassos >= intervaloPassos)
            {
                if(somPassos != null && audioSource != null)
                {
                    audioSource.PlayOneShot(somPassos);
                }
                timerPassos = 0f;
            }
        }
        else
        {
            timerPassos = intervaloPassos; // Reseta o timer para tocar o som imediatamente ao começar a andar
        }
    }

    private void FixedUpdate()
    {
        // Aplica o movimento horizontal mantendo a força Y da gravidade/pulo
        rb.linearVelocity = new Vector2(moveInputX * moveSpeed, rb.linearVelocity.y);
    }

    private void InverterSprite()
    {
        if (moveInputX > 0.1f)
        {
            spriteRenderer.flipX = false; // Olhando para a direita
        }
        else if (moveInputX < -0.1f)
        {
            spriteRenderer.flipX = true; // Olhando para a esquerda
        }
    }

    public void TocarSomPulo()
    {
        if (somPulo != null && audioSource != null)
        {
            audioSource.PlayOneShot(somPulo);
        }
    }
}
