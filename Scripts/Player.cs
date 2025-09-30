using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int hpCount;
    [SerializeField] private int maxHP;
    [SerializeField] private int coins;

    bool IsAlive;

    void Start()
    {
        GameObject.FindGameObjectWithTag("HpUI").GetComponent<TextMeshProUGUI>().text = $"Hp: {hpCount} / {maxHP}";
        GameObject.FindGameObjectWithTag("CoinsUI").GetComponent<TextMeshProUGUI>().text = $"Coins: {coins}";
    }

    // Геттеры и сеттеры для HP
    public int HpCount
    {
        get { return hpCount; }
        set
        {
            hpCount = value;
            UpdateHpUI();
        }
    }

    public int MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = value;
            UpdateHpUI();
        }
    }

    // Геттеры и сеттеры для монет
    public int Coins
    {
        get { return coins; }
        set
        {
            coins = value;
            UpdateCoinsUI();
        }
    }

    // Методы для обновления UI
    private void UpdateHpUI()
    {
        GameObject hpUI = GameObject.FindGameObjectWithTag("HpUI");
        if (hpUI != null)
            hpUI.GetComponent<TextMeshProUGUI>().text = $"Hp: {hpCount} / {maxHP}";
        if (hpCount <= 0)
        {
            Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
            game.Lose();
        }
    }

    private void UpdateCoinsUI()
    {
        IsAlive = hpCount > 0;
        GameObject coinsUI = GameObject.FindGameObjectWithTag("CoinsUI");
        if (coinsUI != null)
            coinsUI.GetComponent<TextMeshProUGUI>().text = $"Coins: {coins}";
    }

    // Дополнительные методы для удобства
    public void AddCoins(int amount)
    {
        Coins += amount;
        UpdateCoinsUI();
    }

    public void TakeDamage(int damage)
    {
        HpCount = Mathf.Max(0, HpCount - damage);
        UpdateHpUI();
    }

    //public void Heal(int healAmount)
    //{
    //    HpCount = Mathf.Min(MaxHP, HpCount + healAmount);
    //}

    
}