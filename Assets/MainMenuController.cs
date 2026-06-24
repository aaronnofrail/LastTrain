using UnityEngine;
using UnityEngine.SceneManagement; // WAJIB untuk urusan pindah scene

public class MainMenuController : MonoBehaviour
{
    [Header("Pengaturan Scene")]
    // Tulis nama scene gerbong pertama kamu di Inspector nanti (misal: GerbongAwal)
    public string namaSceneGame = "GerbongAwal";

    // Fungsi ini akan dipanggil saat tombol Start diklik
    public void MulaiGame()
    {
        Debug.Log("Memulai Game... Loading ke " + namaSceneGame);

        // Pindah ke scene gameplay utama
        SceneManager.LoadScene(namaSceneGame);
    }
}