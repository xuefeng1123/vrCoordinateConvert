using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UnityEngine;


namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    public class cubeScript1 : MonoBehaviour
    {
        void Start()
        {
            // changePosition();
        }

        // Update is called once per frame
        void Update()
        {
            changePosition();
        }

        void changePosition()
        {
            System.Random ro = new System.Random();
            if (CustomTrackedObject != null)
            {
                int iResult;
                iResult = ro.Next(-2, 2);
                Debug.Log("iResult: " + iResult);
                CustomTrackedObject.transform.position = Vector3.up * iResult;
                CustomTrackedObject.transform.position = Vector3.left * iResult;
                Debug.Log("CustomTrackedObject.transform.position: " + CustomTrackedObject.transform.position);
                System.Threading.Thread.Sleep(10); //1 second
                LogGazeDirectionOrigin();
            }

        }

        void LogGazeDirectionOrigin()
        {
            Debug.Log("Position at which the gaze hit an object: "
                + CoreServices.InputSystem.GazeProvider.HitInfo.point);
            Debug.Log("Gaze is looking in direction: "
                + CoreServices.InputSystem.GazeProvider.GazeDirection);
            Debug.Log("Gaze origin is: "
                    + CoreServices.InputSystem.GazeProvider.GazeOrigin);

        }



        [SerializeField]
        private GameObject CustomTrackedObject = null;

        private SolverHandler handler;
        private Solver currentSolver;

        private TrackedObjectType trackedType = TrackedObjectType.Head;
        public TrackedObjectType TrackedType
        {
            get { return trackedType; }
            set
            {
                if (trackedType != value)
                {
                    trackedType = value;
                    RefreshSolverHandler();
                }
            }
        }

        private void Awake()
        {
            SetRadialView();
        }

        public void SetTrackedHead()
        {
            TrackedType = TrackedObjectType.Head;
        }

        public void SetTrackedController()
        {
            TrackedType = TrackedObjectType.ControllerRay;
        }

        public void SetTrackedHands()
        {
            TrackedType = TrackedObjectType.HandJoint;
        }

        public void SetTrackedCustom()
        {
            TrackedType = TrackedObjectType.CustomOverride;
        }

        public void SetRadialView()
        {
            DestroySolver();

            AddSolver<RadialView>();
        }

        public void SetOrbital()
        {
            DestroySolver();

            AddSolver<Orbital>();

            // Modify properties of solver custom to this example
            var orbital = currentSolver as Orbital;
            orbital.LocalOffset = new Vector3(3.0f, -1.5f, 3.0f);
        }

        public void SetSurfaceMagnetism()
        {
            DestroySolver();

            AddSolver<SurfaceMagnetism>();

            // Modify properties of solver custom to this example
            var surfaceMagnetism = currentSolver as SurfaceMagnetism;
            surfaceMagnetism.SurfaceNormalOffset = 0.2f;
        }

        private void AddSolver<T>() where T : Solver
        {
            currentSolver = gameObject.AddComponent<T>();
            handler = GetComponent<SolverHandler>();
            RefreshSolverHandler();
        }

        private void RefreshSolverHandler()
        {
            if (handler != null)
            {
                handler.TrackedTargetType = TrackedType;
                handler.TrackedHandness = Handedness.Both;
                if (CustomTrackedObject != null)
                {
                    handler.TransformOverride = CustomTrackedObject.transform;
                }
            }
            LogGazeDirectionOrigin();
        }

        private void DestroySolver()
        {
            if (currentSolver != null)
            {
                Destroy(currentSolver);
                currentSolver = null;
            }
        }

    }
}
