using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Stop spped
        if(Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
         }
 
        //Direction Sprite
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if(Mathf.Abs(rigid.velocity.x) < 0.3 )
        anim.SetBool("iswalking", false);
        else
        anim.SetBool("iswalking", true);

        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isjumping")) {
        rigid.AddForce(Vector2.up*jumpPower, ForceMode2D.Impulse);
        anim.SetBool("isjumping",true);
        }


    }

    void FixedUpdate()
    {
        //Move speded
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1))
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        //Landing Platform

        if(rigid.velocity.y < 0) {
        Debug.DrawRay(rigid.position, Vector3.down, new Color (0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if(rayHit.collider != null) {
            if (rayHit.distance < 0.5f)
            anim.SetBool("isjumping",false);

        }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy") {
            //Attack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y){
                OnAttack(collision.transform);

            }
            
            
            else //Damaged
            OnDamaged(collision.transform.position);


        }
            
    }

    void OnAttack(Transform enemy)
    {
        //Point

        //Enemy Die
        enemy_move enemyMove = enemy.GetComponent<enemy_move>();
        enemyMove.OnDamaged();
    }

    

    void OnDamaged(Vector2 targetPos)
    {
        //Change Layer(Immortal Active)
        gameObject.layer = 9;

        //View Alpha
        spriteRenderer.color = new Color(1,1,1, 0.4f);

        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1:-1;
        rigid.AddForce(new Vector2(dirc,1)*7,ForceMode2D.Impulse);
        Invoke("OffDamaged",2);


    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1,1,1,1);
    }
}
