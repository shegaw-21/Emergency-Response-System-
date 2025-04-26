// NAME: SHEGAW AFELE
// ID: DBU1501469
// Username: admin
// Password: 1234

using System;
using System.Collections.Generic;
using System.Linq;

// Abstract Emergency Unit
abstract class EmergencyUnit
{
    public string Name { get; set; }
    public int Speed { get; set; }

    public EmergencyUnit(string name, int speed)
    {
        Name = name;
        Speed = speed;
    }

    public abstract bool CanHandle(string incidentType);
    public abstract void RespondToIncident(Incident incident, int responseTime);
}

// Unit Types
class Police : EmergencyUnit
{
    public Police(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType.ToLower() == "crime";
    public override void RespondToIncident(Incident incident, int responseTime)
    {
        Console.WriteLine($"{Name} is handling a crime at {incident.Location} (Response time: {responseTime}ms).");
    }
}

class Firefighter : EmergencyUnit
{
    public Firefighter(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType.ToLower() == "fire";
    public override void RespondToIncident(Incident incident, int responseTime)
    {
        Console.WriteLine($"{Name} is extinguishing a fire at {incident.Location} (Response time: {responseTime}ms).");
    }
}

class Ambulance : EmergencyUnit
{
    public Ambulance(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType.ToLower() == "medical";
    public override void RespondToIncident(Incident incident, int responseTime)
    {
        Console.WriteLine($"{Name} is treating patients at {incident.Location} (Response time: {responseTime}ms).");
    }
}

class RescueTeam : EmergencyUnit
{
    public RescueTeam(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType.ToLower() == "rescue";
    public override void RespondToIncident(Incident incident, int responseTime)
    {
        Console.WriteLine($"{Name} is rescuing people at {incident.Location} (Response time: {responseTime}ms).");
    }
}

class Hazmat : EmergencyUnit
{
    public Hazmat(string name, int speed) : base(name, speed) { }
    public override bool CanHandle(string incidentType) => incidentType.ToLower() == "chemical";
    public override void RespondToIncident(Incident incident, int responseTime)
    {
        Console.WriteLine($"{Name} is handling hazardous material at {incident.Location} (Response time: {responseTime}ms).");
    }
}

// Incident class
class Incident
{
    public string Type { get; set; }
    public string Location { get; set; }
    public int Difficulty { get; set; }

    public Incident(string type, string location, int difficulty)
    {
        Type = type.ToLower(); // Force incident type to lowercase
        Location = location;
        Difficulty = difficulty;
    }
}

class Program
{
    static List<EmergencyUnit> units = new List<EmergencyUnit>
    {
        new Police("Police Unit 1", 80),
        new Firefighter("Firefighter Unit 1", 70),
        new Ambulance("Ambulance Unit 1", 90),
        new RescueTeam("Rescue Team 1", 60),
        new Hazmat("Hazmat Unit 1", 50)
    };

    static string[] incidentTypes = { "fire", "crime", "medical", "rescue", "chemical" };
    static string[] locations = { "Downtown", "City Hall", "Mall", "Airport", "Suburbs", "Harbor" };
    static Random random = new Random();
    static int score = 0;

    static void Main()
    {
        Console.WriteLine("==== Emergency Response System ====");

        Login(); // Always login successfully, keeps retrying

        while (true)
        {
            Console.WriteLine("\nChoose mode:");
            Console.WriteLine("1. Automatic Mode (simulate incidents)");
            Console.WriteLine("2. Manual Mode (enter incidents manually)");
            Console.WriteLine("3. Exit");
            Console.Write("Select mode: ");
            string mode = Console.ReadLine();

            if (mode == "1")
            {
                SimulateIncidents();
            }
            else if (mode == "2")
            {
                ManualIncidentLoop();
            }
            else if (mode == "3")
            {
                Console.WriteLine("\nExiting system. Final Score: " + score);
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }

    static void Login()
    {
        while (true)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (username == "admin" && password == "1234")
            {
                Console.WriteLine("Login successful!");
                return;
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.\n");
            }
        }
    }

    static void SimulateIncidents()
    {
        for (int round = 1; round <= 5; round++)
        {
            Console.WriteLine($"\n--- Turn {round} ---");
            string type = incidentTypes[random.Next(incidentTypes.Length)];
            string location = locations[random.Next(locations.Length)];
            int difficulty = random.Next(1, 6);

            Incident incident = new Incident(type, location, difficulty);
            HandleIncidentAuto(incident);
        }
        Console.WriteLine("\nSimulation finished. Current Score: " + score);
    }

    static void ManualIncidentLoop()
    {
        while (true)
        {
            ManualIncident();
            Console.Write("\nDo you want to report another incident? Enter 'y' for yes and 'n' for no: ");
            string again = Console.ReadLine();
            if (again.ToLower() != "y")
            {
                Console.WriteLine("Returning to main menu...");
                break;
            }
        }
    }

    static void ManualIncident()
    {
        Console.Write("\nEnter Incident Type (fire/crime/medical/rescue/chemical): ");
        string type = Console.ReadLine().ToLower();

        if (!incidentTypes.Contains(type))
        {
            Console.WriteLine("Invalid incident type.");
            return;
        }

        Console.Write("Enter Location: ");
        string location = Console.ReadLine();

        Console.Write("Enter Difficulty (1-5): ");
        if (!int.TryParse(Console.ReadLine(), out int difficulty) || difficulty < 1 || difficulty > 5)
        {
            Console.WriteLine("Invalid difficulty.");
            return;
        }

        Incident incident = new Incident(type, location, difficulty);

        var availableUnits = units.Where(u => u.CanHandle(type)).ToList();
        if (availableUnits.Count == 0)
        {
            Console.WriteLine("No available unit for this type of incident.");
            score -= 5;
            return;
        }

        Console.WriteLine("Select a unit to dispatch:");
        for (int i = 0; i < availableUnits.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {availableUnits[i].Name} (Speed: {availableUnits[i].Speed})");
        }

        if (int.TryParse(Console.ReadLine(), out int selection) &&
            selection > 0 && selection <= availableUnits.Count)
        {
            var unit = availableUnits[selection - 1];
            int responseTime = CalculateResponseTime(unit.Speed, incident.Difficulty);
            unit.RespondToIncident(incident, responseTime);
            UpdateScore(responseTime, incident.Difficulty);
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    static void HandleIncidentAuto(Incident incident)
    {
        Console.WriteLine($"Incident: {incident.Type} at {incident.Location} (Difficulty: {incident.Difficulty})");

        foreach (var unit in units)
        {
            if (unit.CanHandle(incident.Type))
            {
                int responseTime = CalculateResponseTime(unit.Speed, incident.Difficulty);
                unit.RespondToIncident(incident, responseTime);
                UpdateScore(responseTime, incident.Difficulty);
                return;
            }
        }

        Console.WriteLine("No unit available to handle this incident.");
        score -= 5;
    }

    static int CalculateResponseTime(int speed, int difficulty)
    {
        return (int)(difficulty * 100.0 / speed);
    }

    static void UpdateScore(int responseTime, int difficulty)
    {
        int basePoints = 10;
        int timePenalty = responseTime;
        int difficultyBonus = difficulty * 5;
        int roundPoints = basePoints + difficultyBonus - timePenalty;

        roundPoints = Math.Max(roundPoints, 0);
        score += roundPoints;

        Console.WriteLine($"Points earned: {roundPoints} | Current Score: {score}");
    }
}
