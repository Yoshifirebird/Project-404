/*
 * PlayerPikminManager.cs
 * Created by: Ambrosia
 * Created on: 12/2/2020 (dd/mm/yy)
 * Created for: managing the Players Pikmin and the associated data
 */

using UnityEngine;
using System.Collections.Generic;

public class PlayerPikminManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform _FormationCenter;

    [Header("Settings")]
    public float _MaxPikminDistanceForThrow = 5;

    [HideInInspector] public GameObject _ClosestPikmin;

    HashSet<GameObject> _PikminOnField = new HashSet<GameObject>(); // // How many Pikmin there are currently alive
    int _SquadCount = 0;        // How many Pikmin there are currently in the Player's squad

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GetPikminOnFieldCount() > 0)
        {
            
        }
    }

    #region Global Setters
    public void AddPikminOnField(GameObject toAdd) => _PikminOnField.Add(toAdd);
    public void IncrementSquadCount() => _SquadCount++;
    public void RemovePikminOnField(GameObject toRem) => _PikminOnField.Remove(toRem);
    public void DecrementSquadCount() => _SquadCount--;
    #endregion

    #region Global Getters
    public HashSet<GameObject> GetPikminOnField() => _PikminOnField;
    public int GetPikminOnFieldCount() => _PikminOnField.Count;
    public int GetSquadCount() => _SquadCount;
    public Transform GetFormationCenter() => _FormationCenter;
    #endregion
}
