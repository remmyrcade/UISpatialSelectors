using UnityEngine;
using UnityEngine.UI;

namespace UISpatialSelectors
{
    public class UICollisionSelector : UISpatialSelectorBase
    {
        [SerializeField] private UICollisionObject _hoverObject;
        [SerializeField] private UICollisionObject _clickerObject;
        [SerializeField] private float _hoverRadius = 0.3f;
        [SerializeField] private float _clickerRadius = 0.1f;

        private void Awake()
        {
            Setup(_hoverRadius, _clickerRadius);
        }

        private void Start()
        {
            _hoverObject.ClosestSelectableChanged += HandleHoverSelectableChanged;
            _clickerObject.ClosestSelectableChanged += HandleClickerSelectableChanged;
        }

        private void OnDisable()
        {
            ReleaseUp();
            HoverOver(null);
        }

        public void Setup(float hoverRadius, float clickerRadius)
        {
            _hoverRadius = hoverRadius;
            _clickerRadius = clickerRadius;
            _hoverObject.transform.localScale = Vector3.one * _hoverRadius;
            _clickerObject.transform.localScale = Vector3.one * _clickerRadius;
        }

        private void HandleClickerSelectableChanged(Selectable s)
        {
            switch (_selectorState)
            {
                case SelectorState.Up:
                    if (_activeSelectable == s)
                    {
                        PressDown();
                    }
                    break;
                case SelectorState.Down:
                    if(s != _activeSelectable)
                    {
                        ReleaseUp();
                    }
                    if(s != null)
                    {
                        HoverOver(s);
                        PressDown();
                    }
                    break;
            }
        }

        private void HandleHoverSelectableChanged(Selectable s)
        {
            HoverOver(s);
        }
    }
}
