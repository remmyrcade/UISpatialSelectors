using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace UISpatialSelectors
{
    [RequireComponent(typeof(RectTransform))]
    public class UISpatialSelectable : Selectable
    {
        public UnityEvent OnHoverBegin;
        public UnityEvent OnHoverEnd;
        public UnityEvent OnClick;

        private BoxCollider _collider;
        private RectTransform _rectTransform;
        private RectTransform _scrollRect;
        private bool _insideScrollWindow;
        private bool _visible = true;
        private bool _initialized;

        private const float ColliderZScale = 0.01f;

        private struct SearchResult<T>
        {
            public bool success;
            public T behavior;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData != null)
            {
                base.OnPointerDown(eventData);
            }
            else
            {
                DoStateTransition(SelectionState.Pressed, false);
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            OnClick?.Invoke();
            if (eventData != null)
            {
                base.OnPointerUp(eventData);
                OnDeselect(eventData);
            }
            else
            {
                OnDeselect(null);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverBegin?.Invoke();
            if (eventData != null)
            {
                base.OnPointerEnter(eventData);
            }
            else
            {
                DoStateTransition(SelectionState.Highlighted, false);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            OnHoverEnd?.Invoke();
            if (eventData != null)
            {
                base.OnPointerExit(eventData);
            }
            else
            {
                DoStateTransition(SelectionState.Normal, false);
            }
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(InitYield());
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }
            if (_insideScrollWindow)
            {
                //TODO: change this to only check when scroll window position updates
                bool prevVisible = _visible;
                _visible = VisibleInsideScrollWindow();
                if (prevVisible != _visible)
                {
                    _collider.enabled = _visible;
                }
            }
        }

        private IEnumerator InitYield()
        {
            //delayed execution so that layout group elements have a frame to update their values correctly
            yield return new WaitForEndOfFrame();
            Init();
        }

        private void Init()
        {
            if (_initialized) return;
            //need to delay execution of this because ui elements inside a scroll view aren't initialized with the 
            //correct sizes until after their layout elements update in the first frame
            SearchResult<RectTransform> rectTransformResult = new SearchResult<RectTransform>
            {
                success = false,
                behavior = default(RectTransform),
            };
            rectTransformResult = RecursiveGetComponent<RectTransform>(rectTransformResult, transform);
            if (rectTransformResult.success)
            {
                _rectTransform = rectTransformResult.behavior;
                _collider = GetComponent<BoxCollider>();
                if (_collider == null) _collider = gameObject.AddComponent<BoxCollider>();
                _collider.size = new Vector3(_rectTransform.rect.width, _rectTransform.rect.height, ColliderZScale);
                _collider.isTrigger = true;
            }
            SearchResult<ScrollRect> scrollRectResult = new SearchResult<ScrollRect>
            {
                success = false,
                behavior = default(ScrollRect),
            };
            scrollRectResult = RecursiveGetComponent<ScrollRect>(scrollRectResult, transform);
            if (scrollRectResult.success)
            {
                _insideScrollWindow = true;
                _scrollRect = scrollRectResult.behavior.GetComponent<RectTransform>();
            }
            _initialized = true;
        }

        private bool VisibleInsideScrollWindow()
        {
            float heightBuffer = _rectTransform.rect.height * 0.1f;
            float scrollMinY = _scrollRect.rect.yMin + heightBuffer;
            float scrollMaxY = _scrollRect.rect.yMax - heightBuffer;
            Vector3 selectableWorldPos = _rectTransform.parent.TransformPoint(_rectTransform.localPosition);
            Vector3 selectableScrollPos = _scrollRect.transform.InverseTransformPoint(selectableWorldPos);
            float selectableMinY = selectableScrollPos.y + _rectTransform.rect.yMin;
            float selectableMaxY = selectableScrollPos.y + _rectTransform.rect.yMax;
            if (selectableMaxY > scrollMinY && selectableMinY < scrollMaxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private SearchResult<T> RecursiveGetComponent<T>(SearchResult<T> result, Transform root)
        {
            T behavior = root.GetComponent<T>();
            if (!result.success)
            {
                if (behavior != null)
                {
                    result.behavior = behavior;
                    result.success = true;
                    return result;
                }
                else if (root.parent != null)
                {
                    result = RecursiveGetComponent<T>(result, root.parent);
                }
            }
            return result;
        }
    }
}
