using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called before the first frame update
    void Awake() 
    {
        //유니티 RigidBody의 설정을 rigid에 저장
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    
    void Update() 
    {
        //Jump
        if(Input.GetButtonDown("Jump") && !animator.GetBool("IsJumping")){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            }

        

        if(Input.GetButtonUp("Horizontal")) //버튼을 떼었을때 속도 감소 but 어느 방향인지 알 수 없다
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f /*속도를 단위벡터로 변환*/, rigid.velocity.y); 
        }

        //방향전환
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX  = (Input.GetAxisRaw("Horizontal") == -1);
        
        //Animation
        if(rigid.velocity.normalized.x == 0)
            animator.SetBool("IsWalking", false);
        else
            animator.SetBool("IsWalking", true);

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //수평방향의 값을 정수로 받아서 h에 저장
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);

        if(rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Flatform
        if(rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if(rayHit.collider != null) {
                if(rayHit.distance < 0.5f)
                    Debug.Log(rayHit.collider.name);
                    animator.SetBool("IsJumping", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            //Attack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y){
                OnAttack(collision.transform);
            }
            else{//Damaged
                Debug.Log("Player has been hit");
                OnDamage(collision.transform.position);
            } //괄호 범위 신경쓰기 괄호 없으면 디버그만 실행됨 ondamage는 예외처리가 안된단 말씀
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Item"){
            //Point
            bool isBronze = collision.gameObject.name.Contains("bron");
            bool isSilver = collision.gameObject.name.Contains("sli");
            bool isGold = collision.gameObject.name.Contains("glo");

            if (isBronze){
                gameManager.stagePoint += 50;
            }
            else if (isSilver){
                gameManager.stagePoint += 100;
            }
            else if(isGold){
                gameManager.stagePoint += 200;
            }
            //Deactivate Item
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish"){
            //Next Stage
            gameManager.NextStage();
        }
    }

    void OnAttack(Transform enemy){
        //Point
        gameManager.stagePoint += 100;
        //Reaction Force
        rigid.AddForce(Vector2.up * 5,ForceMode2D.Impulse);

        //Enemy Die
        enemy enemyMove = enemy.GetComponent<enemy>(); //enemy.cs 파일에서 변수에 대한 정보를 가져옴
        enemyMove.OnDamaged(); //따라서 enemy.cs에 OnDamage() 함수에 대한 정보가 있어야함.
    }

    void OnDamage(Vector2 targetPos)
    {
        //Change Layer
        gameObject.layer = 9;

        //Minus Health
        gameManager.health -= 1;

        //View Alpha
        spriteRenderer.color = new Color(1,1,1,0.4f);

        //Reaction Force
        int dirc = transform.position.x - targetPos.x >0 ?1 :-1;
        rigid.AddForce(new Vector2(dirc,1) * 7,ForceMode2D.Impulse);

        //Animation
        animator.SetTrigger("BeDamaged");

        //recursive function
        Invoke("OffDamaged",2);
    }

    void OffDamaged()
    {
        gameObject.layer = 6;
        spriteRenderer.color = new Color(1,1,1,1);
    }
}
