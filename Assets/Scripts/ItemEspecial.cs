using UnityEngine;

public enum TipoItemEspecial { Cura, Chave }

public class ItemEspecial : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private TipoItemEspecial tipoItem = TipoItemEspecial.Cura;
    [SerializeField] private int quantidadeCura = 1; // Quantidade de vida que o item de cura recupera

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ComportamentoPlayer player = collision.GetComponent<ComportamentoPlayer>();

            if (player != null)
            {
                switch (tipoItem)
                {
                    case TipoItemEspecial.Cura:
                        player.HoraDeCurar(quantidadeCura); // Passa o valor para curar

                        // Destrói o item após ser coletado
                        Destroy(gameObject);
                        break;
                    case TipoItemEspecial.Chave:
                        // Aqui você pode adicionar a lógica para coletar a chave
                        Debug.Log("Chave coletada!");

                        // Tenta chamar o método de morte no próprio Player
                        player.SendMessage("ReiniciarCena", SendMessageOptions.DontRequireReceiver);
                        break;
                }
            }
        }
    }
}
