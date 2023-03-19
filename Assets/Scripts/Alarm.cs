using JetBrains.Annotations;
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
    [SerializeField] private UnityEvent _invasionIsOver;

    private AudioSource _audioSource;

    private Animator _animator;

    private float _volumeUnit = 0.1f;

    private Coroutine _shutdownRoutine;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        IntruderChecker.Penetration += SwitchAlarmActivation;
        IntruderChecker.IntruderIsGone += SwitchOffAlarm;
    }

    private void OnDestroy()
    {
        IntruderChecker.Penetration -= SwitchAlarmActivation;
        IntruderChecker.IntruderIsGone -= SwitchOffAlarm;
    }

    private void SwitchAlarmActivation()
    {
        if (_shutdownRoutine!= null)
        {
            StopCoroutine(_shutdownRoutine);
        }
        
        _shutdownRoutine = StartCoroutine(TurnOnAlarm());
    }

    private void SwitchOffAlarm()
    {
        if (_shutdownRoutine != null)
        {
            StopCoroutine(_shutdownRoutine);
        }

        _shutdownRoutine = StartCoroutine(TurnOffAlarm());
    }

    private IEnumerator TurnOnAlarm()
    {
        var waitForSeconds = new WaitForSeconds(1);
        _animator.SetBool(AnimatorHome.Params.IsIntruderInside, true);
        _invasion?.Invoke();

        while (_audioSource.volume != 1)
        {
            _audioSource.volume += _volumeUnit;
            yield return waitForSeconds;
        }
    }

    private IEnumerator TurnOffAlarm()
    {
        var waitForSeconds = new WaitForSeconds(1);

        while (_audioSource.volume != 0)
        {
            _audioSource.volume -= _volumeUnit;
            yield return waitForSeconds;
        }
        _animator.SetBool(AnimatorHome.Params.IsIntruderInside, false);
        _invasionIsOver?.Invoke();
    }
}
