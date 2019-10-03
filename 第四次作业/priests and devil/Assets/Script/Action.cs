using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class SSAction : ScriptableObject{
    public bool enable = true;
    public bool destroy = false;
    public bool judge = true;
    public GameObject gameobject;
    public Transform transform;
    public ISSActionCallback callback;

    public virtual void Start(){
        throw new System.NotImplementedException();
    }

    public virtual void Update(){
        throw new System.NotImplementedException();
    }
}

public class SSMoveToAction : SSAction{	//移动函数
    public Vector3 target;

    public static SSMoveToAction GetSSAction(Vector3 target){
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
        action.target = target;
        return action;
    }

    public override void Start(){
        this.transform.position = target;
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
}

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback{	//回调函数接口
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null);
}

public class SSActionManager : MonoBehaviour, ISSActionCallback{                    //动作管理
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();               

    protected void Update(){
        foreach (SSAction ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
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

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null){}
}

public class MySceneActionManager : SSActionManager{ //场景动作管理器
    private SSMoveToAction move;
    public Controller sceneController;

    protected void Start(){
        sceneController = (Controller)SSDirector.GetInstance().CurrentScenceController;
        sceneController.actionManager = this;
    }

    public void Move(GameObject gameobject, Vector3 position){
        move = SSMoveToAction.GetSSAction(position);
        this.RunAction(gameobject, move, this);
    }

    public int Judge(LandModel left_land, LandModel right_land, ShipModel ship){	//裁判函数,把胜负判定从场记转移到此处
        int right_priest = (right_land.GetRoleNum())[0];
        int right_devil = (right_land.GetRoleNum())[1];
        int left_priest = (left_land.GetRoleNum())[0];
        int left_devil = (left_land.GetRoleNum())[1];
        if (left_priest + left_devil == 6)
            return 2;
        int[] ship_role_num = ship.GetRoleNumber();
        if (!ship.GetShipSign()){
            right_priest += ship_role_num[0];
            right_devil += ship_role_num[1];
        }
        else{
            left_priest += ship_role_num[0];
            left_devil += ship_role_num[1];
        }
        if ((right_priest > 0 && right_priest < right_devil) || (left_priest > 0 && left_priest < left_devil))
            return 1;
        return 0;
    }
}