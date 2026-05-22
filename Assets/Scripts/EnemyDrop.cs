using UnityEngine;

// Повесить на врага. Когда здоровье вызывает Die, дёргаем DropAndDestroy
public class EnemyDrop : MonoBehaviour
{
    [SerializeField] private GameObject goldCoinPrefab;

    public void DropAndDestroy()
    {
        if (goldCoinPrefab != null)
            Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
