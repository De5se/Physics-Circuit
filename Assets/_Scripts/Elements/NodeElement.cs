using System;
using System.Collections.Generic;
using Elements;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Elements
{
    public class NodeElement : ElementWithMotion
    {
        [SerializeField, EnableIf(nameof(Disabled))] private List<ElementWithMotion> elementsFromThis = new();
        [SerializeField, EnableIf(nameof(Disabled))] private List<ElementWithMotion> elementsToThis = new();

        [SerializeField, EnableIf(nameof(Disabled))] private List<LineRenderer> lines = new();
        private bool Disabled => false;
        [ShowNonSerializedField, Foldout("Info")]
        private string _outNode;
        [ShowNonSerializedField, Foldout("Info")]
        private string _infoNode;
        
        public override string OutNode
        {
            get
            {
                // if we had parallel connection of wires without elements we use the same node
                if (elementsToThis.Count > 0 && elementsToThis[0].GetComponent<NodeElement>() != null)
                {
                    var previousNode = elementsToThis[0].OutNode;
                    var previousNodesTheSame = true;;
                    foreach (var element in elementsToThis)
                    {
                        if (element.GetComponent<NodeElement>() == null){continue;}
                        
                        var elementOutNode = element.OutNode;
                        previousNodesTheSame =  elementOutNode == previousNode && previousNodesTheSame;
                    }
                    if (previousNodesTheSame)
                    {
                        return _infoNode = previousNode;
                    }
                }
                return _infoNode = _outNode;;
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
            for (int i = 0; i < elementsFromThis.Count; i++)
            {
                if (lines.Count > i)
                {
                    DrawLine(lines[i], elementsFromThis[i]);
                }
            }
        }

        public override void AddElementsFromThis(ElementWithMotion elementWithMotion)
        {
            if (elementsFromThis.Contains(elementWithMotion))
            {
                for (var i = 0; i < elementsFromThis.Count; i++)
                {
                    if (elementsFromThis[i] != elementWithMotion) continue;
                    
                    elementsFromThis.Remove(elementsFromThis[i]);
                    Destroy(lines[i].gameObject);
                    lines.Remove(lines[i]);
                    return;
                }
                return;
            }
            lines.Add(Instantiate(wire, outputPoint));
            elementsFromThis.Add(elementWithMotion);
        }

        public override void AddElementsToThis(ElementWithMotion elementWithMotion)
        {
            base.AddElementsToThis(elementWithMotion);
        }

        private void OnDestroy()
        {
            foreach (var elementToThis in elementsToThis)
            {
                elementToThis.AddElementsFromThis(this);
            }
        }
    }
}