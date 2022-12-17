using System;
using System.Collections.Generic;
using Elements;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Elements
{
    public class NodeElement : ElementWithMotion
    {
        [SerializeField, EnableIf(nameof(disabled))] private List<ElementWithMotion> _elementsFromThis = new();
        private readonly List<ElementWithMotion> _elementsToThis = new();

        [SerializeField, EnableIf(nameof(disabled))] private List<LineRenderer> _lines = new();
        private bool disabled => false;
        private string _outNode;
        public override string OutNode
        {
            get
            {
                // if we had parallel connection of wires without elements we use the same node
                if (_elementsToThis.Count > 0 && _elementsToThis[0].TryGetComponent(out NodeElement nodeElement))
                {
                    var outNode = nodeElement.OutNode;
                    foreach (var element in _elementsToThis)
                    {
                        if (element.OutNode != outNode)
                        {
                            return _outNode;
                        }
                    }
                    return outNode;
                }
                return _outNode;
            }
        }

        private protected override void Start()
        {
            base.Start();
            _outNode = CircuitSimulator.CreateNode();
        }

        private protected override void Update()
        {
            base.Update();
            for (int i = 0; i < _elementsFromThis.Count; i++)
            {
                if (_lines.Count > i)
                {
                    DrawLine(_lines[i], _elementsFromThis[i]);
                }
            }
        }

        public override void AddElementsFromThis(ElementWithMotion elementWithMotion)
        {
            if (_elementsFromThis.Contains(elementWithMotion))
            {
                for (var i = 0; i < _elementsFromThis.Count; i++)
                {
                    if (_elementsFromThis[i] != elementWithMotion) continue;
                    
                    _elementsFromThis.Remove(_elementsFromThis[i]);
                    Destroy(_lines[i].gameObject);
                    _lines.Remove(_lines[i]);
                    return;
                }
                return;
            }
            _lines.Add(Instantiate(wire, outputPoint));
            _elementsFromThis.Add(elementWithMotion);
        }

        public override void AddElementsToThis(ElementWithMotion elementWithMotion)
        {
            base.AddElementsToThis(elementWithMotion);
        }

        private void OnDestroy()
        {
            foreach (var elementToThis in _elementsToThis)
            {
                elementToThis.AddElementsFromThis(this);
            }
        }
    }
}