public class Presenca
{
  private int id;
  private DateTime horarioChegada;
  private Inscricao inscricao;
  private string? feedback;

  public Presenca(DateTime horarioChegada, Inscricao inscricao)
  {
    this.id = new Random().Next(1, 10000);
    this.horarioChegada = horarioChegada;
    this.inscricao = inscricao;
    this.feedback = null;
  }

  public int GetId()
  {
    return id;
  }

  public DateTime GetHorarioChegada()
  {
    return horarioChegada;
  }

  public Inscricao GetInscricao()
  {
    return inscricao;
  }

  public string? GetFeedback()
  {
    return feedback;
  }

  public void SetFeedback(string feedback)
  {
    this.feedback = feedback;
  }

  public bool TemFeedback()
  {
    return !string.IsNullOrEmpty(feedback);
  }
}