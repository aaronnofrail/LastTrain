using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [Header("Pengaturan Pergerakan")]
    public float speed = 2f;          // Kecepatan jalan zombie
    public float jarakKejar = 10f;     // Jarak maksimal zombie bisa melihat character
    public float jarakSerang = 1.5f;   // Jarak minimal zombie untuk mulai menyerang

    [Header("Pengaturan Serangan Zombie")]
    public int damageSerang = 1;       // Berapa darah character yang berkurang
    public float jedaSerang = 1.5f;    // Jeda waktu antar serangan (detik)
    private float waktuSerangBerikutnya = 0f;

    [Header("Pengaturan Nyawa Zombie")]
    public int maxHealthZombie = 2;    // Zombie mati setelah 2 kali tebasan pisau
    private int currentHealthZombie;

    [Header("Pengaturan Sound Effect (SFX)")]
    public AudioClip suaraKenaHit;     // Tarik file MonsterPain.wav ke sini
    public AudioClip suaraMati;        // Tarik file MonsterMati.wav ke sini
    private AudioSource audioSource;   // Komponen internal speaker zombie

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

        // Mengambil komponen AudioSource yang menempel pada zombie
        audioSource = GetComponent<AudioSource>();

        // Setel nyawa zombie penuh di awal game
        currentHealthZombie = maxHealthZombie;

        GameObject playerObj = GameObject.Find("character");

        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerHealth = playerObj.GetComponent<NyawaPlayer>();
        }
        else
        {
            Debug.LogError("Objek bernama 'character' tidak ditemukan di Hierarchy! Pastikan tulisannya sama persis.");
        }
    }

    void Update()
    {
        if (playerTarget == null) return;

        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (anim != null) anim.SetFloat("Kecepatan", 0f);
            return;
        }

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
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (anim != null) anim.SetFloat("Kecepatan", 0f);
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

        if (anim != null) anim.SetFloat("Kecepatan", Mathf.Abs(rb.linearVelocity.x));
    }

    void BerhentiDanSerang()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (anim != null) anim.SetFloat("Kecepatan", 0f);

        if (Time.time >= waktuSerangBerikutnya)
        {
            AksiMenyerang();
            waktuSerangBerikutnya = Time.time + jedaSerang;
        }
    }

    void AksiMenyerang()
    {
        Debug.Log("Zombie Menyerang Character!");

        if (anim != null) anim.SetTrigger("serang");

        if (playerHealth != null)
        {
            playerHealth.AmbilDamage(damageSerang);
        }
    }

    // ... (Kodingan atas kamu tetap sama) ...

    public void ZombieTerkenaHit(int damagePisau)
    {
        if (currentHealthZombie <= 0) return;

        currentHealthZombie -= damagePisau;
        Debug.Log("Zombie Terluka! Nyawa Zombie sekarang: " + currentHealthZombie);

        if (currentHealthZombie > 0)
        {
            if (anim != null) anim.SetTrigger("terluka");

            // PENGAMAN: Hanya putar suara Hurt jika speaker tidak sedang sibuk berteriak
            if (audioSource != null && suaraKenaHit != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(suaraKenaHit);
            }
        }
        else
        {
            ZombieMati();
        }
    }

    void ZombieMati()
    {
        Debug.Log("Zombie MATI!");

        if (anim != null) anim.SetTrigger("mati");

        // PENGAMAN MATI: Hentikan suara sakit yang sedang berjalan, ganti dengan suara mati sampai tuntas
        if (audioSource != null && suaraMati != null)
        {
            audioSource.Stop(); // Matikan suara MonsterPain.wav jika masih berbunyi
            audioSource.PlayOneShot(suaraMati);
        }

        // PENTING: Jangan gunakan Destroy(gameObject) di sini agar mayat dan suaranya tidak langsung lenyap!
        this.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }
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
}