public class EventosService
{
  private readonly List<Evento> eventos = [];

  public void AdicionarEvento(Evento evento)
  {
    bool estaDisponivel = VerificarDisponibilidade(evento);

    if (!estaDisponivel)
    {
      Console.WriteLine("Evento não disponível!");
      return;
    }


    eventos.Add(evento);
  }

  public bool VerificarDisponibilidade(Evento eventoACadastrar)
  {
    bool estaDisponivel = true;

    foreach (var evento in eventos)
    {
      if (evento.GetData().Date == eventoACadastrar.GetData().Date && evento.GetCategoriaEvento().Equals(eventoACadastrar.GetCategoriaEvento()))
      {
        estaDisponivel = false;
        break;
      }
    }

    Evento? eventoComMesmoId = eventos.FirstOrDefault(evento => evento.GetId() == eventoACadastrar.GetId() && evento.GetStatus().Equals(StatusEvento.Aberto));

    if (eventoComMesmoId != null) estaDisponivel = false;

    return estaDisponivel;
  }

  public List<Evento> ObterTodosEventos()
  {
    return eventos;
  }

  public int ObterQuantidadeEventos()
  {
    return eventos.Count;
  }
}