using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//매니저 파일이다. 매니저는 점수와 스테이지를 관리한다.
public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public PlayerMove player;
    public int health;
    public GameObject[] Stages;
    // Start is called before the first frame update
    public void NextStage()
    {
        if (stageIndex < Stages.Length -1 ){
        //Change stage
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else {//Game Clear
            //Player Control Lock
            Time.timeScale = 0;
        }
        //Calculate Score
        totalPoint += stageIndex;
        stagePoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealthDown(){
        if(health > 1)
            health -=1;
        else {
            //Player die effect
            player.OnDie();

            //Result Ui

            //Retry Button Ui

        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"){

            //Player Reposition
            if(health > 0){
                PlayerReposition();
            }
            HealthDown();
        }
    }

    void PlayerReposition(){
        player.transform.position = new Vector3(-8,2,-1);
        player.VelocityZero();
    }

}
