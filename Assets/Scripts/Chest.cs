using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    public TextMeshProUGUI text;
    [SerializeField] private float fadeTime;
    public bool isOpen;
    private PlayerController pc;

    [Header("Buff")]
    public float buffAttackDMG;
    public float buffAttackCost;
    public float buffMoveSpeed;
    public float buffMaxHealth;


    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pc = GameManager.instance.player.GetComponent<PlayerController>();

        text.gameObject.SetActive(false);
        isOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOpen)
            {
                PlayerOpen();
            }
        }
    }

    public void PlayerOpen()
    {
        isOpen = true;
        text.gameObject.SetActive(true);

        AudioManager.instance.PlayPlayerGetChestAudio();

        SelectBuff();

        StartCoroutine(Fade());
    }

    public void SelectBuff()
    {
        switch ((int)Random.Range(0, 8))
        {
            case 0:
                pc.attackDMG += pc.attackDMG * buffAttackDMG;
                text.text = "获得增益：攻击力上升";
                break;
            case 1:
                pc.attackDMG -= pc.attackDMG * buffAttackDMG;
                text.text = "获得减益：攻击力下降";
                break;
            case 2:
                pc.attackCost += pc.attackCost * buffAttackCost;
                text.text = "获得减益：攻击损耗上升";
                break;
            case 3:
                pc.attackCost -= pc.attackCost * buffAttackCost;
                text.text = "获得增益：攻击损耗下降";
                break;
            case 4:
                pc.normalMoveSpeed += pc.normalMoveSpeed * buffMoveSpeed;
                text.text = "获得增益：移动速度上升";
                break;
            case 5:
                pc.normalMoveSpeed -= pc.normalMoveSpeed * buffMoveSpeed;
                text.text = "获得减益：移动速度下降";
                break;
            case 6:
                pc.maxHealth += pc.maxHealth * buffMaxHealth;
                pc.currentHealth = pc.maxHealth;
                text.text = "获得增益：最大生命值上升";
                break;
            case 7:
                pc.maxHealth -= pc.maxHealth * buffMaxHealth;
                pc.currentHealth = pc.currentHealth > pc.maxHealth ? pc.maxHealth : pc.currentHealth;
                text.text = "获得减益：最大生命值下降";
                break;

        }
    }

    IEnumerator Fade()
    {
        float timer = fadeTime;
        while (timer >= 0)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, timer / fadeTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, timer / fadeTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        Destroy(this);
    }
}
