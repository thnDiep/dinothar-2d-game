using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private PlayerManager.Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playMateLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerInputConfig playerInput;
    private SkillBarUI skillBarUI;

    // check trạng thái Hanging
    private float lastGroundedTime;

    // Shot
    private bool canShoot = true;
    private float countDownShootTime = 1.0f;

    // Skill: 20 giây sử dụng, 10 giây hồi chiêu
    private Bullet.BulletType bulletType;

    private bool canUseSingleSkill = true;
    private bool canUseCombineSkill = true;
    IEnumerator IESetCanUseSingleSkill = null;
    IEnumerator IESetCanUseCombineSkill = null;

    private float skillDurationTime = 10f;
    private float singleSkillCooldownTime = 20f;
    private float combineSkillCooldownTime = 30f;

    public enum PlayerState
    {
        Idle,
        Running,
        Sitting,
        Jumping,
        Carrying,
    }

    private PlayerState state;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerInput = new PlayerInputConfig(player);
    }

    private void Start()
    {
        if (player == PlayerManager.Player.Player1)
            skillBarUI = PlayerManager.Instance.uIInGame.skillBar1;
        else
            skillBarUI = PlayerManager.Instance.uIInGame.skillBar2;

        direction = new Vector2(1, 0); // right
        bulletType = Bullet.BulletType.basic;
    }

    private void Update()
    {
        //horizontalInput = 0f;

        if (isGrounded())
            lastGroundedTime = Time.time;

        // Cõng bạn
        if (bottomPlayer() && !isSitting())
            setPlayerState(PlayerState.Carrying);
        else if (isCarrying() && !bottomPlayer())
            setPlayerState(PlayerState.Idle);

        // Ngồi
        if (Input.GetKeyDown(playerInput.moveDown) && isGrounded())
        {
            setPlayerState(PlayerState.Sitting);
            PlayerManager.Instance.State = PlayerManager.PlayerState.Rotate;
        }

        if (isSitting() && Input.GetKeyUp(playerInput.moveDown))
        {
            setPlayerState(PlayerState.Idle);
            PlayerManager.Instance.State = PlayerManager.PlayerState.Normal;
        }

        if (!isSitting() && !isCarrying())
        {
            setPlayerState(PlayerState.Idle);

            // Chạy
            if (Input.GetKey(playerInput.moveLeft) || Input.GetKey(playerInput.moveRight))
            {
                Move();
                setPlayerState(PlayerState.Running);
            }

            // Nhảy
            if (Input.GetKeyDown(playerInput.moveUp) && isGrounded())
            {
                Jump();
                setPlayerState(PlayerState.Jumping);
            }
        }

        if (Input.GetKeyDown(playerInput.shoot) && canShoot)
            Shoot();

        if (PlayerManager.Instance.Stage == PlayerManager.PlayerStage.Fight)
        {
            skillBarUI.gameObject.SetActive(true);

            if (Input.GetKeyDown(playerInput.useSkill) && canUseSingleSkill)
                UseSingleSkill();
        }

        updateAnimation();  // Cập nhật Animation theo trạng thái của player
        updateFriction();   // Cập nhật ma sát
        updateConstraint(); // Cập nhật constraint
    }

    private void Move()
    {
        if (Input.GetKey(playerInput.moveLeft))
        {
            direction = new Vector2(-1, 0);
        }
        else if (Input.GetKey(playerInput.moveRight))
        {
            direction = new Vector2(1, 0);
        }

        // Nếu thời gian lần cuối chạm đất lớn hơn 1s -> player bị treo lơ lửng -> xoay
        if (PlayerManager.Instance.State == PlayerManager.PlayerState.Rotate && !isGrounded() && Time.time - lastGroundedTime >= 1f)
            rb.AddForce(direction * rotationSpeed);
        else
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 15);
    }

    private void Shoot()
    {
        anim.SetTrigger("shoot");
        GameObject bulletObject = Instantiate(bulletPrefab, rb.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(direction, PlayerManager.Instance.ATTACK_SPEED, bulletType, PlayerManager.Instance.ATK);
        //Debug.Log("damage:" + currentAttackDamage);

        StartCoroutine(SetCanShoot());
    }

    public void UseSingleSkill()
    {
        // Nếu đang sử dụng combine skill thì ngừng sử dụng và đếm ngược thời gian hồi chiêu của combine skill
        if (IESetCanUseCombineSkill != null && bulletType == Bullet.BulletType.combineSkill)
        {
            Debug.Log("Stop use combine skill");
            StopCoroutine(IESetCanUseCombineSkill);
            StartCoroutine(CooldownCombineSkill());
        }

        IESetCanUseSingleSkill = SetCanUseSingleSkill();
        StartCoroutine(IESetCanUseSingleSkill);
    }

    public void UseCombineSkill()
    {
        // Nếu đang sử dụng single skill thì ngừng sử dụng và đếm ngược thời gian hồi chiêu của single skill
        if (IESetCanUseSingleSkill != null && bulletType == Bullet.BulletType.singleSkill)
        {
            Debug.Log("Stop use single skill");
            StopCoroutine(IESetCanUseSingleSkill);
            StartCoroutine(CooldownSingleSkill());
        }

        IESetCanUseCombineSkill = SetCanUseCombineSkill();
        StartCoroutine(IESetCanUseCombineSkill);
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("hurt");
        PlayerManager.Instance.TakeDamage(damage);
    }

    private void updateAnimation()
    {

        anim.SetBool("grounded", isGrounded());
        anim.SetBool("run", isRunning());
        anim.SetBool("sit", isSitting() || isCarrying());

        if (isJumping())
            anim.SetTrigger("jump");

        // Xử lý quay mặt
        transform.localScale = new Vector3(direction.x, 1, 1);
    }

    public void updateFriction()
    {
        if (state == PlayerState.Sitting)
            boxCollider.sharedMaterial = PlayerManager.Instance.maxFriction;
        else if (state == PlayerState.Idle)
            boxCollider.sharedMaterial = PlayerManager.Instance.highFriction;
        else
            boxCollider.sharedMaterial = PlayerManager.Instance.normalFriction;
    }

    public void updateConstraint()
    {
        if (state == PlayerState.Sitting)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void setPlayerState(PlayerState state)
    {
        this.state = state;
    }

    public PlayerState getPlayerState()
    {
        return this.state;
    }

    public bool isRunning()
    {
        return state == PlayerState.Running;
    }

    public bool isSitting()
    {
        return state == PlayerState.Sitting;
    }

    public bool isJumping()
    {
        return state == PlayerState.Jumping;
    }

    public bool isCarrying()
    {
        return state == PlayerState.Carrying;
    }

    // Player tiếp đất khi đứng trên mặt đất hoặc đứng trên player khác
    public bool isGrounded()
    {
        RaycastHit2D groundLayerRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D playMateLayerRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, playMateLayer);

        return groundLayerRaycastHit.collider != null || playMateLayerRaycastHit.collider != null;
    }

    // Bị player khác leo lên đầu
    public bool bottomPlayer()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, playMateLayer);
        return raycastHit.collider != null;
    }

    // Đang hướng về người chơi khác:
    // 1. Người chơi khác đứng bên phải và chạy về bên phải
    // 2. Người chơi khác đứng bên trái và chạy về bên trái
    public bool isRunToPlayer(PlayerController otherPlayer)
    {
        // xác định vị trí người chơi khác so với người chơi hiện tại
        float positionSign = Mathf.Sign(otherPlayer.transform.position.x - transform.position.x); // -1: left, 1: right
        return (positionSign == direction.x);
    }

    public bool getCanUseCombineSkill()
    {
        return canUseCombineSkill;
    }

    IEnumerator SetCanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(countDownShootTime);
        canShoot = true;
    }

    IEnumerator SetCanUseSingleSkill()
    {
        //Debug.Log("Start use single skill");
        canUseSingleSkill = false;
        skillBarUI.startUseSingleSkillCooldown();

        // Sử dụng kỹ năng
        bulletType = Bullet.BulletType.singleSkill;

        // Đếm ngược thời gian sử dụng skill
        float remainingTime = skillDurationTime;
        while (remainingTime > 0)
        {
            skillBarUI.applySingleSkillCooldown(remainingTime, skillDurationTime, false);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // Hết thời gian sử dụng kỹ năng
        bulletType = Bullet.BulletType.basic;
        //Debug.Log("End use single skill");

        StartCoroutine(CooldownSingleSkill());
    }

    IEnumerator CooldownSingleSkill()
    {
        // Đếm ngược thời gian hồi chiêu skill
        float remainingTime = singleSkillCooldownTime;
        while (remainingTime > 0)
        {
            skillBarUI.applySingleSkillCooldown(remainingTime, singleSkillCooldownTime, true);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // Hết thời gian hồi chiêu, có thể sử dụng skill trở lại
        canUseSingleSkill = true;
        skillBarUI.stopUseSingleSkillCooldown();
        IESetCanUseSingleSkill = null;
        //Debug.Log("Can use single skill");
    }

    IEnumerator SetCanUseCombineSkill()
    {
        //Debug.Log("Start use combine skill");
        canUseCombineSkill = false;
        skillBarUI.startUseCombineSkillCooldown();

        // Sử dụng kỹ năng
        bulletType = Bullet.BulletType.combineSkill;

        // Đếm ngược thời gian sử dụng skill
        float remainingTime = skillDurationTime;
        while (remainingTime > 0)
        {
            skillBarUI.applyCombineSkillCooldown(remainingTime, skillDurationTime, false);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // Hết thời gian sử dụng kỹ năng 
        bulletType = Bullet.BulletType.basic;
        //Debug.Log("End use combine skill");

        StartCoroutine(CooldownCombineSkill());
    }

    IEnumerator CooldownCombineSkill()
    {
        // Đếm ngược thời gian hồi chiêu
        float remainingTime = combineSkillCooldownTime;
        while (remainingTime > 0)
        {
            skillBarUI.applyCombineSkillCooldown(remainingTime, combineSkillCooldownTime, true);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // Hết thời gian hồi chiêu, có thể sử dụng skill trở lại
        canUseCombineSkill = true;
        skillBarUI.stopUseCombineSkillCooldown();
        IESetCanUseCombineSkill = null;
        //Debug.Log("Can use combine skill");
    }
}
