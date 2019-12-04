using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

// 获得两岸的角色数量（包括船上的）
// 定义AI下一步该怎么走的函数（比如返回 左岸的船上两个牧师）
// 定义该调用controller的函数（船上下乘客、满足条件后开船）

public class AI : MonoBehaviour{
	private IUserAction action;
	int left_priest, left_devil, right_priest, right_devil, ship_priest, ship_devil;
	RoleModel[] roles;
	ShipModel ship;

	void Start(){
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
	}

	public void getNumber(){
		bool left_right;
		left_priest	= left_devil = right_priest	= right_devil = 0;
		roles = action.GetRoles();
		ship = action.GetShip();
		for (int i = 0; i < 6; i++){
			if (roles[i].on_ship)
				left_right = ship.ship_left;
			else
				left_right = roles[i].GetLandModel().left_right;

			if ((roles[i].GetId() < 3) && (left_right == true))
				left_priest++;
			if ((roles[i].GetId() >= 3) && (left_right == true))
				left_devil++;				
			if ((roles[i].GetId() < 3) && (left_right == false))
				right_priest++;
			if ((roles[i].GetId() >= 3) && (left_right == false))
				right_devil++;
		}
	}

	public void AI_logic(){
		roles = action.GetRoles();
		ship = action.GetShip();
		if (ship.ship_left == false){
			if ((right_priest == 3) && (right_devil >= 2)){
				ship_priest = 0;
				ship_devil = 2;
			}
			if ((right_priest == 3) && (right_devil == 1)){
				ship_priest = 2;
				ship_devil = 0;
			}
			if (right_priest == 2){
				ship_priest = 2;
				ship_devil = 0;
			}
			if (right_priest == 1){
				ship_priest = 1;
				ship_devil = 1;
			}
			if ((right_priest == 0) && (right_devil >= 2)){
				ship_priest = 0;
				ship_devil = 2;
			}
			if ((right_priest == 0) && (right_devil == 1)){
				ship_priest = 0;
				ship_devil = 1;
			}
		}
		else{
			if ((left_priest == 3) && (left_devil == 3)){
				ship_priest = 0;
				ship_devil = 0;
			}
			if ((left_priest == 3) && (left_devil < 3)){
				ship_priest = 0;
				ship_devil = 1;
			}
			if (left_priest == 2){
				ship_priest = 1;
				ship_devil = 1;
			}
			if (left_priest == 1){
				ship_priest = 1;
				ship_devil = 0;
			}
			if (left_priest == 0){
				ship_priest = 0;
				ship_devil = 1;
			}
		}
	}

	public void AI_move(){
		int i, j;	
		bool[] if_move = new bool[2];
		ship = action.GetShip();
		getNumber();
		AI_logic();
		for (i = 0; i < 2; i++){	//下船
			if_move[i] = true;
			if (ship.roles[i] != null){
				if (ship.roles[i].GetId() < 3){
					if (ship_priest == 0){
						action.MoveRole(ship.roles[i]);
						return;
					}
					else{
						ship_priest = ship_priest - 1;
						if_move[i] = false;
					}
				}
				if (ship.roles[i].GetId() >= 3){
					if (ship_devil == 0){
						action.MoveRole(ship.roles[i]);
						return;
					}
					else{
						ship_devil = ship_devil - 1;
						if_move[i] = false;
					}
				}
			}
		}
		for (i = 0; i < 2; i++){	//上船
			if (if_move[i]){
				if (ship_priest > 0){
					ship_priest = ship_priest - 1;
					for (j = 0; j < 3; j++)
						if (!roles[j].on_ship && (same_side(roles[j], ship))){
							action.MoveRole(roles[j]);
							return;
						}
				}
				else if (ship_devil > 0){
					ship_devil = ship_devil - 1;
					for (j = 3; j < 6; j++)
						if ((!roles[j].on_ship) && (same_side(roles[j], ship))){
							action.MoveRole(roles[j]);
							return;
						}
				}
			}
		}
		action.MoveShip();
	}

	public bool same_side(RoleModel role, ShipModel ship){
		if (role.land_model.left_right == ship.ship_left)
			return true;
		else return false;
	}
}
