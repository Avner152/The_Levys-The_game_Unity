using TMPro;
using UnityEngine;

public class Teams : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public GameObject[] team1Players;
    public GameObject[] team2Players;
    public bool humanPlayerInFirstTeam;
    private int[] teamsHealth;

    public bool InSameTeam(GameObject p1, GameObject p2)
    {
        int p1Team = -1;
        int p2Team = -1;
        foreach (GameObject teamPlayer in team1Players)
        {
            if (teamPlayer == p1)
            {
                p1Team = 0;
            }
            else if (teamPlayer == p2)
            {
                p2Team = 0;
            }
        }
        foreach (GameObject teamPlayer in team2Players)
        {
            if (teamPlayer == p1)
            {
                p1Team = 1;
            }
            else if (teamPlayer == p2)
            {
                p2Team = 1;
            }
        }
        print((p1Team, p2Team));
        return p1Team == p2Team;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetTeamsHealth();
    }

    private void GetTeamsHealth()
    {
        teamsHealth = new int[] { 0, 0 };
        foreach (GameObject team1Player in team1Players)
        {
            teamsHealth[0] += team1Player.GetComponent<Health>().health;
        }
        foreach (GameObject team2Player in team2Players)
        {
            teamsHealth[1] += team2Player.GetComponent<Health>().health;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetTeamsHealth();
        if (teamsHealth[0] == 0)
        {
            gameOverText.SetText("<size=200%>GAME OVER!</size>\nAll memebers in team 1 died");
            gameOverText.gameObject.SetActive(true);
            // Team 1 died
            if (humanPlayerInFirstTeam)
            {
                // Human lost
            }
        }
        else if (teamsHealth[1] == 0)
        {
            gameOverText.SetText("<size=200%>GAME OVER!</size>\nAll memebers in team 2 died");
            gameOverText.gameObject.SetActive(true);
            // Team 2 died
            if (!humanPlayerInFirstTeam)
            {
                // Human lost
            }
        }
    }
}
