using System;

public class CellSelectRequest
{

    private Func<Cell, BaseObject> onSelectCell;
    private Action onCancel;

	public CellSelectRequest(Func<Cell, BaseObject> onSelectCell, Action onCancel)
	{
		this.onSelectCell = onSelectCell;
		this.onCancel = onCancel;
	}

	public BaseObject SelectCell(Cell cell)
    {
        return onSelectCell(cell);
    }
    
    public void Cancel()
    {
        onCancel();
    }

}
