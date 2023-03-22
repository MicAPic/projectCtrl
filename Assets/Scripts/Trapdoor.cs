using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    public bool isLocked;
    private Color offColor = new(0.5f, 0.5f, 0.5f, 0.5f);
    private Color baseColor = Color.gray;
    
    public void Activate(bool state)
    {
        GetComponent<SpriteRenderer>().color = state ? offColor : baseColor;
        GetComponent<BoxCollider2D>().enabled = !state;
    }
}
