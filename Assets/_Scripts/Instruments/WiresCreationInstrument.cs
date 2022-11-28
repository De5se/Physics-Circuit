using _Scripts.UI;
using Elements;

public class WiresCreationInstrument : CreationInstrument
{
    private ElementController _element1;
    private ElementController _element2;
    protected override void Create()
    {
        
    }

    protected override void ToggleListener(bool isOn)
    {
        base.ToggleListener(isOn);
        if (isOn) return;
        // clear data
        _element1 = null;
        _element2 = null;
    }
}