using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Animation")]
    public Animator spriteAnimator;
    [SerializeField]
    private SpriteRenderer playerSprite;

    [Header("Movement")]
    public bool canMove;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float movementAcceleration;
    [SerializeField]
    private float velocityPower;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float fallGravityMultiplier;

    private Vector2 _throwDirection;
    private bool _isJumping;

    // thx to Dawnosaur 4 this btw:
    [Header("Jump Assist")]
    private readonly float _coyoteTime = 0.15f; // grace period after falling off a platform, where you can still jump
    private readonly float _jumpInputBufferTime = 0.1f; // grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met
    [SerializeField]
    private Transform groundCheckPoint; // size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField]
    private Vector2 groundCheckSize = new(0.49f, 0.03f);
    private float _lastPressedJumpTime;
    private float _lastOnGroundTime;
    //

    [Header("Ctrl")]
    [SerializeField]
    private float pickUpSensitivity;
    private BoxCollider2D _decoyCtrlCollider; // the fake collider we use when we cold Ctrl
    private ControlController _ctrl;

    [Header("Utility")]
    [SerializeField]
    private int itemLayerIndex;
    private LayerMask _itemLayerMask;
    [SerializeField]
    private int groundLayerIndex;
    private LayerMask _groundLayerMask;
    public Rigidbody2D _rigidbody;

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
        _decoyCtrlCollider = GetComponent<BoxCollider2D>();
        _decoyCtrlCollider.enabled = false;
        _ctrl = ControlController.Instance;

        if (!_ctrl.isHeld)
        {
            spriteAnimator.SetBool("IsHoldingCtrl", false);
        }

        // Layers
        _itemLayerMask = 1 << itemLayerIndex;
        _groundLayerMask = (1 << groundLayerIndex) | (1 << itemLayerIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Animation
        spriteAnimator.SetBool("IsWalking", _rigidbody.velocity.magnitude > 0.15f);

        // Timers
        _lastOnGroundTime -= Time.deltaTime;
        _lastPressedJumpTime -= Time.deltaTime;

        // Checks
        if (_isJumping && _rigidbody.velocity.y < 0)
        {
            _isJumping = false;
        }

        Collider2D col = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, _groundLayerMask);

        if (col && !_isJumping) //checks if set box overlaps with ground
        {
            _lastOnGroundTime = _coyoteTime; //if so sets the lastGrounded to coyoteTime

            if (!col.transform.CompareTag("CTRL"))
            {
                transform.parent = col.transform;
            }
        }

        if (!canMove) return;
        
        // Turning
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            playerSprite.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            playerSprite.flipX = true;
        }

        // Input Processing
        if (Input.GetKeyDown(KeyCode.Space)) // jump
        {
            _lastPressedJumpTime = _jumpInputBufferTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !_ctrl.isHeld) // pick up
        {
            var check = Physics2D.OverlapCircle(transform.position, pickUpSensitivity, _itemLayerMask);
            if (check == null) return;
            {
                //TODO: light up the CTRL item
            }
            _decoyCtrlCollider.enabled = true;
            _ctrl.transform.parent = transform;
            _ctrl.GetPickedUp();
            spriteAnimator.SetBool("IsHoldingCtrl", true);
        }
        else if (_ctrl.isHeld) // throw
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RecalculateThrowDirection();
                _decoyCtrlCollider.enabled = false;
                _ctrl.transform.parent = null;
                _ctrl.GetThrown(_throwDirection, _rigidbody.velocity);
                spriteAnimator.SetBool("IsHoldingCtrl", false);
            }
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        // move left & right
        var targetSpeed = Input.GetAxisRaw("Horizontal") * movementSpeed;
        var speedDifference = targetSpeed - _rigidbody.velocity.x;
        _rigidbody.AddForce(Mathf.Pow(Mathf.Abs(speedDifference) * movementAcceleration, velocityPower) *
                            Mathf.Sign(speedDifference) * Vector2.right, ForceMode2D.Force);

        // jump
        if (CanJump() && _lastPressedJumpTime > 0)
        {
            _isJumping = true;

            _lastPressedJumpTime = 0;
            _lastOnGroundTime = 0;

            _rigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        // gravity
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.gravityScale = 1 * fallGravityMultiplier;
        }
        else
        {
            _rigidbody.gravityScale = 1;
        }
    }

    public Vector3 GetCenterBasedViewPortPosition()
    {
        var viewportPosition = Camera.main!.WorldToViewportPoint(transform.position);
        return viewportPosition - new Vector3(0.5f, 0.5f, 0);
    }

    public Vector3 GetScreenPosition()
    {
        return Camera.main!.WorldToScreenPoint(transform.position);
    }

    private void RecalculateThrowDirection()
    {
        _throwDirection = Input.mousePosition - Camera.main!.WorldToScreenPoint(transform.position);
    }

    private bool CanJump()
    {
        return _lastOnGroundTime > 0 && !_isJumping && !_ctrl.isHeld;
    }

    public void Restart()
    {
        canMove = false;
        _rigidbody.velocity = Vector2.zero;
        spriteAnimator.SetBool("IsWalking", false);
        StartCoroutine(Transition.Instance.Fade('r', SceneManager.GetActiveScene().name));
    }
}
