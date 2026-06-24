using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private Vector2 knockbackDirection;
    private float knockbackTime;

    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private void Update()
    {
        if (knockbackTime > 0)
        {
            transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
            knockbackTime -= Time.deltaTime;
        }
    }

    public void TakeDamage(Vector2 attackerPosition)
    {
        knockbackDirection = ((Vector2)transform.position - attackerPosition).normalized;
        knockbackTime = knockbackDuration;
    }

}
