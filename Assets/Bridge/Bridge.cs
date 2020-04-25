/*
 * Bridge.cs
 * Created by: Ambrosia
 * Created on: 25/4/2020 (dd/mm/yy)
 * Created for: needing an object to cross an area that takes a certain amount of time to complete
 */

using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour, IPikminAttack, IHealth
{
	[Header("Components")]
	[SerializeField] Transform _Destination;
    [SerializeField] Transform _PileObject;

	[Header("Settings")]
	[SerializeField] float _HealthUntilStep = 10;
	float _CurrentHealth = 0;

    // Step Variables
    [SerializeField] float _StepDistance = 1;
    int _CurrentStep = 0;
    int _StepsUntilFinish = 0;

	List<PikminBehavior> _AttackingPikmin = new List<PikminBehavior>();

	void Awake()
	{
		_CurrentHealth = _HealthUntilStep;

        float distance = MathUtil.DistanceTo(transform.position, _Destination.position, false);
        print(distance);
	}
	
	void Update()
	{
		if (_CurrentHealth <= 0)
        {
            _CurrentHealth = _HealthUntilStep;
            print("Building forward!");
            Step();
        }
	}

    void Step()
    {
        
    }

    #region Pikmin Attacking Implementation
    public void Attack(PikminBehavior attacking, float damage)
    {
        // take damage ._.
        TakeHealth(damage);
    }

    public void OnAttackStart(PikminBehavior attachedPikmin)
    {
        _AttackingPikmin.Add(attachedPikmin);

        attachedPikmin.ChangeState(PikminBehavior.States.Attacking);
        attachedPikmin.LatchOntoObject(transform);
    }

    public void OnAttackEnd(PikminBehavior detachedPikmin)
    {
        _AttackingPikmin.Remove(detachedPikmin);
        detachedPikmin.LatchOntoObject(null);
    }
    #endregion

    #region Health Implementation

    // 'Getter' functions
    public float GetHealth() => _CurrentHealth;
	public float GetMaxHealth() => _HealthUntilStep;
	// 'Setter' functions
	public void GiveHealth(float give) => _CurrentHealth += give;
	public void TakeHealth(float take) => _CurrentHealth -= take;
	public void SetHealth(float set) => _CurrentHealth = set;

	#endregion
}
