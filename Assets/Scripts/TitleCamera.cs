using System.Collections;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    void Awake()
    {
        if (CheckpointManager.Instance == null) return;
        
        gameObject.SetActive(false);
        enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayTransition());
    }

    private IEnumerator DelayTransition()
    {
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }
}
