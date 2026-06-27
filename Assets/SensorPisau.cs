using UnityEngine;

public class SensorPisau : MonoBehaviour
{
    public Vector2 chara;

    // Berapa damage tebasan pisau karakter ke zombie
    public int damagePisau = 1;

    private void Update()
    {
        chara = new Vector2(transform.position.x, transform.position.y);  
    }

    // Fungsi bawaan Unity yang otomatis jalan jika collider "Is Trigger" menyentuh objek lain
    private void OnTriggerEnter2D(Collider2D objekLain)
    {
        // Cek apakah objek yang disentuh punya Tag "Zombie"
        if (objekLain.CompareTag("Zombie"))
        {
            // Ambil komponen script ZombieAI dari objek zombie yang kita tusuk
            ZombieAI scriptZombie = objekLain.GetComponent<ZombieAI>();

            // Jika script ZombieAI ketemu, suruh zombie menerima damage dan memicu animasi terluka/mati
            if (scriptZombie != null)
            {
                scriptZombie.ZombieTerkenaHit(damagePisau, chara);
            }
        }
    }
}