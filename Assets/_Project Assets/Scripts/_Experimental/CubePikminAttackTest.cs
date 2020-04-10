/*
 * CubePikminAttackTest.cs
 * Created by: Ambrosia
 * Created on: 15/2/2020 (dd/mm/yy)
 * Created for: testing Pikmin attacking
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubePikminAttackTest : MonoBehaviour, IPikminAttack, IHealth
{
    [SerializeField] Image _BillboardHealth;
    [SerializeField] float _HealthCircleSpeed = 7.5f;
    [SerializeField] float _MaxHealth = 10;
    float _CurrentHealth = 0;
    readonly List<GameObject> _AttachedPikmin = new List<GameObject>();

    void Awake()
    {
        _CurrentHealth = _MaxHealth;
    }

    void Update()
    {
        // Smoothly transition between values to avoid hard changing
        _BillboardHealth.fillAmount = Mathf.Lerp(_BillboardHealth.fillAmount,
                                                 _CurrentHealth / (float)_MaxHealth,
                                                 _HealthCircleSpeed * Time.deltaTime);
        _BillboardHealth.color = Color.Lerp(Color.red, Color.green, _CurrentHealth / (float)_MaxHealth);

        // Check we're dead, if so then detach the pikmin and destroy ourself
        if (_CurrentHealth <= 0)
        {
            foreach (GameObject attached in _AttachedPikmin)
            {
                // Make the Pikmin's parent null, so they don't
                // follow us wherever we go
                attached.transform.parent = null;
            }
            
            // Should be an animation here, but because
            // it was a test we can just destroy the gameObject
            Destroy(gameObject);
        }
    }

    #region Pikmin Attacking Implementation

    public void Attack(GameObject attacking, float damage)
    {
        // On first hit, enable the health circle
        if (_CurrentHealth == _MaxHealth)
            _BillboardHealth.transform.parent.gameObject.SetActive(true);

        TakeHealth(damage);
    }

    public void OnAttach(GameObject attachedPikmin) => _AttachedPikmin.Add(attachedPikmin);
    public void OnDetach(GameObject detachedPikmin) => _AttachedPikmin.Remove(detachedPikmin);

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
