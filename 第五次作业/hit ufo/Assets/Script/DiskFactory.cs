using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour{
    public GameObject disk = null;
    private List<DiskData> used = new List<DiskData>();
    private List<DiskData> free = new List<DiskData>();

    //生成飞碟
    public GameObject GetDisk(int round){
        int choice = 0;
        int scope1 = 2, scope2 = 4, scope3 = 6;
        float start_y = -10f;
        string tag;
        disk = null;
        if (round <= 2)
            choice = Random.Range(0, scope1);
        else if (round <= 4)
            choice = Random.Range(0, scope2);
        else
            choice = Random.Range(0, scope3);
        if (choice <= scope1)
            tag = "disk1";
        else if (choice <= scope2)
            tag = "disk2";
        else
            tag = "disk3";
        for(int i = 0; i < free.Count; i++){
            if(free[i].tag == tag){
                disk = free[i].gameObject;
                free.Remove(free[i]);
                break;
            }
        }
        if(disk == null){
            if (tag == "disk1")
                disk = Instantiate(Resources.Load<GameObject>("disk1"), new Vector3(0, start_y, 0), Quaternion.identity);
            if (tag == "disk2")
                disk = Instantiate(Resources.Load<GameObject>("disk2"), new Vector3(0, start_y, 0), Quaternion.identity);
            if (tag == "disk3")
                disk = Instantiate(Resources.Load<GameObject>("disk3"), new Vector3(0, start_y, 0), Quaternion.identity);
            float ran_x = Random.Range(-1f, 1f) < 0 ? -1f : 1f;
            disk.GetComponent<Renderer>().material.color = disk.GetComponent<DiskData>().color;
            disk.GetComponent<DiskData>().direction = new Vector3(ran_x, start_y, 0);
            disk.transform.localScale = disk.GetComponent<DiskData>().scale;
        }
        used.Add(disk.GetComponent<DiskData>());
        return disk;
    }

    //回收飞碟
    public void FreeDisk(GameObject disk){
        for (int i = 0; i < used.Count; i++)
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID()){
                used[i].gameObject.SetActive(false);
                free.Add(used[i]);
                used.Remove(used[i]);
                break;
            }
    }
}