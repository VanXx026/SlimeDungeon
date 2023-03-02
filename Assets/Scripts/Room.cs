using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    public GameObject doorRight;
    public GameObject doorLeft;
    public GameObject doorUp;
    public GameObject doorDown;
    public int doorNum;
    public List<GameObject> doorList;

    public bool roomRight;
    public bool roomLeft;
    public bool roomUp;
    public bool roomDown;

    public int setpToStart;
    public TextMeshProUGUI text; // test

    [Header("房间战斗逻辑")]
    public bool isFight;
    public List<Enemy> roomEnemys;
    public int roomEnemysInitNum;
    public string roomType;
    public GameObject enemyPrefab;
    public bool isFirstInRoom;
    public bool isFinish;
    public Vector2 roomSize;
    public GameObject chestPrefab;
    public GameObject EndPointPrefab;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        doorList = new List<GameObject>();
    }

    private void OnEnable()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    private void Start()
    {
        doorRight.SetActive(false);
        doorLeft.SetActive(false);
        doorUp.SetActive(false);
        doorDown.SetActive(false);

        roomSize = boxCollider2D.size;
        isFinish = false;
        isFight = false;

        if(roomType == "EndRoom")
        {
            Instantiate(EndPointPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (isFight)
        {
            RoomEnemysDead();
        }

    }

    public void UpdateRoom(float xOffset, float yOffset)
    {
        setpToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));
        text.text = setpToStart.ToString();

        if (roomUp)
        {
            doorNum++;
            doorList.Add(doorUp);
        }
        if (roomDown)
        {
            doorNum++;
            doorList.Add(doorDown);
        }
        if (roomLeft)
        {
            doorNum++;
            doorList.Add(doorLeft);
        }
        if (roomRight)
        {
            doorNum++;
            doorList.Add(doorRight);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isFinish)
            {
                isFight = true;
                OpenDoor(false);
                GenerateRoomEnemy();
            }

            // 地图渲染用
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

            Camera.main.GetComponent<CameraController>().ChangeTarget(transform);
        }
    }

    private void GenerateRoomEnemy()
    {
        if (roomEnemysInitNum == 0)
        {
            return;
        }
        for (int i = 0; i < roomEnemysInitNum; i++)
        {
            float enemyPosX = transform.position.x + Random.Range(-1f, 1f) * roomSize.x / 2.3f;
            float enemyPosY = transform.position.y + Random.Range(-1f, 1f) * roomSize.y / 2.3f;
            var enemy = Instantiate(enemyPrefab, new Vector2(enemyPosX, enemyPosY), Quaternion.identity);
            roomEnemys.Add(enemy.GetComponent<Enemy>());
        }
    }

    private void RoomEnemysDead()
    {
        for (int i = 0; i < roomEnemys.Count; i++)
        {
            if (roomEnemys[i].isDead)
            {
                roomEnemys.Remove(roomEnemys[i]);
                i--;
            }
        }
        if (roomEnemys.Count == 0)
        {
            isFinish = true;
            isFight = false;

            if(roomType == "NormalRoom")
            {
                Instantiate(chestPrefab, transform.position, Quaternion.identity);
            }

            OpenDoor(true);
        }
        else
        {
            isFinish = false;
        }
    }

    private void OpenDoor(bool isOpen)
    {
        foreach (var door in doorList)
        {
            door.SetActive(!isOpen);
        }
    }
}
