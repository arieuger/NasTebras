using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Jump and groundcheck")]
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallGravityScale;
    [SerializeField] private float buttonTime = 0.5f;
    [SerializeField] private float cancelRate = 100f;
    private bool _jump;
    private bool _jumpCancelled;
    private float _jumpTime;
    public bool Grounded { get; private set; }
    private float _defaultGravityScale;

    [Header ("Movement and velocity")]
    [SerializeField] private float speed;

    [SerializeField] private bool smoothActivated;
    [SerializeField] [Range(0f,0.3f)] private float movementSmooth = 0.3f;
    public float XInput { private set; get; }
    private Vector3 _velocity = Vector3.zero;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    private Vector2 _dashingDirection;
    public bool IsDashing { get; private set; }
    private bool _canDash = true;
    private bool _dashInput;
    
    [Header ("Animation States")]
    [SerializeField] private State idleState;
    [SerializeField] private State runState;
    [SerializeField] private State airState;
    [SerializeField] private State dashState;
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
        dashState.Setup(_body, _animator, this);
        
        _state = idleState;
    }

    void Update()
    {
        GetInput();
        ManageDash();
        ManageJump();
        SelectState();
        _state.Do();
    }

    void FixedUpdate()
    {
        CheckGround();
        CheckGravityScale();
        MoveWithInput();
    }

    private void SelectState()
    {
        State oldState = _state;

        if (IsDashing) _state = dashState;
        else if (Grounded) _state = XInput <= 0.2 && XInput >= -0.2 ? idleState : runState;
        else _state = airState;

        if (oldState != _state || oldState.IsComplete) {
            oldState.Exit();
            _state.Initialise();
            _state.Enter();
        }
    }

    void GetInput()
    {
        XInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && Grounded && !IsDashing) StartJump();
        _dashInput = Input.GetKeyDown(KeyCode.X);
    }

    private void StartJump()
    {
        float calculatedJumpForce = Mathf.Sqrt(jumpForce * -2 * (Physics2D.gravity.y * _body.gravityScale));
        _body.AddForce(new Vector2(0, calculatedJumpForce), ForceMode2D.Impulse);
        _jump = true;
        _jumpCancelled = false;
        _jumpTime = 0;
    }

    private void ManageJump()
    {
        if (!IsDashing && _jump)
        {
            _jumpTime += Time.deltaTime;
            if (Input.GetButtonUp("Jump")) _jumpCancelled = true;
            if (_jumpTime > buttonTime) _jump = false;
        }
    }

    private void ManageDash()
    {
        if (_dashInput && _canDash)
        {
            _canDash = false;
            IsDashing = true;
            StartCoroutine(DashCo());
        }
    }

    private IEnumerator DashCo()
    {
        _body.gravityScale = 0;
        _body.linearVelocity = new((_lookingRight ? 1 : -1) * dashingVelocity, 0);
        
        yield return new WaitForSeconds(dashingTime);

        _body.gravityScale = _defaultGravityScale;
        IsDashing = false;
        
        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }

    private void MoveWithInput()
    {
        if (IsDashing) return;
        
        Turn();
        Vector3 targetVelocity = new Vector2(XInput * speed, _body.linearVelocity.y);
        _body.linearVelocity = smoothActivated ? Vector3.SmoothDamp(_body.linearVelocity, targetVelocity, ref _velocity, movementSmooth) : targetVelocity;
        if (_jumpCancelled && _jump && _body.linearVelocity.y > 0) _body.AddForce(Vector2.down * cancelRate);

        // Proba para actualizar as capas da música en función da posición do player
        float distance = 1 - (Mathf.Abs(_body.position.x) - 5) / 18;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("DistanciaASereas", distance);
    }

    void CheckGround()
    {
        Grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        // if (Grounded && _jump) _jump = false;
    }
    
    private void CheckGravityScale()
    {
        if (IsDashing) return;
        _body.gravityScale = _body.linearVelocity.y < -0.1f ? fallGravityScale : _defaultGravityScale;
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
