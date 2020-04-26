/*
 * PikminBehavior.cs
 * Created by: Neo, Ambrosia, Kman
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: making the Pikmin have Artificial Intelligence
 */

using UnityEngine;

// TODO: Optimise!
[RequireComponent (typeof (Rigidbody))]
public class PikminBehavior : MonoBehaviour, IPooledObject {
    public enum States {
        Idle,
        Formation,
        Attacking,
        AttackAndFollow,
        Carrying,
        Held,
        ShakenOff,
        Thrown,

        Dead,
    }

    States _State;
    States _PreviousState;

    [Header ("Components")]
    public PikminSO _Data;
    [HideInInspector] public bool _InSquad;
    Player _Player;
    PlayerPikminManager _PlayerPikminManager;
    Rigidbody _Rigidbody;
    Collider _Collider;
    Animator _Animator;

    [Header ("Head Types")]
    [SerializeField] Transform _LeafSpawn;
    [SerializeField] Headtype _StartingHeadType;
    GameObject[] _HeadTypeModels;
    Headtype _CurrentHeadType;

    // State specific variables

    float _AttackTimer = 0;
    GameObject _AttackingObject;
    IPikminAttack _AttackingData;

    GameObject _CarryingObject;
    IPikminCarry _CarryingData;

    Collider _ObjectCollider;

    bool _Spawned = false;

    void Spawn () {
        if (_Spawned)
            return;
        _Spawned = true;

        // Assign required variables if needed
        if (_Player == null) {
            _Player = Globals._Player;
            _PlayerPikminManager = _Player.GetPikminManager ();
        }

        if (_Collider == null)
            _Collider = GetComponent<Collider> ();

        if (_Rigidbody == null)
            _Rigidbody = GetComponent<Rigidbody> ();

        if (_Animator == null)
            _Animator = GetComponent<Animator> ();

        // Add ourself into the stats
        /*PlayerStats.GetFieldStats(_Data._Colour, out PlayerStats.PikminStats stats);
        stats.Increment (_CurrentHeadType);

        PlayerStats.GetTotalStats(_Data._Colour, out stats);
        stats.Increment (_CurrentHeadType);*/

        PlayerStats._OnField.Add (this);

        // Reset state machines
        _State = States.Idle;
        _PreviousState = States.Idle;

        // Reset state-specific variables
        _AttackingObject = null;
        _CarryingObject = null;
        _AttackTimer = 0;

        _HeadTypeModels = new GameObject[(int) Headtype.SIZE];
        _HeadTypeModels[0] = Instantiate (_Data._Leaf, _LeafSpawn);
        _HeadTypeModels[1] = Instantiate (_Data._Bud, _LeafSpawn);
        _HeadTypeModels[2] = Instantiate (_Data._Flower, _LeafSpawn);

        SetHead (_StartingHeadType);
    }

    void Start () {
        Spawn ();
    }

    void IPooledObject.OnObjectSpawn () {
        Spawn ();
    }

    void Update () {
        // Check if we've been attacking and we still have a valid attacking object
        if (_PreviousState == States.Attacking && _AttackingObject != null) {
            // Call the OnDetach function
            IPikminAttack aInterface = _AttackingObject.GetComponent<IPikminAttack> ();

            if (aInterface != null)
                aInterface.OnAttackEnd (this);

            // Remove the attacking object and reset the timer
            _AttackingObject = null;
            _AttackTimer = 0;
        }

        switch (_State) {
            case States.Idle:
                HandleIdle ();
                break;
            case States.Formation:
                HandleFormation ();
                break;
            case States.Attacking:
                HandleAttacking ();
                break;
            case States.AttackAndFollow:
                MoveTowards (_ObjectCollider.ClosestPoint (transform.position));
                HandleAttacking ();
                break;
            case States.Dead:
                HandleDeath ();
                break;

            case States.Carrying:
            case States.Held:
            case States.Thrown:
            default:
                break;
        }
    }

    void LateUpdate () {
        HandleAnimation ();
    }

    void OnCollisionEnter (Collision collision) {
        if (_State == States.Thrown) {
            Transform colTransform = collision.transform;

            // Explicitly check because we don't want to land on another Pikmin or the Player
            if (colTransform.CompareTag ("Pikmin") == false && colTransform.CompareTag ("Player") == false) {
                ChangeState (States.Idle);
            }

            if (colTransform.CompareTag ("Interactable")) {
                CheckForAttack (collision.gameObject);
                CheckForCarry (collision.gameObject);

                // If it was neither Attack or Carry, then just set it to idle
                if (_State == States.Thrown) {
                    ChangeState (States.Idle);
                }
            }
        } else if (_State == States.ShakenOff) {
            ChangeState (States.Attacking);
        } else if (_InSquad == false && collision.transform.CompareTag ("Player")) {
            // Player has collided with us and we're not in squad!
            AddToSquad ();
        }

        if (_State != States.Carrying && collision.transform.CompareTag ("Interactable")) {
            if (GetComponent<IPikminCarry> () != null) {
                // TODO: Push pikmin away from carriable object
            }
        }
    }

    void HandleAnimation () {
        _Animator.SetBool ("Thrown", _State == States.Thrown);

        if (_State == States.Idle || _State == States.Formation) {
            Vector2 horizonalVelocity = new Vector2 (_Rigidbody.velocity.x, _Rigidbody.velocity.z);
            _Animator.SetBool ("Walking", horizonalVelocity.magnitude >= 3);
        }
    }

    void HandleIdle () {
        // Look for a target object if there isn't one
        if (_CarryingObject == null || _AttackingObject == null) {
            //Check every object and see if we can do anything with it
            Collider[] surroundings = Physics.OverlapSphere (transform.position, _Data._SearchRange);
            foreach (Collider obj in surroundings) {
                // Handle Interactable objects
                if (!obj.CompareTag ("Interactable")) {
                    continue;
                }

                _CarryingData = obj.GetComponent<IPikminCarry> ();
                if (_CarryingData != null) {
                    // If there is a spot for the Pikmin to carry
                    if (_CarryingData.PikminSpotAvailable ()) {
                        // Works out the height difference between the two objects, and then skips the object
                        // based on that difference
                        float heightDif = Mathf.Abs (obj.transform.position.y - transform.position.y);
                        if (heightDif >= _Data._HeightDifferenceMax)
                            continue;

                        _ObjectCollider = obj;
                        _CarryingObject = obj.gameObject;
                        break;
                    } else {
                        // Because there wasn't a spot for us, reset the carrying data and move on
                        _CarryingData = null;
                    }
                    // We can skip the Pikmin attack check because you can't carry an attackable
                    continue;
                }

                _AttackingData = obj.GetComponentInParent<IPikminAttack> ();
                if (_AttackingData != null) {
                    // Works out the height difference between the two objects, and then skips the object
                    // based on that difference
                    float heightDif = Mathf.Abs (obj.transform.position.y - transform.position.y);
                    if (heightDif >= _Data._HeightDifferenceMax)
                        continue;

                    _ObjectCollider = obj;
                    _AttackingObject = obj.gameObject;
                    break;
                }
            }
        }

        if (_AttackingObject != null) {
            if (_AttackingData == null || _ObjectCollider == null) {
                _AttackingObject = null;
                _AttackingData = null;
                _ObjectCollider = null;
                return;
            }

            // Move towards the object
            MoveTowards (_ObjectCollider.ClosestPoint (transform.position));

            // And check if we're close enough to start attacking
            if (MathUtil.DistanceTo (transform.position, _AttackingObject.transform.position) <= _Data._AttackDistance) {
                _AttackingData.OnAttackStart (this);
            }
        } else if (_CarryingObject != null) {
            // Check if there isn't a spot available for us, or the carrying data doesn't exist
            if (_ObjectCollider == null || _CarryingData == null || _CarryingData.PikminSpotAvailable () == false) {
                _CarryingObject = null;
                _CarryingData = null;
                _ObjectCollider = null;
                return;
            }

            // Move towards the object
            MoveTowards (_ObjectCollider.ClosestPoint (transform.position));

            // And check if we're close enough to start carrying
            if (MathUtil.DistanceTo (transform.position, _CarryingObject.transform.position) <= _Data._CarryDistance) {
                _CarryingData.OnCarryStart (this);
            }
        }
    }

    void HandleFormation () {
        MoveTowards (_PlayerPikminManager.GetFormationCenter ().position);
    }

    void HandleDeath () {
        // We may not have been properly removed from the squad, so do it ourself
        if (_InSquad) {
            RemoveFromSquad ();
        }

        PlayerStats._OnField.Remove (this);
        /*PlayerStats.GetFieldStats(_Data._Colour, out PlayerStats.PikminStats stats);
        stats.Increment (_CurrentHeadType);
        
        PlayerStats.GetTotalStats (_Data._Colour, out stats);
        stats.Decrement(_CurrentHeadType);*/

        // TODO: handle death animation + timer later
        //ObjectPooler.Instance.StoreInPool("Pikmin");
        Destroy (gameObject);
    }

    #region Carrying

    void CheckForCarry (GameObject toCheck) {
        _CarryingData = toCheck.GetComponent<IPikminCarry> ();
        if (_CarryingData == null)
            return;

        _CarryingObject = toCheck;
        _CarryingData.OnCarryStart (this);
    }

    #endregion

    #region Attacking
    void HandleAttacking () {
        // If the thing we were Attacking with doesn't exist anymore
        if (_AttackingObject == null) {
            ChangeState (States.Idle);
            return;
        }

        // Increment the timer to check if we can attack again
        _AttackTimer += Time.deltaTime;
        if (_AttackTimer < _Data._TimeBetweenAttacks)
            return;

        // Attack
        _AttackingData.Attack (this, _Data._AttackDamage);
        _AttackTimer = 0;
    }

    void CheckForAttack (GameObject toCheck) {
        if (toCheck.CompareTag ("Interactable") == false) {
            return;
        }

        // Check if the object in question has the PikminAttack interface implemented
        _AttackingData = toCheck.GetComponentInParent<IPikminAttack> ();
        if (_AttackingData != null) {
            // It does, so we can call the OnAttackStart function
            _AttackingObject = toCheck;
            _AttackingData.OnAttackStart (this);
        }
    }

    public void LatchOntoObject (Transform parent) {
        transform.parent = parent;
        _Rigidbody.isKinematic = parent != null;
    }
    #endregion

    void ActivateHead () {
        // Get the headtype we want to enable
        int type = GetHeadTypeInt ();
        // Iterate over all of the heads, and activate the one that matches the type we want
        for (int i = 0; i < (int) Headtype.SIZE; i++) {
            _HeadTypeModels[i].SetActive (i == type);
        }
    }

    public void SetHead (Headtype newHead) {
        /* At the end of the day, in the onion, moves this to the global stats
        PlayerStats.GetFieldStats(_Data._Colour, out PlayerStats.PikminStats stats);
        stats.Decrement (_CurrentHeadType);*/

        _CurrentHeadType = newHead;

        /*PlayerStats.GetFieldStats(_Data._Colour, out stats);
        stats.Increment (_CurrentHeadType);*/

        ActivateHead ();
    }

    public void EnterWater () {
        if (_Data._Colour != Colour.Blue) {
            // TODO: add struggling to get out of water
            ChangeState (States.Dead);
        }
    }

    // Gets called when the Player grabs the Pikmin
    public void ThrowHoldStart () {
        _Rigidbody.isKinematic = true;
        _Rigidbody.useGravity = false;
        _Collider.enabled = false;

        if (_Animator.GetBool ("Walking") == true)
            _Animator.SetBool ("Walking", false);

        _Animator.SetBool ("Holding", true);

        ChangeState (States.Held);
    }

    // Gets called when the Player releases the Pikmin
    public void ThrowHoldEnd () {
        _Rigidbody.isKinematic = false;
        _Rigidbody.useGravity = true;
        _Collider.enabled = true;
        _Animator.SetBool ("Holding", false);

        RemoveFromSquad ();
        ChangeState (States.Thrown);
    }

    // Gets called when an object Shakes the Pikmin off of it
    public void ShakeOff (Vector3 shakingObjectPos, float shakeForce) {
        _Rigidbody.AddExplosionForce (shakeForce, shakingObjectPos, Vector3.Distance (shakingObjectPos, transform.position));
    }

    #region General Purpose Useful Functions

    void MoveTowards (Vector3 towards) {
        Vector3 direction = (towards - _Rigidbody.position).normalized * _Data._MovementSpeed * Mathf.Pow (_Data._HeadSpeedMultiplier, GetHeadTypeInt () + 1);
        direction.y = _Rigidbody.velocity.y;
        _Rigidbody.velocity = direction;

        // reset the Y axis so the body doesn't rotate up or down
        direction.y = 0;
        // look at the direction of the object we're moving towards and smoothly interpolate to it
        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (direction), _Data._RotationSpeed * Time.deltaTime);
    }

    #region Squad
    public void AddToSquad () {
        if (!_InSquad && _State != States.Thrown) {
            PlayerStats._InSquad.Add (this);

            ChangeState (States.Formation);
            _InSquad = true;
        }
    }

    public void RemoveFromSquad () {
        if (_InSquad) {
            ChangeState (States.Idle);
            PlayerStats._InSquad.Remove (this);
            _InSquad = false;
        }
    }
    #endregion

    #region Setters
    public void SetData (PikminSO setTo) => _Data = setTo;

    public void ChangeState (States setTo, bool unparent = true) {
        _PreviousState = _State;
        _State = setTo;

        // Check if we have to unparent the Pikmin, in some scenarios we don't want to do that
        if (unparent && transform.parent != null)
            transform.parent = null;

        // Handle State-specific transitions
        if (_PreviousState == States.Carrying && _CarryingData != null || _CarryingObject != null) {
            _CarryingData.OnCarryLeave (this);
            _CarryingData = null;

            _CarryingObject = null;
        } else if (_PreviousState == States.Attacking && _AttackingData != null || _AttackingObject != null) {
            _AttackingData.OnAttackEnd (this);
            _AttackingData = null;

            _AttackingObject = null;
        } else if (_PreviousState == States.AttackAndFollow) {
            _ObjectCollider = null;
        }
    }
    #endregion

    #region Getters
    public States GetState () => _State;

    public int GetHeadTypeInt () => (int) _CurrentHeadType;
    #endregion
    #endregion
}