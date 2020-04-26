/*
 * IHealth.cs
 * Created by: Ambrosia
 * Created on: 8/2/2020 (dd/mm/yy)
 * Created for: requiring some objects to have health
 */

public interface IHealth {
    // Sets health
    void SetHealth (float set);
    // Removes health
    void TakeHealth (float take);
    // Gives health
    void GiveHealth (float give);

    // Returns health
    float GetHealth ();
    // Returns the maximum value the Health variable can be
    float GetMaxHealth ();
}