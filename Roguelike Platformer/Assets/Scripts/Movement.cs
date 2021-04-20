using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Ground Settings")] [SerializeField]
    private LayerMask groundLayer;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkGroundRadius;
    private bool _isGrounded;

    private readonly int _speedAnimName = Animator.StringToHash("speed");
    private readonly int _isJumpAnimName = Animator.StringToHash("isJump");

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private bool _isFlipRight;

    private void Start()
    {
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
        transform.localRotation = Quaternion.Euler(
            0,
            Input.GetAxis("Horizontal") > 0 ? 0 : 180,
            0
        );
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkGroundRadius, groundLayer);
    }

    private void JumpLogic()
    {
        //todo прыжок перед касанием
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        _animator.SetBool(_isJumpAnimName, !_isGrounded);
    }

    private void Attack()
    {
        _animator.SetBool("isAttacking", false);
        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetBool("isAttacking", true);
            Debug.Log("down");
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