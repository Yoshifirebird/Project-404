/*
 * WhistleController.cs
 * Created by: Ambrosia & Kaelan
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Controlling the whistle
 */

using UnityEngine;

public class WhistleController : MonoBehaviour
{
    Camera _Camera;

    [Header("Components")]
    [SerializeField] Transform _Player;
    [SerializeField] Transform _Throw;

    [Header("Settings")]
    [SerializeField] LayerMask _GroundLayer;
    [SerializeField] float _WhistleMaxDistance = Mathf.Infinity;
    [SerializeField] float _ThrowMaxDistance = Mathf.Infinity;
    [SerializeField] float _YAxisOffset = 0.5f;

    [Header("Whistling")]
    [SerializeField] Vector2 _WhistleScale;
    [SerializeField] [Range(0, 1)] float _WhistleRadiusGrowSpeed;
    [SerializeField] [Range(0, 1)] float _WhistleHeightGrowSpeed;
    [SerializeField] float _WhistleMaxTime;
    float _CurrentWhistleTime;
    Vector3 _TargetScale;
    Vector3 _StartingScale;
    bool _IsWhistling;

    void Awake()
    {
        _StartingScale = transform.localScale;
        _TargetScale = _StartingScale;
        _Camera = Camera.main;
    }

    void Update()
    {
        // Shoot a raycast from the mouse position for an infinite distance, filtering out any
        // hits other than objects that are on the _GroundLayer layer
        if (Physics.Raycast(_Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, _GroundLayer.value, QueryTriggerInteraction.Ignore))
        {
            Vector3 position = hit.point;
            position.y += _YAxisOffset;
            position -= _Player.position;
            RaycastHit point;

            // Determine position for the whistle
            position = Vector3.ClampMagnitude(position, _WhistleMaxDistance) + _Player.position;
            if(Physics.Raycast(position, Vector3.down, out point, Mathf.Infinity, _GroundLayer.value, QueryTriggerInteraction.Ignore))
            {
                transform.position = point.point;
            }

            // Reset position for the next object
            position = hit.point;
            position.y += _YAxisOffset;
            position -= _Player.position;

            // Determine position for the throw position
            position = Vector3.ClampMagnitude(position, _ThrowMaxDistance) + _Player.position;
            if (Physics.Raycast(position, Vector3.down, out point, Mathf.Infinity, _GroundLayer.value, QueryTriggerInteraction.Ignore))
            {
                _Throw.transform.position = point.point;
            }
        }

        HandleWhistle();
    }

    void OnTriggerEnter(Collider other)
    {
        if (_IsWhistling)
        {
            CheckPikmin(other.gameObject);
        }
    }

    void CheckPikmin(GameObject toCheck)
    {
        var pikminComponent = toCheck.GetComponent<PikminBehavior>();
        if (pikminComponent == null)
            return;

        pikminComponent.LatchOntoObject(null);
        pikminComponent.AddToSquad();
    }

    void HandleWhistle()
    {
        //Determines whether the player is whistling
        if (Input.GetButtonDown("Whistle"))
        {
            _IsWhistling = true;
            SetVector3ToVector2(ref _TargetScale, _WhistleScale);
            _CurrentWhistleTime = 0;
        }
        else if (Input.GetButtonUp("Whistle"))
        {
            _IsWhistling = false;
            _TargetScale = _StartingScale;
            transform.localScale = _TargetScale;
        }

        //If the whistle's radius is almost maxed, start adding time
        if (transform.localScale.x + 1.5f >= _WhistleScale.x)
        {
            _CurrentWhistleTime += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x,
                                               Mathf.Lerp(transform.localScale.y, _TargetScale.y, _WhistleHeightGrowSpeed),
                                               transform.localScale.z);
        }

        if (_CurrentWhistleTime >= _WhistleMaxTime)
        {
            _IsWhistling = false;
            _TargetScale = _StartingScale;
            transform.localScale = _TargetScale;
        }

        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, _TargetScale.x, _WhistleRadiusGrowSpeed),
                                           transform.localScale.y,
                                           Mathf.Lerp(transform.localScale.z, _TargetScale.z, _WhistleRadiusGrowSpeed));

    }

    void SetVector3ToVector2(ref Vector3 set, Vector2 to)
    {
        set.x = to.x;
        set.z = to.x;

        set.y = to.y;
    }
}
