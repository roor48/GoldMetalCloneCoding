//if (anim.GetInteger("vAxisRaw") != v)
//{
//    anim.SetBool("isChange", true);
//    anim.SetInteger("vAxisRaw", (int)v);
//}
//else if (anim.GetInteger("hAxisRaw") != h)
//{
//    anim.SetBool("isChange", true);
//    anim.SetInteger("hAxisRaw", (int)h);
//}
//else
//    anim.SetBool("isChange", false);


using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] GameManager gameManger;

    public float speed;
    float h, v;

    bool isHmove;


    Rigidbody2D rigid;
    Animator anim;
    Vector3 dirVec;
    GameObject scanObject;
    GameObject scanObj;

    Dictionary<string, int> uiMove = new Dictionary<string, int>()
    {
        {"U", 0 },
        {"D", 0 },
        {"R", 0 },
        {"L", 0 }
    };
    bool hOn, vOn;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        h = gameManger.isAction ? 0 : Input.GetAxisRaw("Horizontal") + uiMove["R"] + (uiMove["L"] * -1);
        v = gameManger.isAction ? 0 : Input.GetAxisRaw("Vertical") + uiMove["U"] + (uiMove["D"] * -1);

        bool hDown = Input.GetButtonDown("Horizontal") || hOn;
        bool vDown = Input.GetButtonDown("Vertical") || vOn;   // 1프레임만 true


        //if (hDown)
        //{
        //    isHmove = true;
        //}
        //else if (vDown)
        //{
        //    isHmove = false;
        //}
        //else if (hUp || vUp)
        //{
        //    isHmove = h != 0;
        //}                     골드메탈 코드

        if (h != 0 || v != 0)
            if (vDown || h == 0)
                isHmove = false;
            else if (hDown || v == 0)
                isHmove = true;



        // 애니메이션

        if (anim.GetInteger("vAxisRaw") != v)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        }
        else if (anim.GetInteger("hAxisRaw") != h)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
        }
        else
            anim.SetBool("isChange", false);


        // Direction
        if (h != 0 && isHmove)
            dirVec = Vector3.right * h;
        else if (v != 0)
            dirVec = Vector3.up * v;

        // Scan Obj
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
        {
            gameManger.Action(scanObject);
        }
    }

    void FixedUpdate()
    {
        Vector2 moveVec = isHmove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = Input.GetKey(KeyCode.LeftShift) ? speed * moveVec * 2 : speed * moveVec;

        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        scanObj = rayHit.collider == null ? null : rayHit.collider.gameObject;
        if (scanObj != null || !gameManger.isAction)
        {
            scanObject = scanObj;
        }
    }

    public void ButtonActive(string type)
    {
        switch(type)
        {
            case "U":
            case "D":
                hOn = false;
                vOn = true;
                break;
            
            case "R":
            case "L":
                vOn = false;
                hOn = true;
                break;
            case "A":
                if (scanObject != null)
                    gameManger.Action(scanObject);
                break;
            case "C":
                gameManger.SubMenu();
                break;
        }
        uiMove[type] = 1;
    }
    public void ButtonExit(string type)
    {
        uiMove[type] = 0;
    }
}
