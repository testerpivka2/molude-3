using UnityEngine;
using UnityEngine.UI;

public class BarScript: MonoBehaviour
{
    private float HP=100f;
    public Image bar;
    public void Start()
    {

    }

    void Update()
    {
        
    }
    private void CheckContact(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            HP -= 5;
            bar.fillAmount = HP / 100f;
        }
    }
}
