using UnityEngine;

public class LastBossAI : MonoBehaviour
{
    [Header("Pengaturan Pergerakan")]
    public float speed = 2f;
    public float jarakKejar = 12f;

    [Header("Serangan Jarak Dekat (Melee)")]
    public float jarakMelee = 1.5f;
    public int damageMelee = 2;
    public float jedaMelee = 1.5f;
    private float waktuMeleeBerikutnya = 0f;

    [Header("Serangan Jarak Jauh (Muntah)")]
    public float jarakMuntah = 5f;
    public float jedaMuntah = 4f;
    private float waktuMuntahBerikutnya = 0f;

    [Header("Pengaturan Nyawa Last Boss")]
    public int maxHealthBoss = 20;
    private int currentHealthBoss;

    private Transform playerTarget;
    private NyawaPlayer playerHealth;
    private Rigidbody2D rb;
    private Animator anim;
    private bool menghadapKanan = false;
    private bool sedangMenyerang = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealthBoss = maxHealthBoss;

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

        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            HentikanGerak();
            return;
        }

        if (sedangMenyerang) return;

        float jarakKePlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (jarakKePlayer <= jarakMelee)
        {
            if (Time.time >= waktuMeleeBerikutnya)
            {
                SeranganMelee();
            }
            else
            {
                HentikanGerak();
            }
        }
        else if (jarakKePlayer <= jarakMuntah)
        {
            if (Time.time >= waktuMuntahBerikutnya)
            {
                SeranganMuntah();
            }
            else
            {
                KejarPlayer();
            }
        }
        else if (jarakKePlayer <= jarakKejar)
        {
            KejarPlayer();
        }
        else
        {
            HentikanGerak();
        }
    }

    void KejarPlayer()
    {
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

        if (anim != null) anim.SetBool("IsWalking", true);
    }

    void SeranganMelee()
    {
        HentikanGerak();
        sedangMenyerang = true;
        waktuMeleeBerikutnya = Time.time + jedaMelee;

        if (anim != null) anim.SetTrigger("SerangMelee");

        if (playerHealth != null)
        {
            playerHealth.AmbilDamage(damageMelee);
        }
    }

    void SeranganMuntah()
    {
        HentikanGerak();
        sedangMenyerang = true;
        waktuMuntahBerikutnya = Time.time + jedaMuntah;

        if (anim != null) anim.SetTrigger("Muntah");
    }

    // Dipanggil lewat Animation Event
    public void SelesaiSerang()
    {
        sedangMenyerang = false;
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

    public void ZombieTerkenaHit(int damagePisau)
    {
        if (currentHealthBoss <= 0) return;

        currentHealthBoss -= damagePisau;

        if (currentHealthBoss <= 0)
        {
            Mati();
        }
    }

    void Mati()
    {
        HentikanGerak();
        this.enabled = false;

        if (GetComponent<Collider2D>() != null)
            GetComponent<Collider2D>().enabled = false;

        if (anim != null) anim.SetTrigger("mati");
    }
}