
using System;

/// <summary>
/// Обработка денежных транзакций
/// </summary>
public static class MoneyManager
{
    public static int MoneyCount
    {
        get => SaveManager.Data.MoneyCount;
        set
        {
            if (value < 0) return;
            int old = MoneyCount;
            SaveManager.Data.MoneyCount = value;
            OnMoneyCountChanged?.Invoke(old, value);
        }
    }
    public static event Action<int, int> OnMoneyCountChanged;
}
