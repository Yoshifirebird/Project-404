/*
 * IHealth.cs
 * Created by: Ambrosia
 * Created on: 8/2/2020 (dd/mm/yy)
 * Created for: requiring some objects to have health
 */

public interface IHealth {
    void SetHealth (float set); // Sets health
    void TakeHealth (float take); // Removes health
    void GiveHealth (float give); // Adds health

    float GetHealth ();
    float GetMaxHealth ();
}