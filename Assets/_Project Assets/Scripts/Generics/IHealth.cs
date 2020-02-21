/*
 * IHealth.cs
 * Created by: Ambrosia
 * Created on: 8/2/2020 (dd/mm/yy)
 * Created for: requiring some objects to have health
 */

public interface IHealth
{
    void SetHealth(int set);    // Sets health
    void TakeHealth(int take);  // Removes health
    void GiveHealth(int give);  // Adds health

    int GetHealth();
    int GetMaxHealth();
}
