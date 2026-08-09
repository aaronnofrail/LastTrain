using UnityEngine;
using UnityEngine.UI; // Wajib untuk mendeteksi komponen UI Image

public class UIBackgroundBergerak : MonoBehaviour
{
    [Header("Pengaturan Kecepatan")]
    public float kecepatanBergerak = 100f; // Kecepatan pergeseran pixel UI

    private Image gambarUI;
    private RectTransform rectTransform;
    private Vector2 posisiAwal;
    private float lebarGambar;

    void Start()
    {
        gambarUI = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        
        posisiAwal = rectTransform.anchoredPosition;
        
        // Mengambil lebar asli sprite gambar agar tahu kapan harus reset posisi
        if (gambarUI != null && gambarUI.sprite != null)
        {
            lebarGambar = gambarUI.sprite.rect.width * rectTransform.localScale.x;
        }
        else
        {
            lebarGambar = rectTransform.rect.width;
        }
    }

    void Update()
    {
        // Geser posisi X koordinat UI Canvas secara konstan berdasarkan waktu
        rectTransform.anchoredPosition += Vector2.left * kecepatanBergerak * Time.deltaTime;

        // Jika gambar sudah bergeser sejauh lebarnya sendiri, kembalikan ke posisi semula (Looping Sempurna)
        if (Mathf.Abs(rectTransform.anchoredPosition.x - posisiAwal.x) >= lebarGambar)
        {
            rectTransform.anchoredPosition = posisiAwal;
        }
    }
}