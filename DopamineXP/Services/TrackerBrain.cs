using DopamineXP.Components.Pages;

namespace DopamineXP.Services;
using System.Text.Json;
using System.IO;

public class TrackerBrain
{
    public double ProgrammingXp { get; set; } = 0;
    public int ProgrammingLevel { get; set; } = 1;
    public int ProgrammingStreak { get; set; } = 0;
    
    public double GymXp { get; set; } = 0;
    public int GymLevel { get; set; } = 1;
    public int GymStreak { get; set; } = 0;
    
    public double LanguagesXp { get; set; } = 0;
    public int LanguagesLevel { get; set; } = 1;
    public int LanguagesStreak { get; set; } = 0;

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
        
        this.GymXp = loadedBrain.GymXp;
        this.GymLevel = loadedBrain.GymLevel;
        this.GymStreak = loadedBrain.GymStreak;
        
        this.LanguagesXp = loadedBrain.LanguagesXp;
        this.LanguagesLevel = loadedBrain.LanguagesLevel;
        this.LanguagesStreak = loadedBrain.LanguagesStreak;
    }
}