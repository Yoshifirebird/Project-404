/*
 * Bulborb.cs
 * Created by: Ambrosia
 * Created on: 12/4/2020 (dd/mm/yy)
 */

using UnityEngine;
using UnityEngine.AI;

//TODO
[RequireComponent (typeof (EnemyDamageScript), typeof (Rigidbody), typeof (NavMeshAgent))]
public class Bulborb : MonoBehaviour {
    // The order of the states goes from Sleeping -> Dead
    public enum States {
        Sleeping,
        WakingUp,
        Searching,
        Attacking,
        Shaking,
        Dying, // Plays animation, then transition to death
        Dead
    }

    [Header ("Components")]
    [SerializeField] GameObject _DeadObject;
    NavMeshAgent _Agent;

    [Header ("Settings")]
    [SerializeField] float _CheckRadius;
    [SerializeField] float _ShakeForce;

    States _State;
    EnemyDamageScript _DamageScript;

    void Awake () {
        _DamageScript = GetComponent<EnemyDamageScript> ();
        _Agent = GetComponent<NavMeshAgent> ();
    }

    void Update () {
        if (_DamageScript._AttachedPikmin.Count > 0) {
            for (int i = 0; i < _DamageScript._AttachedPikmin.Count; i++) {
                GameObject pikmin = _DamageScript._AttachedPikmin[i];
                PikminBehavior pikminComponent = pikmin.GetComponent<PikminBehavior> ();
                pikminComponent.ChangeState (PikminBehavior.States.ShakenOff);
                pikminComponent.ShakeOff (transform.position, _ShakeForce);
            }
        }
    }
}