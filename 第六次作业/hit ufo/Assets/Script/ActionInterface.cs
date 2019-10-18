using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//接口类
public class SSAction : ScriptableObject{
    public bool enable = true;
    public bool destroy = false;
    public GameObject gameobject;
    public Transform transform;
    public ISSActionCallback callback;

    protected SSAction() { }                        
    //子类可以使用下面这两个函数
    public virtual void Start(){
        throw new System.NotImplementedException();
    }
    public virtual void Update(){
        throw new System.NotImplementedException();
    }
}

//动作管理器
public class SSActionManager : MonoBehaviour, ISSActionCallback{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();               

    protected void Update(){
        foreach (SSAction action in waitingAdd)
            actions[action.GetInstanceID()] = action;
        waitingAdd.Clear();
        foreach (KeyValuePair<int, SSAction> kv in actions){
            SSAction action = kv.Value;
            if (action.destroy)         
                waitingDelete.Add(action.GetInstanceID());
            else if (action.enable)
                action.Update();
        }
        foreach (int key in waitingDelete){
            SSAction action = actions[key];
            actions.Remove(key);
            DestroyObject(action);
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

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null){}
}