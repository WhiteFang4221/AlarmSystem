using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(AudioSource))]

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
    private Home _home;

    private AudioSource _audioSource;

    private Animator _animator;

    private Coroutine _volumeChangerRoutine;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _home = GetComponent<Home>();

        _home.StateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        _home.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(float targetVolume)
    {
        if (_volumeChangerRoutine!= null)
        {
            StopCoroutine(_volumeChangerRoutine); 
        }
        _volumeChangerRoutine = StartCoroutine(ChangeAlarmState(targetVolume));
    }

    private IEnumerator ChangeAlarmState(float targetVolume)
    {
        if (targetVolume != 0)
        {
            _animator.SetBool(AnimatorHome.Params.IsIntruderInside, true);
            _audioSource.Play();
        }

        while(_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, Time.deltaTime * 0.2f);
            yield return null;
        }

        if (targetVolume == 0)
        {
            _animator.SetBool(AnimatorHome.Params.IsIntruderInside, false);
            _audioSource.Stop();
        }
    }
}
