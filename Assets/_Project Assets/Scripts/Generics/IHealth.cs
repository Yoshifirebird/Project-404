/*
 * IHealth.cs
 * Created by: Ambrosia
 * Created on: 8/2/2020 (dd/mm/yy)
 * Created for: requiring a general purpose interface to easily add to scripts
 */

public interface IHealth
{
    // Supposed to set the health
    void SetHealth(int set);
    // Supposed to take some health away
    void TakeHealth(int take);
    // Supposed to add some health
    void GiveHealth(int give);

    // Supposed to return the amount of health an object / entity has
    int GetHealth();
    int GetMaxHealth();
}
