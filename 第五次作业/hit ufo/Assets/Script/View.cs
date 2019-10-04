using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour{
    private IUserAction action;
    GUIStyle bold_style = new GUIStyle();
    GUIStyle score_style = new GUIStyle();
    GUIStyle text_style = new GUIStyle();
    GUIStyle over_style = new GUIStyle();
    private int high_score = 0;
    private bool game_start = false;

    void Start (){
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }
	
	void OnGUI (){
        bold_style.normal.textColor = new Color(1, 0, 0);
        bold_style.fontSize = 16;
        text_style.normal.textColor = new Color(1, 1, 1);
        text_style.fontSize = 16;
        score_style.normal.textColor = new Color(1, 1, 0);
        score_style.fontSize = 16;
        over_style.normal.textColor = new Color(1, 1, 1);
        over_style.fontSize = 25;

        if (game_start){
            if (Input.GetButtonDown("Fire1")){
                Vector3 pos = Input.mousePosition;
                action.Hit(pos);
            }
            GUI.Label(new Rect(10, 5, 200, 50), "分数:", text_style);
            GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), score_style);
            GUI.Label(new Rect(Screen.width / 2 - 20, 5, 200, 50), "Round " + action.GetRound().ToString(), text_style);
            GUI.Label(new Rect(Screen.width - 135, 5, 50, 50), "生命值:", text_style);
            for (int i = 0; i < action.GetLife(); i++)
                GUI.Label(new Rect(Screen.width - 75 + 15 * i, 5, 50, 50), "♡", bold_style);
            if (action.GetLife() == 0){
                action.GameOver();
                high_score = high_score > action.GetScore() ? high_score : action.GetScore();
                GUI.Label(new Rect(Screen.width / 2 - 30, 50, 100, 100), "游戏结束", over_style);
                GUI.Label(new Rect(Screen.width / 2 - 15, 100, 50, 50), "最高分:" + high_score.ToString(), text_style);
                if (GUI.Button(new Rect(Screen.width / 2 - 35, 200, 100, 50), "重新开始")){
                    action.ReStart();
                    return;
                }
            }
        }
        else{
            GUI.Label(new Rect(Screen.width / 2 - 30, 50, 100, 100), "Hit UFO", over_style);
            GUI.Label(new Rect(Screen.width / 2 - 60, 100, 400, 100), "鼠标点击来消灭飞碟", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 35, 200, 100, 50), "游戏开始")){
                game_start = true;
                action.BeginGame();
            }
        }
    }
}