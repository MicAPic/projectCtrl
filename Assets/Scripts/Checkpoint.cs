using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] 
    private GameObject checkpointManager;
    
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

        //TODO: Animate the checkpoint
    }
}
