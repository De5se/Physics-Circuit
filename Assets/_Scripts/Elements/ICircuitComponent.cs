using System;
using UnityEngine;

namespace _Scripts.Elements
{
    public interface ICircuitComponent
    {
        public Action DestroyAction { get; set; }
        
        public Transform GetInputPoint();
        public Transform GetOutputPoint();
        
        public string GetInNode();
        public string GetOutNode();

        public void Destroy();
    }
}