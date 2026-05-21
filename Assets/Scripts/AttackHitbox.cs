using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 10;
    public string[] targetTags = { "Enemy" };

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in targetTags)
        {
            if (other.CompareTag(tag))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); 
                }
                break;
            }
        }
    }
}