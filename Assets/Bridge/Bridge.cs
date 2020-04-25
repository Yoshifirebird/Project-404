/*
 * Bridge.cs
 * Created by: Ambrosia
 * Created on: 25/4/2020 (dd/mm/yy)
 * Created for: needing an object to cross an area that takes a certain amount of time to complete
 */

using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour, IPikminAttack, IHealth {
    [Header ("Components")]
    [SerializeField] Transform _Destination;
    [SerializeField] Transform _PileObject;

    [Header ("Prefabs")]
    [SerializeField] GameObject _Endpoint;
    [SerializeField] GameObject _Piece;

    [Header ("Settings")]
    [SerializeField] float _HealthUntilStep = 10;
    [SerializeField] float _HeightCorrection = 1;
    float _CurrentHealth = 0;

    [Header ("Stepping")]
    [SerializeField] float _StepDistance = 1;
    int _CurrentStep = 0;
    int _StepsUntilFinish = 0;

    List<PikminBehavior> _AttackingPikmin = new List<PikminBehavior> ();

    void Awake () {
        _CurrentHealth = _HealthUntilStep;

        float distance = Mathf.Sqrt (MathUtil.DistanceTo (transform.position, _Destination.position, false));
        _StepsUntilFinish = Mathf.FloorToInt (distance / _StepDistance);

        InvokeRepeating ("Debug_DecreaseHealth", 0.1f, 0.01f);
    }

    void Debug_DecreaseHealth () {
        _CurrentHealth = 0;
    }

    void Update () {
        if (_CurrentHealth <= 0) {
            _CurrentHealth = _HealthUntilStep;
            Step ();
        }
    }

    void Step () {

    }

    #region Pikmin Attacking Implementation
    public void Attack (PikminBehavior attacking, float damage) {
        // take damage ._.
        TakeHealth (damage);
    }

    public void OnAttackStart (PikminBehavior attachedPikmin) {
        _AttackingPikmin.Add (attachedPikmin);

        attachedPikmin.ChangeState (PikminBehavior.States.Attacking);
        attachedPikmin.LatchOntoObject (transform);
    }

    public void OnAttackEnd (PikminBehavior detachedPikmin) {
        _AttackingPikmin.Remove (detachedPikmin);
        detachedPikmin.LatchOntoObject (null);
    }
    #endregion

    #region Health Implementation

    // 'Getter' functions
    public float GetHealth () => _CurrentHealth;
    public float GetMaxHealth () => _HealthUntilStep;
    // 'Setter' functions
    public void GiveHealth (float give) => _CurrentHealth += give;
    public void TakeHealth (float take) => _CurrentHealth -= take;
    public void SetHealth (float set) => _CurrentHealth = set;

    #endregion
}