using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    [SerializeField] private int minValue = 10;
    [SerializeField] private int maxValue = 20;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        int value = Random.Range(minValue, maxValue + 1);
        GameManager.Instance.AddGold(value);
        Destroy(gameObject);
    }
}
