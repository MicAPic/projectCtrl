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
        }
        else
        {
            CheckpointManager.Instance.SetPosition();   
        }

        loopAnimation.Pause();
        transform.DOMove(transform.position - fadeOffset, fadeDuration);
        GetComponent<SpriteRenderer>().DOColor(Color.clear, fadeDuration);
    }
}
