using DefaultNamespace;

public class Evento
{
  private int id;
  private string descricao;
  private DateTime data;
  private CategoriaEvento categoriaEvento;
  private int capacidadeMaxima;
  private StatusEvento status;
  private string local;
  private DateTime dataHoraInicio;
  private DateTime dataHoraFim;

  public Evento(string descricao, DateTime data, CategoriaEvento categoriaEvento, int capacidadeMaxima, StatusEvento status, string local, DateTime dataHoraInicio, DateTime dataHoraFim)
  {
    this.id = new Random().Next(1, 10000);
    this.descricao = descricao;
    this.data = data;
    this.categoriaEvento = categoriaEvento;
    this.capacidadeMaxima = capacidadeMaxima;
    this.status = status;
    this.local = local;
    this.dataHoraInicio = dataHoraInicio;
    this.dataHoraFim = dataHoraFim;
  }

  public int GetId()
  {
    return id;
  }

  public string GetDescricao()
  {
    return descricao;
  }

  public DateTime GetData()
  {
    return data;
  }

  public CategoriaEvento GetCategoriaEvento()
  {
    return categoriaEvento;
  }

  public int GetCapacidadeMaxima()
  {
    return capacidadeMaxima;
  }

  public StatusEvento GetStatus()
  {
    return status;
  }

  public string GetLocal()
  {
    return local;
  }

  public DateTime GetDataHoraInicio()
  {
    return dataHoraInicio;
  }

  public DateTime GetDataHoraFim()
  {
    return dataHoraFim;
  }

  public int GetCapacidadeMaxima()
  {
    return capacidadeMaxima;
  }

  public string GetDescricao()
  {
    return descricao;
  }
}