using UnityEngine;

public class ControlController : MonoBehaviour
{
    public static ControlController Instance;
    
    [Header("General")] 
    private Vector2 _defaultPos;
    public bool isHeld;

    [Header("Accessibility")] 
    [SerializeField]
    private GameObject pointer;

    [Header("Physics")] 
    [SerializeField] 
    private float throwingForce;
    [SerializeField] 
    private Transform groundCheckPoint;
    private Vector2 groundCheckSize = new(0.49f, 0.03f);
    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;

    public LayerMask _groundLayerMask;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        
        _defaultPos = new Vector3(0, 1.54999995f, 0);
    }

     //Update is called once per frame

     void Update()
     {
        Collider2D col = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, _groundLayerMask);
        if (col)
        {
            if (!col.transform.CompareTag("CTRL"))
            {
                transform.parent = col.transform;
            }
        }
     }

    public void GetThrown(Vector2 direction, Vector2 velocity)
    {
        _rigidbody.simulated = true;
        _collider.enabled = true;
        isHeld = false;
        pointer.SetActive(false);

        _rigidbody.velocity = velocity;
        _rigidbody.AddForce(direction.normalized * throwingForce, ForceMode2D.Impulse);
    }

    public void GetPickedUp()
    {
        _rigidbody.simulated = false;
        _collider.enabled = false;
        isHeld = true;
        pointer.SetActive(true);
        
        transform.localPosition = _defaultPos;
    }
}
