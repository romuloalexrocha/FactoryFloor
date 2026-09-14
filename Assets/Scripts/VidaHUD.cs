using UnityEngine;
using TMPro;

public class VidaHUD : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI textoVida; // Referência ao componente TextMeshProUGUI para exibir a vida
    private ComportamentoPlayer player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<ComportamentoPlayer>();
        AtualizarTextoVida();
    }

    // Update is called once per frame
    void Update()
    {
        AtualizarTextoVida();
    }

    public void AtualizarTextoVida()
    {
        if (player != null && textoVida != null)
        {
            textoVida.text = "VIDAS: " + player.vida.ToString();

        }
        else
        {
            Debug.LogWarning("Referência ao player ou ao texto de vida não está definida.");
        }
    }
}
