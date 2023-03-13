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

public class Alarm : MonoBehaviour
{
    [SerializeField] private UnityEvent _invasion;

    private AudioSource _audioSource;
    private Animator _animator;

    private float _volumeUnit = 0.1f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator TurnOnAlarm()
    {
        var waitForSeconds = new WaitForSeconds(1);
        _animator.SetBool(AnimatorHome.Params.IsIntruderInside, true);

        while (_audioSource.volume != 1)
        {
            _audioSource.volume += _volumeUnit;
            yield return waitForSeconds;
        }
    }

    public IEnumerator TurnOffAlarm()
    {
        var waitForSeconds = new WaitForSeconds(1);

        while (_audioSource.volume != 0)
        {
            _audioSource.volume -= _volumeUnit;
            yield return waitForSeconds;
        }
        _animator.SetBool(AnimatorHome.Params.IsIntruderInside, false);
    }
}
