using UnityEngine;

public class Medkit : MonoBehaviour
{
    [Header("Pengaturan Medkit")]
    public int jumlahHeal = 1; 

    [Header("Pengaturan Audio")]
    public AudioClip suaraHeal; // Tarik file Heal.mp3 ke sini di Inspector

    private bool playerDiDekatMedkit = false;
    private NyawaPlayer scriptNyawaPlayer;

    void Update()
    {
        // Cek apakah player berada di dekat medkit DAN menekan tombol E
        if (playerDiDekatMedkit && Input.GetKeyDown(KeyCode.E))
        {
            if (scriptNyawaPlayer != null)
            {
                // Cek jika darah sudah penuh, jangan biarkan diambil
                if (scriptNyawaPlayer.currentHealth >= scriptNyawaPlayer.maxHealth)
                {
                    Debug.Log("Darah sudah penuh, medkit tidak diambil.");
                    return; 
                }

                AmbilMedkit();
            }
        }
    }

    void AmbilMedkit()
    {
        // 1. Tambah darah player
        scriptNyawaPlayer.currentHealth += jumlahHeal;

        if (scriptNyawaPlayer.currentHealth > scriptNyawaPlayer.maxHealth)
        {
            scriptNyawaPlayer.currentHealth = scriptNyawaPlayer.maxHealth;
        }

        // 2. MAINKAN SUARA HEAL SECARA MANDIRI (Anti-Putus saat Destroy)
        if (suaraHeal != null)
        {
            // Membuat sumber suara sementara di posisi medkit berada
            AudioSource.PlayClipAtPoint(suaraHeal, transform.position);
        }

        Debug.Log("Medkit diambil dengan tombol E! Suara Heal dimainkan.");
        
        // 3. Hancurkan objek medkit di lantai gerbong
        Destroy(gameObject);
    }

    // Deteksi saat karakter mendekat
    private void OnTriggerEnter2D(Collider2D objekLain)
    {
        if (objekLain.name == "character" || objekLain.CompareTag("Player"))
        {
            playerDiDekatMedkit = true;
            scriptNyawaPlayer = objekLain.GetComponent<NyawaPlayer>();
            Debug.Log("Tekan 'E' untuk mengambil Medkit");
        }
    }

    // Deteksi saat karakter menjauh
    private void OnTriggerExit2D(Collider2D objekLain)
    {
        if (objekLain.name == "character" || objekLain.CompareTag("Player"))
        {
            playerDiDekatMedkit = false;
            scriptNyawaPlayer = null;
        }
    }
}