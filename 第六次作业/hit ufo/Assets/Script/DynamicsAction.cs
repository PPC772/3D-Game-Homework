using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//飞碟飞行管理器
public class DynamicsActionManager : SSActionManager{
    public DynamicsFlyAction fly;
    public Controller scene_controller; 

    public void UFOFly(GameObject disk, float angle, float power){
        fly = DynamicsFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, power);
        this.RunAction(disk, fly, this);
    }
}

public class DynamicsFlyAction : SSAction{
    private Vector3 start_vector;
    private Vector3 force;
    private Vector3 gravity = new Vector3(0, -0.2f, 0);
    private float time;
    private bool if_add_force = false;

    private DynamicsFlyAction(){}

    //中间函数，封装行为参数
    public static DynamicsFlyAction GetSSAction(Vector3 direction, float angle, float power){
        DynamicsFlyAction action = CreateInstance<DynamicsFlyAction>();
        if (direction.x == -1){
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
            action.force = new Vector3(Random.Range(-18, -12), Random.Range(8, 12), 0);
        }
        else{
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
            action.force = new Vector3(Random.Range(12, 18), Random.Range(8, 12), 0);
        }
        return action;
    }

    //通过给飞碟施加作用力来模拟飞碟作抛物线运动（一个初始扔飞碟的力和一个持续的重力）
    public override void Update(){
        if (!if_add_force){
            if_add_force = true;
            gameobject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
        if (this.transform.position.y < -10){
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
        gameobject.GetComponent<Rigidbody>().AddForce(gravity, ForceMode.Impulse);
    }

    public override void Start(){}
}