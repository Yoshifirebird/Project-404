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

    [Header("Prefabs")]
    [SerializeField] GameObject _Endpoint;
    [SerializeField] GameObject _Piece;

	[Header("Settings")]
	[SerializeField] float _HealthUntilStep = 10;
    [SerializeField] float _HeightCorrection = 1;
	float _CurrentHealth = 0;

    [Header("Stepping")]
    [SerializeField] float _StepDistance = 1;
    int _StepsUntilFinish = 0;

	List<PikminBehavior> _AttackingPikmin = new List<PikminBehavior>();

	void Awake()
	{
        // Make the Pile look at the destination, so we can step towards it
        _PileObject.LookAt(_Destination.position);

		_CurrentHealth = _HealthUntilStep;

        float distance = Mathf.Sqrt(MathUtil.DistanceTo(transform.position, _Destination.position, false));
        _StepsUntilFinish = Mathf.CeilToInt(distance / _StepDistance);
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
        _StepsUntilFinish--;
        
        if (_StepsUntilFinish == 0)
        {
            Vector3 finalPos = Vector3.MoveTowards(_PileObject.position, _Destination.position, _StepDistance);
            Quaternion finalRot = Quaternion.LookRotation(finalPos - _PileObject.position);

            Instantiate(_Endpoint, finalPos, finalRot);

            print("Arrived at destination :D");
            // De-activate the bridge behaviour as it's been built
            Destroy(_PileObject.gameObject);
            Destroy(gameObject);
        }

        Vector3 nextPos = Vector3.MoveTowards(_PileObject.position, _Destination.position + Vector3.up * _HeightCorrection, _StepDistance);
        Quaternion nextRot = Quaternion.LookRotation(nextPos - _PileObject.position);

        nextRot = Quaternion.Euler(nextRot.eulerAngles.x - 26, nextRot.eulerAngles.y, nextRot.eulerAngles.z);
        Instantiate(_Piece, _PileObject.position, nextRot);
        _PileObject.rotation = nextRot;
        _PileObject.position = nextPos;
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
