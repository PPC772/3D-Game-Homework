using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController{
    void LoadResources();                                  
}

public interface IUserAction{
    void Hit(Vector3 pos);
    int GetScore();
    int GetRound();
    int GetLife();
    void GameOver();
    void ReStart();
    void BeginGame();
}

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
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