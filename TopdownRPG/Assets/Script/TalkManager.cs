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
        talkData.Add(1000, new string[] { "안녕?|0",
                                          "이 곳에 처음 왔구나?|1" });
        talkData.Add(2000, new string[] { "여어|1",
                                          "히사시부리~|0",
                                          "오랜만이라는 뜻~|2" });
        talkData.Add(100, new string[] { "평범한 나무상자다." });
        talkData.Add(200, new string[] { "누군가 사용했던 흔적이 있는 책상이다." });

        // Quest Talk
        talkData.Add(10 + 1000, new string[] { "어서 와.|0",
                                               "이 마을에 놀라운 전설이 있다는데|1",
                                               "오른쪽 호수 쪽에 있는 루도가 알려줄거야.|0"});
        talkData.Add(11 + 1000, new string[] { "아직도 못 만났어?|0",
                                               "루도는 오른쪽 호수에 있어.|1"});
        talkData.Add(11 + 2000, new string[] { "오호~.|1",
                                               "이 호수의 전설을 들으러 온 거야?|0",
                                               "그럼 일 좀 하나 해주면 좋을텐데...|1",
                                               "내 집 근처에 떨어진 동전 좀 주워줬으면 해.|2"});

        talkData.Add(20 + 1000, new string[] { "루도의 동전?|1",
                                               "돈을 흘리고 다니면 못 쓰지!|3",
                                               "나중에 루도에게 한마디 해야겠어.|3"});
        talkData.Add(20 + 2000, new string[] { "찾으면 꼭 좀 가져다 줘.|1" });
        talkData.Add(20 + 5000, new string[] { "근처에서 동전을 찾았다." });

        talkData.Add(21 + 2000, new string[] { "엇, 찾아줘서 고마워|2" });

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
