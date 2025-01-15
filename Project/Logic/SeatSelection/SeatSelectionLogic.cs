using static Project.Helpers.SeatSelectionHelpers;
using static Project.Logic.SeatSelection.GridNavigator;
using static Project.Presentation.SeatingPresentation;

namespace Project.Logic.SeatSelection;

public class SeatSelectionLogic
{
    public GridNavigator GridGenerator { get; }
    public LayoutGenerator LayoutGenerator { get; }

    private readonly int _seatCount;
    private readonly List<Seat> _selectedSeats = [];
    private readonly List<Position> _takenSeats;
    private readonly int _showingId;
    private LogicManager _logicManager;

    public SeatSelectionLogic(int showingId, int seatCount, LogicManager logicManager)
    {
        _showingId = showingId;
        var room = GetRoomByShowing(showingId);
        _seatCount = seatCount;
        _takenSeats = GetTakenSeats(showingId);
        GridGenerator = new GridNavigator();
        LayoutGenerator = new LayoutGenerator(room.Id,  showingId, GridGenerator, ref _selectedSeats);
        _logicManager = logicManager;
    }

    public List<Seat> StartSeatSelection()
    {
        var layout = CinemaLayoutParser.GetSeatingLayoutMap(GetRoomByShowing(_showingId).Id);
        var allowedSeats = CinemaLayoutParser.GenerateNonDecoyPositions(layout);
        var firstPosAvailable = CinemaLayoutParser.GetFirstNonDecoyPosition(layout);

        GridGenerator.X = firstPosAvailable.X;
        GridGenerator.Y = firstPosAvailable.Y;

        GridGenerator.SelectAction = (nav) => ActionMethod(nav, _logicManager.ShowingsLogic.FindShowingByIdReturnShowing(_showingId).Room);
        GridGenerator.MovePredicate = futurePos => allowedSeats.Contains(futurePos);
        GridGenerator.RefreshAction = _ =>
            UpdateSeatingPresentation(LayoutGenerator.BuildSeatingLayoutV3, layout, _seatCount - _selectedSeats.Count);
        GridGenerator.ConfirmationAction = nav =>
        {
            var curPos = nav.Cursor;
            if (_selectedSeats.Any(s => s.Position == curPos) || _takenSeats.Contains(curPos))
            {
                NotAdjacentResult();
                return false;
            }

            if (!IsAdjacentOnSameRow(curPos, _selectedSeats.Select(s => s.Position).ToList()) && _selectedSeats.Count != 0)
            {
                AlreadyTakenResult();
                return false;
            }

            var seat = layout.SelectMany(row => row).FirstOrDefault(seat => seat.Position.Equals(nav.Cursor));
            if (seat == null)
                return false;
            seat.IsReserved = true;
            return true;
        };

        AddTakenSeatsToLayout(layout);

        GridGenerator.Start();

        return _selectedSeats;

        /*var seatSelector = new SeatSelector();
        seatSelector.Start(_showingId);
        return [];*/
    }

    private void AddTakenSeatsToLayout(List<List<Seat>> seats)
    {
        var taken = GetTakenSeats(_showingId);
        var allSeats = seats.SelectMany(r => r).ToList();
        foreach (var pos in taken)
        {
            allSeats.Find(s => s.Position.Equals(pos))!.IsTaken = true;
        }
    }

    //return true if the WHOLE selection is done
    private Func<bool> ActionMethod(GridNavigator navigator, int roomId)
    {
        var curPos = navigator.Cursor;
        _selectedSeats.Add(CinemaLayoutParser.GetSeatByPosition(curPos with {}, roomId)); //deep copy

        if (_selectedSeats.Count < _seatCount)
            return () => false;

        Console.Clear();
        return () => SuccessfulSelection(PositionsToRowSeatString(_selectedSeats.Select(s => s.Position).ToList()));
    }
}


