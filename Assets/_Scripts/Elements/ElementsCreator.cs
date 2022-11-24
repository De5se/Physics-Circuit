using Elements;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Elements
{
    public class ElementsCreator : Singleton<ElementsCreator>
    {
        public void CreateElement(ElementController targetElement, Vector2 creationPosition)
        {
            Instantiate(targetElement, creationPosition, quaternion.identity, transform);
        }
    }
}