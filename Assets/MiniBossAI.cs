using UnityEngine;

public class MiniBossAI : MonoBehaviour
{
    [Header("Pengaturan Pergerakan")]
    public float speed = 1.5f;         
    public float jarakKejar = 10f;     
    public float jarakSerang = 2.5f;   

    [Header("Pengaturan Serangan (Muntah)")]
    public float jedaSerang = 3f;      
    private float waktuSerangBerikutnya = 0f;
    private bool sedangMuntah = false;

    [Header("Pengaturan Nyawa Mini Boss")]
    public int maxHealthBoss = 5;      
    private int currentHealthBoss;

    // Komponen internal
    private Transform playerTarget;
    private NyawaPlayer playerHealth;
    private Rigidbody2D rb;
    private Animator anim;
    private bool menghadapKanan = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealthBoss = maxHealthBoss;

        // Mencari karakter utama persis seperti di ZombieAI
        GameObject playerObj = GameObject.Find("character");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerHealth = playerObj.GetComponent<NyawaPlayer>();
        }
    }

    void Update()
    {
        if (playerTarget == null || currentHealthBoss <= 0) return;

        // Jika player sudah mati, Mini Boss diam
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            HentikanGerak();
            return;
        }

        // Jika sedang animasi muntah, jangan jalan
        if (sedangMuntah) return;

        float jarakKePlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (jarakKePlayer <= jarakKejar && jarakKePlayer > jarakSerang)
        {
            KejarPlayer();
        }
        else if (jarakKePlayer <= jarakSerang)
        {
            BerhentiDanSerang();
        }
        else
        {
            HentikanGerak();
        }
    }

    void KejarPlayer()
    {
        // Logika jalan sama persis dengan ZombieAI
        float arah = playerTarget.position.x - transform.position.x;

        if (arah > 0)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            AturArahHadap(true);
        }
        else if (arah < 0)
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            AturArahHadap(false);
        }

        // Ingat: Karena kita pakai IsWalking kemarin, saya panggil IsWalking di sini
        if (anim != null) anim.SetBool("IsWalking", true); 
    }

    void BerhentiDanSerang()
    {
        HentikanGerak();

        if (Time.time >= waktuSerangBerikutnya)
        {
            AksiMuntah();
            waktuSerangBerikutnya = Time.time + jedaSerang;
        }
    }

    void AksiMuntah()
    {
        sedangMuntah = true;
        if (anim != null) anim.SetTrigger("Muntah");
    }

    // Fungsi ini DIPANGGIL LEWAT ANIMATION EVENT di frame terakhir muntah
    public void SelesaiSerang()
    {
        sedangMuntah = false;
    }

    void HentikanGerak()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (anim != null) anim.SetBool("IsWalking", false);
    }

    void AturArahHadap(bool hadapKanan)
    {
        if (hadapKanan != menghadapKanan)
        {
            menghadapKanan = hadapKanan;
            Vector3 skala = transform.localScale;
            skala.x *= -1;
            transform.localScale = skala;
        }
    }

    // Fungsi untuk menerima serangan dari Player
    // Fungsi untuk menerima serangan dari Player
    public void ZombieTerkenaHit(int damagePisau)
    {
        if (currentHealthBoss <= 0) return;

        currentHealthBoss -= damagePisau;
        Debug.Log("Mini Boss Terluka! Nyawa sisa: " + currentHealthBoss);
        
        if (currentHealthBoss <= 0)
        {
            Mati();
        }
    }

    void Mati()
    {
        HentikanGerak();
        this.enabled = false; // Mematikan AI agar tidak bergerak/muntah lagi

        // Mematikan hitbox badannya agar mayatnya bisa dilewati (tidak menghalangi Player)
        if (GetComponent<Collider2D>() != null)
            GetComponent<Collider2D>().enabled = false;

        // Memainkan animasi mati! (Pastikan hurufnya sama persis dengan di Animator nanti)
        if (anim != null) anim.SetTrigger("mati"); 
    }
}