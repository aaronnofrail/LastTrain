using UnityEngine;
using System.Collections; 

public class NyawaPlayer : MonoBehaviour
{
    [Header("Pengaturan Nyawa")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Pengaturan Visual Flash")]
    public Color warnaFlash = Color.red;
    public float durasiFlash = 0.1f;

    private Animator anim;
    private SpriteRenderer spriteRenderer; 
    private Coroutine flashCoroutine; 

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        Debug.Log("Nyawa Player Penuh: " + currentHealth + "/" + maxHealth);
    }

    public void AmbilDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Character Terluka! Nyawa sekarang: " + currentHealth);

        if (anim != null && currentHealth > 0)
        {
            anim.SetTrigger("terluka"); 
        }

        MulaiEfekFlash();

        if (currentHealth <= 0)
        {
            Mati();
        }
    }

    void MulaiEfekFlash()
    {
        if (spriteRenderer == null) return;
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(EfekFlashRoutine());
    }

    IEnumerator EfekFlashRoutine()
    {
        spriteRenderer.color = warnaFlash;           
        yield return new WaitForSeconds(durasiFlash); 
        spriteRenderer.color = Color.white;          
        flashCoroutine = null;
    }

    void Mati()
    {
        Debug.Log("Character MATI!");
        if (anim != null) anim.SetTrigger("mati"); 

        if (GetComponent<Gerakan>() != null) GetComponent<Gerakan>().enabled = false; 

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;     
            rb.bodyType = RigidbodyType2D.Static; 
        }

        BoxCollider2D colliderBadan = GetComponent<BoxCollider2D>();
        if (colliderBadan != null) colliderBadan.enabled = false;
    }
}