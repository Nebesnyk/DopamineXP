namespace DopamineXP.Models;

public class DopamineTask
{
    public string Name { get; set; } = "New Task";
    public string HexColor { get; set; } = "#009dff";

    public TaskStats Stats { get; set; } = new();
    public TaskHabit Habit { get; set; } = new();
    public TaskEconomy Shop { get; set; } = new();
    public TaskLaboratory Lab { get; set; } = new();
    
    
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

    public void SoftReset()
    {
        Stats.Level = 1;
        Stats.XP = 0;
        
        Shop.Points = 0;
        Shop.MultiplierPrice = 3;
        Shop.StreakFreezePrice = 5;
    }
    
    public void CheckDailyReset()
    {
        if (Habit.IsHardcore && Habit.LastMinutesLoggedResetDateTime < DateTime.Today)
        {
            if (Habit.MinutesLoggedToday < 60)
            {
                SoftReset();
                Habit.Streak = 0;
            }

            Habit.MinutesLoggedToday = 0;
            Habit.LastMinutesLoggedResetDateTime = DateTime.Today;
        }
    }
    
    public void LogMinutes(int minutes)
    {
        Habit.MinutesLoggedToday += minutes;
        
        double shopBuff = DateTime.Now <= Shop.MultiplierExpiration ? Shop.Multiplier : 1.0;
        double streakBuff = Habit.Streak * 0.5 + 1;
        double prestigeBuff = Stats.PrestigeCount + 1;

        Stats.XP += (int)(2 * minutes * streakBuff) * shopBuff * prestigeBuff;
    }
    
    public void LevelUp()
    {
        while (Stats.XP >= Stats.Threshold) 
        {
            Stats.XP -= Stats.Threshold;
            Shop.Points += Stats.Level * Lab.SynthesizerMultiplier;
            Stats.Level++;
        }
    }

    public void ApplyPrestige()
    {
        Lab.Cores += Stats.Level - 10 + 1;
        
        Stats.PrestigeCount++;
        
        if (Stats.PrestigeCount == 1)
            Lab.LastShardUpdate = DateTime.Now;
        
        SoftReset();
    }

    public string FormatNumber(int number)
    {
        if (number < 1000) return number.ToString(); 
    
        return number.ToString("0.0e0");

    }
    
    public string FormatNumber(double number)
    {
        if (number < 1000) return Math.Floor(number).ToString(); 
    
        return number.ToString("0.0e0");

    }
    
    public void TryBuyOverclocker()
    {
        if (Lab.Shards < Lab.OverclockerPrice || Lab.OverclockerLevel == 5) return;

        Lab.Shards -= Lab.OverclockerPrice;
        Lab.OverclockerPrice *= 10;
        Lab.OverclockerLevel++;
        
        Shop.Multiplier++;
    }
    
    public void TryBuySynthesizer()
    {
        if (Lab.Shards < Lab.SynthesizerPrice || Lab.SynthesizerLevel == 4) return;

        Lab.Shards -= Lab.SynthesizerPrice;
        Lab.SynthesizerPrice *= 10;
        Lab.SynthesizerLevel++;

        Lab.SynthesizerMultiplier += 1;

    }
    
    public void TryBuyResonator()
    {
        if (Lab.Shards < Lab.ResonatorPrice || Lab.ResonatorLevel == 5) return;

        Lab.Shards -= Lab.ResonatorPrice;
        Lab.ResonatorPrice *= 10;
        Lab.ResonatorLevel++;

        Lab.ResonatorMultiplier += 1;
    }

    public void TryBuyFountain()
    {
        if (Lab.Shards < Lab.FountainPrice || Lab.IsFountainBought) return;

        Lab.IsFountainBought = true;
    }
}

public class TaskStats
{
    public double XP { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int PrestigeCount { get; set; } = 0;
    public int Threshold => (int)(Math.Pow(1.2, Level) * 20);
}

public class TaskHabit
{
    public bool IsHardcore { get; set; } = true;
    public int Streak { get; set; } = 0;
    public double MinutesLoggedToday { get; set; } = 0;
    public string DailyStreakMessage { get; set; } = "";
    
    public DateTime LastStreakEarnedDateTime { get; set; } = DateTime.MinValue;
    public DateTime LastMinutesLoggedResetDateTime { get; set; } = DateTime.Today;
}

public class TaskEconomy
{
    public int Points { get; set; } = 0;
    
    public bool HasFreeze { get; set; } = false;
    public int Multiplier = 2;
    public DateTime MultiplierExpiration { get; set; } = DateTime.MinValue;
    
    public int MultiplierPrice { get; set; } = 3;
    public int StreakFreezePrice { get; set; } = 5;
    
    public bool CanBuyMultiplier()
    {
        if (Points >= MultiplierPrice)
        {
            Points -= MultiplierPrice;
            MultiplierPrice *= Multiplier;
            MultiplierExpiration = DateTime.Now.AddHours(24);
            return true;
        }

        return false;
    }

    public bool CanBuyStreakFreeze()
    {
        if (Points >= StreakFreezePrice)
        {
            Points -= StreakFreezePrice;
            StreakFreezePrice *= 2;
            HasFreeze = true;
            return true;
        }
        return false;
    }
}

public class TaskLaboratory
{
    public int Cores { get; set; } = 0 ;
    public double Shards { get; set; } = 111111110;
    public int ShardGenerators { get; set; } = 0;
    public DateTime LastShardUpdate { get; set; } = DateTime.Now;

    public int OverclockerPrice { get; set; } = 100;
    public int SynthesizerPrice { get; set; } = 150;
    public int ResonatorPrice { get; set; } = 250;
    public int FountainPrice { get; set; } = 1_000_000;
    
    public int OverclockerLevel { get; set; } = 1;
    public int SynthesizerLevel { get; set; } = 0;
    public int ResonatorLevel { get; set; } = 0;
    public bool IsFountainBought { get; set; } = false;

    public int SynthesizerMultiplier = 1;
    public int ResonatorMultiplier = 1;


}