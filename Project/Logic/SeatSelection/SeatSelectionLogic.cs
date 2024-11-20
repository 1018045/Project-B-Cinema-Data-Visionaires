using static Project.Helpers.SeatSelectionHelpers;
using static Project.Logic.SeatSelection.GridNavigator;
using static Project.Presentation.SeatingPresentation;

namespace Project.Logic.SeatSelection;

public class SeatSelectionLogic
{
    public GridNavigator GridGenerator { get; }
    public LayoutGenerator LayoutGenerator { get; }

    private readonly int _seatCount;
    private readonly RoomModel _room;
    private readonly List<Position> _selectedSeats = [];

    public SeatSelectionLogic(int showingId, int seatCount)
    {
        var room = GetRoomByShowing(showingId);

        _room = room;
        _seatCount = seatCount;
        GridGenerator = new GridNavigator(room.SeatDepth, room.Rows);
        LayoutGenerator = new LayoutGenerator(room.Id,  showingId, GridGenerator, ref _selectedSeats);
    }

    public List<string> StartSeatSelection()
    {
        GridGenerator.SelectAction = ActionMethod;
        GridGenerator.MoveAction = _ => UpdateSeatingPresentation(LayoutGenerator.GenerateSeatingLayout());
        GridGenerator.Start();
        return PositionsToStrings(_selectedSeats);
    }

    //return true if the WHOLE selection is done
    private bool ActionMethod(GridNavigator navigator)
    {
        var curPos = navigator.Cursor;
        if (_selectedSeats.Contains(curPos))
            return false;

        _selectedSeats.Add(curPos with {});

        if (_selectedSeats.Count < _seatCount)
            return false;

        Console.Clear();
        return true;
    }
}


