using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    
    private Vector2 savedPos;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadPosition()
    {
        PlayerController.Instance.transform.position = savedPos;
    }

    public void SetPosition()
    {
        savedPos = PlayerController.Instance.transform.position;
    }
}
