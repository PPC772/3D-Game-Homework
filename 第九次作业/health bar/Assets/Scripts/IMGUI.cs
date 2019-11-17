using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUI : MonoBehaviour{
    public float health = 75f;
    public float max_health = 100f;
    private float real_health;
    private Rect health_bar;
    private Rect health_add;
    private Rect health_sub;

    void Start(){
        health_bar = new Rect(Screen.width / 2 - 50, 30, 100, 20);
        health_add = new Rect(Screen.width / 2 - 75, 30, 20, 20);
        health_sub = new Rect(Screen.width / 2 + 55, 30, 20, 20);
        real_health = health / max_health;
    }

    void OnGUI(){
        if (GUI.Button(health_sub, "+")){
            real_health = real_health + 0.05f;
            if (real_health > 1)
            	real_health = 1;
        }
        if (GUI.Button(health_add, "-")){
            real_health = real_health - 0.05f;
            if (real_health < 0)
            	real_health = 0;
        }
        health = real_health * max_health;
        // 用水平滚动条的宽度作为血条的显示值
        GUI.HorizontalScrollbar(health_bar, 0.0f, real_health, 0.0f, 1.0f);
    }
}