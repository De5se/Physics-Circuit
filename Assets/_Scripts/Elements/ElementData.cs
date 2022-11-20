using UnityEngine;

namespace Elements
{
    [System.Serializable]
    public class ElementData
    {
        [SerializeField] private string elementName;

        public string ElementName => elementName;
    }
}