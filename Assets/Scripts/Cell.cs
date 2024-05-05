public class Cell
{
    private int _xPosition;
    private int _yPosition;
    private string _type;
    private bool _filled;
    private bool _usable;

    public int XPosition => _xPosition;
    public int YPosition => _yPosition;
    
    public bool Usable
    {
        get => _usable;
        set => _usable = value;
    }

    public Cell(int xPosition, int yPosition)
    {
        this._xPosition = xPosition;
        this._yPosition = yPosition;
    }
    
        
}
