using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject hintUI;     // "Нажми E для магазина"
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private string playerTag = "Player";

    private bool playerInside = false;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            bool willOpen = !shopPanel.activeSelf;
            shopPanel.SetActive(willOpen);
            hintUI.SetActive(!willOpen && playerInside);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInside = true;
        if (!shopPanel.activeSelf) hintUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInside = false;
        hintUI.SetActive(false);
        shopPanel.SetActive(false);
    }
}
