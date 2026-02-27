using DopamineXP.Components.Pages;
using DopamineXP.Models;

namespace DopamineXP.Services;
using System.Text.Json;
using System.IO;

public class TrackerBrain
{
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
        
        LoadFromFile();
        
        
    }
    
    private string ToJson()
    {
        return JsonSerializer.Serialize(CustomTasks, new JsonSerializerOptions{WriteIndented = true});
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

        List<DopamineTask> loadedBrain = JsonSerializer.Deserialize<List<DopamineTask>>(jsonString);

        if (loadedBrain != null)
        {
            this.CustomTasks = loadedBrain;
            
            foreach (var task in CustomTasks)
            {
                if (task.Shop == null) task.Shop = new TaskEconomy(); 
                if (task.Habit == null) task.Habit = new TaskHabit();
                if (task.Fountains == null) task.Fountains = new TaskFountains();
                if (task.Lab == null) task.Lab = new TaskLaboratory();
                if (task.Stats == null) task.Stats = new TaskStats();
            }
        }
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