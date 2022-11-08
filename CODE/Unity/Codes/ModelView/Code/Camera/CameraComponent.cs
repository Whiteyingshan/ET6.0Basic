using Cinemachine;
using UnityEngine;

namespace ET
{
    public class CameraComponent : Entity
    {
        public GameObject Target;
        public Camera MainCamera;
        public CinemachineBrain MainCameraBrain;
        public CinemachineVirtualCamera VirtualCamera;
        public CinemachineVirtualCamera VTCamera2D;
        public bool IsQingZhu;
      
    }
}