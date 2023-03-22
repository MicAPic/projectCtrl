using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    private static SoundEffectsPlayer instance;
 
    public static SoundEffectsPlayer getInstance()
    {
        if (instance == null)
            instance = new SoundEffectsPlayer();
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    
    public Animator musicSourceAnimator;
    public AudioSource sfxSource;
    public AudioClip button;
    public AudioClip door;
    public AudioClip hurt;
    

    public void Button()
    {
        sfxSource.volume = 1.0f;
        sfxSource.clip = button;
        sfxSource.Play();
    }
    public void Door()
    {
        sfxSource.volume = 0.7f;
        sfxSource.clip = door;
        sfxSource.Play();
    }
    public void Hurt()
    {
        sfxSource.volume = 0.5f;
        sfxSource.clip = hurt;
        sfxSource.Play();
    }
}
