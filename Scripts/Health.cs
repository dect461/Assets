using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP = 5;
    public event System.Action OnDeath;
    private int hp;

    void Awake() { hp = maxHP; }

    public void ApplyDamage(int dmg)
    {
        hp -= Mathf.Max(0, dmg);
        if (hp <= 0)
        {
            var game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
            int x = -1, y = -1;
            if (TryGetComponent<ChessMan>(out var cm)) { x = cm.GetXBoard(); y = cm.GetYBoard(); }

            if (x >= 0 && y >= 0) game.SetPositionEmpty(x, y);
            Destroy(gameObject, 0.1f);
            OnDeath?.Invoke();
            
        }
    }
}
