/*
 * 
 * SENAI - Serviço Nacional de Aprendizagem Industrial
 * Atividade Prática - Programando Armadilhas
 * Rômulo Alexandre da Rocha
 * 
 */

using UnityEngine;

public class ArmadilhasConfig : MonoBehaviour
{
    public enum TipoArmadilha { Espeto, Torreta } // armazena lista de dados

    [Header("Configuração da Armadilha")] // Header: cabeçalho para mostrar no Inspector da Unity
    [SerializeField] private TipoArmadilha tipo = TipoArmadilha.Espeto;
    [SerializeField] private int quantidadeDano = 1;

    [Header("Configurações Gerais")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D colisorDano;
    [SerializeField] private float atrasoInicial;

    [Header("Configurações do Espeto")]
    [SerializeField] private float tempoAtivo; // tempo com os espetos para fora
    [SerializeField] private float tempoInativo; // tempo com os espetos recolhidos

    [Header("Configurações da Torreta")]
    [SerializeField] private float tempoEntreDisparos;
    [SerializeField] private GameObject prefabProjetil;
    [SerializeField] private Transform pontoDisparo;
    [SerializeField] private float velocidadeProjetil;

    // Variáveis de controle
    private float cronometro;
    private bool emAtraso = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicialização de componentes
        if (animator == null) animator = GetComponent<Animator>();
        if (colisorDano == null) colisorDano = GetComponent<Collider2D>();

        // Lógica de atraso do comportamento: se houver algum valor de atraso,
        // alteramos o valor de emAtraso para true e desligamos as armadilhas.
        if (atrasoInicial > 0)
        {
            emAtraso = true;
            DefinirEstadoColisor(false);
            DefinirAnimacao(0); // Definimos a animação inicial como Spike_Recolhido ou Torreta_Idle
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ativamos as armadilhas de acordo com o seu tipo
        switch (tipo)
        {
            case TipoArmadilha.Espeto:
                //Debug.Log("Selecionado: Espeto");
                AtualizarEspeto();
                break;
            case TipoArmadilha.Torreta:
                //Debug.Log("Selecionado: Torreta");
                if(quantidadeDano <= 2) quantidadeDano = 2;
                AtualizarTorreta();
                break;
            default:
                //Debug.Log("Selecionado: Nada (Default)");
                break;
        }
    }

    private void AtualizarEspeto()
    {
        // Lógica do atraso inicial
        // Só executa esse if se emAtraso for true - ou seja, quando houver
        // algum valor em atrasoInicial
        if (emAtraso)
        {
            cronometro += Time.deltaTime; // Incrementamos o tempo até chegar ao valor de atrasoInicial
            if (cronometro >= atrasoInicial)
            {
                // Mudamos o valor de emAtraso e zeramos o crnômetro
                emAtraso = false;
                cronometro = 0f;
                // Acionamos animação Spike_Ativado
                DefinirEstadoColisor(true);
                DefinirAnimacao(1);
            }
            return; // saímos do if
        } // end if emAtraso

        // Lógica do ciclo de vida do Espeto
        cronometro += Time.deltaTime;
        // Pega o estado atual do Animator, se ele existir. Caso contrário, retorna 0
        int estadoAtual = animator != null ? animator.GetInteger("estado") : 0;
        //float cicloTotal = tempoAtivo + tempoInativo;

        switch (estadoAtual)
        {
            case 0: // Spike_Recolhido
                if (cronometro >= tempoInativo)
                {
                    cronometro = 0f;
                    DefinirEstadoColisor(true);
                    DefinirAnimacao(1);
                }
                break;
            case 1: // Spike_Ativado
                // Verifica se a animação Spike_Ativado terminou de tocar (normalizedTime >= 1.0)
                if (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    cronometro = 0f;
                    DefinirAnimacao(2);
                }
                break;
            case 2: // Spike_Ligado
                if (cronometro >= tempoAtivo)
                {
                    cronometro = 0f;
                    DefinirAnimacao(3);
                }
                break;
            case 3: // Spike_Desligado
                // Verifica se a animação Spike_Desativado terminou de tocar (normalizedTime >= 1.0)
                if (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    cronometro = 0f;
                    DefinirEstadoColisor(false);
                    DefinirAnimacao(0); // Volta para Spike_Recolhido
                }
                break;
            default:
                break;
        }
    }

    private void AtualizarTorreta()
    {
        // Lógica do atraso inicial
        // Só executa esse if se emAtraso for true - ou seja, quando houver
        // algum valor em atrasoInicial
        if (emAtraso)
        {
            cronometro += Time.deltaTime; // Incrementamos o tempo até chegar ao valor de atrasoInicial
            if (cronometro >= atrasoInicial)
            {
                // Mudamos o valor de emAtraso e zeramos o crnômetro
                emAtraso = false;
                cronometro = 0f;
                // Acionamos animação Torreta_Disparando
                DefinirAnimacao(1);
            }
            return; // saímos do if
        } // end if emAtraso

        // Lógica do ciclo de vida do Espeto
        cronometro += Time.deltaTime;
        // Pega o estado atual do Animator, se ele existir. Caso contrário, retorna 0
        int estadoAtual = animator != null ? animator.GetInteger("estado") : 0;

        //if (animator != null && tempoEntreDisparos != 0f)
        //{
        //    // Exemplo: velocidade inversamente proporcional ao tempoEntreDisparos
        //    animator.speed = 1f / tempoEntreDisparos;
        //}

        switch (estadoAtual)
        {
            case 0: // Torreta_Aguardando
                if (cronometro >= tempoEntreDisparos)
                {
                    cronometro = 0f;
                    DefinirAnimacao(1);
                }
                break;
            case 1: // Torreta_Disparando
                if (cronometro >= tempoEntreDisparos)
                {
                    cronometro = 0f;
                    // DefinirAnimacao(1);
                }
                break;
            default:
                break;
        }
    }

    private void DefinirAnimacao(int novoEstado)
    {
        if (animator != null) animator.SetInteger("estado", novoEstado);
    }

    private void DefinirEstadoColisor(bool novoEstado)
    {
        if (colisorDano != null) colisorDano.enabled = novoEstado;
    }

    public void DispararProjetil()
    {
        if (prefabProjetil != null)
        {
            GameObject projetil = Instantiate(prefabProjetil, pontoDisparo.position, pontoDisparo.rotation);
            Rigidbody2D rb = projetil.GetComponent<Rigidbody2D>();
            ComportamentoProjetil scriptProjetil = projetil.GetComponent<ComportamentoProjetil>();
            if (rb != null)
            {
                SpriteRenderer sprite = GetComponent<SpriteRenderer>(); // Pegamos o SpriteRenderer do objeto atual (a torreta)
                float direcao = (sprite != null && sprite.flipX) ? 1f : -1f; // Determina a direção com base no flipX do sprite
                rb.linearVelocity = new Vector2(velocidadeProjetil * direcao, 0f); // Define a velocidade do projetil na direção correta
            }

            if(scriptProjetil != null)
            {
                scriptProjetil.ConfigurarDano(quantidadeDano);
            }
        }
    }

    // --- SISTEMA DE DANO E MORTE ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tipo == TipoArmadilha.Espeto) ProcessarMorte(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (tipo == TipoArmadilha.Espeto) ProcessarMorte(collider.gameObject);
    }

    private void ProcessarMorte(GameObject objetoAtingido)
    {
        if (objetoAtingido.CompareTag("Player"))
        {
            // Tenta chamar o método de morte no próprio Player
            objetoAtingido.SendMessage("HoraDeMorrer", quantidadeDano, SendMessageOptions.DontRequireReceiver);
        }
    }

}
