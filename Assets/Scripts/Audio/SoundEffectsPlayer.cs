using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public static SoundEffectsPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(transform.parent.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);
    }
    
    public Animator musicSourceAnimator;
    public AudioSource sfxSource;
    public AudioClip button;
    public AudioClip door;
    public AudioClip hurt;
    public AudioClip checkpoint;
    

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
    public void Checkpoint()
    {
        sfxSource.volume = 0.1f;
        sfxSource.clip = checkpoint;
        sfxSource.Play();
    }
}
