using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

namespace UISpatialSelectors
{
    public class UIRaycasterSelector : UISpatialSelectorBase
    {
        [SerializeField] private LineRenderer _raycastLine;
        [SerializeField, Range(0.5f, 1f)] private float _triggerRecognizedThreshold = 0.9f;
        [SerializeField] private float _rayPointerNoHitVisualLength = 0.1f;

        private MLInputController _controller;

        private void OnEnable()
        {
            _raycastLine.positionCount = 2;
        }

        private void Start()
        {
            InitalizeController();
        }

        private void OnDisable()
        {
            _raycastLine.positionCount = 0;
        }

        private void Update()
        {
            UpdateSelectorState();
        }

        private void UpdateSelectorState()
        {
            //check for hover 
            CheckForHover();
            if (_selectorState == SelectorState.Up)
            {
                _raycastLine.positionCount = 2;
                //check for down
                if (_controller.TriggerValue >= _triggerRecognizedThreshold)
                {
                    PressDown();
                }
            }
            else
            {
                _raycastLine.positionCount = 0;
                //check for up
                if (_controller.TriggerValue < _triggerRecognizedThreshold)
                {
                    ReleaseUp();
                }
            }
        }

        private void CheckForHover()
        {
            Ray ray = new Ray(_controller.Position, (_controller.Orientation * Vector3.forward));
            RaycastHit hit;
            _raycastLine.SetPosition(0, ray.origin);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                _raycastLine.material.color = Color.cyan;
                _raycastLine.SetPosition(1, hit.point);
                Selectable hitSelectable = hit.collider.GetComponent<Selectable>();
                HoverOver(hitSelectable);
            }
            else
            {
                //no selectable hovered
                _raycastLine.material.color = Color.magenta;
                _raycastLine.SetPosition(1, ray.origin + (ray.direction * _rayPointerNoHitVisualLength));
                HoverOver(null);
            }
        }

        private void InitalizeController()
        {
            if (!MLInput.IsStarted)
            {
                MLResult result = MLInput.Start();
                if (!result.IsOk)
                {
                    Debug.LogErrorFormat("Error: UIRaycasterSelector failed starting MLInput, disabling script. Reason: {0}", result);
                    enabled = false;
                    return;
                }
            }
            _controller = MLInput.GetController(0);
        }
    }
}
