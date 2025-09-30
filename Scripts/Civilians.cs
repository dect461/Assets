using UnityEngine;

public class Civilians : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }
}
