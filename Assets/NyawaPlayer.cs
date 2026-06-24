using UnityEngine;

public class NyawaPlayer : MonoBehaviour
{
    [Header("Pengaturan Nyawa")]
    public int maxHealth = 5;

    // Diubah menjadi public agar script ZombieAI bisa mengecek apakah player masih hidup
    public int currentHealth;

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        Debug.Log("Nyawa Player Penuh: " + currentHealth + "/" + maxHealth);
    }

    public void AmbilDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Character Terluka! Nyawa sekarang: " + currentHealth);

        if (anim != null && currentHealth > 0)
        {
            anim.SetTrigger("terluka"); // Picu animasi kaget/terluka
        }

        if (currentHealth <= 0)
        {
            Mati();
        }
    }

    void Mati()
    {
        Debug.Log("Character MATI!");
        
        if (anim != null)
        {
            anim.SetTrigger("mati"); 
        }

        // 1. Matikan script gerakan
        GetComponent<Gerakan>().enabled = false; 

        // 2. Bekukan fisik Rigidbody2D agar diam di tempat
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;     
            rb.bodyType = RigidbodyType2D.Static; 
        }

        // 3. Matikan Box Collider 2D agar tidak menopang udara
        BoxCollider2D colliderBadan = GetComponent<BoxCollider2D>();
        if (colliderBadan != null)
        {
            colliderBadan.enabled = false;
        }
    }
}