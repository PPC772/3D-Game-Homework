﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//分数记录类
public class ScoreRecorder : MonoBehaviour{
    public int score;
    void Start (){
        score = 0;
    }

    public void Record(GameObject disk){
        score += disk.GetComponent<DiskData>().score;
    }

    public void Reset(){
        score = 0;
    }
}