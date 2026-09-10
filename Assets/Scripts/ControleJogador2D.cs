using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ControleJogador2D : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float moveInputX;
    private bool estaNoChao;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void HoraDeMorrer()
    {
        // 1. Esconde a arte e desativa colisão/física
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Collider2D colisor = GetComponent<Collider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (sprite != null) sprite.enabled = false;
        if (colisor != null) colisor.enabled = false;
        if (rb != null) rb.simulated = false;

        // 2. O próprio Player agenda o recarregamento da cena (não depende de nenhum outro objeto)
        Invoke(nameof(ReiniciarCena), 2f);
    }

    private void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
