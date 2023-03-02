using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("Enemy基本信息")]
    [SerializeField] public float maxHealth;
    [SerializeField] public float currentHealth;
    [SerializeField] private float attackDMG;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackForce;
    [SerializeField] private Vector2 attackDirection;
    [SerializeField] private float attackInterval;
    [SerializeField] private bool isAttackReady;
    public bool isDead;
    // [SerializeField] private float attackRadius;
    // public CircleCollider2D attackColl;
    private float timer;
    private Vector2 currentVelocity;

    [Header("GetHitFX")]
    [SerializeField] private Color originColor;
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashTime;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // attackColl.radius = attackRadius;
        timer = attackInterval;
    }

    private void Update()
    {
        if (GameManager.instance.player != null)
        {
            // 攻击方向时刻对着玩家
            attackDirection = (GameManager.instance.player.transform.position - transform.position).normalized;
        }

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
                Dash(attackDirection);
                timer = attackInterval;
            }
        }

        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        isDead = true;
        Destroy(gameObject);
    }

    // 向玩家冲撞
    private void Dash(Vector2 attackDirection)
    {
        rb.velocity = attackDirection * attackSpeed;
        // StartCoroutine(DashIE(attackDirection));
    }

    // IEnumerator DashIE(Vector2 attackDirection)
    // {
    //     Vector2 playerPos = GameManager.instance.player.transform.position;
    //     while(playerPos.sqrMagnitude - transform.position.sqrMagnitude > 0.01f)
    //     {
    //         transform.position = Vector2.SmoothDamp(transform.position, playerPos, ref currentVelocity, 0.2f);
    //         yield return null;
    //     }
    //     timer = attackInterval;
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TakeDamage(other.gameObject);
        }
    }

    private void TakeDamage(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.currentHealth -= attackDMG;
        
        pc.SpawnOutPS();

        AudioManager.instance.PlayPlayerGetHitAudio();

        float randomPosX = Random.Range(-1, 1);
        float randomPosY = Random.Range(-1, 1);
        GameObject bullet = Instantiate(pc.bulletPrefab, player.transform.position + new Vector3(randomPosX, randomPosY, 0), Quaternion.identity);
        
        player.GetComponent<PlayerController>().bullets.Add(bullet.GetComponent<Bullet>());

        bullet.GetComponent<Bullet>().isStop = true;

        // 击退
        Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
        prb.velocity = Vector2.zero;
        rb.velocity = Vector2.zero;

        // Vector2 hitBackPos = new Vector2(player.transform.position.x, player.transform.position.y) + attackDirection * attackForce;
        // player.transform.position = hitBackPos;
        // player.transform.position = Vector2.SmoothDamp(player.transform.position, hitBackPos, ref currentVelocity, 0.5f); 
        // StartCoroutine(HitBackIE(player));
    }

    // IEnumerator HitBackIE(GameObject player)
    // {
    //     Vector2 hitBackPos = new Vector2(player.transform.position.x, player.transform.position.y) + attackDirection * attackForce;
    //     while(player.transform.position.sqrMagnitude - hitBackPos.sqrMagnitude > 0.01f)
    //     {
    //         player.GetComponent<PlayerController>().enabled = false;
    //         player.transform.position = Vector2.SmoothDamp(player.transform.position, hitBackPos, ref currentVelocity, 0.2f); 
    //         yield return null;
    //     }
    //     player.GetComponent<PlayerController>().enabled = true;
    // }


    // 受伤效果
    public void FlashColor()
    {
        spriteRenderer.color = flashColor;
        Invoke("ResetColor", flashTime);
    }
    public void ResetColor()
    {
        spriteRenderer.color = originColor;
    }
}
