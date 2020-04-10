/*
 * WhistleController.cs
 * Created by: Ambrosia & Kaelan
 * Created on: 9/2/2020, overhaul on 31/3/2020 (dd/mm/yy)
 * Created for: Controlling the whistle
 */

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(AudioSource))]
public class WhistleController : MonoBehaviour
{
    /* We use the localScale to keep track of how scaled the Whistle
       actually is, as opposed to just changing the decals localScale */

    [Header("Components")]
    [SerializeField] GameObject _WhistleParticle;
    [SerializeField] AudioClip _BlowSound;
    [SerializeField] Transform _Reticle;

    [Header("Particles")]
    [SerializeField] uint _ParticleDensity = 15;
    [SerializeField] float _ParticleRotationSpeed = 1;
    [SerializeField] float _ParticleRaycastAddedHeight = 100;
    [SerializeField] float _HeightOffset = 0.5f;

    [Header("Settings")]
    [SerializeField] float _StartingRadius = 1;
    [SerializeField] float _ExpandedRadius = 10;
    [SerializeField] float _MaxBlowTime = 3;
    [SerializeField] float _OffsetFromSurface = 0.5f;

    [Header("Raycast Settings")]
    [SerializeField] float _MaxDistance = Mathf.Infinity;
    [SerializeField] LayerMask _MapMask;

    [Header("Debugging")]
    [SerializeField] uint _WhistleCircleSegments = 20;

    AudioSource _Source;
    GameObject[] _Particles;
    Camera _MainCamera;

    bool _Blowing = false;
    float _TimeBlowing = 0;

    void Awake()
    {
        // Generate particles for blowing later on
        _Particles = new GameObject[_ParticleDensity + 1];
        for (int i = 0; i < _Particles.Length; i++)
        {
            _Particles[i] = Instantiate(_WhistleParticle);
        }
        AssignParticlePositions();
        SetParticlesActive(false);

        // Reset the local scale
        transform.localScale = Vector3.one * _StartingRadius;

        // Get local components
        _Source = GetComponent<AudioSource>();
        _MainCamera = Camera.main;
    }

    void Update()
    {
        // Moving the whistle
        Ray ray = _MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _MaxDistance, _MapMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 target = hit.point + hit.normal * _OffsetFromSurface;
            transform.position = _Reticle.position = target;
        }


        // Detecting Player input
        if (Input.GetButtonDown("Whistle"))
        {
            transform.localScale = Vector3.one * _StartingRadius;

            // Start the particles
            SetParticlesActive(true);

            _Blowing = true;
            // Load the clip into the AudioSource and play
            _Source.clip = _BlowSound;
            _Source.Play();
        }
        if (Input.GetButtonUp("Whistle"))
        {
            EndBlow();
        }
        // Handling Blowing
        if (_Blowing)
        {
            // Used to keep track of how long we've been blowing for
            _TimeBlowing += Time.deltaTime;
            if (_TimeBlowing >= _MaxBlowTime)
            {
                EndBlow();
            }

            // Grow the scale of the whistle to the radius we want it to become
            float t = _TimeBlowing / _MaxBlowTime;
            transform.localScale = Vector3.Lerp(transform.localScale, _2Dto3D(Vector2.one * _ExpandedRadius, 1), t);

            // Handle collisions with Pikmin
            Collider[] collisions = Physics.OverlapCapsule(transform.position + Vector3.down * 20, transform.position + Vector3.up * 20, transform.localScale.x);
            for (int i = 0; i < collisions.Length; i++)
            {
                PikminBehavior pikComponent = collisions[i].GetComponent<PikminBehavior>();
                if (pikComponent != null)
                {
                    pikComponent.AddToSquad();
                }
            }
        }

        // Handle Particle movement
        AssignParticlePositions();
    }

    /// <summary>
    /// Displays debug information about the Whistle when selected in the editor
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            // Draw particles
            AssignParticlePositions();
            foreach (var particle in _Particles)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(particle.transform.position, 0.05f * transform.localScale.x);
            }
        }

        // Draw default whistle radius
        for (int i = 0; i < _WhistleCircleSegments + 1; i++)
        {
            Vector3 pos = transform.position + _2Dto3D(MathUtil.CalcPosInCirc(_WhistleCircleSegments, i)) * _StartingRadius;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pos, 0.05f * _StartingRadius);
        }
        // Draw expanded whistle radius
        for (int i = 0; i < _WhistleCircleSegments + 1; i++)
        {
            Vector3 pos = transform.position + _2Dto3D(MathUtil.CalcPosInCirc(_WhistleCircleSegments, i)) * _ExpandedRadius;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pos, 0.05f * _ExpandedRadius);
        }
    }

    void EndBlow()
    {
        transform.localScale = Vector3.one * _StartingRadius;
        _TimeBlowing = 0;
        _Blowing = false;
        SetParticlesActive(false);
        _Source.Stop();
    }

    void SetParticlesActive(bool isActive)
    {
        for (int i = 0; i < _Particles.Length; i++)
        {
            _Particles[i].gameObject.SetActive(isActive);
        }
    }

    /// <summary>
    /// Assigns the positions of the blow particles
    /// </summary>
    void AssignParticlePositions()
    {
        RaycastHit hitInfo;
        Transform cacheTransform = transform;
        for (int i = 0; i < _ParticleDensity + 1; i++)
        {
            Vector3 localPos = _2Dto3D(MathUtil.CalcPosInCirc(_ParticleDensity, i, _TimeBlowing * _ParticleRotationSpeed)) * cacheTransform.localScale.x;
            // Offset the local position to be global
            localPos += cacheTransform.position;
            //Cache the Y for later
            float originalY = localPos.y;

            // Put the Y of the particle waay above everything else, so it can raycast downwards onto surfaces that may be above it 
            localPos.y += _ParticleRaycastAddedHeight;

            // Check if there is a surface beneath the particle
            if (Physics.Raycast(localPos, Vector3.down, out hitInfo, _MaxDistance, _MapMask, QueryTriggerInteraction.Ignore))
            {
                localPos.y = hitInfo.point.y + _HeightOffset;
            }
            else
            {
                // We couldn't find anything, so reset back to the original Y
                localPos.y = originalY;
            }

            _Particles[i].transform.position = localPos;
        }
    }

    /// <summary>
    /// Converts between 2D and 3D on the X and Z axis
    /// </summary>
    /// <param name="conv">The vector to convert</param>
    /// <returns>Vector3 with X and Z set to the X and Y of the Vector2</returns>
    Vector3 _2Dto3D(Vector2 conv, float y = 0) => new Vector3(conv.x, y, conv.y);
}
