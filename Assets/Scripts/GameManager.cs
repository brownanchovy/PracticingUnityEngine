using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//매니저 파일이다. 매니저는 점수와 스테이지를 관리한다.
public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public PlayerMove player;
    public int health;
    public GameObject[] Stages;
    public Image[] UIhealth;
    public Text UIStage;
    public Text UIPoint;
    public GameObject Restart_Button;
    // Start is called before the first frame update
    public void NextStage()
    {
        if (stageIndex < Stages.Length -1 ){
        //Change stage
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE" + (stageIndex +1);
        }
        else {//Game Clear
            //Player Control Lock
            Time.timeScale = 0;
            Text buttonText = Restart_Button.GetComponentInChildren<Text>();
            buttonText.text = "GameClear!";
            Restart_Button.SetActive(true);
        }
        //Calculate Score
        totalPoint += stageIndex;
        stagePoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void HealthDown(){
        if(health > 1)
            {health -=1;
            UIhealth[health].color = new Color(1, 0, 0, 0.2f);
            }
        else {
            UIhealth[0].color = new Color(1, 0, 0, 0.2f);
            //Player die effect
            player.OnDie();

            //Result Ui
            Invoke("Postpone",1f);
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

    public void Restart()
    {
        Time.timeScale =1;
        SceneManager.LoadScene(0);
    }
    public void Postpone(){
        Restart_Button.SetActive(true);
    }
}
