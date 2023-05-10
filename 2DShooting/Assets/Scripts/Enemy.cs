using UnityEngine;
public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore;
    public float speed;
    public int health;
    public Sprite[] sprites;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject player;
    public GameManager gameManager;
    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;
    Animator anim;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;
    public static bool isDead;
    public bool isPattern;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //isDead = false;
        if (enemyName == "B")
            anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        switch (enemyName)
        {
            case "B":
                health = 300; Invoke("Stop", 2); break;
            case "L":
                health = 40; break;
            case "M":
                health = 10; break;
            case "S":
                health = 3; break;
        }
    }
    void Stop()
    {
        if (!gameObject.activeSelf)
            return;
        patternIndex = 1;
        curPatternCount = 0;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke(nameof(Think), 2);
    }
    void Think()
    {
        if (isPattern   )
            return;

        patternIndex = patternIndex == 4 ? 1 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 1:
                FireFoward(); break;
            case 2:
                FireShot(); break;
            case 3:
                FireArc(); break;
            case 4:
                FireAround(); break;
        }
    }

    void FireFoward()
    {
        Debug.Log("1FireFoward");
        isPattern = true;

        // Fire 4 Bullet Foward
        GameObject bulletR = objectManager.MakeObj("BulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("BulletBossA");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletL = objectManager.MakeObj("BulletBossA");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("BulletBossA");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        // Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex - 1] && gameObject.activeSelf)
            Invoke(nameof(FireFoward), 2);
        else
        {
            Invoke(nameof(Think), 3);
            isPattern = false;
        }
    }
    void FireShot()
    {
        Debug.Log("2FireShot");
        isPattern = true;

        // Fire 5 Random Shotgun Bullet to Player
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex - 1] && gameObject.activeSelf)
            Invoke(nameof(FireShot), 3.5f);
        else
        {
            Invoke(nameof(Think), 3);
            isPattern = false;
        }
    }
    void FireArc()
    {
        Debug.Log("3부채 모양으로 발사");
        isPattern = true;

        GameObject bullet = objectManager.MakeObj("BulletEnemyA");
        bullet.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex - 1]), -1);
        Debug.Log(patternIndex);
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex - 1] && gameObject.activeSelf)
            Invoke(nameof(FireArc), 0.15f);
        else
        {
            Invoke(nameof(Think), 3);
            isPattern = false;
        }
    }
    void FireAround()
    {

        // Fire Around
        Debug.Log("4원 형태로 전체 공격.");
        isPattern = true;

        int roundNum = curPatternCount%2 == 0 ? 40 : 30;
        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum)
                                        ,Mathf.Sin(Mathf.PI * 2 * i / roundNum));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);



            Vector3 rotVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex - 1] && gameObject.activeSelf)
            Invoke(nameof(FireAround), 0.7f);
        else
        {
            Invoke(nameof(Think), 4);
            isPattern = false;
        }
    }
    void Update()
    {
        if (enemyName == "B")
            return;
        Fire();
        Reload();
    }
    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            GameObject bulletL = objectManager.MakeObj("BulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidR.AddForce(dirVecR.normalized * 3, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 3, ForceMode2D.Impulse);
        }
        curShotDelay = 0;

    }
    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        health -= dmg;
        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {

            spriteRenderer.sprite = sprites[1];
            Invoke(nameof(ReturnSprite), 0.1f);
        }

        if (health <= 0)
        {
            isDead = true;
            // Player playerLogic = player.GetComponent<Player>();
            Player.score += enemyScore;

            // Random Ratio Item Drop
            if (health <= 0)
            {
                int ran = enemyName == "B" ? 0 : Random.Range(0, 10);
                if (ran < 5)
                    Debug.Log("Not Item");
                else if (ran < 8) //Coin
                {
                    itemCoin = objectManager.MakeObj("ItemCoin");
                    itemCoin.transform.position = transform.position;
                }

                else if (ran == 8) // Power
                {
                    itemPower = objectManager.MakeObj("ItemPower");
                    itemPower.transform.position = transform.position;
                }

                else if (ran == 9) // Boom
                {
                    itemBoom = objectManager.MakeObj("ItemBoom");
                    itemBoom.transform.position = transform.position;
                }
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            gameManager.CallExplosion(transform.position, enemyName);

            // Boss Kill
            if(enemyName== "B")
                gameManager.StageEnd();
        }
    }
    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            collision.gameObject.SetActive(false);
        }
    }
}
