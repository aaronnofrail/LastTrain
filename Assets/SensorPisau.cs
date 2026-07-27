using UnityEngine;

public class SensorPisau : MonoBehaviour
{
    // Berapa damage tebasan pisau karakter ke zombie
    public int damagePisau = 1;

    // Fungsi bawaan Unity yang otomatis jalan jika collider "Is Trigger" menyentuh objek lain
    private void OnTriggerEnter2D(Collider2D objekLain)
    {
        // Cek apakah objek yang disentuh punya Tag "Zombie"
        if (objekLain.CompareTag("Zombie"))
        {
            // 1. Coba cari script zombie biasa
            ZombieAI scriptZombie = objekLain.GetComponent<ZombieAI>();
            if (scriptZombie != null)
            {
                scriptZombie.ZombieTerkenaHit(damagePisau);
            }

            // 2. Coba cari script Mini Boss (TAMBAHAN BARU)
            MiniBossAI scriptMiniBoss = objekLain.GetComponent<MiniBossAI>();
            if (scriptMiniBoss != null)
            {
                scriptMiniBoss.ZombieTerkenaHit(damagePisau); 
            }

            // Cari script Last Boss
            LastBossAI scriptLastBoss = objekLain.GetComponent<LastBossAI>();
            if (scriptLastBoss != null)
            {
                scriptLastBoss.ZombieTerkenaHit(damagePisau); 
            }
        }
    }
}