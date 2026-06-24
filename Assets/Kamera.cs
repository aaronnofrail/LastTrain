using UnityEngine;

public class Kamera : MonoBehaviour
{
    // Masukkan objek Player kamu ke kolom ini di Inspector
    public Transform targetPlayer;

    // Seberapa halus gerakan kamera (makin kecil angkanya, makin lambat/halus)
    [SerializeField] private float smoothSpeed = 0.125f;

    // Jarak aman kamera dari Player (untuk game 2D, Z harus tetap minus, misal -10)
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    // Variabel untuk membatasi agar kamera tidak keluar dari gerbong kereta
    [Header("Batas Gerak Kamera (Ujung Gerbong)")]
    public bool gunakanBatas = true;
    public float batasKiri = -5f;
    public float batasKanan = 45f;
    public float batasBawah = -2f;
    public float batasAtas = 5f;

    void LateUpdate()
    {
        // Pastikan Player tidak kosong agar tidak error
        if (targetPlayer == null) return;

        // 1. Tentukan posisi tujuan kamera berdasarkan posisi Player + offset
        Vector3 posisiTujuan = targetPlayer.position + offset;

        // 2. Jika menggunakan pembatas, kunci posisi tujuan agar tidak melewati batas gerbong
        if (gunakanBatas)
        {
            float posisiXDikunci = Mathf.Clamp(posisiTujuan.x, batasKiri, batasKanan);
            float posisiYDikunci = Mathf.Clamp(posisiTujuan.y, batasBawah, batasAtas);
            posisiTujuan = new Vector3(posisiXDikunci, posisiYDikunci, posisiTujuan.z);
        }

        // 3. Gerakkan kamera dari posisi sekarang ke posisi tujuan secara smooth (Lerp)
        Vector3 posisiHalus = Vector3.Lerp(transform.position, posisiTujuan, smoothSpeed);

        // 4. Terapkan posisi baru ke kamera
        transform.position = posisiHalus;
    }
}