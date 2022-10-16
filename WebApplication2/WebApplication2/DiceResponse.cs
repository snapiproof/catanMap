namespace WebApplication2;

public class DiceResponse
{
    public int Dice { get; }
    
    public string EventCube { get; }
    
    public int RedCube { get; }
    
    public Dictionary<int, int> NumberToCount { get; }
    
    public string InfoAboutWarriors { get; }

    public DiceResponse(string eventCube, int dice, int redCube, Dictionary<int, int> numberToCount, string infoAboutWarriors)
    {
        EventCube = eventCube;
        Dice = dice;
        RedCube = redCube;
        NumberToCount = numberToCount;
        InfoAboutWarriors = infoAboutWarriors;
    }
}