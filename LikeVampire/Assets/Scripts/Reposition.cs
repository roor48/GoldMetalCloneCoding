using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D coll;

    private string myTag;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        myTag = transform.tag;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (myTag)
        {
            case "Ground":

                float diffX = myPos.x - playerPos.x;
                float diffY = myPos.y - playerPos.y;
                float dirX = diffX > 0 ? -1 : 1;
                float dirY = diffY > 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * 60);

                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * 60);
                else
                    transform.Translate(60 * (Vector3.right * dirX + Vector3.up * dirY));
                break;

            case "Enemy":
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(dist * 2);
                }

                break;
        }
    }
}