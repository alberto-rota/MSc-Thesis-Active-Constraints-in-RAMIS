// Copyright (c) 2022 Alberto Rota
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
   void Update()
    {
        GameObject psm = GameObject.Find("PSM");
        // Quits when the ESC key is pressed
        if(Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }

        // Toggles the VFs when the V key is pressed
        if(Input.GetKeyDown(KeyCode.V)){
            psm.GetComponent<SumForces>().enabled = !psm.GetComponent<SumForces>().enabled;
            if (psm.GetComponent<SumForces>().enabled) {
                GameObject.Find("Text/CanvasVF/VFActiveText").GetComponent<UnityEngine.UI.Text>().text="VF ACTIVE";
                GameObject.Find("Text/CanvasVF/VFActiveText").GetComponent<UnityEngine.UI.Text>().color=Color.green;
            } else {
                GameObject.Find("Text/CanvasVF/VFActiveText").GetComponent<UnityEngine.UI.Text>().text="VF INACTIVE";
                GameObject.Find("Text/CanvasVF/VFActiveText").GetComponent<UnityEngine.UI.Text>().color=Color.red;
            }
        }

        // Starts data logging when the R key is pressed
        if(Input.GetKeyDown(KeyCode.R)){    
            if (psm.GetComponent<LogDataTraining1>() != null){
                psm.GetComponent<LogDataTraining1>().enabled = !psm.GetComponent<LogDataTraining1>().enabled;
                if (psm.GetComponent<LogDataTraining1>().enabled) psm.GetComponent<LogDataTraining1>().Start();
            }else if (psm.GetComponent<LogDataTraining2>() != null){
                psm.GetComponent<LogDataTraining2>().enabled = !psm.GetComponent<LogDataTraining2>().enabled;
                if (psm.GetComponent<LogDataTraining2>().enabled) psm.GetComponent<LogDataTraining2>().Start();
            }else if (psm.GetComponent<LogDataTraining3>() != null){
                psm.GetComponent<LogDataTraining3>().enabled = !psm.GetComponent<LogDataTraining3>().enabled;
                if (psm.GetComponent<LogDataTraining3>().enabled) psm.GetComponent<LogDataTraining3>().Start();
            }else if (psm.GetComponent<LogDataLiverResection>() != null){
                psm.GetComponent<LogDataLiverResection>().enabled = !psm.GetComponent<LogDataLiverResection>().enabled;
                if (psm.GetComponent<LogDataLiverResection>().enabled) psm.GetComponent<LogDataLiverResection>().Start();
            }else if (psm.GetComponent<LogDataThymectomy>() != null){
                psm.GetComponent<LogDataThymectomy>().enabled = !psm.GetComponent<LogDataThymectomy>().enabled;
                if (psm.GetComponent<LogDataThymectomy>().enabled) psm.GetComponent<LogDataThymectomy>().Start();
            }else if (psm.GetComponent<LogDataNephrectomy>() != null){
                psm.GetComponent<LogDataNephrectomy>().enabled = !psm.GetComponent<LogDataNephrectomy>().enabled;
                if (psm.GetComponent<LogDataNephrectomy>().enabled) psm.GetComponent<LogDataNephrectomy>().Start();
            }

            if (psm.GetComponent<LogDataTraining1>().enabled) {
                GameObject.Find("Text/CanvasVF/LoggingText").GetComponent<UnityEngine.UI.Text>().text="\n\nGO";
                GameObject.Find("Text/CanvasVF/LoggingText").GetComponent<UnityEngine.UI.Text>().color=Color.green;
            } else {
                GameObject.Find("Text/CanvasVF/LoggingText").GetComponent<UnityEngine.UI.Text>().text="\n\nSTOP";
                GameObject.Find("Text/CanvasVF/LoggingText").GetComponent<UnityEngine.UI.Text>().color=Color.red;
            }
        }

        // Loads a scene of choice when the user presses the corresponding key
        if(Input.GetKey(KeyCode.Alpha1)){
            SceneManager.LoadScene("Assets/Tasks/Training1.unity");
        }else if(Input.GetKey(KeyCode.Alpha2)){
            SceneManager.LoadScene("Assets/Tasks/Training2.unity"); 
        }else if(Input.GetKey(KeyCode.Alpha3)){
            SceneManager.LoadScene("Assets/Tasks/Training3.unity");
        }else if(Input.GetKey(KeyCode.Alpha4)){
            SceneManager.LoadScene("Assets/Tasks/Thymectomy.unity");
        }else if(Input.GetKey(KeyCode.Alpha5)){
            SceneManager.LoadScene("Assets/Tasks/Nephrectomy.unity");
        }else if(Input.GetKey(KeyCode.Alpha6)){
            SceneManager.LoadScene("Assets/Tasks/LiverResection.unity");
        }
    }
}