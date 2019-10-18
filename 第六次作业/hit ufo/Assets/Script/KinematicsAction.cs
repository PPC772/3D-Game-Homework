using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//飞碟飞行管理器
public class KinematicsActionManager : SSActionManager{
    public KinematicsFlyAction fly;
    public Controller scene_controller; 

    public void UFOFly(GameObject disk, float angle, float power){
        fly = KinematicsFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, power);
        this.RunAction(disk, fly, this);
    }
}
//飞碟飞行
public class KinematicsFlyAction : SSAction{
    public float gravity = -5;
    private Vector3 start_vector;
    private Vector3 gravity_vector = Vector3.zero;
    private float time;
    private Vector3 current_angle = Vector3.zero;

    private KinematicsFlyAction(){}

    //中间函数，封装行为参数
    public static KinematicsFlyAction GetSSAction(Vector3 direction, float angle, float power){
        KinematicsFlyAction action = CreateInstance<KinematicsFlyAction>();
        if (direction.x == -1)
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        else
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        return action;
    }

    //通过改变飞碟坐标来模拟飞碟作抛物线运动
    public override void Update(){
        time += Time.fixedDeltaTime;
        gravity_vector.y = gravity * time;
        transform.position += (start_vector + gravity_vector) * Time.fixedDeltaTime;
        current_angle.z = Mathf.Atan((start_vector.y + gravity_vector.y) / start_vector.x) * Mathf.Rad2Deg;
        transform.eulerAngles = current_angle;
        if (this.transform.position.y < -10){
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
    }

    public override void Start(){}
}