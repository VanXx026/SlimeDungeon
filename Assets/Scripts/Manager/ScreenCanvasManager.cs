using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenCanvasManager : MonoBehaviour
{
    public static ScreenCanvasManager instance;
    public GameObject playerStatesPanel;
    public GameObject uiButtonPanel;
    public GameObject mapPanel;
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject operationPanel;
    [SerializeField] private bool isMapPanelOpen;
    [SerializeField] private bool isMenuPanelOpen;
    [SerializeField] private bool isOperationPanelOpen;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);

        isMapPanelOpen = false;
        isMenuPanelOpen = false;
        isOperationPanelOpen = false;

    }

    private void Update()
    {
        // 如果游戏没有结束
        if (!GameManager.instance.isGameOver)
            UpdatePlayerHealthBar();

        // Menu Panel
        if (Input.GetKeyDown(KeyCode.Escape) && !isMenuPanelOpen)
        {
            OpenMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isMenuPanelOpen)
        {
            CloseMenu();
        }

        // Map panel
        if (Input.GetKeyDown(KeyCode.M) && !isMapPanelOpen)
        {
            OpenMap();
        }
        else if (Input.GetKeyDown(KeyCode.M) && isMapPanelOpen)
        {
            CloseMap();
        }
    }

    // 实时显示血条变化
    public void UpdatePlayerHealthBar()
    {
        if (GameManager.instance.player != null)
        {
            PlayerController pc = GameManager.instance.player.GetComponent<PlayerController>();
            Transform healthBar = playerStatesPanel.transform.GetChild(0);
            Image slider = healthBar.GetChild(0).GetComponent<Image>();
            float healthRate = pc.currentHealth / pc.maxHealth;
            slider.fillAmount = healthRate;
        }
    }

    public void OpenPlayerStatesPanel()
    {
        playerStatesPanel.SetActive(true);
    }

    public void ClosePlayerStatesPanel()
    {
        playerStatesPanel.SetActive(false);
    }

    public void OpenUIButtionPanel()
    {
        uiButtonPanel.SetActive(true);
    }

    public void CloseUIButtonPanel()
    {
        uiButtonPanel.SetActive(false);
    }

    public void OpenMap()
    {
        mapPanel.SetActive(true);
        isMapPanelOpen = true;
        Time.timeScale = 0;
    }

    public void CloseMap()
    {
        mapPanel.SetActive(false);
        isMapPanelOpen = false;
        Time.timeScale = 1;
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        isMenuPanelOpen = true;
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        isMenuPanelOpen = false;
        Time.timeScale = 1;
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseGameOverPanel()
    {

        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenWinPanel()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseWinPanel()
    {
        winPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenOperationPanel()
    {
        operationPanel.SetActive(true);
        isOperationPanelOpen = true;
        Time.timeScale = 0;
    }

    public void CloseOperationPanel()
    {
        operationPanel.SetActive(false);
        isOperationPanelOpen = false;
        Time.timeScale = 1;
    }

    /// Menu Button
    public void ClickBackButton()
    {
        CloseMenu();
    }

    public void ClickRemakeButton()
    {
        GameManager.instance.isGameOver = true;
        SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        GameManager.instance.isGameOver = false;
    }

    public void ClickOperationButton()
    {
        // TODO: 打开操作说明页面
        if(!isOperationPanelOpen)
            OpenOperationPanel();
    }

    public void ClickExitButton()
    {
        GameManager.instance.isGameOver = true;
        // TODO: 返回主菜单
        SceneManager.LoadSceneAsync("MainMenu");
        GameManager.instance.isGameOver = false;
    }
}
