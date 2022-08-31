using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//매니저 파일이다. 매니저는 점수와 스테이지를 관리한다.
public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health;
    // Start is called before the first frame update
    public void NextStage()
    {
        stageIndex++;
        totalPoint += stageIndex;
        stagePoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"){
            health -=1;

            //Player Reposition
            collision.attachedRigidbody.velocity = Vector2.zero;
            collision.transform.position = new Vector3(-8,2,-1);
        }
    }
}
