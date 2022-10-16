namespace WebApplication2;

public class DiceGenerator
{
    public DiceGenerator()
    {
        NumberToCount = InitNumberToCount();
        Dices = InitDices();
    }

    public DiceResponse GetDiceResponse()
    {
        var r = rng.Next(6);
        (int, int) dice;
        if (r == 0)
        {
            var red = rng.Next(1, 7);
            dice = (red, 7 - red);
        }
        else
        {
            dice = GetDice();
        }
        
        var (eventCube, warriorInfo) = GetEventAndWarriorInfo();
        return new DiceResponse(eventCube, dice.Item1 + dice.Item2, dice.Item1, NumberToCount, warriorInfo);
    }

    private (string Event, string WarriorInfo) GetEventAndWarriorInfo()
    {
        var r = rng.Next(0, 6);
        var warriorInfo = $"До варваров осталось {WarriorConst - WarriorCount % WarriorConst} приходов.";
        switch (r)
        {
            case 5: return (EventCube.Blue, warriorInfo);
            case 4: return (EventCube.Yellow, warriorInfo);
            case 3: return (EventCube.Green, warriorInfo);
            default:
            {
                WarriorCount++;
                if (WarriorCount % WarriorConst == 0)
                    warriorInfo = "Варвары пришли!";
                else
                    warriorInfo = $"До варваров осталось {WarriorConst - WarriorCount % WarriorConst} приходов.";
                return (EventCube.Warrior, warriorInfo);
            }
        }
    }

    private (int, int) GetDice()
    {
        if (Dices.Count == 0)
        {
            Dices = InitDices();
            NumberToCount = InitNumberToCount();
        }
        
        var next = rng.Next(Dices.Count);
        var first = Dices[next].Item1; 
        var second = Dices[next].Item2;
        Dices.RemoveAt(next);
        NumberToCount[first + second]--;
        
        return (first, second);
    }

    private const int WarriorConst = 7;
    private int WarriorCount = 0;
    private Random rng = new Random();
    private List<(int, int)> Dices;
    private Dictionary<int, int> NumberToCount = new Dictionary<int, int>();
    private const int pul = 3;

    private List<(int, int)> InitDices()
    {
        var a = new List<(int, int)>();
        for (int i = 0; i < pul; i++)
        {
            for (int j = 1; j < 7; j++)
            {
                for (int k = 1; k < 7; k++)
                {
                    if (j + k != 7)
                        a.Add((j, k));
                }
            }
        }

        return a;
    }

    private Dictionary<int, int> InitNumberToCount()
    {
        var result = new Dictionary<int, int>();
        var array = new List<int> {1, 2, 3, 4, 5};
        array = array.Select(x => x * pul).ToList();
        
        for (int i = 0; i < array.Count; i++)
        {
            result[i + 2] = array[i];
            result[12 - i] = array[i];
        }

        return result;
    }
}