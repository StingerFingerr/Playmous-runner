using System;
using System.Collections;
using Blocks;
using UnityEngine;
using UnityEngine.UI;

public class Player: MonoBehaviour
{
    public float jumpForce = 3f;
    public float doubleJumpForce = 6f;
    public float speed = 3f;
    public LayerMask groundLayer;
    
    public GameObject startWindow;
    public PlayerInput input;
    public Animator animator;
    public Rigidbody rb;
    public Text healthText;
    public BaseBlock LastBlock { get; private set; }
    public bool IsInvulnerable { get; set; }

    private PlayerStats _stats;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int DoubleJump1 = Animator.StringToHash("DoubleJump");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int Reset = Animator.StringToHash("Reset");

    public bool isRunning;
    public bool isGrounded;

    private Action _onWin;
    private Action _onLose;

    private float _currentHealth;

    private void Awake()
    {
        input.onTap += Jump;
        input.onDoubleTap += DoubleJump;
    }

    private void Update()
    {
        if (isRunning)        
            transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void ResetPlayer(Vector3 pos, Action onWin, Action onLose)
    {
        _onWin = onWin;
        _onLose = onLose;
        
        transform.position = pos + Vector3.up;
        transform.eulerAngles = Vector3.zero;
        
        input.gameObject.SetActive(false);
        startWindow.SetActive(true);

        LastBlock = null;
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        animator.SetBool(Run, false);
        animator.SetTrigger(Reset);

        speed = _stats.speed;
        if (LastBlock)
        {
            if (LastBlock.savePos)
                transform.position = LastBlock.savePos.position;
            else
                transform.position = LastBlock.transform.position + Vector3.up;
            
            speed *= .2f;
            StartCoroutine(RestoreSpeed());
            StartRun();
        }

        _currentHealth = _stats.health;
        UpdateHealth();
    }

    public void Initialize(PlayerStats stats) => 
        _stats = stats;

    public void StartRun()
    {
        startWindow.SetActive(false);
        input.gameObject.SetActive(true);
        animator.SetBool(Run, true);
        isRunning = true;
    }

    private void DoubleJump()
    {
        if(isGrounded)
        {
            rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            animator.SetTrigger(DoubleJump1);
        }
    }

    private void Jump()
    {
        if(isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger(Jump1);
        }
    }

    private void OnCollisionEnter(Collision other) => isGrounded = true;

    private void OnCollisionStay(Collision other) => isGrounded = true;

    private void OnCollisionExit(Collision other) => isGrounded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            LastBlock = other.gameObject.GetComponentInParent<BaseBlock>();
            if(SetDamage())
                return;
            if(LastBlock.savePos)
                transform.position = LastBlock.savePos.position;
            return;
        }

        if (other.CompareTag("Finish"))
        {
            animator.SetBool(Run, false);
            isRunning = false;
            input.gameObject.SetActive(false);
            _onWin.Invoke();
        }
    }

    private bool SetDamage()
    {
        if (IsInvulnerable)
            return false;
        
        _currentHealth--;
        UpdateHealth();
        if (_currentHealth <= 0)
        {
            Die();
            return true;
        }
        
        return false;
    }

    private IEnumerator RestoreSpeed()
    {
        while (Math.Abs(speed - _stats.speed) > .1)
        {
            speed = Mathf.Lerp(speed, _stats.speed, Time.deltaTime * .5f);
            yield return null;
        }

        speed = _stats.speed;
    }

    private void Die()
    {
        animator.SetTrigger(Die1);
        isRunning = false;
        input.gameObject.SetActive(false);
        
        
        _onLose.Invoke();
    }

    public void AddHealth(int hp)
    {
        _currentHealth += hp;
        UpdateHealth();
    }

    private void UpdateHealth() => 
        healthText.text = $"HP: {_currentHealth}";
}