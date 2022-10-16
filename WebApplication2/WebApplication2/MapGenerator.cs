namespace WebApplication2;

public class MapGenerator
{
    public static CatanMap[] GenerateMap()
    {
        var types = GenerateTypes();
        var numbers = GenerateNumbers(types);
        
        while (!IsCorrectNumbers(numbers))
            numbers = GenerateNumbers(types);
        
        var result = new CatanMap[19];
        for (int i = 0; i < 19; i++)
        {
            result[i] = new CatanMap(types[i], numbers[i], i);
        }

        return result;
    }

    private static int[] GenerateNumbers(string[] types)
    {
        var result = new int[19];
        var numbers = new List<int> {2, 12, 3, 3, 11, 11, 4, 4, 10, 10, 5, 5, 9, 9, 6, 6, 8, 8};
        var sum = 7;
        
        var balanceRes = new Dictionary<string, int>
        {
            {Gex.Ore, 0},
            {Gex.Forest, 0},
            {Gex.Wheat, 0},
            {Gex.Sheep, 0},
            {Gex.Brick, 0},
        };

        for (int i = 0; i < 19; i++)
        {
            var type = types[i];
            if (type == Gex.Desert)
            {
                result[i] = 0;
                continue;
            }

            var next = 0;
            
            if (balanceRes[type] > sum || IsNestedTop(result, i))
                next = random.Next(numbers.Count / 2);
            else if (balanceRes[type] < sum || IsNestedBot(result, i))
                next = random.Next(numbers.Count / 2, numbers.Count);
            else
                next = random.Next(numbers.Count);
            
            var nextNumber = numbers[next];
            result[i] = nextNumber;
            balanceRes[type] += Chance[nextNumber];
            numbers.RemoveAt(next);
        }

        return result;
    }

    private static bool IsNestedTop(int[] result, int i)
    {
        foreach (var t in Nested[i])
        {
            if (result[t] == 6 || result[t] == 8)
                return true;
        }

        return false;
    }
    
    private static bool IsNestedBot(int[] result, int i)
    {
        foreach (var t in Nested[i])
        {
            if (result[t] == 2 || result[t] == 12)
                return true;
        }

        return false;
    }
    
    private static bool IsNestedSameType(string[] result, int i)
    {
        foreach (var t in Nested[i])
        {
            if (result[t] == result[i])
                return true;
        }

        return false;
    }

    private static bool IsCorrectNumbers(int[] numbers)
    {
        for (int i = 0; i < 19; i++)
        {
            foreach (var t in Nested[i])
            {
                if ((numbers[i] == 6 || numbers[i] == 8) && (numbers[t] == 6 || numbers[t] == 8))
                    return false;
            }
            
            foreach (var t in Nested[i])
            {
                if ((numbers[i] == 2 || numbers[i] == 12) && (numbers[t] == 2 || numbers[t] == 12))
                    return false;
            }
        }

        return true;
    }
    
    private static string[] GenerateTypes()
    {
        var init = new List<string>
        {
            Gex.Ore, Gex.Ore, Gex.Ore,
            Gex.Forest, Gex.Forest, Gex.Forest, Gex.Forest,
            Gex.Wheat, Gex.Wheat, Gex.Wheat, Gex.Wheat, Gex.Desert,
            Gex.Sheep, Gex.Sheep, Gex.Sheep, Gex.Sheep,
            Gex.Brick, Gex.Brick, Gex.Brick,
        };
        
        var resurses = new string[19];
        
        foreach (var (id, ports) in idToPorts)
        {
            var r = random.Next(init.Count);
            var type = init[r];
            while (ports.Contains(type) && IsNestedSameType(resurses, id))
            {
                r = random.Next(init.Count);
                type = init[r];
            }

            resurses[id] = type;
            init.RemoveAt(r);
        }

        foreach (var id in e)
        {
            var r = random.Next(init.Count);
            var type = init[r];
            resurses[id] = type;
            
            var tries = 0;
            while (IsNestedSameType(resurses, id) && tries < 7)
            {
                tries++;
                r = random.Next(init.Count);
                type = init[r];
                resurses[id] = type;
            }
            
            init.RemoveAt(r);
        }

        return resurses;
    }

    private static bool IsNested(int x, int y)
    {
        return Nested[x].Contains(y);
    }

    private static Dictionary<int, HashSet<int>> Nested = new Dictionary<int, HashSet<int>>
    {
        {0, new HashSet<int> {1, 3, 4}},
        {1, new HashSet<int> {0, 2, 4, 5}},
        {2, new HashSet<int> {1, 5, 6}},
        {3, new HashSet<int> {0, 4, 7, 8}},
        {4, new HashSet<int> {0, 1, 3, 5, 8, 9}},
        {5, new HashSet<int> {1, 2, 4, 6, 9, 10}},
        {6, new HashSet<int> {2, 5, 10, 11}},
        {7, new HashSet<int> {3, 8, 12}},
        {8, new HashSet<int> {3, 4, 7, 9, 12, 13}},
        {9, new HashSet<int> {4, 5, 8, 10, 13, 14}},
        {10, new HashSet<int> {5, 6, 9, 11, 14, 15}},
        {11, new HashSet<int> {6, 10, 15}},
        {12, new HashSet<int> {7, 8, 13, 16}},
        {13, new HashSet<int> {8, 9, 12, 14, 16, 17}},
        {14, new HashSet<int> {9, 10, 13, 15, 17, 18}},
        {15, new HashSet<int> {10, 11, 14, 18}},
        {16, new HashSet<int> {12, 13, 17}},
        {17, new HashSet<int> {13, 14, 16, 18}},
        {18, new HashSet<int> {14, 15, 17}},
    };

    private static Random random = new Random();

    private static Dictionary<int, int> Chance = new Dictionary<int, int>
    {
        {2, 1},
        {3, 2},
        {4, 3},
        {5, 4},
        {6, 5},
        {8, 5},
        {9, 4},
        {10, 3},
        {11, 2},
        {12, 1},
    };

    private static Dictionary<int, HashSet<string>> idToPorts = new Dictionary<int, HashSet<string>>
    {
        {6, new HashSet<string>{Gex.Brick}},
        {11, new HashSet<string>{Gex.Brick, Gex.Forest}},
        {12, new HashSet<string>{Gex.Wheat, Gex.Ore}},
        {17, new HashSet<string>{Gex.Wheat}},
        {16, new HashSet<string>{Gex.Ore}},
        {3, new HashSet<string>{Gex.Sheep}},
        {15, new HashSet<string>{Gex.Forest}},
        {0, new HashSet<string>{Gex.Sheep}},
    };

    private static List<int> e = new List<int> {1, 2, 4, 5, 7, 8, 9, 10, 13, 14, 18};
}