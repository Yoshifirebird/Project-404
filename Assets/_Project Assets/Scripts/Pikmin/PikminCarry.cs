/*
 * PikminCarry.cs
 * Created by: Ambrosia, Kman
 * Created on: 11/4/2020 (dd/mm/yy)
 * Created for: testing how carrying would work
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PikminCarry : MonoBehaviour, IPikminCarry
{
	List<GameObject> _EndPoints;
    Transform _TargetPoint;
	NavMeshAgent _Agent;

	[Header("Settings")]
	[SerializeField] int _BaseAmountRequired;
	[SerializeField] int _MaxAmountRequired;
	[SerializeField] float _Speed;
	[SerializeField] float _AddedSpeed;
	public float _Radius;

    bool _IsBeingCarried = false;

	List<PikminBehavior> _CarryingPikmin = new List<PikminBehavior>();

	void Awake()
	{
		_Agent = GetComponent<NavMeshAgent>();
		_Agent.updateRotation = false;
		_Agent.speed = _Speed;
		_Agent.enabled = false;
        _EndPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Carry Point"));
    }

	void Update()
	{
        for (int i = 0; i < _CarryingPikmin.Count; i++)
        {
            PikminBehavior pikminObj = _CarryingPikmin[i];
            pikminObj.transform.position = transform.position + (MathUtil._2Dto3D(MathUtil.CalcPosInCirc((uint)_CarryingPikmin.Count, i)) * _Radius);
            pikminObj.transform.rotation = Quaternion.LookRotation(transform.position - pikminObj.transform.position);
        }

        if (_IsBeingCarried && Vector3.Distance(transform.position, _TargetPoint.position) <= 1)
		{
            Deliver();
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, _Radius);
	}

    private void Deliver()
    {
        _Agent.enabled = false;

        // Make every pikmin stop carrying
        while (_CarryingPikmin.Count > 0)
        {
            OnCarryLeave(_CarryingPikmin[0]);
        }

        gameObject.SetActive(false);
    }

	public void OnCarryLeave(PikminBehavior p)
	{
		_CarryingPikmin.Remove(p);
        CalculatePikminPosition();

        if (_CarryingPikmin.Count < _BaseAmountRequired)
		{
			_Agent.enabled = false;
            _IsBeingCarried = false;
        }
	}

	public void OnCarryStart(PikminBehavior p)
	{
		if (_CarryingPikmin.Count >= _MaxAmountRequired)
		{
			OnCarryLeave(p);
			p.ChangeState(PikminBehavior.States.Idle);
			return;
		}

		_CarryingPikmin.Add(p);

		p.LatchOntoObject(transform);
		p.ChangeState(PikminBehavior.States.Carrying);

        CalculatePikminPosition();

        if (_CarryingPikmin.Count >= _BaseAmountRequired)
		{
            _TargetPoint = FindNearestEndPoint();

            if(_TargetPoint == null)
            {
                print("No End Point Exists!!!");
                return;
            }

			if (_Agent.enabled == false)
				_Agent.enabled = true;

            _Agent.SetDestination(_TargetPoint.position);
            _Agent.speed += _AddedSpeed;
            _IsBeingCarried = true;
		}
    }

    public Transform FindNearestEndPoint()
    {
        Transform target = null;
        float distance = Mathf.Infinity;
        foreach(GameObject point in _EndPoints)
        {
            float tDistance = Vector3.Distance(point.transform.position, transform.position);
            if (tDistance < distance)
            {
                target = point.transform;
                distance = tDistance;
            }
        }
        return target;
    }

    public void CalculatePikminPosition()
    {
        
    }


	public bool PikminSpotAvailable()
	{
		return _CarryingPikmin.Count < _MaxAmountRequired;
	}
}
