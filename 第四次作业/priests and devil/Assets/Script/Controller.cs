using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class Controller : MonoBehaviour, ISceneController, IUserAction{
    public LandModel right_land;
    public LandModel left_land;
    public ShipModel ship;
    private RoleModel[] roles;
    View view;
    public MySceneActionManager actionManager;

    void Start (){
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        view = gameObject.AddComponent<View>() as View;
        LoadResources();
        actionManager = gameObject.AddComponent<MySceneActionManager>() as MySceneActionManager;    
    }
	
    public void LoadResources(){
        right_land = new LandModel(false);
        left_land = new LandModel(true);
        ship = new ShipModel();
        roles = new RoleModel[6];

        for (int i = 0; i < 3; i++){
            RoleModel role = new RoleModel(i);
            role.SetPosition(right_land.GetPosition(i));
            role.GoLand(right_land);
            right_land.AddRole(role);
            roles[i] = role;
        }

        for (int i = 0; i < 3; i++){
            RoleModel role = new RoleModel(i + 3);
            role.SetPosition(right_land.GetPosition(i + 3));
            role.GoLand(right_land);
            right_land.AddRole(role);
            roles[i + 3] = role;
        }
    }

    public void MoveShip(){
        if (ship.IsEmpty() || view.sign != 0)
        	return;
        actionManager.Move(ship.GetShip(), ship.GetShipMovePosition());
        view.sign = Check();
    }

    public void MoveRole(RoleModel role){
        if (view.sign != 0)
        	return;
        if (role.IsOnShip()){
            LandModel land;
            if (ship.GetShipSign())
                land = left_land;
            else
                land = right_land;
            ship.DeleteRoleByName(role.GetId());

            actionManager.Move(role.GetRole(), land.GetPosition(role.GetId()));
            role.GoLand(land);
            land.AddRole(role);
        }
        else{                                
            LandModel land = role.GetLandModel();
            if (ship.IsFull() || land.GetLandSign() != ship.GetShipSign())
            	return;
            land.DeleteRole(role.GetId());
            actionManager.Move(role.GetRole(), ship.GetEmptyPosition());
            role.GoShip(ship);
            ship.AddRole(role);
        }
        view.sign = Check();
    }

    public void Restart(){
        right_land.Reset();
        left_land.Reset();
        ship.Reset();
        for (int i = 0; i < roles.Length; i++){
            if (roles[i] != null)
                roles[i].Reset();
        }
    }

    public int Check(){
        return actionManager.Judge(left_land, right_land, ship);
    }
}