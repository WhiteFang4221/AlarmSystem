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

    private IntruderChecker _intruderChecker;

    private float _volumeUnit = 0.1f;
    private float _minVolume = 0.001f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator ChangeVolume()
    {
        var waitForSeconds = new WaitForSeconds(1);
        _audioSource.volume += _minVolume;

        while (_audioSource.volume != 0)
        {
            if (_intruderChecker.IsAlarmOn == true)
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
