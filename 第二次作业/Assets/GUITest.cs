using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour {
    double show = 0, buffer = 0;
    bool if_have_symble = false, if_input = false, negative_symble = false, if_dot = false;
    string dot_tail = "";
    string temp_symble = "";   

    double combine(){
        if (!if_dot && !negative_symble)
            return show;
        else if (!if_dot && negative_symble)
            return -1 * show;
        else if (if_dot && !negative_symble)
            return show + int.Parse(dot_tail) / pow(dot_tail.Length);
        else
            return -1 * (show + int.Parse(dot_tail) / pow(dot_tail.Length));
    }

    void cal(){
        show = combine();
        if (temp_symble == "+")
            show = buffer + show;
        if (temp_symble == "-")
            show = buffer - show;
        if (temp_symble == "*")
            show = buffer * show;
        if (temp_symble == "/")
            show = buffer / show;
        negative_symble = false;
        buffer = show;
    }

    double pow(int x){
        double result = 1;
        while (x-- > 0)
            result *= 10;
        return result;
    }

    string head(){
        if (negative_symble)
            return "-";
        return "";
    }

    string tail(){
        if (!if_input)
            return "";
        string result = "";
        if (if_dot)
            result += ".";
        return result + dot_tail;
    }
    
    void OnGUI () {
        GUI.Box(new Rect(0, 0, 185, 275), "Calculator");
        GUI.Box(new Rect(5, 25, 175, 20), head() + show.ToString() + tail());

        if (GUI.Button(new Rect(5, 50, 40, 40), "AC")){
            show = buffer = 0;
            if_have_symble = if_input = negative_symble = false;
            temp_symble = dot_tail = "";
        }
        if (GUI.Button(new Rect(50, 50, 40, 40), "C")){
            show = 0;
            dot_tail = "";
            if_input = if_dot = negative_symble = false;
        }
        if (GUI.Button(new Rect(95, 50, 85, 40), "Backspace")){
            if (!if_dot){
                show = (int)show / 10;
            }
            else{
                if (dot_tail != "")
                    dot_tail = dot_tail.Substring(0, dot_tail.Length - 1);
                else
                    if_dot = false;
            }
        }


        if (GUI.Button(new Rect(5, 95, 40, 40), "7")){
            if (!if_input){
                show = 7;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 7;
                else{
                    dot_tail += "7";
                }
            }
        }
        if (GUI.Button(new Rect(5, 140, 40, 40), "4")){
            if (!if_input){
                show = 4;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 4;
                else{
                    dot_tail += "4";
                }
            }
        }
        if (GUI.Button(new Rect(5, 185, 40, 40), "1")){
            if (!if_input){
                show = 1;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 1;
                else{
                    dot_tail += "1";
                }
            }
        }
        if (GUI.Button(new Rect(5, 230, 40, 40), "0")){
            if (!if_input){
                show = 0;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10;
                else{
                    dot_tail += "0";
                }
            }
        }
        if (GUI.Button(new Rect(50, 95, 40, 40), "8")){
            if (!if_input){
                show = 8;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 8;
                else{
                    dot_tail += "8";
                }
            }
        }
        if (GUI.Button(new Rect(50, 140, 40, 40), "5")){
            if (!if_input){
                show = 5;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 5;
                else{
                    dot_tail += "5";
                }
            }
        }
        if (GUI.Button(new Rect(50, 185, 40, 40), "2")){
            if (!if_input){
                show = 2;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 2;
                else{
                    dot_tail += "2";
                }
            }
        }
        if (GUI.Button(new Rect(50, 230, 40, 40), ".") && (!if_dot)){
            if_dot = true;
            if (!if_input){
                show = 0;
                if_input = true;
                dot_tail = "";
            }
        }
        if (GUI.Button(new Rect(95, 95, 40, 40), "9")){
            if (!if_input){
                show = 9;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 9;
                else{
                    dot_tail += "9";
                }
            }
        }
        if (GUI.Button(new Rect(95, 140, 40, 40), "6")){
            if (!if_input){
                show = 6;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 6;
                else{
                    dot_tail += "6";
                }
            }
        }
        if (GUI.Button(new Rect(95, 185, 40, 40), "3")){
            if (!if_input){
                show = 3;
                if_input = true;
                dot_tail = "";
                if_dot = false;
            }
            else{
                if (!if_dot)
                    show = show * 10 + 3;
                else{
                    dot_tail += "3";
                }
            }
        }      

        if (GUI.Button(new Rect(140, 95, 40, 40), "+")){
            if (if_input){
                if (if_have_symble)
                    cal();
                else{
                    buffer = combine();
                    show = buffer;
                }
                if_have_symble = true;
                temp_symble = "+";
                if_input = negative_symble = false;
            }
            else{
                negative_symble = false;
                show = 0;
            }
        }
        if (GUI.Button(new Rect(140, 140, 40, 40), "-")){

            if (if_input){
                if (if_have_symble)
                    cal();
                else{
                    buffer = combine();
                    show = buffer;
                }
                if_have_symble = true;
                temp_symble = "-";
                if_input = negative_symble = false;
            }
            else{
                negative_symble = true;
                show = 0;
            }


        }
        if (GUI.Button(new Rect(140, 185, 40, 40), "*") && if_input){
            if (if_have_symble)
                cal();
            else{
                buffer = combine();
                show = buffer;
            }
            if_have_symble = true;
            temp_symble = "*";
            if_input = negative_symble = false;
        }
        if (GUI.Button(new Rect(140, 230, 40, 40), "/") && if_input){
            if (if_have_symble)
                cal();
            else{
                buffer = combine();
                show = buffer;
            }
            if_have_symble = true;
            temp_symble = "/";
            if_input = negative_symble = false;
        }
        if (GUI.Button(new Rect(95, 230, 40, 40), "=") && if_input){
            if (if_have_symble)
                cal();
            else{
                buffer = combine();
                show = buffer;
            }
            temp_symble = "";
            if_have_symble = if_input = negative_symble = false;
        }
    }
}