using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using TMPro;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [Header("Horizontal Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runMultiplier;
    [SerializeField] private float _horitzontalMovement;

    [Header("Vertical Movement")]
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector2 _jumpVector;
    [SerializeField] private int _doubleJumpCounter;
    [SerializeField] private int _doubleJumpLimit;

    [Header("Sprites")]
    [SerializeField] private int _currentDirection;
    [SerializeField] private int _newDirection;
    [SerializeField] private SpriteRenderer _playerSprite;
    private const int _goingRight = 1;
    private const int _goingLeft = 0;
    [SerializeField] private float _spriteRotation;
    [SerializeField] private float _rotationTimer;
    [SerializeField] private float _resetRotationTimer;

    [Header("Animations")]
    [SerializeField] Animator _animator;
    [SerializeField] private bool _isJumping;

    [Header("Damage and Death")]
    [SerializeField] private Vector2 _damageVector;
    [SerializeField] private float _damageForce;
    [SerializeField] private Vector2 _deathVector;
    [SerializeField] private float _deathForce;
    [SerializeField] private int _health;
    [SerializeField] private Collider2D _playerCollider;
    [SerializeField] private TextMeshProUGUI _youDiedText;

    [Header("Collectables")]
    [SerializeField] private int _numberOfCoins;

    public UnityEvent<int> OnTotalHealthChanged;
    public UnityEvent OnDeath;

    private int TotalHealth
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            OnTotalHealthChanged.Invoke(_health);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _jumpVector = new Vector2(0, _jumpForce);
        _currentDirection = _goingRight;
        _playerSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerCollider = GetComponent<Collider2D>();
        _gameManager = FindObjectOfType<GameManager>();
        TotalHealth = _health;
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        VerticalMovement();
        StandBackUp();
        if(transform.position.y < -10)
        {
            Die();
        }
    }

    /// <summary>
    /// Horizontal Movement allows walk speed and runs when Left Shift is pressed.  
    /// </summary>
    private void HorizontalMovement()
    {
        _horitzontalMovement = _walkSpeed;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newDirection = _goingRight;
            SpriteFlip(_currentDirection, _newDirection);
            _currentDirection = _goingRight;
            if(Input.GetKey(KeyCode.LeftShift))
            {
                _horitzontalMovement *= _runMultiplier;
                _animator.SetFloat("runSpeed", 2f);
            } else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                _animator.SetFloat("runSpeed", 1f);
            }
            transform.Translate(_horitzontalMovement * Time.deltaTime, 0, 0);
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isIdle", false);

        } else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newDirection = _goingLeft;
            SpriteFlip(_currentDirection, _newDirection);
            _currentDirection = _goingLeft;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _horitzontalMovement *= _runMultiplier;
                _animator.SetFloat("runSpeed", 2f);
            } else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _animator.SetFloat("runSpeed", 1f);
            }
            transform.Translate(-_horitzontalMovement * Time.deltaTime, 0, 0);
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isIdle", false);
        } else
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isIdle", true);
        }
    }

    /// <summary>
    /// VerticalMovement allows jumping and double jumping
    /// </summary>
    private void VerticalMovement()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _doubleJumpCounter <= _doubleJumpLimit)
        {
            _playerRigidBody.AddForce(_jumpVector, ForceMode2D.Impulse);
            _animator.SetTrigger("isJumpStart");
            _animator.SetBool("isJumping", true);
            _isJumping = true;
            _doubleJumpCounter++;
        }
    }
    /// <summary>
    /// Checks:
    ///     1) double jump counter reset when touching the ground
    ///     2)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _doubleJumpCounter = 0;
            if(_isJumping)
            {
                _animator.SetTrigger("isLanding");
                _animator.SetBool("isJumping", false);
                _isJumping = false;
            }
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            _numberOfCoins++;
            _gameManager.CollectCollectables();
        }
    }

    private void TakeDamage(int damage)
    {
        _damageVector = new Vector2(-_damageForce, _damageForce);
        _playerRigidBody.AddForce(_damageVector, ForceMode2D.Impulse);
        transform.Rotate(0f, 0f, 90f);
        _health -= damage;
        TotalHealth = _health;
        if(_health <= 0)
        {
            Die();
        }
    }

    private void Death()
    {
        OnDeath.Invoke();
    }

    private void Die()
    {
        _deathVector = new Vector2(0, _deathForce);
        _playerRigidBody.AddForce(_deathVector, ForceMode2D.Impulse);
        transform.Rotate(0f, 0f, 180f);
        _playerCollider.enabled = false;
        _youDiedText.enabled = true;
        Invoke("Death", 5f);
    }
    /// <summary>
    /// Flips the direction sprite is facing
    /// </summary>
    /// <param name="currentDirection"></param>
    /// <param name="newDirection"></param>
    private void SpriteFlip(int currentDirection, int newDirection)
    {
        if(currentDirection == newDirection)
        {
            return;
        } else
        {
            _playerSprite.flipX = !_playerSprite.flipX;
            return;
        }
    }
    /// <summary>
    /// Fixes bug where sprite falls sideways after a period of time
    /// </summary>
    private void StandBackUp()
    {
        _spriteRotation = Mathf.Abs(transform.rotation.eulerAngles.z);
        if (_spriteRotation >= 80f)
        {
            _rotationTimer += Time.deltaTime;
            if (_rotationTimer > _resetRotationTimer)
            {
                transform.rotation = Quaternion.identity;
                _rotationTimer = 0;
            }
        } else
        {
            _rotationTimer = 0;
        }
    }
}
