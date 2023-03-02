using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Player基本信息")]
    public float maxHealth;
    public float currentHealth;

    [Header("Player移动")]
    private Vector2 movement; // 键盘移动指令
    public float normalMoveSpeed;
    public float moveSpeed;
    public float moveSpeedUpMult;

    [Header("Player攻击")]
    public float attackDMG;
    public float attackInterval;
    public GameObject bulletPrefab;
    public Transform shootRing;
    public Transform shootPoint;
    private Vector2 mousePosition; // 鼠标位置
    private Vector2 attackDirection;
    public float attackCost;
    public bool isDead;

    private Vector2 currentVelocity;
    private float timer; // 计时器
    public ParticleSystem outPS;

    [Header("回收子弹")]
    public List<Bullet> bullets;
    private bool isTakeBack;
    private Vector2 currentVelocity2;

    private void Awake()
    {
        moveSpeed = normalMoveSpeed;

        bullets = new List<Bullet>();

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 获取键盘移动指令
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 获取鼠标位置
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 收回子弹
        if (Input.GetMouseButtonDown(1) && bullets.Count != 0)
        {
            isTakeBack = true;
        }

        shoot();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead();
        }
        // 根据血量更新
        UpdateHealthBuff();
    }

    private void FixedUpdate()
    {
        // rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        Vector2 targetPos = new Vector2(transform.position.x + movement.x * moveSpeed * Time.deltaTime, transform.position.y + movement.y * moveSpeed * Time.deltaTime);
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref currentVelocity, 0.2f);

        // 收回子弹
        if (isTakeBack)
        {
            TakeBack();
        }
    }

    private void Dead()
    {
        // gameover
        isDead = true;
        Destroy(gameObject);
        GameManager.instance.GameOver(isDead);
        // Destroy(gameObject);
    }

    private void shoot()
    {
        attackDirection = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        shootRing.eulerAngles = new Vector3(shootRing.eulerAngles.x, shootRing.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan2(attackDirection.y, attackDirection.x));

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (timer == 0)
            {
                Fire();
                timer = attackInterval;
            }
        }
    }

    private void Fire()
    {
        if (currentHealth - attackCost <= 0)
        {
            return;
        }

        // TODO：玩家分裂动画
        SpawnOutPS();

        currentHealth -= attackCost;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullets.Add(bullet.GetComponent<Bullet>());

        // TODO：发射音效
        AudioManager.instance.PlayPlayerAttackAudio();
        bullet.GetComponent<Bullet>().SetSpeed(attackDirection);
    }

    public void TakeDamage(GameObject enemy)
    {
        Enemy en = enemy.GetComponent<Enemy>();
        en.currentHealth -= attackDMG;
        AudioManager.instance.PlayEnemyGetHitAudio();

        // TODO: 变红特效
        en.FlashColor();
    }

    // 由血量引发的效果
    private void UpdateHealthBuff()
    {
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        float rate = currentHealth / maxHealth;

        transform.localScale = new Vector3(1 - 0.5f * (1 - rate), 1 - 0.5f * (1 - rate), transform.localScale.z);

        moveSpeed = normalMoveSpeed + moveSpeedUpMult * (1 - rate);
    }

    public void SpawnOutPS()
    {
        outPS.Play();
    }

    private void TakeBack()
    {
        foreach (var bullet in bullets)
        {
            if (bullet.isStop)
            {
                bullet.isStop = false;
                bullet.isTakebacking = true;
                bullet.BulletTakeBack();

                AudioManager.instance.PlayPlayerTakeBackAudio();
            }
        }
        isTakeBack = false;
    }

    // IEnumerator TakeBackIE(Bullet bullet)
    // {
    // while(Mathf.Abs(transform.position.sqrMagnitude - bullet.transform.position.sqrMagnitude) > 0.05f)
    // {
    //     bullet.transform.position = Vector2.SmoothDamp(bullet.transform.position, transform.position, ref currentVelocity2, 0.2f);
    //     yield return null;
    // }
    // bullet.isStop = true;
    // }
}
