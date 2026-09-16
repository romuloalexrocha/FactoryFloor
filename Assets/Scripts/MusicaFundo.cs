using UnityEngine;

public class MusicaFundo : MonoBehaviour
{
    // Variável estática que vai guardar 'este' objeto oficial da música
    private static MusicaFundo instancia;

    private void Awake()
    {
        // Se a variável estática estiver vazia, significa que não existe nenhum objeto oficial da música
        if (instancia == null)
        {
            // Então, este objeto se torna o oficial
            instancia = this;
            // E não será destruído ao carregar uma nova cena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se já existir um objeto oficial da música, este objeto será destruído
            Destroy(gameObject);
        }
    }
}
