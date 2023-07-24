using UI;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Functions")] 
    [SerializeField]
    private string transitionToScene;
    [Header("Utility")]
    //TODO: move this to GameManager, if we make one
    [SerializeField] 
    private float sensitivityRadius;
    //
    [SerializeField] 
    private LayerMask playerLayer;
    [SerializeField] 
    private GameObject blockedIcon;
    private bool _soundWasPlayed;
    private bool _collisionDetected;

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, sensitivityRadius, playerLayer) && 
            ControlController.Instance.isHeld && !_collisionDetected)
        {
            PlayerController.Instance.canMove = false;
            PlayerController.Instance._rigidbody.velocity = Vector2.zero;
            
            SoundEffectsPlayer.Instance.Door();
            _collisionDetected = true;

            Transition.Instance.Fade('o', sceneToLoad:transitionToScene);
        }
        
        blockedIcon.SetActive(!ControlController.Instance.isHeld);
    }
}
