using UnityEngine;

public class Gerakan : MonoBehaviour
{
    [Header("Pengaturan Gerakan")]
    public int Speed;
    public float Jump;
    public Rigidbody2D rd;

    [Header("Pengaturan Lompat & Tanah")]
    public Transform groundCheck;      
    public float radiusCek = 0.2f;     
    public LayerMask layerTanah;       
    private bool sedangDiTanah;        

    [Header("Komponen Kamera")]
    public Camera mainCamera;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public bool isAttack;

    void Start()
    {
        Debug.Log("unity start");
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (groundCheck != null)
        {
            sedangDiTanah = Physics2D.OverlapCircle(groundCheck.position, radiusCek, layerTanah);
        }

        // KIRI DATA DI TANAH KE ANIMATOR
        if (anim != null)
        {
            anim.SetBool("diTanah", sedangDiTanah);
        }

        Bergerak();

        if (Input.GetKeyDown(KeyCode.Space) && sedangDiTanah)
        {
            rd.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);
        }
    }

    void Bergerak()
    {
        BalikBadanKeArahKursor();
        float GerakSamping = Input.GetAxisRaw("Horizontal");
        rd.linearVelocity = new Vector2(GerakSamping * Speed, rd.linearVelocity.y);

        if (anim != null)
        {
            anim.SetFloat("Kecepatan", Mathf.Abs(GerakSamping));
        }

        //if (GerakSamping > 0)
        //{
        //    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        //}
        //else if (GerakSamping < 0)
        //{
        //    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        //}

        if (Input.GetMouseButton(0))
        {
            BalikBadanKeArahKursor();
        }
        else
        {
            if (GerakSamping > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (GerakSamping < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    void BalikBadanKeArahKursor()
    {
        Vector3 posisiMouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (posisiMouse.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (posisiMouse.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, radiusCek);
        }
    }
}