namespace WebApplication2;

public class CatanMap
{
    public int id { get; set; }
    public string type { get; set; }
    public int number { get; set; }
    

    public CatanMap(string type, int number, int id)
    {
        this.type = type;
        this.number = number;
        this.id = id;
    }
}