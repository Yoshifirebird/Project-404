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
    [SerializeField] float _HealthCircleSpeed = 5;
    [SerializeField] int _MaxHealth;
    int _CurrentHealth;
    readonly List<GameObject> _AttachedPikmin = new List<GameObject>();

    CubePikminAttackTest() => _MaxHealth = 10;
    void Awake() => _CurrentHealth = _MaxHealth;
    void Update()
    {
        _BillboardHealth.fillAmount = Mathf.Lerp(_BillboardHealth.fillAmount,
                                                 _CurrentHealth / (float)_MaxHealth,
                                                 _HealthCircleSpeed * Time.deltaTime);

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

    public void Attack(GameObject attacking, int damage) => TakeHealth(damage);

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
