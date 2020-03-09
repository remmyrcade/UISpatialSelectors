using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UISpatialSelectors
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class UICollisionObject : MonoBehaviour
    {
        public event Action<Selectable> ClosestSelectableChanged;

        private Selectable _closestSelectable;
        private List<Selectable> _selectablesInRange = new List<Selectable>();
        private Collider _collider;

        private void OnEnable()
        {
            if(_collider == null)
            {
                _collider = GetComponent<Collider>();
                _collider.isTrigger = true;
            }
            //best I can do is grab all the colliders in a sphere radius around us (sorry if you're not using a sphere collider)
            Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, transform.localScale.sqrMagnitude);
            foreach(Collider c in overlappingColliders)
            {
                Selectable s = c.GetComponent<Selectable>();
                if(s != null)
                {
                    _selectablesInRange.Add(s);
                }
            }
        }

        private void Update()
        {
            CheckForClosestSelectable();
        }

        private void OnDisable()
        {
            _closestSelectable = null;
            _selectablesInRange.Clear();
            ClosestSelectableChanged?.Invoke(null);
        }

        private void OnTriggerEnter(Collider other)
        {
            Selectable selectable = other.GetComponent<Selectable>();
            if (selectable != null)
            {
                if (!_selectablesInRange.Contains(selectable))
                {
                    _selectablesInRange.Add(selectable);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Selectable selectable = other.GetComponent<Selectable>();
            if (selectable != null)
            {
                if (_selectablesInRange.Contains(selectable))
                {
                    _selectablesInRange.Remove(selectable);
                }
            }
        }

        private void CheckForClosestSelectable()
        {
            if (_selectablesInRange.Count > 0)
            {
                //search for closest selectable
                Selectable closestSelectable = null;
                float closestDist = Mathf.Infinity;
                foreach (Selectable s in _selectablesInRange)
                {
                    float dist = Vector3.Distance(transform.position, s.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestSelectable = s;
                    }
                }
                //check if the closest selectable is different from the current
                if (closestSelectable != _closestSelectable)
                {
                    ClosestSelectableChanged(closestSelectable);
                    _closestSelectable = closestSelectable;
                }
            }
            else if (_closestSelectable != null)
            {
                ClosestSelectableChanged(null);
                _closestSelectable = null;
            }
        }
    }
}
