using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    // public static RoomGenerator instance;

    public enum Direction { UP, DOWN, LEFT, RIGHT };
    public string[] roomType = new string[3] { "NormalRoom", "StartRoom", "EndRoom" };
    [SerializeField] private Direction direction;
    public List<Room> rooms = new List<Room>();

    [Header("房间信息")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color startColor;
    public Color endColor;
    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject endRoom;

    [Header("位置控制")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;

    [Header("生成逻辑")]
    public LayerMask roomLayer;
    private List<GameObject> farRooms = new List<GameObject>();
    private List<GameObject> lessFarRooms = new List<GameObject>();
    private List<GameObject> oneWayRooms = new List<GameObject>();

    public WallType wallType;

    public int enemyMinNum;
    public int enemyMaxNum;
    public GameObject playerPrefab;


    // private void Awake()
    // {
    //     if(instance != null)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //     instance = this;

    //     DontDestroyOnLoad(this);
    // }

    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            var room = Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>();
            room.roomType = "NormalRoom";
            rooms.Add(room); // 列表加入新生成的房间

            // 改变point位置
            ChangePointPos();
        }

        // 设定初始房间和终点房间
        startRoom = rooms[0].gameObject;
        startRoom.GetComponent<SpriteRenderer>().color = startColor;
        startRoom.GetComponent<Room>().roomType = "StartRoom";

        endRoom = rooms[0].gameObject;
        foreach (var room in rooms)
        {
            // // 从世界坐标原点（初始房间）到该房间的坐标点判断
            // if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)
            // {
            //     endRoom = room.gameObject; // 离初始房间最远的房间是终点房间
            // }
            InitRoom(room, room.transform.position);
        }
        FindEndRoom();
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
        endRoom.GetComponent<Room>().roomType = "EndRoom";

        GameManager.instance.player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        AddEnemy();
    }


    void Update()
    {
        // TestRoomGenerate();

    }

    // 改变房间生成点的位置
    public void ChangePointPos()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);

            switch (direction)
            {
                case Direction.UP:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.DOWN:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.LEFT:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.RIGHT:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer));

    }

    // 初始化房间
    public void InitRoom(Room newRoom, Vector3 roomPosition)
    {
        // 判断当前房间上下左右四个方向是不是有房间
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(xOffset, yOffset);

        // 根据房间的门的数量和门的位置来分别生成不同的房间
        switch (newRoom.doorNum)
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.singleUp, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(wallType.singleLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(wallType.singleRight, roomPosition, Quaternion.identity);
                if (newRoom.roomDown)
                    Instantiate(wallType.singleDown, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.doubleUD, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(wallType.doubleUL, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.doubleDL, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.doubleDR, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.tripleUDL, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.tripleUDR, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.tripleULR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.tripleDLR, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.fourUDLR, roomPosition, Quaternion.identity);
                break;
        }
    }

    // 找到相对合适的终点房间
    public void FindEndRoom()
    {
        // 找到当前生成房间的距初始房间最大步数
        int maxStep = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].setpToStart > maxStep)
            {
                maxStep = rooms[i].setpToStart;
            }
        }

        // 根据最大步数找到最大步数和次大步数的房间
        foreach (var room in rooms)
        {
            if (room.setpToStart == maxStep)
            {
                farRooms.Add(room.gameObject);
            }
            if (room.setpToStart == maxStep - 1)
            {
                lessFarRooms.Add(room.gameObject);
            }
        }

        // 找到最大步数和次大步数房间中只有一个门的房间
        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNum == 1)
            {
                oneWayRooms.Add(farRooms[i]);
            }
        }
        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNum == 1)
            {
                oneWayRooms.Add(lessFarRooms[i]);
            }
        }

        // 判断有没有只有一个门的房间
        if (oneWayRooms.Count != 0)
        {
            endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
        }
        else // 如果恰好没有一个门的房间，就在最远处的房间集合里随便找一个
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }

    }

    // 给每个房间随机添加敌人（还未生成）
    public void AddEnemy()
    {
        foreach (var room in rooms)
        {
            if (room.roomType == "NormalRoom")
            {
                room.roomEnemysInitNum = Random.Range(enemyMinNum, enemyMaxNum + 1);
            }
        }
    }

    // 测试用
    // private void TestRoomGenerate()
    // {
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    //     }
    // }
}


// 可被unity识别
[System.Serializable]
public class WallType
{
    // 单门
    public GameObject singleLeft;
    public GameObject singleRight;
    public GameObject singleUp;
    public GameObject singleDown;


    // 双门
    public GameObject doubleUL;
    public GameObject doubleUR;
    public GameObject doubleUD;
    public GameObject doubleDL;
    public GameObject doubleDR;
    public GameObject doubleLR;


    // 三门
    public GameObject tripleUDL;
    public GameObject tripleUDR;
    public GameObject tripleULR;
    public GameObject tripleDLR;


    // 四门
    public GameObject fourUDLR;

}
