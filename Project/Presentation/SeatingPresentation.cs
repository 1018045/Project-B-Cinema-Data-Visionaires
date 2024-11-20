using Project.Logic;
using Project.Logic.SeatSelection;

namespace Project.Presentation;

public class SeatingPresentation
{
    //returns the seats selected in the format "1,2,3"  where 1-3 are seat numbers
    public static List<string> Present(int showingId)
    {
        int seatCount = -1;
        for (bool resolved = false; resolved == false;)
        {
            Console.WriteLine("How many seats would you like to book?");
            var seatAmountInput = Console.ReadLine() ?? "";

            resolved = int.TryParse(seatAmountInput, out seatCount);
        }

        if (seatCount == -1)
            return [];

        var logic = new SeatSelectionLogic(showingId, seatCount);
        return logic.StartSeatSelection();
    }

    //used to constantly refresh the seating
    public static void UpdateSeatingPresentation(string presentation)
    {
        Console.Clear();

        Console.WriteLine("Select your seat using the arrow keys (< ^ > v) and use the enter key to lock in your seat(s) (your choice is final)");
        Console.WriteLine();

        Console.WriteLine(presentation);
    }
}