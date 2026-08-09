using UnityEngine;

public class DamageAreaMuntah : MonoBehaviour
{
    public int damageMuntah = 1;

    // UBAH NAMA FUNGSINYA DI SINI (dari Stay menjadi Enter)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            NyawaPlayer playerHealth = collision.GetComponent<NyawaPlayer>();
            
            if (playerHealth != null)
            {
                playerHealth.AmbilDamage(damageMuntah);
                Debug.Log("Player terkena muntahan! Darah berkurang.");
            }
        }
    }
}