using System.Collections.Generic;
using _Scripts.Elements;
using _Scripts.UI;
using Elements;

public class CircuitSimulator : Singleton<CircuitSimulator>
{
    private readonly List<CircuitElement> _elements = new();
    
    private CircuitElement _source;
    
    public void AddElement(CircuitElement element)
    {
        if (element.IsSourceElement)
        {
            _source = element;
        }
        _elements.Add(element);
    }

    public void RemoveElement(CircuitElement element)
    {
        _elements.Remove(element);
        UpdateCircuit();
    }

    public void UpdateCircuit()
    {
        ClearValues();
        if (_source != null)
        {
            bool isSmthChanged = _source.UpdateValues(false);
            while (isSmthChanged)
            {
                isSmthChanged = _source.UpdateValues(false);
            }
        }
        
        ElementSettings.Instance.UpdateSettingsValues();
    }

    public void ClearValues()
    {
        foreach (var element in _elements)
        {
            element.ClearValues();
        }
    }
}
