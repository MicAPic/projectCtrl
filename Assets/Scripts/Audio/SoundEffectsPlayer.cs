using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    private static SoundEffectsPlayer _instance;
 
    public static SoundEffectsPlayer GetInstance()
    {
        if (_instance == null)
            _instance = new SoundEffectsPlayer();
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }
    
    public Animator musicSourceAnimator;
    public AudioSource sfxSource;
    public AudioClip button;
    public AudioClip door;
    public AudioClip hurt;
    

    public void Button()
    {
        sfxSource.volume = 0.5f;
        sfxSource.clip = button;
        sfxSource.Play();
    }
    public void Door()
    {
        sfxSource.volume = 0.1f;
        sfxSource.clip = door;
        sfxSource.Play();
    }
    public void Hurt()
    {
        sfxSource.volume = 0.1f;
        sfxSource.clip = hurt;
        sfxSource.Play();
    }
}
