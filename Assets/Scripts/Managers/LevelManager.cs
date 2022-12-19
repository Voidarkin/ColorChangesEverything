using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    public static LevelManager Instance;

    void Awake()
    {    
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

	void Start () 
    {
 
	}
	
	void Update () 
    {
       
    }

    public Player GetPlayer()
    {
        return m_CurrentPlayer;
    }

    public void RegisterPlayer(Player player)
    {
        m_CurrentPlayer = player;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 3000, 60), "Color Changes Everything Alpha");
    }

    Player m_CurrentPlayer;
}

