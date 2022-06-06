using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This class contains various useful, miscellaneous functions
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private DungeonUI ui;
    public static int attempts = 1;
    public static int level = 1;
    public static int goal = 3;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("More than one Game Manager in the scene!");
            return;
        }

        instance = this;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        ui.GameOver();
        attempts++;
        ui.SetStartupScreen(1, attempts);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        GameSerializer.instance.SaveGame();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Start()
    {
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<DungeonUI>();
        ui.SetStartupScreen(level, attempts);
        StartCoroutine(LateStart());
    }

    public IEnumerator LateStart()
    {
        yield return null;
        DistributeOncePerLevelItems();
        GameSerializer.instance.LoadGame();
    }

    public void NextLevel()
    {
        level++;

        if(level == goal)
        {
            ui.ShowLevelCompleteScreen();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().FreezePlayer();
            return;
        }

        attempts = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KillPlayer(bool ShowAnimation)
    {
        if (ShowAnimation)
        {
            DungeonPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
            player.Die();
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    public void DistributeOncePerLevelItems()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> used = new List<GameObject>();
        foreach (ItemTemplate i in References.instance.PossibleItems)
        {
            if (i.spawnType == ItemTemplate.SpawnType.OnePerLevel)
            {
                if (used.Count < enemies.Length)
                {
                    Enemy e = enemies[Random.Range(0, enemies.Length)].GetComponent<Enemy>();
                    e.ChanceToDropItem = 100;
                    e.itemDrops = new ItemTemplate[] { i };
                }
            }
        }
    }
}
