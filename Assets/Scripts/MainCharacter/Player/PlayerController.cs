using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerInputConfig playerInput;
    public GameObject bulletPrefab;

    [SerializeField] private PhysicsMaterial2D maxFriction;
    [SerializeField] private PhysicsMaterial2D highFriction;
    [SerializeField] private PhysicsMaterial2D normalFriction;


    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float rotationSeed = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playMateLayer;
    [SerializeField] private PlayerManager.Player player;

    // Cách chỉ số người chơi
    [SerializeField] private float attackSpeed = 300f;
    [SerializeField] private float attackDamage = 10f;

    private float lastGroundedTime;

    // Shoot
    private bool canShoot = true;
    private float countDownShoot = 2f;
    private float currentAttackDamage;

    // Skill: 20 giây sử dụng, 10 giây hồi chiêu
    private Bullet.BulletType bulletType;

    private bool canUseSingleSkill = true;
    private bool canUseCombineSkill = true;
    IEnumerator IESetCanUseSingleSkill = null;
    IEnumerator IESetCanUseCombineSkill = null;

    private float skillDuration = 10f;
    private float cooldown = 20f;

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

        direction = new Vector2(1, 0); // right

        bulletType = Bullet.BulletType.basic;
        currentAttackDamage = attackDamage * (int)bulletType;
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

        if (Input.GetKeyDown(playerInput.useSkill) && canUseSingleSkill)
            UseSingleSkill();

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
            rb.AddForce(direction * rotationSeed);
        else 
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 10);
    }

    private void Shoot()
    {
        anim.SetTrigger("shoot");
        GameObject bulletObject = Instantiate(bulletPrefab, rb.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(direction, attackSpeed, bulletType);
        Debug.Log(currentAttackDamage);

        StartCoroutine(SetCanShoot());
    }

    IEnumerator SetCanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(countDownShoot);
        canShoot = true;
    }
    public void UseSingleSkill()
    {
        if (IESetCanUseCombineSkill != null && bulletType == Bullet.BulletType.combineSkill)
        {
            Debug.Log("Stop use combine skill");
            StopCoroutine(IESetCanUseCombineSkill);
            StartCoroutine(cooldownCombineSkill());
        }

        IESetCanUseSingleSkill = SetCanUseSingleSkill();
        StartCoroutine(IESetCanUseSingleSkill);
    }

    IEnumerator SetCanUseSingleSkill()
    {
        Debug.Log("Start use single skill");
        canUseSingleSkill = false;

        // Sử dụng kỹ năng nên sức mạnh tấn công tăng gấp đôi
        bulletType = Bullet.BulletType.singleSkill;
        currentAttackDamage = attackDamage * (int)bulletType;

        // Hết thời gian sử dụng kỹ năng sức mạnh trở về bình thường
        yield return new WaitForSeconds(skillDuration);
        bulletType = Bullet.BulletType.basic;
        currentAttackDamage = attackDamage * (int)bulletType;
        Debug.Log("End use single skill");

        // Hết thời gian hồi chiêu, có thể sử dụng skill trở lại
        yield return new WaitForSeconds(cooldown);
        canUseSingleSkill = true;
        IESetCanUseSingleSkill = null;
        Debug.Log("Can use single skill");
    }

    IEnumerator cooldownSingleSkill()
    {
        // Hết thời gian hồi chiêu, có thể sử dụng skill trở lại
        yield return new WaitForSeconds(cooldown);
        canUseSingleSkill = true;
        Debug.Log("Can use single skill");
    }

    public void UseCombineSkill()
    {
        if(IESetCanUseSingleSkill != null && bulletType == Bullet.BulletType.singleSkill)
        {
            Debug.Log("Stop use single skill");
            StopCoroutine(IESetCanUseSingleSkill);
            StartCoroutine(cooldownSingleSkill());
        }

        IESetCanUseCombineSkill = SetCanUseCombineSkill();
        StartCoroutine(IESetCanUseCombineSkill);
    }

    IEnumerator SetCanUseCombineSkill()
    {
        Debug.Log("Start use combine skill");
        canUseCombineSkill = false;

        // Sử dụng kỹ năng nên sức mạnh tấn công tăng gấp đôi
        bulletType = Bullet.BulletType.combineSkill;
        currentAttackDamage = attackDamage * (int)bulletType;

        // Hết thời gian sử dụng kỹ năng sức mạnh trở về bình thường
        yield return new WaitForSeconds(skillDuration);
        bulletType = Bullet.BulletType.basic;
        currentAttackDamage = attackDamage * (int)bulletType;
        Debug.Log("End use combine skill");

        // Hết thời gian hồi chiêu, có thể sử dụng skill trở lại
        yield return new WaitForSeconds(cooldown);
        canUseCombineSkill = true;
        IESetCanUseCombineSkill = null;
        Debug.Log("Can use combine skill");
    }

    IEnumerator cooldownCombineSkill()
    {
        // Hết thời gian hồi chiêu, có thể sử dụng skill trở lại
        yield return new WaitForSeconds(cooldown);
        canUseCombineSkill = true;
        Debug.Log("Can use combine skill");
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
            boxCollider.sharedMaterial = maxFriction;
        else if (state == PlayerState.Idle)
            boxCollider.sharedMaterial = highFriction;
        else
            boxCollider.sharedMaterial = normalFriction;
    }

    public void updateConstraint()
    {
        if(state == PlayerState.Sitting) 
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void setPlayerState(PlayerState state)
    {
        this.state = state;
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
}
