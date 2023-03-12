using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AnimatorHome
{
    public static class Params
    {
        public const string IsIntruderInside = "isIntruderInside";
    }

    public static class States
    {
        public const string Idle = nameof(Idle);
        public const string Alarm = nameof(Alarm);
    }
}

public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private UnityEvent _invasion;

    private AudioSource _audioSource;
    private Animator _animator;

    private float _volumeUnit = 0.1f;

    public bool IsAlarmOn { get; private set; }

    private void Start()

    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out Thief thief))
        {
            IsAlarmOn = true;
            _invasion?.Invoke();
            _animator.SetBool(AnimatorHome.Params.IsIntruderInside, true);
            StartCoroutine(ChangeVolume());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsAlarmOn = false;
    }

    private IEnumerator ChangeVolume()
    {
        var waitForSeconds = new WaitForSeconds(1);
        _audioSource.volume += _volumeUnit;

        while (_audioSource.volume != 0)
        {
            if (IsAlarmOn == true)
            {
                _audioSource.volume += _volumeUnit;
            }
            else
            {
                _audioSource.volume -= _volumeUnit;
            }

            yield return waitForSeconds;
        }

        _animator.SetBool(AnimatorHome.Params.IsIntruderInside, false);
    }
}
