using Project.Logic.SeatSelection;
using static Project.Helpers.SeatSelectionHelpers;

namespace Project.Presentation;

public class SeatingPresentation
{
    public static List<string> Present(int showingId)
    {
        var seatCount = -1;
        for (var resolved = false; resolved == false;)
        {
            Console.WriteLine("How many seats would you like to book?");
            var seatAmountInput = Console.ReadLine() ?? "";
            resolved = int.TryParse(seatAmountInput, out seatCount)
                       && CanFitAdjacentSeats(seatCount, GetTakenSeats(showingId), GenerateSeatingLayoutContent(showingId));
        }

        if (seatCount == -1)
            return [];

        var logic = new SeatSelectionLogic(showingId, seatCount);
        return logic.StartSeatSelection();
    }

    //used to constantly refresh the seating
    public static void UpdateSeatingPresentation(string presentation, int seatsLeft)
    {
        Console.Clear();

        Console.WriteLine("Select your seat using the arrow keys (< ^ > v) and use the enter key to lock in your seat(s) (your choice is final)");
        Console.WriteLine("The seats you select must be adjacent on the same row without gaps");
        Console.WriteLine($"You need to select {seatsLeft} more seat(s)");
        Console.WriteLine();

        Console.WriteLine(presentation);
    }

    public static bool SuccessfulSelection(string seatDisplay)
    {
        // Commented out this code so the confirmation only gets printed in the confirm menu
        
        // Console.Clear();
        // Console.WriteLine($"You've selected the following seats: {seatDisplay}");
        // Thread.Sleep(5000);

        return true;
    }

    public static bool NotAdjacentResult()
    {
        Console.Clear();
        Console.WriteLine("You can only select seats that are next to each other");
        Thread.Sleep(2000);
        return false;
    }

    public static bool AlreadyTakenResult()
    {
        Console.Clear();
        Console.WriteLine("This seat is already taken (TIP: [x] means taken)");
        Thread.Sleep(2000);
        return false;
    }
}