using UnityEngine;

public class ArmadilhasConfig : MonoBehaviour
{
    public enum TipoArmadilha { Espeto, Torreta } // armazena lista de dados

    [Header("Configuração da Armadilha")] // Header: cabeçalho para mostrar no Inspector da Unity
    [SerializeField] private TipoArmadilha tipo = TipoArmadilha.Espeto;

    [Header("Configurações Gerais")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D colisorDano;
    [SerializeField] private float atrasoInicial;

    [Header("Configurações do Espeto")]
    [SerializeField] private float tempoAtivo; // tempo com os espetos para fora
    [SerializeField] private float tempoInativo; // tempo com os espetos recolhidos

    //[Header("Configurações da Torreta")]
    //[SerializeField] private float tempoEntreDisparos;
    //[SerializeField] private GameObject prefabProjetil;
    //[SerializeField] private Vector2 pontoDisparo;
    //[SerializeField] private float velocidadeProjetil;

    // Variáveis de controle
    private float cronometro;
    private bool emAtraso = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicialização de componentes
        if(animator == null) animator = GetComponent<Animator>();
        if(colisorDano == null) colisorDano = GetComponent<Collider2D>();

        // Lógica de atraso do comportamento: se houver algum valor de atraso,
        // alteramos o valor de emAtraso para true e desligamos as armadilhas.
        if(atrasoInicial > 0)
        {
            emAtraso = true;
            DesligarEspeto();
            DesligarTorreta();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ativamos as armadilhas de acordo com o seu tipo
        switch (tipo)
        {
            case TipoArmadilha.Espeto:
                Debug.Log("Selecionado: Espeto");
                AtualizarEspeto();
                break;
            case TipoArmadilha.Torreta:
                Debug.Log("Selecionado: Torreta");
                AtualizarTorreta();
                break;
            default:
                Debug.Log("Selecionado: Nada (Default)");
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
            if(cronometro >= atrasoInicial)
            {
                // Mudamos o valor de emAtraso e zeramos o crnômetro
                emAtraso = false;
                cronometro = 0f;
            }
            return; // saímos do if
        } // end if emAtraso

        // Animação inicial ativando o Espeto, a executar depois do atraso
        animator.SetInteger("estado", 1);

        // Lógica do ciclo de vida do Espeto
        cronometro += Time.deltaTime;
        float cicloTotal = tempoAtivo + tempoInativo;
        Debug.Log($"cicloTotal = " + cicloTotal);

        if (cronometro < tempoAtivo)
        {
            LigarEspeto();
        }
        else if (cronometro < cicloTotal)
        {
            DesligarEspeto();
        }
        else
        {
            cronometro = 0f;
            animator.SetInteger("estado", 0);
        }
    }

    private void LigarEspeto()
    {
        Debug.Log($"Chamando método LigarEspeto. Cronômetro: {cronometro}");
        if (colisorDano != null) colisorDano.enabled = true;
        if (animator != null) animator.SetInteger("estado", 2);
    }

    private void DesligarEspeto()
    {
        Debug.Log($"Chamando método DesligarEspeto. Cronômetro: {cronometro}");
        if (colisorDano != null) colisorDano.enabled = false;
        if (animator != null) animator.SetInteger("estado", 3);
    }

    private void AtualizarTorreta()
    {

    }

    private void DesligarTorreta()
    {

    }

}
