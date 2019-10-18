using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerAdapter : MonoBehaviour, IActionManager{
    public DynamicsActionManager dynamics_action_manager;
    public KinematicsActionManager kinematics_action_manager;
    public void UFOFly(GameObject disk, float angle, float power,bool switch_phy){
        if(switch_phy)
            kinematics_action_manager.UFOFly(disk, angle, power);
        else
            dynamics_action_manager.UFOFly(disk, angle, power);
    }

    void Start (){
        dynamics_action_manager = gameObject.AddComponent<DynamicsActionManager>() as DynamicsActionManager;
        kinematics_action_manager = gameObject.AddComponent<KinematicsActionManager>() as KinematicsActionManager;
    }
}

public interface IActionManager{
    void UFOFly(GameObject disk, float angle, float power,bool isPhy);
}


