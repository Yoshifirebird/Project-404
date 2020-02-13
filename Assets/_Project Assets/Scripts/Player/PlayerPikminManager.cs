/*
 * PlayerPikminManager.cs
 * Created by: Ambrosia
 * Created on: 12/2/2020 (dd/mm/yy)
 * Created for: managing the Players Pikmin and the associated data
 */

using UnityEngine;

public class PlayerPikminManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform _FormationCenter;

    int _PikminOnField = 0;     // How many Pikmin there are currently alive
    int _SquadCount = 0;        // How many Pikmin there are currently in the Player's squad

    #region Global Setters
    public void IncrementPikminOnField() => _PikminOnField++;
    public void IncrementSquadCount() => _SquadCount++;
    public void DecrementPikminOnField() => _PikminOnField--;
    public void DecrementSquadCount() => _SquadCount--;
    #endregion

    #region Global Getters
    public int GetPikminOnField() => _PikminOnField;
    public int GetSquadCount() => _SquadCount;
    public Transform GetFormationCenter() => _FormationCenter;
    #endregion
}
