namespace DopamineXP.Models;

public class DopamineTask
{
    public string Name { get; set; } = "New Task";
    public string HexColor { get; set; } = "#009dff";

    public double XP { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int Points { get; set; } = 0;
    public int Streak { get; set; } = 0;
    public DateTime MultiplierExpiration { get; set; } = DateTime.MinValue;
    public bool HasStreakFreeze { get; set; } = false;
    private int _multiplierPrice = 3;
    private int _streakFreezePrice = 5;
    
    public int MultiplierPrice
    {
        get => _multiplierPrice == 0 ? 3 : _multiplierPrice; 
        set => _multiplierPrice = value;
    }

    public int StreakFreezePrice
    {
        get => _streakFreezePrice == 0 ? 5 : _streakFreezePrice;
        set => _streakFreezePrice = value;
    }

    public DateTime LastLogged { get; set; } = DateTime.MinValue;
    public string DailyStreakMessage { get; set; } = "";
    
    
    public string GetRgba(double alpha)
    {
        string cleanHex = HexColor.Replace("#", "");
        
        int red = Convert.ToInt32(cleanHex.Substring(0, 2), 16);
        int green = Convert.ToInt32(cleanHex.Substring(2, 2), 16);
        int blue = Convert.ToInt32(cleanHex.Substring(4, 2), 16);

        return $"rgba({red}, {green}, {blue}, {alpha})";
    }
    
    public string GetDarkerHex(double factor = 0.6)
    {
        string cleanHex = HexColor.Replace("#", "");
    
        int r = Convert.ToInt32(cleanHex.Substring(0, 2), 16);
        int g = Convert.ToInt32(cleanHex.Substring(2, 2), 16);
        int b = Convert.ToInt32(cleanHex.Substring(4, 2), 16);
        
        int darkR = (int)(r * factor);
        int darkG = (int)(g * factor);
        int darkB = (int)(b * factor);

        // "x2" tells C# to convert the integer back into a 2-digit lowercase Hex string
        return $"#{darkR:x2}{darkG:x2}{darkB:x2}";
    }
}