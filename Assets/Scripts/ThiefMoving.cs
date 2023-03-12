using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Transform))]

public static class AnimatorThiefController
{
    public static class Params
    {
        public const string IsMoving = "isMoving";
    }

    public static class States
    {
        public const string Idle = nameof(Idle);
        public const string Walk = nameof(Walk);
    }
}

public class ThiefMoving : MonoBehaviour
{

    [SerializeField] private CircleCollider2D _targetCollider;
    [SerializeField] private float _speed;

    private Animator _animator;
    private bool _isInHome = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartMoving();
    }

    private void Update()
    {
        if (_animator.GetBool(AnimatorThiefController.Params.IsMoving) == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetCollider.transform.position, _speed * Time.deltaTime);
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
        _animator.SetBool(AnimatorThiefController.Params.IsMoving, true);
    }

    private void StopMoving()
    {
        _animator.SetBool(AnimatorThiefController.Params.IsMoving, false);
    }
}
