public class PresencaService
{
  private readonly List<Presenca> presencas = [];

  public bool RegistrarPresenca(Inscricao inscricao, DateTime horarioChegada)
  {
    if (inscricao == null)
    {
      Console.WriteLine("Erro: Participante cadastrado deve ser informado obrigatoriamente.");
      return false;
    }

    if (horarioChegada == DateTime.MinValue)
    {
      Console.WriteLine("Erro: Horário de chegada deve ser confirmado obrigatoriamente.");
      return false;
    }

    Evento evento = inscricao.GetEvento();
    int presencasNoEvento = presencas.Count(p => p.GetInscricao().GetEvento().GetId() == evento.GetId());

    if (presencasNoEvento >= evento.GetCapacidadeMaxima())
    {
      Console.WriteLine("Erro: Presença não pode exceder capacidade do evento.");
      return false;
    }

    bool jaRegistrado = presencas.Any(p => p.GetInscricao().GetId() == inscricao.GetId());
    if (jaRegistrado)
    {
      Console.WriteLine("Erro: Presença já foi registrada para esta inscrição.");
      return false;
    }

    Presenca novaPresenca = new Presenca(horarioChegada, inscricao);
    presencas.Add(novaPresenca);

    Console.WriteLine($"Presença registrada com sucesso para {inscricao.GetParticipante()}!");
    Console.WriteLine($"Horário de chegada: {horarioChegada.ToString("dd/MM/yyyy HH:mm")}");
    Console.WriteLine($"Total de presenças no evento: {presencasNoEvento + 1}/{evento.GetCapacidadeMaxima()}");

    return true;
  }

  public List<Presenca> ObterTodasPresencas()
  {
    return presencas;
  }

  public List<Presenca> ObterPresencasPorEvento(int eventoId)
  {
    return presencas.Where(p => p.GetInscricao().GetEvento().GetId() == eventoId).ToList();
  }

  public int ContarPresencasPorEvento(int eventoId)
  {
    return presencas.Count(p => p.GetInscricao().GetEvento().GetId() == eventoId);
  }

  public bool VerificarPresencaRegistrada(int inscricaoId)
  {
    return presencas.Any(p => p.GetInscricao().GetId() == inscricaoId);
  }
}