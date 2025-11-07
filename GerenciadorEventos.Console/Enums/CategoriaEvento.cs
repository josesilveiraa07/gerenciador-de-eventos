namespace DefaultNamespace;

public class CategoriaEvento
{
    private readonly string value;

    private CategoriaEvento(string value)
    {
        this.value = value;
    }

    public static readonly CategoriaEvento Seminario = new CategoriaEvento("Seminario");
    public static readonly CategoriaEvento Workshop = new CategoriaEvento("Workshop");
    public static readonly CategoriaEvento Conferencia = new CategoriaEvento("Conferencia");

    public static CategoriaEvento FromString(string value)
    {
        return value switch
        {
            "Seminario" => Seminario,
            "Workshop" => Workshop,
            "Conferencia" => Conferencia,
            _ => throw new ArgumentException("Invalid categoria value")
        };
    }

    public override string ToString()
    {
        return value;
    }
}
