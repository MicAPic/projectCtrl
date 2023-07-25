using DG.Tweening;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] 
    private Vector3 loopOffset;
    [SerializeField] 
    private float loopDuration;
    [SerializeField] 
    private float fadeDuration;
    [SerializeField] 
    private Vector3 fadeOffset;

    private Tween loopAnimation;

    [Header("Rendering")] 
    [SerializeField] 
    private SpriteRenderer[] spriteParts;

    [Header("Utility")]
    [SerializeField] 
    private GameObject checkpointManager;

    void Start()
    {
        var currentPosition = transform.position;
        Vector3[] waypoints = {currentPosition, currentPosition + loopOffset, currentPosition};

        loopAnimation = transform.DOPath(waypoints, loopDuration, PathType.Linear, PathMode.Sidescroller2D)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (CheckpointManager.Instance == null)
        {
            var manager = Instantiate(checkpointManager);
            manager.GetComponent<CheckpointManager>().SetPosition();
            
            // NB: right now the SFX will activate only when there's 1 checkpoint in a level
            SoundEffectsPlayer.Instance.Checkpoint();
        }
        else
        {
            CheckpointManager.Instance.SetPosition();   
        }

        loopAnimation.Pause();
        foreach (var sprite in spriteParts)
        {
            sprite.transform.DOMove(transform.position - fadeOffset, fadeDuration);
            sprite.DOColor(Color.clear, fadeDuration);
        }

        GetComponent<Collider2D>().enabled = false;
    }
}
