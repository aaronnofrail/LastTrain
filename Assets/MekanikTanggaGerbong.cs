using UnityEngine;
using UnityEngine.SceneManagement;

public class MekanikTanggaGerbong : MonoBehaviour
{
    [Header("Link Objek Tangga")]
    public GameObject tanggaLantai;       
    public GameObject tanggaDinding;      

    [Header("Pengaturan Pindah Scene")]
    public string namaSceneAtap = "AtapGerbong"; 

    [Header("Pengaturan Audio Pickup")]
    public AudioClip suaraPickup; // Tempat menaruh file Pick_up.wav di Inspector

    private bool zombieSudahHabis = false;
    private bool playerDiDekatObjek = false;

    void Update()
    {
        // 1. Sistem hitung zombie realtime
        if (!zombieSudahHabis)
        {
            GameObject[] semuaZombie = GameObject.FindGameObjectsWithTag("Zombie");
            int zombieYangMasihHidup = 0;

            foreach (GameObject zb in semuaZombie)
            {
                ZombieAI scriptZombie = zb.GetComponent<ZombieAI>();
                if (scriptZombie != null && zb.activeInHierarchy && scriptZombie.enabled) 
                {
                    zombieYangMasihHidup++;
                }
            }

            if (zombieYangMasihHidup == 0)
            {
                zombieSudahHabis = true;
                Debug.Log("Semua zombie binasa! Tekan 'E' pada tangga lantai untuk mengambil.");
            }
        }

        // 2. Cek interaksi tombol E
        if (playerDiDekatObjek && Input.GetKeyDown(KeyCode.E))
        {
            // JIKA INTERAKSI DENGAN TANGGA LANTAI
            if (gameObject.name == "LadderLoot")
            {
                if (zombieSudahHabis)
                {
                    // MAIN KAN SUARA PICKUP SEBELUM DESTROY
                    if (suaraPickup != null)
                    {
                        AudioSource.PlayClipAtPoint(suaraPickup, transform.position);
                    }

                    PasangTanggaKeDinding();
                    Destroy(gameObject); // Objek tangga lantai hancur, suara tetap aman!
                }
                else
                {
                    Debug.Log("Tangga masih terkunci! Selesaikan sisa zombie dulu.");
                }
            }
            
            // JIKA INTERAKSI DENGAN TANGGA DINDING (UNTUK PINDAH SCENE)
            if (gameObject.name == "LadderDinding")
            {
                NaikKeAtapScene();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D objekLain)
    {
        if (objekLain.name == "character" || objekLain.CompareTag("Player"))
        {
            playerDiDekatObjek = true;
        }
    }

    private void OnTriggerExit2D(Collider2D objekLain)
    {
        if (objekLain.name == "character" || objekLain.CompareTag("Player"))
        {
            playerDiDekatObjek = false;
        }
    }

    void PasangTanggaKeDinding()
    {
        if (tanggaDinding != null)
        {
            tanggaDinding.SetActive(true);
        }
    }

    void NaikKeAtapScene()
    {
        GameObject playerObj = GameObject.Find("character");
        if (playerObj != null)
        {
            NyawaPlayer scriptNyawa = playerObj.GetComponent<NyawaPlayer>();
            if (scriptNyawa != null)
            {
                scriptNyawa.currentHealth = scriptNyawa.maxHealth; 
            }
        }
        SceneManager.LoadScene(namaSceneAtap);
    }
}