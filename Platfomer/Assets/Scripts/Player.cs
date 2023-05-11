using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("매니져")]
    [SerializeField] GameManager gameManager;

    [Header("콤포넌트")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Collider2D coll;
    [SerializeField] SpriteRenderer spriteRend;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSor;

    [Header("스탯")]
    public float maxSpeed;
    public float jumpPower;

    [Header("오디오")]
    [SerializeField] AudioClip audioJump;
    [SerializeField] AudioClip audioAttack;
    [SerializeField] AudioClip audioDamaged;
    [SerializeField] AudioClip audioItem;
    [SerializeField] AudioClip audioDie;
    [SerializeField] AudioClip audioFinish;

    // Animator Hashes
    private readonly int isRunning = Animator.StringToHash("isRunning");
    private readonly int isJumped = Animator.StringToHash("isJumped");
    private readonly int doDamaged = Animator.StringToHash("doDamaged");
    private void Update()
    {
        PlayerJump();
    }
    private void FixedUpdate()
    {
        PlayerMove();
        JumpAnim();
    }

    void Sound(AudioClip audio)
    {
        audioSor.clip = audio;
        audioSor.Play();
    }
    private void PlayerJump()
    {
        if (!Input.GetButtonDown("Jump") || anim.GetBool(isJumped))
            return;
        // Audio
        Sound(audioJump);

        // Jump
        anim.SetBool(isJumped, true);
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }


    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        anim.SetBool(isRunning, h != 0);

        if (Mathf.Abs(rigid.velocity.x) < maxSpeed)
            rigid.AddForce(h * 2 * Vector2.right, ForceMode2D.Impulse);
        if (h != 0)
            spriteRend.flipX = h < 0;
        // if (spriteRend.flipX != h < 0)
        // {
        // }
    }

    private void JumpAnim()
    {
        if (rigid.velocity.y > 0)
            return;
        Debug.DrawRay(rigid.position, Vector3.down * 0.5f, Color.green);

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit)
            anim.SetBool(isJumped, false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (rigid.velocity.y < 0 && rigid.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
                OnDamaged(collision.transform.position);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Item":
                Sound(audioItem);
                // Point
                int score = 0;
                switch (collision.name)
                {
                    case "Bronze":
                        score = 50;
                        break;

                    case "Silver":
                        score = 100;
                        break;

                    case "Gold":
                        score = 300;
                        break;
                }
                gameManager.stagePoint += score;

                collision.gameObject.SetActive(false);
                break;

            case "Finish":
                Sound(audioFinish);
                gameManager.NextStage();
                break;
        }
    }

    void OnAttack(Transform enemyTrans)
    {
        // Point
        Sound(audioAttack);
        gameManager.stagePoint += 100;

        rigid.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        Enemy enemy = enemyTrans.GetComponent<Enemy>();
        enemy.OnDamaged();
    }
    void OnDamaged(Vector2 targetPos)
    {
        Sound(audioDamaged);
        gameManager.HealthDown();

        gameObject.layer = 9;

        spriteRend.color = new Color(1, 1, 1, 0.4f);

        float dir = rigid.position.x > targetPos.x ? 7f : -7f;
        rigid.AddForce(new Vector2(dir, 7f), ForceMode2D.Impulse);

        anim.SetTrigger(doDamaged);

        Invoke(nameof(OffDamaged), 2f);
    }
    void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRend.color = Color.white;
    }

    public void OnDie()
    {
        Sound(audioDie);
        spriteRend.color = new Color(1, 1, 1, 0.4f);

        spriteRend.flipY = true;

        coll.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
