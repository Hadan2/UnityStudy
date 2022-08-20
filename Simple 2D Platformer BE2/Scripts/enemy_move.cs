using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_move : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D collider;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        

        Invoke("Think", 2);
    }

   
    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove,rigid.velocity.y);

        //Platform check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color (0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if(rayHit.collider == null) 
        Turn();

    }

    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1,2); 

        //Sprite Animation
        anim.SetInteger("walkspeed", nextMove);

        //Flip Sprite
        if(nextMove != 0) 
        spriteRenderer.flipX = nextMove ==1;

        //Recursive
        float nextThinkTime = Random.Range(2f,5f);
        Invoke("Think", nextThinkTime);
        


    }

    void Turn()
    {
    nextMove *= -1;
    spriteRenderer.flipX = nextMove == 1;

    CancelInvoke();
    Invoke("Think",2);
    }

    public void OnDamaged()
    {

        //Sprite Alpha
        spriteRenderer.color = new Color(1,1,1,0.4f);

        //Sprite Flip Y
        spriteRenderer.flipY = true;

        //Collider Disable
        collider.enabled = false;

        //Die Effect Jump
        rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);

        //Destroy
        Invoke("DeActive",5);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }


}
