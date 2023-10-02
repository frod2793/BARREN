using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Singleton instance
    private static PlayerData instance;

    // Player data
    private int experience;
    private string playerName;
    private string gender;
    private bool isTutorial = false;
    // Public properties to access player data
    public int Experience { get { return experience; } set { experience = value; } }
    public string PlayerName { get { return playerName; } set { playerName = value; } }
    public string Gender { get { return gender; } set { gender = value; } }

    public bool IsTutorial
    {
        get => isTutorial;
        set => isTutorial = value;
    }
    // Singleton instance property
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                // Create a new GameObject to hold the instance
                GameObject singletonObject = new GameObject("PlayerDataSingleton");
                instance = singletonObject.AddComponent<PlayerData>();
            }
            return instance;
        }
    }

    // Ensure the instance is not destroyed when loading a new scene
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        // Initialize default values
        experience = 0;
        playerName = "Player";
        gender = "Unknown";
    }

    // Update is called once per frame
 
}