/*
 * Player.cs
 * Created by: Ambrosia
 * Created on: 8/2/2020 (dd/mm/yy)
 * Created for: having a generalised manager for the seperate Player scripts
 */

using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerUIController))]
public class Player : MonoBehaviour, IHealth
{
    // Singleton
    public static Player player;

    //[Header("Components")]
    PlayerMovementController _MovementController;

    [Header("Settings")]
    [SerializeField] int _MaxHealth = 100;
    [SerializeField] int _CurrentHealth = 100;

    void Awake()
    {
        // In case the current health or max health was changed in the inspector
        _CurrentHealth = _MaxHealth;
        // Grab the PlayerMovementController component on the player
        _MovementController = GetComponent<PlayerMovementController>();

        if (player == null)
            player = this;
    }

    void Update()
    {
        // Handle health
        if (_CurrentHealth <= 0)
            // If the health is less than or equal to 0, we're dead so play the dead function!
            Die();
    }

    void Die()
    {
        Debug.Log("Player is dead!");
        Debug.Break();
    }

    #region Health Implementation

    // 'Getter' functions
    public int GetHealth() => _CurrentHealth;
    public int GetMaxHealth() => _MaxHealth;
    // 'Setter' functions
    public void GiveHealth(int give) => _CurrentHealth += give;
    public void TakeHealth(int take) => _CurrentHealth -= take;
    public void SetHealth(int set) => _CurrentHealth = set;

    #endregion
}
