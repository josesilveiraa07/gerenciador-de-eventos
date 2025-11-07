public class StatusEvento
{
  private readonly string value;

  private StatusEvento(string value)
  {
    this.value = value;
  }

  public static readonly StatusEvento Aberto = new StatusEvento("Aberto");
  public static readonly StatusEvento EmAndamento = new StatusEvento("EmAndamento");
  public static readonly StatusEvento Concluido = new StatusEvento("Concluido");

  public static StatusEvento FromString(string value)
  {
    return value switch
    {
      "Aberto" => Aberto,
      "EmAndamento" => EmAndamento,
      "Concluido" => Concluido,
      _ => throw new ArgumentException("Invalid status value")
    };
  }

  public override string ToString()
  {
    return value;
  }
}