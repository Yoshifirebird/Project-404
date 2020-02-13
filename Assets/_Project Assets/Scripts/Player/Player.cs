/*
 * Player.cs
 * Created by: Ambrosia
 * Created on: 8/2/2020 (dd/mm/yy)
 * Created for: having a generalised manager for the seperate Player scripts
 */

using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
//[RequireComponent(typeof(PlayerUIController))]
public class Player : MonoBehaviour, IHealth
{
    // Singleton
    public static Player player;

    //[Header("Components")]
    PlayerMovementController _MovementController;
    PlayerPikminManager _PikminManager;

    [Header("Settings")]
    [SerializeField] int _MaxHealth = 100;
    [SerializeField] int _CurrentHealth = 100;

    void Awake()
    {
        _CurrentHealth = _MaxHealth; // Reset health back to Max Health
        _MovementController = GetComponent<PlayerMovementController>();
        _PikminManager = GetComponent<PlayerPikminManager>();

        if (player == null)
            player = this;
    }

    void Update()
    {
        // Handle health
        if (_CurrentHealth <= 0)
            // If the health is less than or equal to 0, we're dead so play the dead function!
            Die();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Break();
            Application.Quit();
        }
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

    #region Global Getters
    public PlayerMovementController GetMovementController() => _MovementController;
    public PlayerPikminManager GetPikminManager() => _PikminManager;
    #endregion
}
