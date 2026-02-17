using ITSupportTicketManager;
using System.Diagnostics;
using System.Linq.Expressions;

public class Program
{

    public static void Main()
    {
        //Create a Ticket Manager
        var manager = new TicketManager();

        //Display Header
        Console.WriteLine("===== IT Support Ticket Manager =====");

        //While Loop to control input
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add Ticket");
            Console.WriteLine("2. Remove Ticket");
            Console.WriteLine("3. Display All Tickets");
            Console.WriteLine("4. Close Ticket");
            Console.WriteLine("5. Reopen Ticket");
            Console.WriteLine("6. Load Tickets to file");
            Console.WriteLine("7. Save Tickets to File");
            Console.WriteLine("8. Show Open Ticket Count");
            Console.WriteLine("9. Exit");
            Console.Write("Choose: ");
            string? choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1": AddTicketMenu(manager); break;
                case "2": RemoveTicketMenu(manager); break;
                case "3": manager.DisplayAllTickets(); break;
                case "4": ChangeStatusMenu(manager, close: true); break;
                case "5": ChangeStatusMenu(manager, close: false); break;
                case "6": LoadMenu(manager); break;
                case "7": SaveMenu(manager); break;
                case "8":
                    Console.WriteLine($"Open / In Progress Tickets: {manager.GetOpenCount()}");
                    break;
                case "9": running = false; break;
                default: Console.WriteLine("Invalid Option."); break;
            }
        }

        //Wish the user a goodbye
        Console.WriteLine("Goodbye!");
    }
    private static void AddTicketMenu(TicketManager manager)
    {
        Console.WriteLine("Enter Ticket ID (e.g. T1001): ");
        string id = Console.ReadLine() ?? "";

        Console.Write("Enter Description: ");
        string desc = Console.ReadLine() ?? "";

        Console.Write("Enter Priority (Low/Medium/High): ");
        string priority = NormalizeCase(Console.ReadLine());

        Console.Write("Enter Status (Open/In Progress/Closed)");
        string status = NormalizeCase(Console.ReadLine());

        var t = new Ticket(id, desc, priority, status);
        manager.AddTicket(t);
        Console.WriteLine("Ticket Added.");
    }

    private static void RemoveTicketMenu(TicketManager manager)
    {
        Console.Write("Enter Ticket ID to remove: ");
        string id = Console.ReadLine() ?? "";
        Console.WriteLine(manager.RemoveTicket(id) ? "Removed." : "Not Found");
    }

    private static void ChangeStatusMenu(TicketManager manager, bool close)
    {
        Console.Write($"Enter Ticket ID to {(close ? "close" : "reopen")}: ");
        string id = Console.ReadLine() ?? "";
        var t = manager.FindTicket(id);
        if (t is null) { Console.WriteLine("Not Found."); return; }

        if (close) t.CloseTicket(); else t.ReopenTicket();
        Console.WriteLine("Status Updated.");
    }

    private static void SaveMenu(TicketManager manager)
    {
        Console.Write("Enter a path to save the CSV (e.g. tickets.csv): ");
        string path = Console.ReadLine() ?? "";
        try
        {
            manager.SaveTickets(path);
            Console.WriteLine($"Saved to {Path.GetFullPath(path)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Save failed: {ex.Message}");
        }
    }

    private static void LoadMenu(TicketManager manager)
    {
        Console.Write("Enter path to load CSV: ");
        string path = Console.ReadLine() ?? "";
        try
        {
            manager.LoadTickets(path);

        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Load failed: {ex.Message}");
        }
    }

    private static string NormalizeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        var s = input.Trim().ToLowerInvariant();
        if (s == "low") return "Low";
        if (s == "medium" || s == "med") return "Medium";
        if (s == "high") return "High";
        if (s == "open") return "Open";
        if (s == "in progress" || s == "in-progress" || s == "progress") return "In Progress";
        if (s == "closed" || s == "close") return "Closed";
        return input.Trim(); //Leave as is, let validation handle the rest
    }
}