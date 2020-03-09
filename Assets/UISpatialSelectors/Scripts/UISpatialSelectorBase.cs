using UnityEngine;
using UnityEngine.UI;

namespace UISpatialSelectors
{
    public class UISpatialSelectorBase : MonoBehaviour
    {
        protected SelectorState _selectorState
        {
            get;
            private set;
        }

        protected Selectable _activeSelectable
        {
            get;
            private set;
        }

        private Selectable _nextHoverSelectable;

        private void Awake()
        {
            _selectorState = SelectorState.Up;
        }

        protected void PressDown()
        {
            if(_selectorState != SelectorState.Up)
            {
                return;
            }
            if (_activeSelectable != null)
            {
                //cache the active selectable as the next hover so we can determine later whether to re-hover over it when we release
                _nextHoverSelectable = _activeSelectable;
                _activeSelectable.OnPointerDown(null);
            }
            _selectorState = SelectorState.Down;
        }

        protected void ReleaseUp()
        {
            if (_selectorState != SelectorState.Down)
            {
                return;
            }
            if (_activeSelectable != null)
            {
                //release up
                _activeSelectable.OnPointerUp(null);
            }
            if(_nextHoverSelectable != null)
            {
                //we can now hover over the next hover selectable and set it to be active if there is one
                _activeSelectable = _nextHoverSelectable;
                _activeSelectable.OnPointerEnter(null);
            }
            else
            {
                //we have no selectable
                _activeSelectable = null;
            }
            _selectorState = SelectorState.Up;
            _nextHoverSelectable = null;
        }

        protected void HoverOver(Selectable hoverSelectable)
        {
            //we only look for selectables to hover over if we are not in a press
            if (_selectorState != SelectorState.Up)
            {
                _nextHoverSelectable = hoverSelectable;
                return;
            }
            //do not re-hover if this selectable is already hovered on
            if(hoverSelectable == _activeSelectable)
            {
                return;
            }
            //un hover the old selectable
            if (_activeSelectable != null)
            {
                _activeSelectable.OnPointerExit(null);
            }
            //set the new selectable
            _activeSelectable = hoverSelectable;
            //hover over the new selectable
            if (_activeSelectable != null)
            {
                _activeSelectable.OnPointerEnter(null);
            }
        }
    }
}
