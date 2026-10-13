using UnityEngine;

public class PlayerAudiosController : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip _footstep;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _land;
    [SerializeField] private AudioClip _dash;
    [SerializeField] private AudioClip _attack1;
    [SerializeField] private AudioClip _attack2;
    [SerializeField] private AudioClip _attack3;
    [SerializeField] private AudioClip _defend;
    [SerializeField] private AudioClip _hurt;
    [SerializeField] private AudioClip _die;

    // Animation Event فقط می‌تونه string، int، float، object بگیره
    public void PlaySound(string soundName)
    {
        AudioClip clip = soundName switch
        {
            "Footstep" => _footstep,
            "Jump" => _jump,
            "Land" => _land,
            "Dash" => _dash,
            "Attack1" => _attack1,
            "Attack2" => _attack2,
            "Attack3" => _attack3,
            "Defend" => _defend,
            "Hurt" => _hurt,
            "Die" => _die,
            _ => null
        };

        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}
