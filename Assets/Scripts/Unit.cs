using System.Collections;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Events;

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

    #region Events
    private UnityEvent OnEnemyDied = new UnityEvent();
    private UnityEvent<Unit> OnIWasChosen = new UnityEvent<Unit>();
    #endregion

    private Unit _target;
    private StatesPreset _state;
    private bool _reloading;
    private bool _iWasChosen;

    public int Health => _health;
    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public Unit Target => _target;
    public StatesPreset State => _state;
    public UnitMovement UnitMovement => _unitMovement;
    #endregion

    #region UnityMethods
    private void Start()
    {
        OnEnemyDied.AddListener(Chase);
        OnEnemyDied.AddListener(CheckOfWin);
        OnEnemyDied.AddListener(SetDestinitionToSelf);

        OnIWasChosen.AddListener(SetTarget);

        UpdateHealth();

        Chase();
    }

    private void OnDrawGizmos()
    {
        if (_target)
            Gizmos.DrawLine(transform.position, _target.transform.position);
    }

    private void Update()
    {
        if (_target)
            transform.LookAt(_target.transform);
    }
    #endregion

    #region Methods
    public void Chase()
    {
        UpdateTarget();
        _unitMovement.Unfreeze();
        StartCoroutine(Chase_Coroutine());
    }

    public void Attack()
    {
        StartCoroutine(Attack_Coroutine());
    }

    public void TakeDamage(int damage, Unit unit)
    {
        _health -= damage;

        UpdateHealth();

        TakeDamageEffect();

        if (_health <= 0)
        {
            Die();
            unit.OnEnemyDied.Invoke();
        }
    }

    public void Die()
    {
        UnitsManager.Instance.Units.Remove(this);
        Destroy(gameObject);
    }

    private void UpdateTarget()
    {
        _target = FindNearestTarget();

        if (_target)
            _target.OnIWasChosen.Invoke(this);

        //if (transform.name == "Unit" && _target == null)
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

    private void UpdateHealth()
    {
        _healthUI.text = _health.ToString();
    }

    private void SetDestinitionToSelf()
    {
        _unitMovement.SetDestinitionToSelf();
    }

    private void SetTarget(Unit unit)
    {
        _target = unit;
    }
    #endregion

    #region Coroutines
    private IEnumerator Chase_Coroutine()
    {
        _state = StatesPreset.Chasing;

        while (_target)
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);

            if (distance > 2f)
            {
                if (_target.State != StatesPreset.Attacking)
                    _unitMovement.Move(_target);
            }
            else
                Attack();
            
            yield return null;
        }
    }

    private IEnumerator Attack_Coroutine()
    {
        if (!_reloading)
        {
            _state = StatesPreset.Attacking;

            _unitMovement.Freeze();

            _target.TakeDamage(_damage, this);

            //print($"transform name: {transform.name} \n current health: {_health}");

            _reloading = true;

            yield return new WaitForSeconds(_reloadingTime);

            _reloading = false;

            _unitMovement.Unfreeze();

            yield return null;
        }
    }

    private IEnumerator TakeDamageEffect_Coroutine()
    {
        MeshRenderer renderer = transform.GetComponent<MeshRenderer>();

        Material defaultMaterial = renderer.material;

        renderer.material = _takeDamageMaterial;

        yield return new WaitForSeconds(.2f);

        renderer.material = defaultMaterial;

    }

    private void CheckOfWin()
    {
        if (UnitsManager.Instance.Units.Count == 1)
            Win();
    }

    private void Win()
    {
        WindowsManager.Instance.Windows.WinnerWindow.Show();
    }
    #endregion

    #region Enums
    public enum StatesPreset
    {
        Chasing,
        Attacking
    }
    #endregion
}
