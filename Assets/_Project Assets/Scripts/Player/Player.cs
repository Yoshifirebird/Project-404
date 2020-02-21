/*
 * Player.cs
 * Created by: Ambrosia
 * Created on: 8/2/2020 (dd/mm/yy)
 * Created for: having a generalised manager for the seperate Player scripts
 */

using UnityEngine;

[RequireComponent(typeof(PlayerUIController),
                  typeof(PlayerPikminManager),
                  typeof(PlayerMovementController))]
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
        _MovementController = GetComponent<PlayerMovementController>();
        _PikminManager = GetComponent<PlayerPikminManager>();

        // Resets the health back to the max if changed in the editor
        _CurrentHealth = _MaxHealth;

        // Singleton Pattern!
        if (player == null)
            player = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // Handle health-related functions
        if (_CurrentHealth <= 0)
            Die();

        // Handle exiting the game/program
        if (Input.GetButtonDown("Exit"))
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
