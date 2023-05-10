using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanel;
    public Animator portraitAnim;
    public TypeEffect talk;
    public Image portraitImg;
    public GameObject menuSet;
    public Text questTalk;
    public Text npcName;
    public GameObject player;

    public int talkIndex;
    public int prevSprite;

    public bool isAction;

    void Start()
    {
        GameLoad();
        questTalk.text = questManager.CheckQuest();
    }

    private void Update()
    {
        // Sub Menu
        if (Input.GetButtonDown("Cancel"))
            SubMenu();
    }

    public void SubMenu()
    {
        menuSet.SetActive(!menuSet.activeSelf);
        Time.timeScale = menuSet.activeSelf ? 0 : 1;
    }
    public void Action(GameObject scanObject)
    {
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);

        npcName.text = scanObject.GetComponent<ObjectData>().name;
        talkPanel.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = 0;
        string talkData = "";

        // Set Talk Data
        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }


        // End Talk
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questTalk.text = questManager.CheckQuest(id);
            return;
        }

        // Continue Talk
        talk.SetMsg(talkData.Split('|')[0]);
        if (isNpc)
        {
            // Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split('|')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
            // Animation Protrait
            if (prevSprite != int.Parse(talkData.Split('|')[1]))
            {
                prevSprite = int.Parse(talkData.Split('|')[1]);
                portraitAnim.SetTrigger("doEffect");
            }

        }
        else
        {
            // Hide Portrait
            portraitImg.color = new Color(1, 1, 1, 0);
        }
        isAction = true;
        talkIndex++;
    }

    void GameSave()
    {
        menuSet.SetActive(false);

        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();
    }
    void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector2(x, y);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
    }
    void GameExit()
    {
        Application.Quit();
    }
}
