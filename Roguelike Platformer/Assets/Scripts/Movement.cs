using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Run Settings")] 
    [SerializeField] private float speed;
    
    [Header("Jump Settings")] 
    [SerializeField] private float jumpForce;
    [SerializeField] private int amountAdditionalJumps;
    [SerializeField] private float delayBetweenJumps;
    
    [Header("Ground Settings")] 
    [SerializeField] private LayerMask groundLayer;  
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkGroundRadius;


    private Animator _animator;
    private readonly int _speedAnimName = Animator.StringToHash("speed");
    private readonly int _isJumpAnimName = Animator.StringToHash("isJump");

    private Rigidbody2D _rigidbody;

    private bool _isFlippedRight;     
    private bool _isGrounded;

    private float _timeAfterLastJump;
    private int _currentAmountAdditionalJumps;

    private void Start()
    {
        _currentAmountAdditionalJumps = amountAdditionalJumps;
        _timeAfterLastJump = delayBetweenJumps;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckGround();
        JumpLogic();
        MoveLogic();
        Flip();
        Attack();
        ExtraAttack();
    }

    private void MoveLogic()
    {
        Vector2 velocity = new Vector2(Input.GetAxis("Horizontal") * speed, _rigidbody.velocity.y);
        _rigidbody.velocity = velocity;

        _animator.SetFloat(_speedAnimName, Mathf.Abs(velocity.x));
    }

    private void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0 && !_isFlippedRight)
        {
            transform.localScale = Vector3.one;
            _isFlippedRight = !_isFlippedRight;
        }
        
        if (Input.GetAxis("Horizontal") < 0 && _isFlippedRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _isFlippedRight = !_isFlippedRight;
        }
        
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkGroundRadius, groundLayer);
        if (_isGrounded)
            _currentAmountAdditionalJumps = amountAdditionalJumps;
    }

    private void JumpLogic()
    {
        //todo прыжок перед касанием
        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded)
            {
                _rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                _timeAfterLastJump = delayBetweenJumps;
                _animator.Play("Jump");
            }
            else if (_currentAmountAdditionalJumps > 0 && _timeAfterLastJump <= 0)
            {
                _rigidbody.AddForce(transform.up * (float) (jumpForce / 1.5), ForceMode2D.Impulse);
                _currentAmountAdditionalJumps--;
                _timeAfterLastJump = delayBetweenJumps;
                _animator.Play("HighJump");
            }
        }
        
        _timeAfterLastJump -= Time.deltaTime;

        //_animator.SetBool(_isJumpAnimName, !_isGrounded);
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _animator.Play("Attack");
        }
    }

    private void ExtraAttack()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            _animator.Play("ExtraAttack");
        }
    }
}