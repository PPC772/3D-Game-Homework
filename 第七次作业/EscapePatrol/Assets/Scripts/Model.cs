﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject{
    public bool enable = true;
    public bool destroy = false;
    public GameObject gameobject;
    public Transform transform;
    public ISSActionCallback callback;

    protected SSAction() { }

    public virtual void Start(){
        throw new System.NotImplementedException();
    }

    public virtual void Update(){
        throw new System.NotImplementedException();
    }
}

public class SSActionManager : MonoBehaviour, ISSActionCallback{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();            

    protected void Update(){
        foreach (SSAction ac in waitingAdd){
            actions[ac.GetInstanceID()] = ac;
        }
        waitingAdd.Clear();
        foreach (KeyValuePair<int, SSAction> kv in actions){
            SSAction ac = kv.Value;
            if (ac.destroy)
                waitingDelete.Add(ac.GetInstanceID());
            else if (ac.enable)
                ac.Update();
        }
        foreach (int key in waitingDelete){
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }
 
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager){
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void SSActionEvent(SSAction source, int intParam = 0, GameObject objectParam = null){
        if(intParam == 0){
            PatrolFollowAction follow = PatrolFollowAction.GetSSAction(objectParam.gameObject.GetComponent<PatrolData>().player);
            this.RunAction(objectParam, follow, this);
        }
        else{
            GoPatrolAction move = GoPatrolAction.GetSSAction(objectParam.gameObject.GetComponent<PatrolData>().start_position);
            this.RunAction(objectParam, move, this);
            Singleton<GameEventManager>.Instance.PlayerEscape();
        }
    }


    public void DestroyAll(){
        foreach (KeyValuePair<int, SSAction> kv in actions){
            SSAction ac = kv.Value;
            ac.destroy = true;
        }
    }
}


public class SSDirector : System.Object{
    private static SSDirector _instance;
    public ISceneController CurrentScenceController { get; set; }
    public static SSDirector GetInstance(){
        if (_instance == null)
            _instance = new SSDirector();
        return _instance;
    }
}