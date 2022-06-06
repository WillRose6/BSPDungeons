using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator LevelLoadScreenAnimator;

    private void Start()
    {
        GameManager.level = 1;
        GameManager.attempts = 1;
    }

    public void ShowLevelLoadScreen()
    {
        LevelLoadScreenAnimator.gameObject.SetActive(true);
        LevelLoadScreenAnimator.SetTrigger("Activate");
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.FreezePlayer();
    }

    public void LoadLevel(LevelParameters parameters)
    {
        References.parameters = parameters;
        GameSerializer.instance.SaveGame();
        SceneManager.LoadScene(1);
    }
}
