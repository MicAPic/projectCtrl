using UnityEngine;

public abstract class Button : MonoBehaviour
{
    [SerializeField] 
    public Animator animator;
    public bool buttonPressed;
    protected bool PlayerPress;
    protected bool CtrlPress;
    private static readonly int Pressed = Animator.StringToHash("Pressed");
    private bool _soundWasPlayed;
    
    void Update()
    {
        if(buttonPressed && !_soundWasPlayed)
        {
            SoundEffectsPlayer.getInstance().Button();
            _soundWasPlayed = true;
        }

        if (!buttonPressed)
        {
            _soundWasPlayed = false;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (buttonPressed) return;

        if (other.CompareTag("Player") || other.CompareTag("CTRL"))
        {
            buttonPressed = true;
        }

        if (other.CompareTag("Player"))
        {
            PlayerPress = true;
        }
        if (other.CompareTag("CTRL"))
        {
            CtrlPress = true;
        }

    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPress = false;
        }
        if (collision.CompareTag("CTRL"))
        {
            CtrlPress = false;
        }
        if (!PlayerPress && !CtrlPress)
        {
            buttonPressed = false;
        }
    }

    protected bool CheckPress()
    {
        return PlayerPress || CtrlPress;
    }

    private void FixedUpdate()
    {
        animator.SetBool(Pressed, CheckPress());
    }
}
