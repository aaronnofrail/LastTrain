using UnityEngine;

public class BackgroundBergerak : MonoBehaviour
{
    [Header("Pengaturan Kecepatan")]
    public float kecepatanBergerak = 0.2f; 

    private SpriteRenderer spriteRenderer;
    private Vector2 offsetOtomatis;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offsetOtomatis = Vector2.zero;
    }

    void Update()
    {
        // Geser koordinat X berdasarkan waktu
        offsetOtomatis.x += kecepatanBergerak * Time.deltaTime;

        // KODE KHUSUS URP: Menggeser offset texture menggunakan SetTextureOffset
        if (spriteRenderer != null && spriteRenderer.material != null)
        {
            spriteRenderer.material.SetTextureOffset("_BaseMap", offsetOtomatis);
        }
    }
}