using Elements;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Elements
{
    public class ElementsCreator : Singleton<ElementsCreator>
    {
        private ConnectionPoint _previousConnectionPoint;
        
        public void CreateElement(ElementController targetElement, Vector2 creationPosition)
        {
            Instantiate(targetElement, creationPosition, quaternion.identity, transform);
        }

        public void CreateWire(ConnectionPoint connectionPoint)
        {
            if (_previousConnectionPoint == null)
            {
                _previousConnectionPoint = connectionPoint;
                connectionPoint.EnableSelectionCircle(true);
                return;
            }
            if (_previousConnectionPoint != connectionPoint)
            {
                _previousConnectionPoint.AddElementsFromThis(connectionPoint);
                connectionPoint.AddElementsToThis(_previousConnectionPoint);
            }
            _previousConnectionPoint.EnableSelectionCircle(false);
            _previousConnectionPoint = null;
        }
    }
}