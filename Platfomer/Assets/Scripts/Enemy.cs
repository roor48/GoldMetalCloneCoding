using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRend;
    [SerializeField] Animator anim;
    [SerializeField] Collider2D coll;

    public int nextMove;

    readonly int isRunning = Animator.StringToHash("isRunning");
    void Awake()
    {
        Invoke(nameof(Think), 1f);
    }
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Turn();
    }

    void Think()
    {
        // 다음 행동
        nextMove = Random.Range(-1, 2);

        // 스프라이트 플립
        if (nextMove != 0)
            spriteRend.flipX = nextMove == 1;
        // 애니메이션 설정
        anim.SetBool(isRunning, nextMove != 0);

        // 재귀
        Invoke(nameof(Think), Random.Range(2f, 5f));
    }
    void Turn()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down * 0.5f, Color.green);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 0.5f, LayerMask.GetMask("Platform"));

        if (!rayHit)
        {
            nextMove = -nextMove;
            spriteRend.flipX = nextMove == 1;
            CancelInvoke(nameof(Think));
            Invoke(nameof(Think), 5f);
        }
    }

    public void OnDamaged()
    {
        spriteRend.color = new Color(1, 1, 1, 0.4f);

        spriteRend.flipY = true;

        coll.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        Invoke(nameof(DeActive), 5f);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
