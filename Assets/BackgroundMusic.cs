using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Variabel static untuk memastikan hanya ada SATU musik latar yang menyala di game
    private static BackgroundMusic instance = null;

    void Awake()
    {
        // JIKA sudah ada musik yang menyala sebelumnya, hancurkan objek tiruan yang baru ini
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        // PERINTAH UTAMA: Jangan hancurkan objek ini saat scene berganti!
        DontDestroyOnLoad(this.gameObject);
    }
}