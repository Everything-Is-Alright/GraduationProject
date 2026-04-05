using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

// 编辑器命名空间，仅用于支持拖拽Scene文件，打包时会自动剔除
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Player")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    [Header("Camera")]
    public CinemachineCamera virtualCamera;

    [Header("Levels")]
    // 🔥 核心：直接拖拽Scene文件的数组，无需字符串/数字
#if UNITY_EDITOR
    public SceneAsset[] levelScenes;
#endif

    public event System.Action OnPlayerRespawn;
    private GameObject currentPlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = playerSpawnPoint.position;

        if (SaveManager.Instance != null && SaveManager.Instance.HasSaveData())
        {
            Vector3 respawnPosition = SaveManager.Instance.GetRespawnPosition();
            if (respawnPosition != Vector3.zero)
                spawnPosition = respawnPosition;
        }

        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        if (virtualCamera != null)
            virtualCamera.Follow = currentPlayer.transform;

        Debug.Log("玩家已生成");
    }

    public void RespawnPlayer()
    {
        if (currentPlayer != null) Destroy(currentPlayer);
        SpawnPlayer();
        OnPlayerRespawn?.Invoke();
    }

    public void OnPlayerDeath()
    {
        Invoke("RespawnPlayer", 2f);
    }

    // 加载下一关（自动从拖拽的Scene文件获取索引）
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int currentIndex = -1;

        // 遍历拖拽的场景列表，找到当前场景
#if UNITY_EDITOR
        for (int i = 0; i < levelScenes.Length; i++)
        {
            // 获取拖拽的Scene文件的构建索引
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(levelScenes[i]));
            if (sceneIndex == currentSceneIndex)
            {
                currentIndex = i;
                break;
            }
        }
#endif

        // 加载下一个场景
        if (currentIndex != -1 && currentIndex < GetLevelCount() - 1)
        {
#if UNITY_EDITOR
            int nextIndex = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(levelScenes[currentIndex + 1]));
            SceneManager.LoadScene(nextIndex);
            Debug.Log("加载下一关: " + levelScenes[currentIndex + 1].name);
#endif
        }
        else
        {
            Debug.LogWarning("已到达最后一关或场景未添加到Build Settings");
        }
    }

    // 辅助方法：获取关卡数量
    private int GetLevelCount()
    {
#if UNITY_EDITOR
        return levelScenes.Length;
#else
        return 0;
#endif
    }
}