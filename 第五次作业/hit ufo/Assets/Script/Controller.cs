using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//模板类
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour{
    protected static T instance;
    public static T Instance{
        get{
            if (instance == null){
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null){
                    Debug.LogError("An instance of " + typeof(T)
                        + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}

public class Controller : MonoBehaviour, ISceneController, IUserAction{
    public FlyActionManager action_manager;
    public DiskFactory disk_factory;
    public View view;
    public ScoreRecorder score_recorder;
    private Queue<GameObject> disk_queue = new Queue<GameObject>();
    private List<GameObject> disk_notshot = new List<GameObject>(); 
    private int round = 0;
    private int life = 5;
    private float speed = 2f;
    private int count = 0;
    private float m_time = 0f;
    private bool sleep = false;
    private bool playing_game = false;
    private bool game_over = false;
    private bool game_start = false;

    void Start(){
        SSDirector director = SSDirector.GetInstance();     
        director.CurrentScenceController = this;             
        disk_factory = Singleton<DiskFactory>.Instance;
        score_recorder = Singleton<ScoreRecorder>.Instance;
        action_manager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        view = gameObject.AddComponent<View>() as View;
    }
	
    //规则函数
    void ruler(){
        if (count % 11 == 0){
            if (speed > 0.6)
                speed -= 0.2f;
            CancelInvoke("LoadResources");
            count++;
            playing_game = false;
            sleep = true;
        }
    }

	void Update(){
        if (sleep){
            m_time += Time.deltaTime;
            if (m_time > 2){
                sleep = false;
                m_time = 0;
                round++;
                if (life < 5)
                    life++;
            }
        }
        else if(game_start){
            if (game_over){
                CancelInvoke("LoadResources");
            }
            if (!playing_game){
                InvokeRepeating("LoadResources", 1f, speed);
                playing_game = true;
            }
            ruler();
            SendDisk();
        }
    }

    //加载飞碟
    public void LoadResources(){
        disk_queue.Enqueue(disk_factory.GetDisk(round)); 
        count++;
    }

    //飞碟飞行时控制器对别的类的操作
    private void SendDisk(){
        float position_x = 16;                       
        if (disk_queue.Count != 0){
            GameObject disk = disk_queue.Dequeue();
            disk_notshot.Add(disk);
            disk.SetActive(true);
            float ran_y = Random.Range(1f, 4f);
            float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            disk.GetComponent<DiskData>().direction = new Vector3(ran_x, ran_y, 0);
            Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * position_x, ran_y, 0);
            disk.transform.position = position;
            float power = Random.Range(10f, 15f);
            float angle = Random.Range(15f, 28f);
            action_manager.UFOFly(disk,angle,power);
        }
        for (int i = 0; i < disk_notshot.Count; i++){
            GameObject temp = disk_notshot[i];
            if (temp.transform.position.y < -10 && temp.gameObject.activeSelf == true){
                disk_factory.FreeDisk(disk_notshot[i]);
                disk_notshot.Remove(disk_notshot[i]);
                if(life > 0)
                    life--;
            }
        }
    }  

    //点击检测函数
    public void Hit(Vector3 pos){
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool not_hit = false;
        for (int i = 0; i < hits.Length; i++){
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<DiskData>() != null){
                for (int j = 0; j < disk_notshot.Count; j++)
                    if (hit.collider.gameObject.GetInstanceID() == disk_notshot[j].gameObject.GetInstanceID())
                        not_hit = true;
                if(!not_hit) return;
                disk_notshot.Remove(hit.collider.gameObject);
                score_recorder.Record(hit.collider.gameObject);
                StartCoroutine(WaitingParticle(0.08f, hit, disk_factory, hit.collider.gameObject));
                break;
            }
        }
    }

    public int GetScore(){
        return score_recorder.score;
    }

    public int GetRound(){
        return round;
    }

    public int GetLife(){
        return life;
    }

    public void ReStart(){
        game_over = false;
        playing_game = false;
        score_recorder.Reset();
        round = 0;
        speed = 2f;
        count = 0;
        life = 5;
    }

    public void GameOver(){
        game_over = true;
    }

    //回收飞碟函数
    IEnumerator WaitingParticle(float wait_time, RaycastHit hit, DiskFactory disk_factory, GameObject obj){
        yield return new WaitForSeconds(wait_time);
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        disk_factory.FreeDisk(obj);
    }
    
    public void BeginGame(){
        game_start = true;
    }
}