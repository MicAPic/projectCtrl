using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
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
