using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    enum EnemyState
    {
        Patrol = 0,
        Investigate = 1,
        Stunned = 2
    }

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _threshold = 0.5f;
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private float _stunnedTime = 3f;
    [SerializeField] private PatrolRoute _patrolRoute;
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyState _state = EnemyState.Patrol;

    public UnityEvent<Transform> onPlayerFound;
    public UnityEvent onInvestigate;
    public UnityEvent onReturnToPatrol;
    public UnityEvent onStunned;

    private bool _moving = false;
    private Transform _currentPoint;
    private int _routeIndex = 0;
    private bool _forwardsAlongPath = true;
    private Vector3 _investigationPoint;
    private float _waitTimer = 0f;
    private bool _playerFound = false;
    private float _stunnedTimer = 0f;
    public float moveSpeed = 3f;
    public float chaseSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _currentPoint = _patrolRoute.route[_routeIndex];
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);

        if (_fov.visibleObjects.Count > 0)
        {
            PlayerFound(_fov.visibleObjects[0].position);
        }

        if (_state == EnemyState.Patrol)
        {
            UpdatePatrol();
        }
        else if (_state == EnemyState.Investigate)
        {
            UpdateInvestigate();
        }
        else if (_state == EnemyState.Stunned)
        {
            _stunnedTimer += Time.deltaTime;
            if (_stunnedTimer >= _stunnedTime)
            {
                ReturnToPatrol();
                _animator.SetBool("Stunned", false);
            }
        }
    }

    public void SetStunned()
    {
        _animator.SetBool("Stunned", true);
        _stunnedTimer = 0f;
        _state = EnemyState.Stunned;
        _agent.SetDestination(transform.position);
        onStunned.Invoke();
    }

    public void InvestigatePoint(Vector3 investigatePoint)
    {
        //Debug.Log("Investigate Point Triggered");
        SetInvestigationPoint(investigatePoint);

        onInvestigate.Invoke();
    }

    private void SetInvestigationPoint(Vector3 investigatePoint)
    {
        _state = EnemyState.Investigate;
        _investigationPoint = investigatePoint;
        _agent.SetDestination(_investigationPoint);
        _agent.speed = chaseSpeed;
    }

    private void PlayerFound(Vector3 investigatePoint)
    {
        SetInvestigationPoint(investigatePoint);

        onPlayerFound.Invoke(_fov.creature.head);
        InvestigatePoint(_fov.creature.head.position);
        _playerFound = true;
    }

    private void UpdateInvestigate()
    {
        if (_fov.visibleObjects.Count > 0)
        {
            PlayerFound(_fov.visibleObjects[0].position);
        }
        else
        {
            ReturnToPatrol();
        }
    }

    private void ReturnToPatrol()
    {
        Debug.Log("Enemy returning to patrol");
        _state = EnemyState.Patrol;
        _waitTimer = 0;
        _moving = false;
        _playerFound = false;
        _agent.speed = moveSpeed;

        onReturnToPatrol.Invoke();
    }

    private void UpdatePatrol()
    {
        if (!_moving)
        {
            NextPatrolPoint();

            _agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        if (_moving && Vector3.Distance(transform.position, _currentPoint.position) < _threshold)
        {
            _moving = false;
        }
    }

    private void NextPatrolPoint()
    {
        if (_forwardsAlongPath)
        {
            if (_patrolRoute.patrolType == PatrolRoute.PatrolType.Random)
            {
                _routeIndex = Random.Range(0, _patrolRoute.route.Count);
            }
            else
            {
                _routeIndex++;
            }
        }
        else
        {
            _routeIndex--;
        }
        if (_routeIndex == _patrolRoute.route.Count)
        {
            if (_patrolRoute.patrolType == PatrolRoute.PatrolType.Loop)
            {
                _routeIndex = 0;
            }
            else if (_patrolRoute.patrolType == PatrolRoute.PatrolType.PingPong)
            {
                _forwardsAlongPath = false;
                _routeIndex -= 2;
            }
        }

        if (_routeIndex == 0)
        {
            _forwardsAlongPath = true;
        }

        //Debug.Log(_routeIndex);

        _currentPoint = _patrolRoute.route[_routeIndex];
    }
}