using UnityEngine;

public class NyawaZombie : MonoBehaviour
{
    // Kita tentukan nyawa zombie di awal adalah 2
    private int nyawa = 2;

    // Fungsi ini akan dipanggil jika zombie terkena pisau
    public void TerkenaPisau()
    {
        nyawa = nyawa - 1; // Kurangi nyawa sebesar 1

        Debug.Log("Zombie kena hit! Sisa nyawa: " + nyawa);

        // Jika nyawa habis (0 atau kurang), zombie mati
        if (nyawa <= 0)
        {
            Mati();
        }
    }

    void Mati()
    {
        Debug.Log("Zombie mati");
        Destroy(gameObject); // Menghapus zombie dari game
    }
}