using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Transform))]
public class Move : MonoBehaviour
{

    [SerializeField] private CircleCollider2D _targetCollider;
    private Transform _transform;
    private Animator _animator;

    private float _speed;
    private float _movingSpeed = 20;

    private bool _isInHome = false;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartMoving();
    }

    private void Update()
    {
        if (_speed > 0)
        {
            _animator.SetFloat("Speed", 1);
            _transform.position = Vector2.MoveTowards(_transform.position, _targetCollider.transform.position, _speed * Time.deltaTime);
        }

        if (_isInHome == true)
        {
            StopMoving();
            Invoke("GetOutHouse", 5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Home>(out Home home))
        {
            GetInHouse();
        }
        else if (collision.TryGetComponent<Exit>(out Exit exit))
        {
            StopMoving();
        }
    }

    private void GetInHouse()
    {
        _isInHome = true;

        StopMoving();
    }

    private void GetOutHouse()
    {
        _isInHome= false;
        StartMoving();
    }

    private void StartMoving()
    {
        _speed = _movingSpeed;
        _animator.SetFloat("Speed", 1);
    }

    private void StopMoving()
    {
        _speed= 0;
        _animator.SetFloat("Speed", 0);
    }
}
