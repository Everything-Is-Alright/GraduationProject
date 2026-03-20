using UnityEngine;
using Unity.Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    
    [Header("Camera")]
    public CinemachineCamera virtualCamera;
    
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
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        SpawnPlayer();
    }
    
    public void SpawnPlayer()
    {
        Vector3 spawnPosition = playerSpawnPoint.position;
        
        // 如果有存档数据，在最后交互的篝火位置复活
        if (SaveManager.Instance != null && SaveManager.Instance.HasSaveData())
        {
            Vector3 respawnPosition = SaveManager.Instance.GetRespawnPosition();
            if (respawnPosition != Vector3.zero)
            {
                spawnPosition = respawnPosition;
            }
        }
        
        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        
        // 设置Cinemachine相机跟随目标
        if (virtualCamera != null)
        {
            virtualCamera.Follow = currentPlayer.transform;
        }
        
        Debug.Log("玩家已生成");
    }
    
    public void RespawnPlayer()
    {
        // 销毁当前玩家
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        
        // 生成新玩家
        SpawnPlayer();
        
        // 触发玩家重生事件
        OnPlayerRespawn?.Invoke();
    }
    
    public void OnPlayerDeath()
    {
        // 延迟一段时间后复活玩家
        Invoke("RespawnPlayer", 2f);
    }
}