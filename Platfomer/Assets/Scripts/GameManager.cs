using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;

    public GameObject[] stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject rstBtn;


    public Player player;

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }
    public void NextStage()
    {
        //Stage Change
        if (stageIndex < stages.Length - 1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            PlayerReposition();
            UIStage.text = "STAGE "+(1+stageIndex);

        }
        else
        {
            // Game Clear
            Time.timeScale = 0;

            Debug.Log("게임 클리어!");
            
            Text btnText = rstBtn.GetComponentInChildren<Text>();
            btnText.text = "Game Clear!";
            rstBtn.SetActive(true);
        }

        //Cal Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        UIhealth[health-1].color = new Color(1,1,1,0.2f);
        if (health > 1)
        {
            health--;
        }
        else
        {
            //Die Effect
            player.OnDie();

            // Result UI
            Debug.Log("죽었다!");

            // Retry Button On
            rstBtn.SetActive(true);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        HealthDown();

        //Reposition
        if (health < 1)
            return;

        PlayerReposition();
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-9.5f, -1f, -1f);
        player.VelocityZero();
    }

    public void RestartBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
