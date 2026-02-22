using DopamineXP.Components.Pages;
using DopamineXP.Models;

namespace DopamineXP.Services;
using System.Text.Json;
using System.IO;

public class TrackerBrain
{
    public double ProgrammingXp { get; set; } = 0;
    public int ProgrammingLevel { get; set; } = 1;
    public int ProgrammingStreak { get; set; } = 1;
    public int ProgrammingPoints { get; set; } = 0;
    public DateTime LastProgrammingLog { get; set; } = DateTime.MinValue;
    
    public double GymXp { get; set; } = 0;
    public int GymLevel { get; set; } = 1;
    public int GymStreak { get; set; } = 1;
    public int GymPoints { get; set; } = 0;
    public DateTime LastGymLog { get; set; } = DateTime.MinValue;
    
    public double LanguagesXp { get; set; } = 0;
    public int LanguagesLevel { get; set; } = 1;
    public int LanguagesStreak { get; set; } = 1;
    public int LanguagesPoints { get; set; } = 0;
    public DateTime LastLanguagesLog { get; set; } = DateTime.MinValue;

    public List<DopamineTask> CustomTasks { get; set; } = new();
    public Action? OnChange;
    
    public void AddTask(DopamineTask task)
    {
        CustomTasks.Add(task);
        OnChange?.Invoke();
    }
    
    
    private string appFile;

    public TrackerBrain()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string folderPath = Path.Combine(documentsPath, "DopamineXP");
        Directory.CreateDirectory(folderPath);
        
        appFile = Path.Combine(folderPath, "DopamineXPData.json");
        
    }
    
    private string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public void SaveToFile()
    {
        File.WriteAllText(appFile, ToJson());
    }

    public void LoadFromFile()
    {
        if (!File.Exists(appFile))
            return;
        string jsonString = File.ReadAllText(appFile);

        TrackerBrain loadedBrain = JsonSerializer.Deserialize<TrackerBrain>(jsonString);

        this.ProgrammingXp = loadedBrain.ProgrammingXp;
        this.ProgrammingLevel = loadedBrain.ProgrammingLevel;
        this.ProgrammingStreak = loadedBrain.ProgrammingStreak;
        this.ProgrammingPoints = loadedBrain.ProgrammingPoints;
        this.LastProgrammingLog = loadedBrain.LastProgrammingLog;
        
        this.GymXp = loadedBrain.GymXp;
        this.GymLevel = loadedBrain.GymLevel;
        this.GymStreak = loadedBrain.GymStreak;
        this.GymPoints = loadedBrain.GymPoints;
        this.LastGymLog = loadedBrain.LastGymLog;
        
        this.LanguagesXp = loadedBrain.LanguagesXp;
        this.LanguagesLevel = loadedBrain.LanguagesLevel;
        this.LanguagesStreak = loadedBrain.LanguagesStreak;
        this.LanguagesPoints = loadedBrain.LanguagesPoints;
        this.LastLanguagesLog = loadedBrain.LastLanguagesLog;
    }

    public int CalculateNewStreak(int currentStreak, DateTime lastLogDate)
    {
        DateTime today = DateTime.Today;

        if (lastLogDate.Date == today)
            return currentStreak;
        if (lastLogDate.Date == today.AddDays(-1))
            return currentStreak + 1;
        return 1;
    }
}