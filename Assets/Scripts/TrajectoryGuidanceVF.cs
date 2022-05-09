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

//    IMPLEMENTED FROM = {A dynamic non-energy-storing guidance constraint with motion redirection for robot-assisted surgery},
//    author = {Nima Enayati and Eva C.Alves Costa and Giancarlo Ferrigno and Elena De Momi},
//    year = {2016},

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrajectoryGuidanceVF : MonoBehaviour
{

    public Transform Trajectory;

    [Range(0,100)]
    public float viscousCoefficient = 1;

    [SerializeField]
    int maxForce = 100;

    Vector3 closest;
    Vector3 prevPosition = Vector3.zero;
    public Vector3 velocity;
    public Vector3 displacement;
    public   Vector3 force;
    [SerializeField]
    bool graphics = true;
    [Range(0,5)]
    public float graphicVectorGain = 1;

    Transform EndEffector;

    void Start()
    {
        EndEffector = gameObject.transform;
        prevPosition = EndEffector.position;
    }

    void Update()
    {
        if (Trajectory == null) {
            Debug.LogWarning("The Reference Trajectory must be assigned!");
            return;
        }
        // DISTANCE
        float mindist = 100000;
        // Vector3[] extractPositions = new Vector3[GetComponent<LineRenderer>().positionCount];
        for (int i=0; i<Trajectory.GetComponent<LineRenderer>().positionCount; i++) {
            Vector3 point = Trajectory.GetComponent<LineRenderer>().GetPosition(i);
            float d = Vector3.Distance(point, EndEffector.position);
            if (d < mindist) {
                mindist = d;
                closest = point;
            }
        }
        displacement = closest - EndEffector.position;

        // VELOCITY
        velocity = (EndEffector.position-prevPosition)/Time.deltaTime;
        prevPosition = EndEffector.position;

        // FORCE
        float b = viscousCoefficient*Mathf.Sqrt((1-Vector3.Dot(velocity.normalized,displacement.normalized))/2);
        float f_mag = b*velocity.magnitude;
        if (f_mag > maxForce) {
            f_mag = maxForce;
        }
        Vector3 f_dir;
        if (Vector3.Dot(velocity.normalized, displacement.normalized)<0) { // When moving away
            f_dir = displacement.normalized;
        } else {  // When approaching
            f_dir = Quaternion.AngleAxis(
                (1+Vector3.Dot(velocity.normalized,displacement.normalized))*Mathf.PI/2*180f/Mathf.PI, // Rotation Angle (in degrees)
                Vector3.Cross(velocity.normalized,displacement.normalized)
                )*velocity.normalized;  //Rotation Axis
        }
        force = f_mag*f_dir;

        if (graphics) {
            Debug.DrawLine(EndEffector.position, EndEffector.position+velocity*graphicVectorGain, Color.green);
            Debug.DrawLine(EndEffector.position, closest, Color.red);
            Debug.DrawLine(EndEffector.position, EndEffector.position+force*graphicVectorGain, Color.blue);
        }
    }
}
