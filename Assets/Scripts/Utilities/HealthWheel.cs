/*
 * HealthWheel.cs
 * Created by: Neo
 * Created on: 15/2/2020 (dd/mm/yy)
 * Created for: Display of enemy health using a bar, or wheel
 */

using UnityEngine;
using UnityEngine.UI;

public class HealthWheel : MonoBehaviour, IPooledObject {
    [SerializeField] float _HealthSpeed = 7.5f;

    [HideInInspector] public bool _InUse = false;
    [HideInInspector] public float _MaxHealth;
    [HideInInspector] public float _CurrentHealth;
    [HideInInspector] public Image _BillboardHealth;

    // Start is called before the first frame update
    void Start () {
        _BillboardHealth = transform.Find ("Health_Display").gameObject.GetComponent<Image> ();
    }

    void IPooledObject.OnObjectSpawn () {
        _BillboardHealth.fillAmount = 1;
    }

    // Update is called once per frame
    void Update () {
        // Smoothly transition between values to avoid hard changing
        _BillboardHealth.fillAmount = Mathf.Lerp (_BillboardHealth.fillAmount,
            _CurrentHealth / (float) _MaxHealth,
            _HealthSpeed * Time.deltaTime);
        _BillboardHealth.color = Color.Lerp (Color.red, Color.green, _CurrentHealth / (float) _MaxHealth);
    }
}