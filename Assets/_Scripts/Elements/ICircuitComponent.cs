using UnityEngine;

namespace _Scripts.Elements
{
    public interface ICircuitComponent
    {
        public Transform GetInputPoint();
        public Transform GetOutputPoint();
        
        public string GetInNode();
        public string GetOutNode();
    }
}