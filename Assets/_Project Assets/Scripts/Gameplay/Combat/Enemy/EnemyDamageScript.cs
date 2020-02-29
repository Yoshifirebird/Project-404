/*
 * EnemyDamageScript.cs
 * Created by: Neo, Ambrosia
 * Created on: 15/2/2020 (dd/mm/yy)
 * Created for: Generic enemy health manager script
 */

using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageScript : MonoBehaviour, IPikminAttack, IHealth
{
    [Header("Settings")]
    [SerializeField] int _MaxHealth = 10;
    [SerializeField] float _HeightOffset = 3.5f;

    readonly List<GameObject> _AttachedPikmin = new List<GameObject>();
    ObjectPooler _ObjectPooler;
    HealthWheel _HWScript;
    int _CurrentHealth = 0;

    void Start()
    {
        _ObjectPooler = ObjectPooler.Instance;
        // Set the current health to the max health
        _CurrentHealth = _MaxHealth;
        // Find a health wheel that hasn't been claimed already

        _HWScript = _ObjectPooler.SpawnFromPool("Health", transform.position + (Vector3.up * _HeightOffset), Quaternion.identity).GetComponentInChildren<HealthWheel>();
            
        // Apply all of the required variables 
        _HWScript._InUse = true;
        _HWScript._MaxHealth = _MaxHealth;
        _HWScript._CurrentHealth = _MaxHealth;
        _HWScript.transform.SetParent(transform);

    }

    void Update ()
    {
        // Check if we're dead, if so then detach the pikmin and destroy ourself
        if (_CurrentHealth <= 0)
        {
            foreach (GameObject attached in _AttachedPikmin)
            {
                // Make the Pikmin's parent null, so they don't
                // follow us wherever we go
                attached.transform.parent = null;
            }

            // Should be an animation here, but because
            // it was a test we can just destroy the gameObject
            Destroy(gameObject);
        }
    }

    #region Pikmin Attacking Implementation

    public void Attack(GameObject attacking, int damage)
    {
        // take damage ._.
        TakeHealth(damage);
        _HWScript._CurrentHealth = _CurrentHealth;
    }

    public void OnAttach(GameObject attachedPikmin) => _AttachedPikmin.Add(attachedPikmin);
    public void OnDetach(GameObject detachedPikmin) => _AttachedPikmin.Remove(detachedPikmin);

    #endregion

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
