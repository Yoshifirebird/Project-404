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
    [Header("Components")]
    [SerializeField] GameObject _DeadObject;

    [Header("Settings")]
    [SerializeField] float _MaxHealth = 10;

    [Header("Health Wheel")]
    [SerializeField] Vector3 _HWOffset = Vector3.up;
    [SerializeField] float _HWScale = 1;

    Animator _Animator;

    List<GameObject> _AttachedPikmin = new List<GameObject>();
    ObjectPooler _ObjectPooler;
    HealthWheel _HWScript;
    float _CurrentHealth = 0;

    void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    void Start()
    {
        _ObjectPooler = ObjectPooler.Instance;
        // Set the current health to the max health
        _CurrentHealth = _MaxHealth;

        // Find a health wheel that hasn't been claimed already
        _HWScript = _ObjectPooler.SpawnFromPool("Health", transform.position + _HWOffset, Quaternion.identity).GetComponentInChildren<HealthWheel>();
        // Apply all of the required variables 
        _HWScript._InUse = true;
        _HWScript._MaxHealth = _MaxHealth;
        _HWScript._CurrentHealth = _MaxHealth;
        _HWScript.transform.SetParent(transform);
        _HWScript.transform.localScale = Vector3.one * _HWScale;
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

            Instantiate(_DeadObject, transform.position, Quaternion.identity);

            // Should be an animation here, but because
            // it was a test we can just destroy the gameObject
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _HWOffset, _HWScale);    
    }

    #region Pikmin Attacking Implementation

    public void Attack(GameObject attacking, float damage)
    {
        // take damage ._.
        TakeHealth(damage);
        _HWScript._CurrentHealth = _CurrentHealth;

        if (_Animator.GetBool("hit") == false)
            _Animator.SetBool("hit", true);
    }

    public void OnAttach(GameObject attachedPikmin) => _AttachedPikmin.Add(attachedPikmin);
    public void OnDetach(GameObject detachedPikmin)
    {
        _AttachedPikmin.Remove(detachedPikmin);

        if (_AttachedPikmin.Count == 0)
            _Animator.SetBool("hit", false);        
    }

    #endregion

    #region Health Implementation

    // 'Getter' functions
    public float GetHealth() => _CurrentHealth;
    public float GetMaxHealth() => _MaxHealth;
    // 'Setter' functions
    public void GiveHealth(float give) => _CurrentHealth += give;
    public void TakeHealth(float take) => _CurrentHealth -= take;
    public void SetHealth(float set) => _CurrentHealth = set;

    #endregion
}
