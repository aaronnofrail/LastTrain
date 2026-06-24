using UnityEngine;

public class InisialisasiSceneAtap : MonoBehaviour
{
    [Header("Link UI dan Player")]
    public HealthDisplay scriptHealthDisplay; // Tarik objek UIManager/Canvas yang punya script display hati
    private NyawaPlayer scriptNyawaPlayer;

    void Start()
    {
        // Mencari objek bernama character di scene baru ini
        GameObject playerObj = GameObject.Find("character");

        if (playerObj != null)
        {
            scriptNyawaPlayer = playerObj.GetComponent<NyawaPlayer>();

            // Hubungkan script nyawa player ke script display hati secara otomatis saat scene mulai
            if (scriptHealthDisplay != null && scriptNyawaPlayer != null)
            {
                // Panggil fungsi refresh UI kamu di sini jika ada, contoh:
                // scriptHealthDisplay.UpdateStatusHati(scriptNyawaPlayer.currentHealth);
                Debug.Log("Sistem Atap: Player dan UI Hati berhasil disinkronkan!");
            }
        }
        else
        {
            Debug.LogError("Karakter 'character' belum dipasang di scene AtapGerbong!");
        }
    }
}