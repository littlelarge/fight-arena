using System.Collections;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private int _health = 10;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _reloadingTime = 1;

    [Header("Components")]
    [SerializeField] private UnitMovement _unitMovement;

    [Header("Effects")]
    [SerializeField] private Material _takeDamageMaterial;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _healthUI;

    private Unit _target;
    private StatesPreset _state = StatesPreset.Alive;
    private bool _reloading;

    public int Health => _health;
    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public Unit Target => _target;
    public StatesPreset State => _state;
    #endregion

    #region UnityMethods
    private void Start()
    {
        Chase();
    }

    private void OnDrawGizmos()
    {
        if (_target)
            Gizmos.DrawLine(transform.position, _target.transform.position);
    }
    #endregion

    #region Methods
    public void Chase()
    {
        UpdateTarget();
        StartCoroutine(Chase_Coroutine());
    }

    public void Attack()
    {
        StartCoroutine(Attack_Coroutine());
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        _healthUI.text = _health.ToString();

        TakeDamageEffect();

        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        _state = StatesPreset.Die;
        UnitsManager.Instance.Units.Remove(this);
        Destroy(gameObject);
    }

    private void UpdateTarget()
    {
        _target = FindNearestTarget();

        //print($"transform name: {transform.name} \n target name: {_target.name}");
    }

    private Unit FindNearestTarget()
    {
        Unit nearest = null;
        float oldDistance = Mathf.Infinity;

        foreach (Unit u in UnitsManager.Instance.Units)
        {
            if (u != this)
            {
                float distance = Vector3.Distance(transform.position, u.transform.position);

                if (distance < oldDistance)
                {
                    nearest = u;
                    oldDistance = distance;
                }
            }
        }

        return nearest;
    }

    private void TakeDamageEffect()
    {
        StartCoroutine(TakeDamageEffect_Coroutine());
    }
    #endregion

    #region Coroutines
    private IEnumerator Chase_Coroutine()
    {
        while (_target.State != StatesPreset.Die)
        {
            _state = StatesPreset.Chasing;

            if (Vector3.Distance(transform.position, _target.transform.position) > 1.3f)
                _unitMovement.Move(_target);
            else
                Attack();

            yield return null;
        }
    }

    private IEnumerator Attack_Coroutine()
    {
        while (_target.State != StatesPreset.Die && !_reloading)
        {
            _state = StatesPreset.Attacking;

            _target.TakeDamage(_damage);

            print($"transform name: {transform.name} \n current health: {_health}");

            _reloading = true;

            yield return new WaitForSeconds(_reloadingTime);

            _reloading = false;

            UpdateTarget();

            yield return null;
        }

        if (_target != null)
            Chase();
    }

    private IEnumerator TakeDamageEffect_Coroutine()
    {
        MeshRenderer renderer = transform.GetComponent<MeshRenderer>();

        Material defaultMaterial = renderer.material;

        renderer.material = _takeDamageMaterial;

        yield return new WaitForSeconds(.2f);

        renderer.material = defaultMaterial;

    }
    #endregion

    #region Enums
    public enum StatesPreset
    {
        Chasing,
        Attacking,
        Alive,
        Die
    }
    #endregion
}
