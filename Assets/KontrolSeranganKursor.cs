using UnityEngine;

public class KontrolSeranganKursor : MonoBehaviour
{
    [Header("Komponen Objek")]
    public Transform pivotPisau;       // Masukkan objek PivotPisau ke sini
    public GameObject visualHitboxPisau; // Masukkan objek HitboxPisau ke sini
    public Camera mainCamera;          // Masukkan Main Camera ke sini

    [Header("Pengaturan Jeda Serangan")]
    public float delaySerang = 0.5f;   // Jeda antar serangan (dalam detik)
    public float durasiTebasan = 0.15f; // Berapa lama efek serangan kelihatan di layar

    [Header("Pengaturan Audio Serangan")]
    public AudioClip suaraTebasan;     // Tarik file Player Menyerang Full.mp3 ke sini
    private AudioSource audioSource;   // Speaker internal karakter

    private float waktuSerangBerikutnya = 0f;

    void Start()
    {
        // Pastikan di awal game efek tebasan/hitbox mati
        visualHitboxPisau.SetActive(false);

        // Ambil komponen AudioSource yang menempel pada karakter utama
        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource == null)
        {
            // Jika script ini menempel langsung di objek character utama, ambil AudioSource miliknya sendiri
            audioSource = GetComponent<AudioSource>();
        }

        // Jika kamera belum dimasukkan di Inspector, cari otomatis
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        //PutarPivotKeArahKursor();

        // Cek input klik kiri mouse DAN apakah waktu cooldown/delay sudah selesai
        if (Input.GetMouseButtonDown(0) && Time.time >= waktuSerangBerikutnya)
        {
            Serang();
            waktuSerangBerikutnya = Time.time + delaySerang; // Setel waktu cooldown berikutnya
        }
    }

    void PutarPivotKeArahKursor()
    {
        Vector3 posisiMouseDiLayar = Input.mousePosition;
        Vector3 posisiMouseDiDunia = mainCamera.ScreenToWorldPoint(posisiMouseDiLayar);
        Vector2 arahKeKursor = posisiMouseDiDunia - pivotPisau.position;
        float sudut = Mathf.Atan2(arahKeKursor.y, arahKeKursor.x) * Mathf.Rad2Deg;
        pivotPisau.rotation = Quaternion.Euler(new Vector3(0, 0, sudut));
    }

    void Serang()
    {
        Debug.Log("Player menebas ke arah kursor!");

        // --- AMBIL ANIMATOR DARI INDUK (CHARACTER) DAN PICU TRIGGER ---
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("serang");
        }

        // --- AMBIL AUDIO SOURCE DAN MAINKAN SUARA TEBASAN ---
        if (audioSource != null && suaraTebasan != null)
        {
            audioSource.PlayOneShot(suaraTebasan);
        }
        // ---------------------------------------------------

        visualHitboxPisau.SetActive(true);
        Invoke("MatikanEfekTebasan", durasiTebasan);
    }

    void MatikanEfekTebasan()
    {
        visualHitboxPisau.SetActive(false);
    }
}