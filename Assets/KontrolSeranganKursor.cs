using System.Collections;
using Unity.VisualScripting;
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

    public float cooldownATK = 1;

    private float coolDownTime;

    private NyawaPlayer NyawaPlayer; //ambil class nyawaPlayer

    private Gerakan gerakan;

    private int startSpeed;

    private void Awake()
    {
        NyawaPlayer = GetComponent<NyawaPlayer>();

        gerakan = GetComponent<Gerakan>();
    }

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

        startSpeed = gerakan.Speed;
    }

    void Update()
    {
        PutarPivotKeArahKursor();

        //player hanya bisa serang jika currentHealth masih ada
        if (NyawaPlayer.currentHealth > 0)
        {
            // Cek input klik kiri mouse DAN apakah waktu cooldown/delay sudah selesai
            //if (Input.GetMouseButtonDown(0) && Time.time >= waktuSerangBerikutnya)
            //{
            //    Serang();
            //    waktuSerangBerikutnya = Time.time + delaySerang; // Setel waktu cooldown berikutnya
            //}

            if(coolDownTime > 0)
            {
                coolDownTime -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0) && coolDownTime <= 0)
            {
                gerakan.Speed = 0;
                Serang();
                coolDownTime = cooldownATK;
            }
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

        StartCoroutine(FreezeWhileATK());
    }

    void MatikanEfekTebasan()
    {
        visualHitboxPisau.SetActive(false);
    }

    IEnumerator FreezeWhileATK()
    {
        yield return new WaitForSeconds(0.5f);
        gerakan.Speed = startSpeed;
        yield return new WaitForSeconds(0.5f);
    }
}