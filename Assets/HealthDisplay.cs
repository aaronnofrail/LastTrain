using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [Header("Daftar 5 Hati di Hierarchy")]
    public Image[] daftarHatiUI;        // Kita akan masukkan 5 objek hati ke sini nanti

    [Header("Aset Gambar Hati")]
    public Sprite heartFull;            // Masukkan sprite Hati Penuh (Merah)
    public Sprite heartEmpty;           // Masukkan sprite Hati Kosong (Hitam/Redup)

    private NyawaPlayer playerHealth;

    void Start()
    {
        // Mencari objek character untuk dipantau nyawanya
        GameObject playerObj = GameObject.Find("character");
        if (playerObj != null)
        {
            playerHealth = playerObj.GetComponent<NyawaPlayer>();
        }
    }

    void Update()
    {
        if (playerHealth == null) return;

        // Lakukan pengecekan kondisi untuk setiap hati di dalam array
        for (int i = 0; i < daftarHatiUI.Length; i++)
        {
            if (daftarHatiUI[i] == null) continue;

            if (i < playerHealth.currentHealth)
            {
                // Jika index hati di bawah sisa nyawa player, tampilkan hati penuh
                daftarHatiUI[i].sprite = heartFull;
                daftarHatiUI[i].enabled = true; // Memastikan gambarnya aktif menyala
            }
            else
            {
                // Jika player sudah kehilangan nyawa di index ini, ubah jadi hati kosong
                daftarHatiUI[i].sprite = heartEmpty;
                
                // TIPS: Jika kamu ingin hatinya langsung MENGHILANG (bukan jadi hitam kosong),
                // matikan garis di atas dan aktifkan perintah di bawah ini:
                // daftarHatiUI[i].enabled = false;
            }
        }
    }
}