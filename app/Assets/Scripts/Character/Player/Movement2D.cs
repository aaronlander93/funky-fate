using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
Code By: Andrew Sha
This code is for the animation of the character.
Drag this code to the entity that needs animation.
This code moves the character around.
Simply drag this code to the entity that wants to be controlled.
There is also crouching that is determined by making a crouch
in the game settings.
This also flips the character when turning right to left.
Add variables in the animator:

"Speed"
"isJumping"
"isGrounded"
"isFalling"
"isCrouching"

Subject to Change:
"WallGrab"
"verticalDirection"
*/

//////////SETUP INSTRUCTIONS//////////
//Attach this script a RigidBody2D to the player GameObject
//Set Body type to Dynamic, Collision detection to continuous and Freeze Z rotation
//Add a 2D Collider (Any will do, but 2D box collider)
//Define the ground and wall mask layers (In the script and in the GameObjects)
//Adjust and play around with the other variables (Some require you to activate gizmos in order to visualize)

public class Movement2D : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;
    private Animator _anim;
    
    [SerializeField] private Collider2D m_CrouchDisableCollider;
    bool crouch = false;
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration = 70f;
    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _groundLinearDrag = 7f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .34f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);
    private bool _facingRight = true;
    private bool _canMove => !_wallGrab || !_isDashing;
    private bool m_wasCrouching = false;

    [Header("Slope Variables")]
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private bool isOnSlope;
    private bool canWalkOnSlope;
    private Vector2 slopeNormalPerp;
    private Vector2 capsuleColliderSize;
    private CapsuleCollider2D _cc;
    private Vector2 newVelocity;




    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _downMultiplier = 12f;
    [SerializeField] private int _extraJumps = 1;
    [SerializeField] private float _hangTime = .1f;
    [SerializeField] private float _jumpBufferLength = .1f;
    private int _extraJumpsValue;
    private float _hangTimeCounter;
    private float _jumpBufferCounter;
    private bool _canJump => _jumpBufferCounter > 0f && (_hangTimeCounter > 0f || _extraJumpsValue > 0 || _onWall);
    private bool _isJumping = false;

    [Header("Wall Movement Variables")]
    [SerializeField] private float _wallSlideModifier = 0.5f;
    [SerializeField] private float _wallRunModifier = 0.85f;
    [SerializeField] private float _wallJumpXVelocityHaltDelay = 0.2f;
    private bool _wallGrab => _onWall && !_onGround && Input.GetButton("WallGrab") && !_wallRun;
    private bool _wallSlide => _onWall && !_onGround && !Input.GetButton("WallGrab") && _rb.velocity.y < 0f && !_wallRun;
    private bool _wallRun => _onWall && _verticalDirection > 0f;

    [Header("Dash Variables")]
    [SerializeField] private float _dashForce = 15f;
    [SerializeField] private float _dashLength = .15f;
    [SerializeField] private float _dashBufferLength = .1f;
    private float _dashBufferCounter;
    private bool _isDashing;
    private bool _canDash => !_isDashing && _dashBufferCounter > 0f;

    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;
    private bool _onGround;

    [Header("Wall Collision Variables")]
    [SerializeField] private float _wallRaycastLength;
    private bool _onWall;
    private bool _onRightWall;

    [Header("Corner Correction Variables")]
    [SerializeField] private float _topRaycastLength;
    [SerializeField] private Vector3 _edgeRaycastOffset;
    [SerializeField] private Vector3 _innerRaycastOffset;
    private bool _canCornerCorrect;
    public class BoolEvent : UnityEvent<bool> { }
    //public BoolEvent OnCrouchEvent;

    [Header("Audio")]
    public GameObject JumpSound;
   
    [Header("Photon")]
    public PhotonView _pv;
    public PhotonTransformViewClassic _ptv;

    private void Awake()
    {
        
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cc = GetComponent<CapsuleCollider2D>();
        _anim = GetComponent<Animator>();

        if (GameConfig.Multiplayer && !_pv.IsMine)
        {
            Destroy(gameObject.GetComponent<Movement2D>());
            
            foreach(Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        capsuleColliderSize = _cc.size;
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;
        _verticalDirection = GetInput().y;

        if (Input.GetButtonDown("Jump")) _jumpBufferCounter = _jumpBufferLength;
        else { _jumpBufferCounter -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) _dashBufferCounter = _dashBufferLength;
        else _dashBufferCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        Animation();

        if (GameConfig.Multiplayer && _pv.IsMine)
        {
            if(_rb.velocity.x > -.2f || _rb.velocity.x < .2f)
            {
                _ptv.SetSynchronizedValues(Vector3.zero, 0);
            }
            else
            {
                _ptv.SetSynchronizedValues(_rb.velocity, 0);
            }
        }

    }
    
    private void FixedUpdate()
    {
        CheckCollisions();
        FallMultiplier();
        ApplyLinearDrag();
        SlopeCheck();
        if (_canMove) MoveCharacter();
        if (_onGround)
        {
            _extraJumpsValue = _extraJumps;
            _hangTimeCounter = _hangTime;
        }
        else
        {
            _hangTimeCounter -= Time.fixedDeltaTime;
            if (!_onWall || _rb.velocity.y < 0f || _wallRun) _isJumping = false;
        }
        if (_canJump)
        {
            if (_onWall && !_onGround)
            {
                if (!_wallRun && (_onRightWall && _horizontalDirection > 0f || !_onRightWall && _horizontalDirection < 0f))
                {
                    StartCoroutine(NeutralWallJump());
                }
                else
                {
                    WallJump();
                }
                Flip();
            }
            else
            {
                Jump(Vector2.up);
                JumpSound.SetActive (true);
                JumpSound.SetActive(false);
            }
        }
        if (_canCornerCorrect) CornerCorrect(_rb.velocity.y);
        if (!_isJumping)
        {
            if (_wallSlide) WallSlide();
            if (_wallGrab) WallGrab();
            if (_wallRun) WallRun();
            if (_onWall && !_onGround) StickToWall();
        }
        if (_canDash) Dash(_horizontalDirection, _verticalDirection); 
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        
       //only control the player if grounded or airControl is turned on
        if (_onGround || m_AirControl)
        {
            if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, _groundLayer))
            {
                crouch = true;
            }
            else
            {
                crouch = false;
            }
        }

            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    //OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                _rb.velocity *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                    
                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    //OnCrouchEvent.Invoke(false);
                }
            }

    }
    if(_onGround && !isOnSlope && !_isJumping){
    _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed && !_isDashing)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
    }
    else if (_onGround && isOnSlope && canWalkOnSlope && !_isJumping) //If on slope
        {
            newVelocity.Set(_maxMoveSpeed * slopeNormalPerp.x * -_horizontalDirection, _maxMoveSpeed * slopeNormalPerp.y * -_horizontalDirection);
            _rb.velocity = newVelocity;
        }
    else if (!_onGround) //If in air
    {
_rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed && !_isDashing)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
    }
    }
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, _groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, _groundLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }
    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, _groundLayer);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && _horizontalDirection == 0.0f)
        {
            _rb.sharedMaterial = fullFriction;
        }
        else
        {
            _rb.sharedMaterial = noFriction;
        }
    }

    private void ApplyLinearDrag()
    {
        if (_isDashing)
        {
            _rb.drag = 0f;
        }
        else if (_onGround)
        {
            if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
            {
                _rb.drag = _groundLinearDrag;
            }
            else
            {
                _rb.drag = 0f;
            }
        }
        else
        {
            _rb.drag = _airLinearDrag;
        }
    }

    private void Jump(Vector2 direction)
    {
        if (_hangTimeCounter <= 0f && !_onWall)
            _extraJumpsValue--;

        _rb.drag = _airLinearDrag;
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _jumpForce, ForceMode2D.Impulse);
        _hangTimeCounter = 0f;
        _jumpBufferCounter = 0f;
        _isJumping = true;
    }

    private void WallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
    }

    IEnumerator NeutralWallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
        yield return new WaitForSeconds(_wallJumpXVelocityHaltDelay);
        _rb.velocity = new Vector2(0f, _rb.velocity.y);
    }

    private void FallMultiplier()
    {
        if (_isDashing)
        {
            _rb.gravityScale = 0;
        }
        else if (_verticalDirection < 0f)
        {
            _rb.gravityScale = _downMultiplier;
        }
        else
        {
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _fallMultiplier;
                // TEMPORARY FIX TO BONKING YOUR HEAD AND SLOWING MOVEMENT
                crouch = false;
            }
            else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                _rb.gravityScale = _lowJumpFallMultiplier;
            }
            else
            {
                _rb.gravityScale = 1f;
            }
        }
    }

    void WallGrab()
    {
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
    }

    void WallSlide()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, -_maxMoveSpeed * _wallSlideModifier);
    }

    void WallRun()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _verticalDirection * _maxMoveSpeed * _wallRunModifier);
    }

    void Dash(float x, float y)
    {
        _isDashing = true;
        //Debug.Log("Start Dash...");
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0;
        _rb.drag = 0f;
        Vector2 dir = new Vector2(x, y);
        //Debug.Log("Direction Vector: " + dir);
        //Debug.Log("Dash Force: " + _dashForce);

        if (dir != Vector2.zero)
        {
            _rb.AddForce(new Vector2(x, y).normalized * _dashForce, ForceMode2D.Impulse);
        }
        else
        {
            if (_facingRight) _rb.AddForce(Vector2.right * _dashForce, ForceMode2D.Impulse);
            else _rb.AddForce(Vector2.left * _dashForce, ForceMode2D.Impulse);
        }

        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        yield return new WaitForSeconds(_dashLength);
        _rb.gravityScale = 0;
        _rb.drag = 0f;
        _isDashing = false;
        //Debug.Log("End Dash!");
    }

    void StickToWall()
    {
        //Push player torwards wall
        if (_onRightWall && _horizontalDirection >= 0f)
        {
            _rb.velocity = new Vector2(1f, _rb.velocity.y);
        }
        else if (!_onRightWall && _horizontalDirection <= 0f)
        {
            _rb.velocity = new Vector2(-1f, _rb.velocity.y);
        }

        //Face correct direction
        if (_onRightWall && !_facingRight)
        {
            Flip();
        }
        else if (!_onRightWall && _facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void Animation()
    {
        if ((_horizontalDirection < 0f && _facingRight || _horizontalDirection > 0f && !_facingRight) && !_wallGrab && !_wallSlide)
        {
            Flip();
        }
        if (_onGround)
        {
            _anim.SetBool("isGrounded", true);
            _anim.SetBool("isFalling", false);
            _anim.SetBool("WallGrab", false);
            _anim.SetFloat("Speed", Mathf.Abs(_horizontalDirection));
            _anim.SetBool("isCrouching", crouch);
        }
        else
        {
            _anim.SetBool("isGrounded", false);
        }
        if (_isJumping)
        {
            _anim.SetBool("isJumping", true);
            _anim.SetBool("isFalling", false);
            _anim.SetBool("WallGrab", false);
            _anim.SetFloat("verticalDirection", 0f);
        }
        else
        {
            _anim.SetBool("isJumping", false);

            if (_wallGrab || _wallSlide)
            {
                _anim.SetBool("WallGrab", true);
                _anim.SetBool("isFalling", false);
                _anim.SetFloat("verticalDirection", 0f);
            }
            else if (_rb.velocity.y < -0.1f)
            {
                _anim.SetBool("isFalling", true);
                _anim.SetBool("WallGrab", false);
                _anim.SetFloat("verticalDirection", 0f);
            }
            if (_wallRun)
            {
                _anim.SetBool("isFalling", false);
                _anim.SetBool("WallGrab", false);
                _anim.SetFloat("verticalDirection", Mathf.Abs(_verticalDirection));
            }
        }
        if(_isDashing)
        {
            _anim.SetBool("isDashing", _isDashing);
        }
        else{
            _anim.SetBool("isDashing", false);
        }
    }

    void CornerCorrect(float Yvelocity)
    {
        //Push player to the right
        RaycastHit2D _hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,Vector3.left, _topRaycastLength, _groundLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x + _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
            return;
        }

        //Push player to the left
        _hit = Physics2D.Raycast(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.right, _topRaycastLength, _groundLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x - _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
        }
    }

    private void CheckCollisions()
    {
        
        //Ground Collisions
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);

        //Corner Collisions
        if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, _groundLayer))
            {
                crouch = true;
                if(!crouch){
        _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) &&
                            !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) ||
                            Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) &&
                            !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer);
        }
        }
        //Wall Collisions
        _onWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer) ||
                    Physics2D.Raycast(transform.position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Ground Check
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);

        //Corner Check
        Gizmos.DrawLine(transform.position + _edgeRaycastOffset, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _edgeRaycastOffset, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _innerRaycastOffset, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength);

        //Corner Distance Check
        Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.left * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.right * _topRaycastLength);

        //Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallRaycastLength);
    }
}