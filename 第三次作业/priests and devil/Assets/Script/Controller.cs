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

    void Start (){
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        view = gameObject.AddComponent<View>() as View;
        LoadResources();
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
        ship.ShipMove();
        view.sign = Check();
    }

    public void MoveRole(RoleModel role)
    {
        if (view.sign != 0)
        	return;
        if (role.IsOnShip()){
            LandModel land;
            if (ship.GetShipSign())
                land = left_land;
            else
                land = right_land;
            ship.DeleteRoleByName(role.GetId());
            role.Move(land.GetPosition(role.GetId()));
            role.GoLand(land);
            land.AddRole(role);
        }
        else{                                
            LandModel land = role.GetLandModel();
            if (ship.IsFull() || land.GetLandSign() != ship.GetShipSign())
            	return;
            land.DeleteRole(role.GetId());
            role.Move(ship.GetEmptyPosition());
            role.GoShip(ship);
            ship.AddRole(role);
        }
        view.sign = Check();
    }

    public void Restart()
    {
        right_land.Reset();
        left_land.Reset();
        ship.Reset();
        for (int i = 0; i < roles.Length; i++){
            if (roles[i] != null)
                roles[i].Reset();
        }
    }

    public int Check(){
        int right_priest = (right_land.GetRoleNum())[0];
        int right_devil = (right_land.GetRoleNum())[1];
        int left_priest = (left_land.GetRoleNum())[0];
        int left_devil = (left_land.GetRoleNum())[1];


        if (left_priest + left_devil == 6)
            return 2;

        int[] ship_role_num = ship.GetRoleNumber();
        if (!ship.GetShipSign())
        {
            right_priest += ship_role_num[0];
            right_devil += ship_role_num[1];
        }
        else
        {
            left_priest += ship_role_num[0];
            left_devil += ship_role_num[1];
        }

        if (right_priest > 0 && right_priest < right_devil){      
            return 1;
        }
        if (left_priest > 0 && left_priest < left_devil){
            return 1;
        }
        return 0;
    }
}