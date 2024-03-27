using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Jump and groundcheck")]
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallGravityScale;
    private bool _jump;
    public bool Grounded { get; private set; }
    private float _defaultGravityScale;

    [Header ("Movement and velocity")]
    [SerializeField] private float speed;

    [SerializeField] private bool smoothActivated;
    [SerializeField] [Range(0f,0.3f)] private float movementSmooth = 0.3f;
    public float XInput { private set; get; }
    private Vector3 _velocity = Vector3.zero;

    [Header ("Animation States")]
    [SerializeField] private State idleState;
    [SerializeField] private State runState;
    [SerializeField] private State airState;
    private State _state;

    private Rigidbody2D _body;
    private Animator _animator;
    private bool _lookingRight = true;
    
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _defaultGravityScale = _body.gravityScale;
        _animator = GetComponentInChildren<Animator>();
        
        idleState.Setup(_body, _animator, this);
        runState.Setup(_body, _animator, this);
        airState.Setup(_body, _animator, this);
        
        _state = idleState;
    }

    void Update()
    {
        GetInput();
        MoveWithInput();
        SelectState();
        _state.Do();
    }

    private void SelectState()
    {
        State oldState = _state;

        if (Grounded) _state = XInput == 0 ? idleState : runState;
        else _state = airState;

        if (oldState != _state || oldState.IsComplete) {
            oldState.Exit();
            _state.Initialise();
            _state.Enter();
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        CheckGravityScale();
    }

    void GetInput()
    {
        XInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")) _jump = true;
    }

    private void MoveWithInput()
    {
        Turn();
        
        Vector3 targetVelocity = new Vector2(XInput * speed, _body.velocity.y);
        _body.velocity = smoothActivated ? Vector3.SmoothDamp(_body.velocity, targetVelocity, ref _velocity, movementSmooth) : targetVelocity;
        
        if (_jump && Grounded) {
            Grounded = false;
            // _body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            _body.velocity = new Vector2(_body.velocity.x, jumpForce);
        }
        _jump = false;
    }

    void CheckGround()
    {
        Grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }
    
    private void CheckGravityScale()
    {
        _body.gravityScale = _body.velocity.y < -0.1f ? fallGravityScale : _defaultGravityScale;
    }
    
    private void Turn()
    {
        if ((!(XInput > 0) || _lookingRight) && (!(XInput < 0) || !_lookingRight)) return;
        
        _lookingRight = !_lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
}
