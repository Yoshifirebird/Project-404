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
    [SerializeField] int _MaxHealth;
    int _CurrentHealth;
    readonly List<GameObject> _AttachedPikmin = new List<GameObject>();

    CubePikminAttackTest() => _MaxHealth = 10;
    void Awake() => _CurrentHealth = _MaxHealth;
    void Update()
    {
        // Smoothly transition between values to avoid hard changing
        _BillboardHealth.fillAmount = Mathf.Lerp(_BillboardHealth.fillAmount,
                                                 _CurrentHealth / (float)_MaxHealth,
                                                 _HealthCircleSpeed * Time.deltaTime);

        // Check we're dead, if so then detach the pikmin and destroy ourself
        if (_CurrentHealth <= 0)
        {
            foreach (GameObject attached in _AttachedPikmin)
            {
                attached.transform.parent = null;
            }
            Destroy(gameObject);
        }
    }

    #region Pikmin Attacking Implementation

    public void Attack(GameObject attacking, int damage)
    {
        // On first hit, enable the health circle
        if (_CurrentHealth == _MaxHealth)
            _BillboardHealth.transform.parent.gameObject.SetActive(true);

        TakeHealth(damage);
    }

    public void OnAttach(GameObject attachedPikmin) => _AttachedPikmin.Add(attachedPikmin);

    #endregion

    #region Health Implementation
    // 'Getter' functions
    public int GetHealth() => _CurrentHealth;
    public int GetMaxHealth() => _MaxHealth;
    // 'Setter' functions
    public void GiveHealth(int give) => _CurrentHealth += give;
    public void TakeHealth(int take) => _CurrentHealth -= take;
    public void SetHealth(int set) => _CurrentHealth = set;
    #endregion
}
