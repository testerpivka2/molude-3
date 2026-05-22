using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject hintText;

    private bool isPlayerInRange = false;
    private PlayerMove playerMove;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenShop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (hintText != null)
                hintText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (hintText != null)
                hintText.SetActive(false);
            CloseShop();
        }
    }

    void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            Time.timeScale = 0f;

            if (hintText != null)
                hintText.SetActive(false);
                Debug.Log("Подсказка выключена");

            if (playerMove != null)
                playerMove.enabled = false;
        }
    }

    public void CloseShop()
    {

        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Time.timeScale = 1f;

            if (playerMove != null)
            {
                playerMove.enabled = true;
            }

        }
    }
}