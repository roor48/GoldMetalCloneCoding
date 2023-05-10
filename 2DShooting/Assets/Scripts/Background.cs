using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewHeight;
    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * -2;
    }
    private void Update()
    {
        Move();
        Scrolling();
    }

    void Move()
    {
        transform.position += speed * Time.deltaTime * Vector3.down;
    }
    void Scrolling()
    {
        if (sprites[endIndex].position.y < viewHeight)
        {
            // Sprite ReUse
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * 10;

            // Cursor Index Change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = startIndexSave - 1 == -1 ? sprites.Length - 1 : startIndexSave - 1;
        }
    }
}
