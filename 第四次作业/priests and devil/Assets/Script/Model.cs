using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model{
    public interface ISceneController{
        void LoadResources();
    }

    public interface IUserAction{
        void MoveShip();
        void Restart();
        void MoveRole(RoleModel role);
        int Check();
    }

    public class SSDirector : System.Object{
        private static SSDirector director;
        public ISceneController CurrentScenceController{ 
        	get; 
        	set;
        }
        public static SSDirector GetInstance(){
            if (director == null)
                director = new SSDirector();
            return director;
        }
    }

    public class LandModel{
        GameObject land;
        Vector3[] positions;
        bool left_right;
        RoleModel[] roles = new RoleModel[6];

        public LandModel(bool flag){
            if (flag){
                positions = new Vector3[] {new Vector3 (-6, -2, 0), new Vector3 (-7, -2, 0), new Vector3 (-8, -2, 0),
                        new Vector3 (-10, -2, 0), new Vector3 (-11, -2, 0), new Vector3 (-12, -2, 0)};
                land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), new Vector3 (-10, -3, 0), Quaternion.identity) as GameObject;
                left_right = true;
            }
            else{
                positions = new Vector3[] {new Vector3 (6, -2, 0), new Vector3 (7, -2, 0), new Vector3 (8, -2, 0),
                        new Vector3 (10, -2, 0), new Vector3 (11, -2, 0), new Vector3 (12, -2, 0)};
                land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), new Vector3 (10, -3, 0), Quaternion.identity) as GameObject;
                flag = false;
            }
        }

        public void AddRole(RoleModel role){
            roles[role.GetId()] = role;
        }

        public RoleModel DeleteRole(int role_id){ 
            if (roles[role_id] != null){
            	RoleModel result = roles[role_id];
            	roles[role_id] = null;
            	return result;
            }
            return null;
        }

        public int[] GetRoleNum(){
            int[] count = {0, 0};
            for (int i = 0; i < roles.Length; i++)
                if (roles[i] != null){
                    if (i < roles.Length / 2)
                        count[0]++;
                    else
                        count[1]++;
                }
            return count;
        }

        public void Reset(){
            roles = new RoleModel[6];
        }

        public Vector3 GetPosition(int i){
        	return positions[i];
        }
        public bool GetLandSign() {
        	return left_right;
        }

    }

    public class ShipModel{
        GameObject ship;                                          
        Vector3[] left_pos;
        Vector3[] right_pos;
        Click click;
        bool ship_pos;
        RoleModel[] roles = new RoleModel[2];

        public ShipModel(){
            ship = Object.Instantiate(Resources.Load("Ship", typeof(GameObject)), new Vector3 (3.5F, -3, 0), Quaternion.identity) as GameObject;
            ship.name = "ship";
            ship_pos = false;
            click = ship.AddComponent(typeof(Click)) as Click;
            click.SetShip(this);
            left_pos = new Vector3[] {new Vector3 (-4.5F, -2, 0), new Vector3 (-2.5F, -2, 0)};
            right_pos = new Vector3[] {new Vector3 (2.5F, -2, 0), new Vector3 (4.5F, -2, 0)};
        }

        public bool IsEmpty(){
            if ((roles[0] == null) && (roles[1] == null))
            	return true;
            return false;
        }

        public bool IsFull(){
            if ((roles[0] != null) && (roles[1] != null))
                return true;
            return false;
        }

        public Vector3 GetShipMovePosition(){
            if (ship_pos){
                ship_pos = false;
                return new Vector3 (3.5F, -3, 0);
            }
            else{
                ship_pos = true;
                return new Vector3 (-3.5F, -3, 0);
            }
        }

        public RoleModel DeleteRoleByName(int role_id){
            if ((roles[0] != null) && (roles[0].GetId() == role_id)){
            	RoleModel result = roles[0];
            	roles[0] = null;
            	return result;
            }
           if ((roles[1] != null) && (roles[1].GetId() == role_id)){
            	RoleModel result = roles[1];
            	roles[1] = null;
            	return result;
            }
            return null;
        }

        public Vector3 GetEmptyPosition(){
        	if (ship_pos){
        		if (roles[0] == null)
        			return left_pos[0];
        		else
        			return left_pos[1];
        	}
        	else{
        		if (roles[0] == null)
        			return right_pos[0];
        		else
        			return right_pos[1];
        	}
        }

        public void AddRole(RoleModel role){
        	if (roles[0] == null)
            	roles[0] = role;
            else if (roles[1] == null)
            	roles[1] = role;
        }

        public void Reset(){
            if (ship_pos){
                ship_pos = false;
                ship.transform.position = new Vector3 (3.5F, -3, 0);
            }
            if (roles[0] != null)
                roles[0].Reset();
            if (roles[1] != null)
                roles[1].Reset();     
            roles = new RoleModel[2];
        }

        public int[] GetRoleNumber(){
            int[] count = {0, 0};
            if (roles[0] != null){
            	if (roles[0].GetId() < 3)
            		count[0]++;
            	else count[1]++;
            }
           if (roles[1] != null){
            	if (roles[1].GetId() < 3)
            		count[0]++;
            	else count[1]++;
            }
            return count;
        }

    	public GameObject GetShip(){
    		return ship;
    	}

    	public bool GetShipSign(){
    		return ship_pos;
    	}
    }

    public class RoleModel{
        GameObject role;
        int id;
        Click click;
        bool on_ship;   
        LandModel land_model = (SSDirector.GetInstance().CurrentScenceController as Controller).right_land;

        public RoleModel(int role_id){
        	id = role_id;
            if (role_id < 3){
                role = Object.Instantiate(Resources.Load("Priest", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            }
            else{
                role = Object.Instantiate(Resources.Load("Devil", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            }
            
            click = role.AddComponent(typeof(Click)) as Click;
            click.SetRole(this);
        }

        public int GetId(){
        	return id;
        }

        public void SetPosition(Vector3	pos){
        	role.transform.position = pos;
        }

        // public void Move(Vector3 vec){
        //     move.MovePosition(vec);
        // }

        public void GoLand(LandModel land){  
            role.transform.parent = null;
            land_model = land;
            on_ship = false;
        }

        public void GoShip(ShipModel ship){
            role.transform.parent = ship.GetShip().transform;
            land_model = null;          
            on_ship = true;
        }

        public void Reset(){
            land_model = (SSDirector.GetInstance().CurrentScenceController as Controller).right_land;
            GoLand(land_model);
            SetPosition(land_model.GetPosition(id));
            land_model.AddRole(this);
        }

        public bool IsOnShip(){
        	return on_ship;
        }

        public LandModel GetLandModel(){
        	return land_model;
        }

        public GameObject GetRole(){
            return role;
        }
    }

    // public class Move : MonoBehaviour{
    //     public void MovePosition(Vector3 position){
    //     	transform.position = position;
    //     }
    // }

    public class Click : MonoBehaviour{
        IUserAction action;
        RoleModel role = null;
        ShipModel ship = null;
        public void SetRole(RoleModel role){
            this.role = role;
        }
        public void SetShip(ShipModel ship){
            this.ship = ship;
        }
        void Start(){
            action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        }
        void OnMouseDown(){
            if (ship == null && role == null) return;
            if (ship != null)
                action.MoveShip();
            else if(role != null)
                action.MoveRole(role);
        }
    }

    public class PlayAnimation : MonoBehaviour{}
}