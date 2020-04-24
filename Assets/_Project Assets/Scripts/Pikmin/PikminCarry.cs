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
    Transform _EndPoint;
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
        _EndPoint = GameObject.FindGameObjectWithTag("Carry Point").transform;
    }

    void Update()
    {
        if (_IsBeingCarried && Vector3.Distance(transform.position, _EndPoint.position) <= 1)
        {
            _Agent.enabled = false;

            // Make every pikmin stop carrying
            while (_CarryingPikmin.Count > 0)
            {
                _CarryingPikmin[0].ChangeState(PikminBehavior.States.Idle);
            }

            gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _Radius);
    }

    public void OnCarryLeave(PikminBehavior p)
    {
        _CarryingPikmin.Remove(p);
        p.LatchOntoObject(null);

        if (_CarryingPikmin.Count < _BaseAmountRequired && _IsBeingCarried)
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

        p.ChangeState(PikminBehavior.States.Carrying);
        p.LatchOntoObject(transform);

        CalculatePikminPositions();

        if (_CarryingPikmin.Count >= _BaseAmountRequired)
        {
            if (_Agent.enabled == false)
                _Agent.enabled = true;

            _Agent.SetDestination(_EndPoint.position);
            _Agent.speed += _AddedSpeed;
            _IsBeingCarried = true;
        }
    }

    public bool PikminSpotAvailable()
    {
        return _CarryingPikmin.Count < _MaxAmountRequired;
    }

    void CalculatePikminPositions()
    {
        for (int i = 0; i < _CarryingPikmin.Count; i++)
        {
            PikminBehavior pikminObj = _CarryingPikmin[i];
            pikminObj.transform.position = transform.position + (MathUtil._2Dto3D(MathUtil.CalcPosInCirc((uint)_CarryingPikmin.Count, i)) * _Radius);
            pikminObj.transform.rotation = Quaternion.LookRotation(transform.position - pikminObj.transform.position);
        }
    }
}
