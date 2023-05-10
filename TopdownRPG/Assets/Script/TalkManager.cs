using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    public Sprite[] portraitArr;

    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        // Talk Data
        // NPC A: 1000, NPC B: 2000
        // Box: 100, Desk: 200
        talkData.Add(1000, new string[] { "�ȳ�?|0",
                                          "�� ���� ó�� �Ա���?|1" });
        talkData.Add(2000, new string[] { "����|1",
                                          "����úθ�~|0",
                                          "�������̶�� ��~|2" });
        talkData.Add(100, new string[] { "����� �������ڴ�." });
        talkData.Add(200, new string[] { "������ ����ߴ� ������ �ִ� å���̴�." });

        // Quest Talk
        talkData.Add(10 + 1000, new string[] { "� ��.|0",
                                               "�� ������ ���� ������ �ִٴµ�|1",
                                               "������ ȣ�� �ʿ� �ִ� �絵�� �˷��ٰž�.|0"});
        talkData.Add(11 + 1000, new string[] { "������ �� ������?|0",
                                               "�絵�� ������ ȣ���� �־�.|1"});
        talkData.Add(11 + 2000, new string[] { "��ȣ~.|1",
                                               "�� ȣ���� ������ ������ �� �ž�?|0",
                                               "�׷� �� �� �ϳ� ���ָ� �����ٵ�...|1",
                                               "�� �� ��ó�� ������ ���� �� �ֿ������� ��.|2"});

        talkData.Add(20 + 1000, new string[] { "�絵�� ����?|1",
                                               "���� �긮�� �ٴϸ� �� ����!|3",
                                               "���߿� �絵���� �Ѹ��� �ؾ߰ھ�.|3"});
        talkData.Add(20 + 2000, new string[] { "ã���� �� �� ������ ��.|1" });
        talkData.Add(20 + 5000, new string[] { "��ó���� ������ ã�Ҵ�." });

        talkData.Add(21 + 2000, new string[] { "��, ã���༭ ����|2" });

        // Portrait Data
        // 0: Normal, 1: Speak, 2: Happy, 3: Angry
        for (int i = 0; i < portraitArr.Length; i++)
        {
            if (i < portraitArr.Length / 2)
                portraitData.Add(1000 + i, portraitArr[i]);
            else
                portraitData.Add(2000 + i - portraitArr.Length / 2, portraitArr[i]);
        }

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            if (talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 10, talkIndex);  // Get First Talk
            else
                return GetTalk(id - id % 100, talkIndex);   // Get First Quest Talk
            //1021 - 1 = 1020
        }
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portratiIndex)
    {


        return portraitData[id + portratiIndex];
    }
}
