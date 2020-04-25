/*
 * EnemyDamageScript.cs
 * Created by: Neo, Ambrosia
 * Created on: 15/2/2020 (dd/mm/yy)
 * Created for: Generic enemy health manager script
 */

using System.Collections.Generic;
using UnityEngine;

// Requires animator, and inside of the animator requires there to be a "hit" bool
[RequireComponent(typeof(Animator))]
public class EnemyDamageScript : MonoBehaviour, IPikminAttack, IHealth
{
    [Header("ENABLE WHEN NOT USED FOR GAMEPLAY")]
    [SerializeField] bool _Showcase = false;

    [Header("Settings")]
    [SerializeField] float _MaxHealth = 10;
    [SerializeField] bool _HandleDeath = false;
    [SerializeField] GameObject _DeadObject;

    [Header("Health Wheel")]
    [SerializeField] Vector3 _HWOffset = Vector3.up;
    [SerializeField] float _HWScale = 1;

    Animator _Animator;

    [HideInInspector]
    public List<GameObject> _AttachedPikmin = new List<GameObject>();
    [HideInInspector] public bool _Dead;
    ObjectPooler _ObjectPooler;
    HealthWheel _HWScript;
    float _CurrentHealth = 0;

    void Awake()
    {
        _Animator = GetComponent<Animator>();
        _CurrentHealth = _MaxHealth;
    }

    void Start()
    {
        if (_Showcase == false)
        {
            _ObjectPooler = ObjectPooler.Instance;
            // Set the current health to the max health

            // Find a health wheel that hasn't been claimed already
            _HWScript = _ObjectPooler.SpawnFromPool("Health", transform.position + _HWOffset, Quaternion.identity).GetComponentInChildren<HealthWheel>();
            // Apply all of the required variables 
            _HWScript._InUse = true;
            _HWScript._MaxHealth = _MaxHealth;
            _HWScript._CurrentHealth = _MaxHealth;
            _HWScript.transform.SetParent(transform);
            _HWScript.transform.localScale = Vector3.one * _HWScale;
        }
    }

    void Update()
    {
        if (_AttachedPikmin.Count == 0 && _Animator.GetBool("hit"))
        {
            _Animator.SetBool("hit", true);
        }

        // Check if we're dead, if so then detach the pikmin and destroy ourself
        if (_CurrentHealth <= 0)
        {
            foreach (GameObject attached in _AttachedPikmin)
            {
                // Make the Pikmin's parent null, so they don't
                // follow us wherever we go
                attached.transform.parent = null;
            }

            _Dead = true;

            if (_HandleDeath)
            {
                Instantiate(_DeadObject, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _HWOffset, _HWScale);
    }

    #region Pikmin Attacking Implementation

    public void Attack(PikminBehavior attacking, float damage)
    {
        // take damage ._.
        TakeHealth(damage);
        _HWScript._CurrentHealth = _CurrentHealth;

        if (_Animator.GetBool("hit") == false)
        {
            _Animator.SetBool("hit", true);
        }
    }

    public void OnAttackStart(PikminBehavior attachedPikmin)
    {
        _AttachedPikmin.Add(attachedPikmin.gameObject);

        attachedPikmin.ChangeState(PikminBehavior.States.Attacking);
        attachedPikmin.LatchOntoObject(transform);
    }

    public void OnAttackEnd(PikminBehavior detachedPikmin)
    {
        _AttachedPikmin.Remove(detachedPikmin.gameObject);
        detachedPikmin.LatchOntoObject(null);
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
