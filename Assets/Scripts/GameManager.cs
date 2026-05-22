using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Стартовые значения")]
    [SerializeField] private int startGold = 0;
    [SerializeField] private int startDamage = 10;
    [SerializeField] private int startHeals = 0;

    public int Gold { get; private set; }
    public int Damage { get; private set; }
    public int Heals { get; private set; }

    // События для обновления UI (HUD, магазин и т.п.)
    public UnityEvent OnStatsChanged = new UnityEvent();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetData();
    }

    // Вызывается вручную при выходе в главное меню
    public void ResetData()
    {
        Gold = startGold;
        Damage = startDamage;
        Heals = startHeals;
        OnStatsChanged.Invoke();
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        OnStatsChanged.Invoke();
    }

    // Возвращает true, если денег хватило и покупка прошла
    public bool TrySpend(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        OnStatsChanged.Invoke();
        return true;
    }

    public void AddDamage(int amount)
    {
        Damage += amount;
        OnStatsChanged.Invoke();
    }

    public void AddHeals(int amount)
    {
        Heals += amount;
        OnStatsChanged.Invoke();
    }
}
