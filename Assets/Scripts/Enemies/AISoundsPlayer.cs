using UnityEngine;

public class AISoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;

    private AudioSource _audioSource;      
    private AudioSource _loopAudioSource;  

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        _loopAudioSource = gameObject.AddComponent<AudioSource>();
        _loopAudioSource.playOnAwake = false;
        _loopAudioSource.loop = true;
        _loopAudioSource.outputAudioMixerGroup = _audioSource.outputAudioMixerGroup;
        _loopAudioSource.spatialBlend = _audioSource.spatialBlend;
        _loopAudioSource.volume = _audioSource.volume;
    }

    public void ThePlaySound(int index)
    {
        if (!_IsIndexValid(index)) return;
        _audioSource.PlayOneShot(_clips[index]);
    }

    public void ThePlayLoopSound(int index)
    {
        if (!_IsIndexValid(index)) return;

        if (_loopAudioSource.isPlaying && _loopAudioSource.clip == _clips[index]) return;

        _loopAudioSource.clip = _clips[index];
        _loopAudioSource.Play();
    }

    public void TheStopLoopSound(int index)
    {
        if (!_IsIndexValid(index)) return;

        if (_loopAudioSource.clip == _clips[index])
            _loopAudioSource.Stop();
    }

    private bool _IsIndexValid(int index)
    {
        if (index < 0 || index >= _clips.Length)
        {
            return false;
        }
        return true;
    }
}