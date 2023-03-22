using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject[] objects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Player")) return;
        foreach (var obj in objects)
        {
            obj.SetActive(!obj.activeInHierarchy);            
        }
        gameObject.SetActive(false);
    }
}
