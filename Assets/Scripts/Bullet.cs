using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed; // 子弹速度
    public float moveTime = 1.0f;
    public float bulletDeadTime = 5.0f;
    public bool isStop; // 子弹停下了
    private bool flagIE;
    private GameObject player;
    public bool isShooting { get; set; }
    public bool isTakebacking { get; set; }

    // [Header("回收子弹")]
    // public float takeBackSpeed;
    // public float takeBackInterval;
    // public float timer;
    // public bool isTakeBack;
    private Vector2 currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (isStop && !flagIE)
        {
            BulletDead();
        }

        // if (Input.GetMouseButtonDown(1))
        // {
        //     isTakeBack = true;
        // }

    }

    private void FixedUpdate()
    {
        // if (isTakeBack)
        // {
        //     TakeBack();
        // }
    }

    public void SetSpeed(Vector2 direction)
    {
        StartCoroutine(BulletSpeed(direction));
    }

    IEnumerator BulletSpeed(Vector2 direction)
    {
        float timer = moveTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0;
            rb.velocity = direction * speed * timer;
            yield return null;
        }
        isStop = true;
    }

    private void BulletDead()
    {
        StartCoroutine(BulletDeadIE());
    }

    IEnumerator BulletDeadIE()
    {
        float timer = bulletDeadTime;
        flagIE = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);

        player.GetComponent<PlayerController>().bullets.Remove(this);

        flagIE = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO:子弹销毁特效

        // TODO:留在地上让玩家捡起


        // 当碰到墙壁
        if (other.CompareTag("Wall"))
        {
            StopAllCoroutines();
            rb.velocity = Vector2.zero;
            isStop = true;
        }
        // 当碰到敌人
        if (other.CompareTag("Enemy") && !isStop)
        {
            StopAllCoroutines();
            rb.velocity = Vector2.zero;
            isStop = true;

            player.GetComponent<PlayerController>().TakeDamage(other.gameObject);
        }
        // 碰到玩家
        if (other.CompareTag("Player") && isStop || other.CompareTag("Player") && isTakebacking)
        {
            Destroy(gameObject);

            player.GetComponent<PlayerController>().bullets.Remove(this);

            AudioManager.instance.PlayPlayerHealAudio();

            PlayerController pc = player.GetComponent<PlayerController>();
            pc.currentHealth += pc.attackCost;
        }
    }

    // 技能 回收子弹
    // public void TakeBack()
    // {
    //     if (isStop)
    //     {
    // isStop = false;
    // transform.position = Vector2.SmoothDamp(transform.position, player.transform.position, ref currentVelocity, 0.2f);
    // if(player.transform.position.sqrMagnitude - transform.position.sqrMagnitude < 0.01f)
    // {
    //     Debug.Log("?");
    //     isStop = true;
    // }
    //     }
    // }

    public void BulletTakeBack()
    {
        if (this != null)
        {
            StartCoroutine(BulletTakeBackIE());
        }
    }

    IEnumerator BulletTakeBackIE()
    {
        if (player != null && this != null)
        {
            while (Mathf.Abs(player.transform.position.sqrMagnitude - transform.position.sqrMagnitude) > 0.5f)
            {
                // Debug.Log(this.name + " " + Mathf.Abs(player.transform.position.sqrMagnitude - transform.position.sqrMagnitude) + " " + player.transform.position);
                transform.position = Vector2.SmoothDamp(transform.position, player.transform.position, ref currentVelocity, 0.3f);
                yield return null;
            }
            isStop = true;
            isTakebacking = false;
        }

    }
}
