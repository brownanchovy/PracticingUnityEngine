using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;
    public int nextMove;
    CircleCollider2D collide;

    public float TimetoThink;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake() 
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        collide = GetComponent<CircleCollider2D>();


        Think();
        
        Invoke("Think",5);
    }

    void Update() 
    {
        
    }

    // Update is called once per frame
    // 기본 움직임
    void FixedUpdate()
    {
        //moving
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //rayhit using
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if(rayHit.collider == null) {
                Turn();
            }
    }

    void Think(){
        nextMove = Random.Range(-1, 2);

        TimetoThink = Random.Range(2f,5f);
        //Think(); recursive function, but there should be delay

        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        //Flip Sprite
        if(nextMove != 0){
            spriteRenderer.flipX = nextMove == 1;
        }
        Invoke("Think",TimetoThink); //recursive function 아래에 있는 것이 좋음
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think",5);
    }


    //몬스터 죽었을때 취해야하는 액션을 구현한다.
    public void OnDamaged(){ //PlayerMoves.cs에서 OnDamaged 함수를 호출하였기 때문에 public으로 설정한다.
        
        //Sprite Alpha
        spriteRenderer.color = new Color(1,1,1,0.4f);

        //Sprite Flip Y 아래로 깨꼬닥
        spriteRenderer.flipY = true;

        //Collider Disabled 몬스터 충돌효과 무시
        collide.enabled = false;

        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Destroy
        Invoke("Deactive", 5);
    }

    void Deactive(){
        gameObject.SetActive(false);
    }
}