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
using System.IO;
using UnityEngine.SceneManagement;


public class LogDataTraining2 : MonoBehaviour
{

    string TASKNAME = "Training2";
    public List<MonoBehaviour> activeConstraints;
    public string saveTo = @"C:\Users\alber\Desktop\Active_Constraints\Task_Data\";
    // public string saveTo = @"C:\Users\User\Desktop\Alberto_Rota_MScThesis\Task_Data\";
    string foldername;
    string folderpath;
    string path;

    string GetUniqueName(string name, string folderPath) {
        string validatedName = name+"_0";
        int tries = 0;
        while (Directory.Exists(folderPath+"\\"+validatedName)) {
            tries++;
            validatedName = name+"_"+tries.ToString();
        }
        return validatedName;
    }

    public void Start() {
        // saveTo += TASKNAME;

        // Checks which VFs are activated and enabled
        activeConstraints = new List<MonoBehaviour>();
        foreach (MonoBehaviour s in GameObject.FindWithTag("ROBOT").GetComponents<MonoBehaviour>()) {
            if ((s.GetType().Name == "ConeApproachGuidanceVF"||
                 s.GetType().Name == "TrajectoryGuidanceVF"||
                 s.GetType().Name == "ObstacleAvoidanceForceFieldVF"||
                 s.GetType().Name == "SurfaceGuidanceVF"||
                 s.GetType().Name == "SurfaceAvoidanceVF")
             && s.enabled == true) {
                activeConstraints.Add(s);
            }
        }   

        // Creates the UNIQUE folder to save the logs
        foldername = GetUniqueName(SceneManager.GetActiveScene().name, saveTo);
        folderpath = saveTo+"\\"+foldername;

        // Creates the folder
        System.IO.Directory.CreateDirectory(folderpath);    
        Debug.Log("Task data will be saved to: "+folderpath);
        // Creates the .m file to save the logs
        File.Copy(saveTo+"\\"+TASKNAME+"_post.py", folderpath+"\\"+TASKNAME+"_post.py");

        // SAVES NON-CHANGING DATA (TRAJECTORIES, OBSTACLES, ...)
        path = folderpath+"\\"+foldername+"_scenetransform.csv";
        Matrix4x4 sceneTransform = GameObject.Find(TASKNAME).transform.worldToLocalMatrix;
        FileStream streamtransf = new FileStream(path, FileMode.Append);  
        using (StreamWriter writer = new StreamWriter(streamtransf))  
        {  
            writer.WriteLine(sceneTransform[0,0]+","+sceneTransform[0,1]+","+sceneTransform[0,2]+","+sceneTransform[0,3]);
            writer.WriteLine(sceneTransform[1,0]+","+sceneTransform[1,1]+","+sceneTransform[1,2]+","+sceneTransform[1,3]);
            writer.WriteLine(sceneTransform[2,0]+","+sceneTransform[2,1]+","+sceneTransform[2,2]+","+sceneTransform[2,3]);
            writer.WriteLine(sceneTransform[3,0]+","+sceneTransform[3,1]+","+sceneTransform[3,2]+","+sceneTransform[3,3]);
        }

        if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<TrajectoryGuidanceVF>())) {
            for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<TrajectoryGuidanceVF>().Length; j++) {
                path = folderpath+"\\"+foldername+"_traj"+j+".csv";
                FileStream streamtraj = new FileStream(path, FileMode.Append);  
                using (StreamWriter writer = new StreamWriter(streamtraj))  
                {  
                    writer.Write("X,Y,Z,\n");
                    for (int i=0; i<GameObject.FindWithTag("ROBOT").GetComponents<TrajectoryGuidanceVF>()[j].Trajectory.GetComponent<LineRenderer>().positionCount; i++) {
                        Vector3 point = GameObject.FindWithTag("ROBOT").GetComponents<TrajectoryGuidanceVF>()[j].Trajectory.GetComponent<LineRenderer>().GetPosition(i);
                        writer.Write(point.x.ToString()+",");
                        writer.Write(point.y.ToString()+",");
                        writer.Write(point.z.ToString()+",\n");
                    }
                }
            }

        }
        if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<ObstacleAvoidanceForceFieldVF>())) {
            for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<ObstacleAvoidanceForceFieldVF>().Length; j++) {
                path = folderpath+"\\"+foldername+"_obst"+j+".csv";
                FileStream streamtraj = new FileStream(path, FileMode.Append);  
                using (StreamWriter writer = new StreamWriter(streamtraj))  
                {  
                    writer.Write("X,Y,Z,\n");
                    Mesh obst_mesh = GameObject.FindWithTag("ROBOT").GetComponents<ObstacleAvoidanceForceFieldVF>()[j].obstacle.GetComponent<MeshFilter>().sharedMesh;
                    for (int i=0; i<obst_mesh.vertices.Length; i++) {
                        Vector3 point = GameObject.FindWithTag("ROBOT").GetComponents<ObstacleAvoidanceForceFieldVF>()[j].obstacle.transform.TransformPoint(obst_mesh.vertices[i]);
                        writer.Write(point.x.ToString()+",");
                        writer.Write(point.y.ToString()+",");
                        writer.Write(point.z.ToString()+",\n");
                    }
                }
            }
        }
        if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SurfaceAvoidanceVF>())) {
            for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SurfaceAvoidanceVF>().Length; j++) {
                path = folderpath+"\\"+foldername+"_surfavoid"+j+".csv";
                FileStream streamtraj = new FileStream(path, FileMode.Append);  
                using (StreamWriter writer = new StreamWriter(streamtraj))  
                {  
                    writer.Write("X,Y,Z,\n");
                    Mesh obst_mesh = GameObject.FindWithTag("ROBOT").GetComponents<SurfaceAvoidanceVF>()[j].surface.GetComponent<MeshFilter>().sharedMesh;
                    for (int i=0; i<obst_mesh.vertices.Length; i++) {
                        Vector3 point = GameObject.FindWithTag("ROBOT").GetComponents<SurfaceAvoidanceVF>()[j].surface.transform.TransformPoint(obst_mesh.vertices[i]);
                        writer.Write(point.x.ToString()+",");
                        writer.Write(point.y.ToString()+",");
                        writer.Write(point.z.ToString()+",\n");
                    }
                }
            }
        }
        if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SurfaceGuidanceVF>())) {
            for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SurfaceGuidanceVF>().Length; j++) {
                path = folderpath+"\\"+foldername+"_surfguide"+j+".csv";
                FileStream streamtraj = new FileStream(path, FileMode.Append);  
                using (StreamWriter writer = new StreamWriter(streamtraj))  
                {  
                    writer.Write("X,Y,Z,\n");
                    Mesh obst_mesh = GameObject.FindWithTag("ROBOT").GetComponents<SurfaceGuidanceVF>()[j].surface.GetComponent<MeshFilter>().sharedMesh;
                    for (int i=0; i<obst_mesh.vertices.Length; i++) {
                        Vector3 point = GameObject.FindWithTag("ROBOT").GetComponents<SurfaceGuidanceVF>()[j].surface.transform.TransformPoint(obst_mesh.vertices[i]);
                        writer.Write(point.x.ToString()+",");
                        writer.Write(point.y.ToString()+",");
                        writer.Write(point.z.ToString()+",\n");
                    }
                }
            }
        }
        if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<ConeApproachGuidanceVF>())) {
            for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<ConeApproachGuidanceVF>().Length; j++) {
                path = folderpath+"\\"+foldername+"_coneapproach"+j+".csv";
                FileStream streamtraj = new FileStream(path, FileMode.Append);  
                using (StreamWriter writer = new StreamWriter(streamtraj))  
                {  
                        Vector3 start = GameObject.FindWithTag("ROBOT").GetComponents<ConeApproachGuidanceVF>()[j].target.position-GameObject.FindWithTag("ROBOT").GetComponents<ConeApproachGuidanceVF>()[j].delta;
                        Vector3 end = GameObject.FindWithTag("ROBOT").GetComponents<ConeApproachGuidanceVF>()[j].target.position;
                        writer.Write("X,Y,Z,\n");
                        writer.Write(start.x.ToString()+",");
                        writer.Write(start.y.ToString()+",");
                        writer.Write(start.z.ToString()+",\n");
                        writer.Write(end.x.ToString()+",");
                        writer.Write(end.y.ToString()+",");
                        writer.Write(end.z.ToString()+",\n");
                    
                }
            }
        }
        if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SumForces>())) {
        }

        path = folderpath+"\\"+foldername+"_VFs.csv";
        FileStream stream = new FileStream(path, FileMode.Append);  
        using (StreamWriter writer = new StreamWriter(stream))  
        {  
            // TIME HEADER
            writer.Write("Time,");
            // POSITION HEADER
            writer.Write("PositionX,PositionY,PositionZ,");

            // VFs HEADERS
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<TrajectoryGuidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<TrajectoryGuidanceVF>().Length; j++) {
                    writer.Write("TrajectoryGuidanceVF_X"+j+",");
                    writer.Write("TrajectoryGuidanceVF_Y"+j+",");
                    writer.Write("TrajectoryGuidanceVF_Z"+j+",");
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<ObstacleAvoidanceForceFieldVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<ObstacleAvoidanceForceFieldVF>().Length; j++) {
                    writer.Write("ObstacleAvoidanceForceFieldVF_X"+j+",");
                    writer.Write("ObstacleAvoidanceForceFieldVF_Y"+j+",");
                    writer.Write("ObstacleAvoidanceForceFieldVF_Z"+j+",");
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SurfaceAvoidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SurfaceAvoidanceVF>().Length; j++) {
                    writer.Write("SurfaceAvoidanceVF_X"+j+",");
                    writer.Write("SurfaceAvoidanceVF_Y"+j+",");
                    writer.Write("SurfaceAvoidanceVF_Z"+j+",");
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SurfaceGuidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SurfaceGuidanceVF>().Length; j++) {
                    writer.Write("SurfaceGuidanceVF_X"+j+",");
                    writer.Write("SurfaceGuidanceVF_Y"+j+",");
                    writer.Write("SurfaceGuidanceVF_Z"+j+",");
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<ConeApproachGuidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<ConeApproachGuidanceVF>().Length; j++) {
                    writer.Write("ConeApproachGuidanceVF_X"+j+",");
                    writer.Write("ConeApproachGuidanceVF_Y"+j+",");
                    writer.Write("ConeApproachGuidanceVF_Z"+j+",");
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SumForces>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SumForces>().Length; j++) {
                    writer.Write("TotalForce_X"+j+",");
                    writer.Write("TotalForce_Y"+j+",");
                    writer.Write("TotalForce_Z"+j+",");
                }
            }

            writer.Write("\n");
        }  

    }

    void Update()
    {
        FileStream stream = new FileStream(path, FileMode.Append);  
        using (StreamWriter writer = new StreamWriter(stream))  
        {  
            // Writes the current time since startup
            writer.Write(Time.realtimeSinceStartup); writer.Write(",");

            // Writes the current position
            writer.Write(GameObject.Find(Global.tooltip_path).transform.position.x); writer.Write(","); 
            writer.Write(GameObject.Find(Global.tooltip_path).transform.position.y); writer.Write(","); 
            writer.Write(GameObject.Find(Global.tooltip_path).transform.position.z); writer.Write(","); 
            Vector3 f;
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<TrajectoryGuidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<TrajectoryGuidanceVF>().Length; j++) {
                    f = GameObject.FindWithTag("ROBOT").GetComponents<TrajectoryGuidanceVF>()[j].force;
                    writer.Write(f.x); writer.Write(","); 
                    writer.Write(f.y); writer.Write(","); 
                    writer.Write(f.z); writer.Write(","); 
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<ObstacleAvoidanceForceFieldVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<ObstacleAvoidanceForceFieldVF>().Length; j++) {
                    f = GameObject.FindWithTag("ROBOT").GetComponents<ObstacleAvoidanceForceFieldVF>()[j].force;
                    writer.Write(f.x); writer.Write(","); 
                    writer.Write(f.y); writer.Write(","); 
                    writer.Write(f.z); writer.Write(","); 
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SurfaceAvoidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SurfaceAvoidanceVF>().Length; j++) {
                    f = GameObject.FindWithTag("ROBOT").GetComponents<SurfaceAvoidanceVF>()[j].force;
                    writer.Write(f.x); writer.Write(","); 
                    writer.Write(f.y); writer.Write(","); 
                    writer.Write(f.z); writer.Write(","); 
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SurfaceGuidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SurfaceGuidanceVF>().Length; j++) {
                    f = GameObject.FindWithTag("ROBOT").GetComponents<SurfaceGuidanceVF>()[j].force;
                    writer.Write(f.x); writer.Write(","); 
                    writer.Write(f.y); writer.Write(","); 
                    writer.Write(f.z); writer.Write(","); 
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<ConeApproachGuidanceVF>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<ConeApproachGuidanceVF>().Length; j++) {
                    f = GameObject.FindWithTag("ROBOT").GetComponents<ConeApproachGuidanceVF>()[j].force;
                    writer.Write(f.x); writer.Write(","); 
                    writer.Write(f.y); writer.Write(","); 
                    writer.Write(f.z); writer.Write(","); 
                }
            }
            if (activeConstraints.Contains(GameObject.FindWithTag("ROBOT").GetComponent<SumForces>())) {
                for (int j=0; j<GameObject.FindWithTag("ROBOT").GetComponents<SumForces>().Length; j++) {
                    f = GameObject.FindWithTag("ROBOT").GetComponents<SumForces>()[j].totalForce;
                    writer.Write(f.x); writer.Write(","); 
                    writer.Write(f.y); writer.Write(","); 
                    writer.Write(f.z); writer.Write(","); 
                }
            }
            
            writer.Write("\n");
        }  
    }
}
