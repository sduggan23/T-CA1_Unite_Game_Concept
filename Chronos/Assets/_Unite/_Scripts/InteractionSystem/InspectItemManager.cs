using Unite.Core.Input;
using Unite.Core.Types;
using Unite.EventSystem;
using Unite.Managers;
using UnityEngine;

namespace Unite.InteractionSystem
{
    /// <summary>
    /// Manager for inspecting and interacting with items in the game.
    /// Video ref: https://www.youtube.com/watch?v=Ya0VkoAjDmY&t=257s&ab_channel=LearnWithYas
    /// </summary>
    public class InspectItemManager : MonoBehaviour
    {
        #region Fields

        private InspectItemData inspectItemData;

        [SerializeField]
        [Tooltip("The GameObject offset used during examination.")]
        private Vector3Type offset;

        [Header("Game events for toggling input when examining:")] 
        [SerializeField]
        private GameEvent onStartExaminingItem;

        [SerializeField] 
        private GameEvent onFinishExaminingItem;

        // [SerializeField]
        // [Tooltip("The Canvas used for examination UI (alternative reference).")]
        // private GameObject examineCanvas;
        #endregion

        private Transform playerCamTransform;

        #region Methods

        private void Start()
        {
            inspectItemData = new InspectItemData();
            playerCamTransform = GameManager.Instance.PlayerCamera.transform;
        }

        /// <summary>
        /// Checks if an object is being examined and initiates the examination process.
        /// </summary>
        /// <param name="pickupInfo">The GameObject being examined.</param>
        public void CheckExamining(PickupInfo pickupInfo)
        {
            inspectItemData.ToggleExamination();

            // Store the currently examined object and its original position and rotation
            if (inspectItemData.IsExamining)
            {
                InputManager.Instance.SwitchToExamineItemActionMap();
                onStartExaminingItem.Raise();
                
                inspectItemData.ExaminedObject = pickupInfo.Transform;
                inspectItemData.ZoomFactor = pickupInfo.ZoomFactor;
                inspectItemData.IsRotationDisabled = pickupInfo.IsRotationDisabled;
                inspectItemData.OriginalPositions[inspectItemData.ExaminedObject] = inspectItemData.ExaminedObject.position;
                inspectItemData.OriginalRotations[inspectItemData.ExaminedObject] = inspectItemData.ExaminedObject.rotation;
            }
            else
            {
                InputManager.Instance.SwitchToDefaultActionMap();
                onFinishExaminingItem.Raise();
            }
        }

        private void Update()
        {
            // Checks if the player is examining an object and updates the UI accordingly.
            if (inspectItemData.IsExamining)
            {
                Examine();
            }
            else
            {
                NonExamine();
            }
        }

        /// <summary>
        /// Handles the examination behaviour, including object movement and rotation.
        /// </summary>
        private void Examine()
        {
            if (inspectItemData.ExaminedObject != null)
            {
                Vector3 playerCamPos = playerCamTransform.position;

                inspectItemData.OriginalPositions.TryGetValue(inspectItemData.ExaminedObject, out var originalItemPos);
                float distanceBetweenCameraAndItem = Vector3.Distance(playerCamPos, originalItemPos);
                Vector3 newOffsetPosition =
                    offset.Value + playerCamTransform.forward * (distanceBetweenCameraAndItem - inspectItemData.ZoomFactor);
                Vector3 targetPosition = Vector3.MoveTowards(inspectItemData.ExaminedObject.position, newOffsetPosition, 5f);
                
                inspectItemData.ExaminedObject.position = Vector3.Lerp(inspectItemData.ExaminedObject.position, targetPosition, 0.2f);
                
                if (inspectItemData.IsRotationDisabled) return;
                
                Vector3 deltaMouse = Input.mousePosition - inspectItemData.LastMousePosition;
                float rotationSpeed = 1.0f;
                inspectItemData.ExaminedObject.Rotate(deltaMouse.x * rotationSpeed * Vector3.up, Space.World);
                inspectItemData.ExaminedObject.Rotate(deltaMouse.y * rotationSpeed * Vector3.left, Space.World);
                inspectItemData.LastMousePosition = Input.mousePosition;
            }
        }

        /// <summary>
        /// Handles behavior when not examining, including resetting the object's position and rotation.
        /// </summary>
        private void NonExamine()
        {
            if (inspectItemData.ExaminedObject != null)
            {
                // Reset the position and rotation of the examined object to its original values
                if (inspectItemData.OriginalPositions.TryGetValue(inspectItemData.ExaminedObject, out var position))
                {
                    inspectItemData.ExaminedObject.position = Vector3.Lerp(inspectItemData.ExaminedObject.position, position, 0.2f);
                }
                
                if (inspectItemData.IsRotationDisabled) return;
                if (inspectItemData.OriginalRotations.TryGetValue(inspectItemData.ExaminedObject, out var rotation))
                {
                    inspectItemData.ExaminedObject.rotation = Quaternion.Slerp(inspectItemData.ExaminedObject.rotation, rotation, 0.2f);
                }
            }
        }
        #endregion
    }
}