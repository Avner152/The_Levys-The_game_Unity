using TMPro;
using UnityEngine;

// Keeps tracks of the health of each team
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
        // Assume that nont of the objects are in a team
        int p1Team = -1;
        int p2Team = -1;
        // Search for the players in team1
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
        // Search for the players in team2
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
        // If the team number for the objects is the same, return true. Otherwise, return false
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
        // Because we can't return 2 values, we qould return an array that contains the health of each team
        int[] teamsHealth = new int[] { 0, 0 };
        // Add the health of every team member of team1 to the first element of the array
        foreach (GameObject team1Player in team1Players)
        {
            teamsHealth[0] += team1Player.GetComponent<Health>().GetCurrentHealth();
        }
        // Add the health of every team member of team2 to the second element of the array
        foreach (GameObject team2Player in team2Players)
        {
            teamsHealth[1] += team2Player.GetComponent<Health>().GetCurrentHealth();
        }
        return teamsHealth;
    }

    // LateUpdate is called at the end of each frame
    void LateUpdate()
    {
        // If the game is still going
        if (!gameOver)
        {
            // Get the health of the players
            int[] teamsHealth = GetTeamsHealth();
            // If the total health of team1 is 0, all the members of it died and the game is over.
            if (teamsHealth[0] == 0)
            {
                // Team 1 died
                gameOverText.SetText("<size=200%>THE ROBOTS DEFEATED YOU!</size>\nYou and your teammate died!");
                gameOverText.gameObject.SetActive(true);
                replayText.gameObject.SetActive(true);
                audioSource.PlayOneShot(gameOverClip);
                gameOver = true;
            }
            // If the total health of team2 is 0, all the members of it died and the game is over.
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
