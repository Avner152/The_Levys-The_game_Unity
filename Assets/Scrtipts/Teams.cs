using TMPro;
using UnityEngine;

public class Teams : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI replayText;
    public GameObject[] team1Players;
    public GameObject[] team2Players;
    public AudioSource audioSource;
    public AudioClip gameOverClip, victoryClip;
    private bool gameOver = false;

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
        return p1Team == p2Team;
    }
    public bool IsGameOver()
    {
        return gameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
    }

    private int[] GetTeamsHealth()
    {
        int[] teamsHealth = new int[] { 0, 0 };
        foreach (GameObject team1Player in team1Players)
        {
            teamsHealth[0] += team1Player.GetComponent<Health>().GetCurrentHealth();
        }
        foreach (GameObject team2Player in team2Players)
        {
            teamsHealth[1] += team2Player.GetComponent<Health>().GetCurrentHealth();
        }
        return teamsHealth;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!gameOver)
        {
            int[] teamsHealth = GetTeamsHealth();
            if (teamsHealth[0] == 0)
            {
                // Team 1 died
                gameOverText.SetText("<size=200%>THE ROBOTS DEFEATED YOU!</size>\nYou and your teammate died!");
                gameOverText.gameObject.SetActive(true);
                replayText.gameObject.SetActive(true);
                audioSource.PlayOneShot(gameOverClip);
                gameOver = true;
            }
            else if (teamsHealth[1] == 0)
            {
                // Team 2 died
                gameOverText.SetText("<size=200%>YOU WON!</size>\nYou killed the robots and saved the day!");
                gameOverText.gameObject.SetActive(true);
                replayText.gameObject.SetActive(true);
                audioSource.PlayOneShot(victoryClip);
                gameOver = true;
            }
        }
    }
}
