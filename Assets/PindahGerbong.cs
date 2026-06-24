using UnityEngine;
using UnityEngine.SceneManagement; // <-- 1. WAJIB ADA DI PALING ATAS SCRIPT

public class PindahGerbong : MonoBehaviour
{
    [Header("Pengaturan Scene Tujuan")]
    // Tulis nama scene tujuanmu di Inspector Unity nanti (misal: AtapGerbong)
    public string namaSceneTujuan = "AtapGerbong";

    private bool playerDiDekatTangga = false;

    void Update()
    {
        // 2. Cek jika player berada di area tangga DAN menekan tombol E
        if (playerDiDekatTangga && Input.GetKeyDown(KeyCode.E))
        {
            ActionPindahScene();
        }
    }

    void ActionPindahScene()
    {
        Debug.Log("Loading ke scene berikutnya...");

        // 3. BARIS UTAMA UNTUK AMBIL SCRIPT PINDAH SCENE
        SceneManager.LoadScene(namaSceneTujuan);
    }

    // Sensor deteksi saat karakter mendekati tangga
    private void OnTriggerEnter2D(Collider2D objekLain)
    {
        if (objekLain.name == "character" || objekLain.CompareTag("Player"))
        {
            playerDiDekatTangga = true;
            Debug.Log("Tekan E untuk pindah gerbong!");
        }
    }

    // Sensor deteksi saat karakter menjauhi tangga
    private void OnTriggerExit2D(Collider2D objekLain)
    {
        if (objekLain.name == "character" || objekLain.CompareTag("Player"))
        {
            playerDiDekatTangga = false;
        }
    }
}