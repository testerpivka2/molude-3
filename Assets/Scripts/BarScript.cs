using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private float HP = 100f;
    private Image bar;

    void Start()
    {
        bar = GameObject.Find("HPBar").GetComponent<Image>();
    }

    void Update()
    {
        if (HP < 100)
        {
            HP += 0.01f;
        }


        bar.fillAmount = HP / 100f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && HP > 0)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                float damage = enemy.damage; 

                if (HP < damage)
                {
                    HP = 0;
                }
                else
                {
                    HP -= damage;
                }

            }
        }
    }
}