using static Project.Logic.SeatSelection.GridNavigator;

namespace Project.Helpers;

public static class SeatSelectionHelpers
{
    private static readonly List<char> Alphabet = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
    public static RoomModel GetRoom(int roomId)
    {
        var data = RoomAccess.LoadAll();
        var room = data.Find(rm => rm.Id == roomId);

        //Exception that should only be called during development when the data isn't correctly linked
        if (room == null)
            throw new ArgumentException("Logic error: The roomId was not found");

        return room;
    }

    public static RoomModel GetRoomByShowing(int showingId)
    {
        var showing = ShowingsAccess.LoadAll().Find(s => s.Id == showingId);

        if (showing == null)
            throw new ArgumentException($"Logic error: Showing with id {showingId} not found");

        return GetRoom(showing.Room);
    }

    public static List<Position> GetTakenSeats(int showingId)
    {
        // Load all reservations and filter for the relevant ones
        var relevantReservations = ReservationsAccess.LoadAll()
            .FindAll(rm => rm.ShowingId == showingId);

        // Transform the seats into a list of Position objects
        var takenSeats = relevantReservations
            .SelectMany(rm => rm.Seats.Split(",")) // Split seat strings by ","
            .Select(seat =>
            {
                // Remove parentheses and split into coordinates
                var coordinates = seat.Trim('(', ')').Split(';');
                return new Position(int.Parse(coordinates[1]) - 1, int.Parse(coordinates[0]) - 1);
            })
            .ToList();

        return takenSeats;
    }

    public static List<string> PositionsToStrings(List<Position> positions)
    {
        return positions.Select(p => $"{p.Y + 1};{p.X + 1}").ToList();
    }

    public static List<Position> StringToPositions(List<string> seats)
    {
        List<Position> positions = new();
        foreach (string seat in seats)
        {
            positions.Add(new Position(Convert.ToInt32(seat.Split(';')[1])-1, Convert.ToInt32(seat.Split(';')[0])-1));
        }
        return positions;
    }

    public static string PositionsToRowSeatString(List<Position> positions)
    {
        return string.Join(", ", positions.Select(p => $"row: {Alphabet[p.Y]}, seat: {p.X + 1}"));
    }

    public static bool IsAdjacentOnSameRow(Position newSeat, List<Position> selectedSeats)
    {
        return selectedSeats.Any(selectedSeat =>
            selectedSeat.Y == newSeat.Y &&
            (selectedSeat.X == newSeat.X - 1 || selectedSeat.X == newSeat.X + 1));
    }

    public static bool CanFitAdjacentSeats(int seatCount, List<Position> takenSeats, Dictionary<int, List<int>> rowSeatMap)
    {
        foreach (var rowEntry in rowSeatMap)
        {
            var rowIndex = rowEntry.Key;
            var rowSeats = rowEntry.Value;

            var takenPositionsInRow = takenSeats
                .Where(seat => seat.Y == rowIndex)
                .Select(seat => seat.X)
                .ToHashSet();

            var currentStreak = 0;

            foreach (var seat in rowSeats)
            {
                if (!takenPositionsInRow.Contains(seat))
                {
                    currentStreak++;
                    if (currentStreak == seatCount)
                    {
                        return true;
                    }
                }
                else
                {
                    currentStreak = 0;
                }
            }
        }

        return false;
    }

    public static Dictionary<int, List<int>> GenerateRowSeatMap(RoomModel room)
    {
        var rowSeatMap = new Dictionary<int, List<int>>();

        AddSeatsToMap(room.SeatCategories.High, rowSeatMap);
        AddSeatsToMap(room.SeatCategories.Medium, rowSeatMap);
        AddSeatsToMap(room.SeatCategories.Low, rowSeatMap);

        return rowSeatMap;
    }

    private static void AddSeatsToMap(SeatCategory category, Dictionary<int, List<int>> rowSeatMap)
    {
        foreach (var row in category.Rows)
        {
            if (!rowSeatMap.ContainsKey(row.RowNumber))
            {
                rowSeatMap[row.RowNumber] = new List<int>();
            }

            rowSeatMap[row.RowNumber].AddRange(row.Seats);
        }
    }

    public static void AddSeats(SeatCategory category, ref Dictionary<Position, ConsoleColor> dictionary)
    {
        var color = StringToConsoleColor(category.Color);
        foreach (var row in category.Rows)
        {
            foreach (var seat in row.Seats)
            {
                dictionary[new Position(seat, row.RowNumber)] = color;
            }
        }
    }

    public static ConsoleColor StringToConsoleColor(string colorName)
    {
        if (Enum.TryParse(colorName, true, out ConsoleColor color))
        {
            return color;
        }
        else
        {
            throw new ArgumentException($"'{colorName}' is not a valid ConsoleColor.");
        }
    }
}