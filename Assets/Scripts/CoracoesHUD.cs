using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoracoesHUD : MonoBehaviour
{
    [Header("Configurações do Grid e Prefab")]
    [SerializeField] private Transform containerCoracoes; // Objeto Pai com o Grid Layout Group
    [SerializeField] private GameObject prefabCoracao; // Prefab do coração

    private ComportamentoPlayer player; // Referência ao script do jogador
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<ComportamentoPlayer>();

        AtualizarCoracoes();
    }

    public void AtualizarCoracoes()
    {
        if (player == null || containerCoracoes == null || prefabCoracao == null) return;

        // Limpa os corações existentes
        foreach (Transform filho in containerCoracoes)
        {
            Destroy(filho.gameObject);
        }

        // Cria corações de acordo com a vida atual do jogador
        for (int i = 0; i < player.vida; i++)
        {
            Instantiate(prefabCoracao, containerCoracoes);
        }
    }
}