using DefaultNamespace;

public class NotificacoesService
{
  private readonly EventosService eventosService;
  private readonly InscricoesService inscricoesService;
  private readonly PresencaService presencaService;

  public NotificacoesService(EventosService eventosService, InscricoesService inscricoesService, PresencaService presencaService)
  {
    this.eventosService = eventosService;
    this.inscricoesService = inscricoesService;
    this.presencaService = presencaService;
  }

  public void VerificarAlertasAutomaticos()
  {
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║                        ALERTAS AUTOMÁTICOS                                 ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════╝\n");

    bool temAlertas = false;

    List<Evento> eventosProximos = VerificarEventosProximos();
    if (eventosProximos.Count > 0)
    {
      temAlertas = true;
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"⚠️  EVENTOS PRÓXIMOS ({eventosProximos.Count}):");
      Console.ResetColor();
      foreach (var evento in eventosProximos)
      {
        int diasRestantes = (evento.GetData().Date - DateTime.Now.Date).Days;
        string urgencia = diasRestantes <= 1 ? "URGENTE" : diasRestantes <= 3 ? "ATENÇÃO" : "AVISO";
        Console.WriteLine($"   [{urgencia}] {evento.GetDescricao()} - {evento.GetData().ToString("dd/MM/yyyy")} ({diasRestantes} dias)");
      }
      Console.WriteLine();
    }

    List<Inscricao> inscricoesPendentes = VerificarInscricoesPendentes();
    if (inscricoesPendentes.Count > 0)
    {
      temAlertas = true;
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine($"📋 INSCRIÇÕES PENDENTES ({inscricoesPendentes.Count}):");
      Console.ResetColor();
      foreach (var inscricao in inscricoesPendentes.Take(5))
      {
        Console.WriteLine($"   • {inscricao.GetParticipante()} - {inscricao.GetEvento().GetDescricao()}");
      }
      if (inscricoesPendentes.Count > 5)
      {
        Console.WriteLine($"   ... e mais {inscricoesPendentes.Count - 5} inscrições");
      }
      Console.WriteLine();
    }

    if (!temAlertas)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("✓ Nenhum alerta no momento. Sistema operando normalmente.");
      Console.ResetColor();
      Console.WriteLine();
    }

    Console.WriteLine("════════════════════════════════════════════════════════════════════════════\n");
    Console.WriteLine("Pressione qualquer tecla para continuar...");
    Console.ReadKey();
  }

  private List<Evento> VerificarEventosProximos()
  {
    List<Evento> eventos = eventosService.ObterTodosEventos();
    DateTime dataLimite = DateTime.Now.AddDays(7);

    return eventos.Where(e =>
        e.GetData().Date >= DateTime.Now.Date &&
        e.GetData().Date <= dataLimite.Date &&
        !e.GetStatus().Equals(StatusEvento.Concluido)
    ).OrderBy(e => e.GetData()).ToList();
  }

  private List<Inscricao> VerificarInscricoesPendentes()
  {
    List<Inscricao> inscricoes = inscricoesService.ListarInscricoes();
    List<Inscricao> pendentes = new List<Inscricao>();

    foreach (var inscricao in inscricoes)
    {
      Evento evento = inscricao.GetEvento();
      if (evento.GetData().Date <= DateTime.Now.Date &&
          !evento.GetStatus().Equals(StatusEvento.Concluido))
      {
        if (!presencaService.VerificarPresencaRegistrada(inscricao.GetId()))
        {
          pendentes.Add(inscricao);
        }
      }
    }

    return pendentes;
  }

  public void GerarRelatorioParticipacaoMensal(int mes, int ano)
  {
    Console.Clear();
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine($"║              RELATÓRIO DE PARTICIPAÇÃO MENSAL - {mes:D2}/{ano}                   ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════╝\n");

    List<Evento> eventos = eventosService.ObterTodosEventos();
    List<Evento> eventosMes = eventos.Where(e =>
        e.GetData().Month == mes &&
        e.GetData().Year == ano
    ).ToList();

    if (eventosMes.Count == 0)
    {
      Console.WriteLine($"Nenhum evento encontrado para {mes:D2}/{ano}.\n");
      Console.WriteLine("Pressione qualquer tecla para voltar...");
      Console.ReadKey();
      return;
    }

    Console.WriteLine($"Total de Eventos no Mês: {eventosMes.Count}\n");
    Console.WriteLine("────────────────────────────────────────────────────────────────────────────");

    int totalInscricoes = 0;
    int totalPresencas = 0;

    foreach (var evento in eventosMes)
    {
      List<Inscricao> inscricoesEvento = inscricoesService.ListarInscricoes()
          .Where(i => i.GetEvento().GetId() == evento.GetId())
          .ToList();

      int presencasEvento = presencaService.ContarPresencasPorEvento(evento.GetId());

      totalInscricoes += inscricoesEvento.Count;
      totalPresencas += presencasEvento;

      Console.WriteLine($"\n📅 {evento.GetDescricao()}");
      Console.WriteLine($"   Data: {evento.GetData().ToString("dd/MM/yyyy")} | Status: {evento.GetStatus()}");
      Console.WriteLine($"   Categoria: {evento.GetCategoriaEvento()} | Local: {evento.GetLocal()}");
      Console.WriteLine($"   Capacidade: {evento.GetCapacidadeMaxima()}");
      Console.WriteLine($"   Inscrições: {inscricoesEvento.Count}");
      Console.WriteLine($"   Presenças: {presencasEvento}");

      if (inscricoesEvento.Count > 0)
      {
        double taxaPresenca = (double)presencasEvento / inscricoesEvento.Count * 100;
        Console.WriteLine($"   Taxa de Presença: {taxaPresenca:F1}%");
      }
    }

    Console.WriteLine("\n════════════════════════════════════════════════════════════════════════════");
    Console.WriteLine("\n📊 RESUMO DO MÊS:");
    Console.WriteLine($"   Total de Inscrições: {totalInscricoes}");
    Console.WriteLine($"   Total de Presenças: {totalPresencas}");

    if (totalInscricoes > 0)
    {
      double taxaGeralPresenca = (double)totalPresencas / totalInscricoes * 100;
      Console.WriteLine($"   Taxa Geral de Presença: {taxaGeralPresenca:F1}%");
    }

    Console.WriteLine("\n════════════════════════════════════════════════════════════════════════════\n");
    Console.WriteLine("Pressione qualquer tecla para voltar...");
    Console.ReadKey();
  }

  public void GerarRelatorioTaxaEngajamento()
  {
    Console.Clear();
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║                     RELATÓRIO DE TAXA DE ENGAJAMENTO                       ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════╝\n");

    List<Evento> eventos = eventosService.ObterTodosEventos();
    List<Inscricao> todasInscricoes = inscricoesService.ListarInscricoes();
    List<Presenca> todasPresencas = presencaService.ObterTodasPresencas();

    if (eventos.Count == 0)
    {
      Console.WriteLine("Nenhum evento cadastrado no sistema.\n");
      Console.WriteLine("Pressione qualquer tecla para voltar...");
      Console.ReadKey();
      return;
    }

    Console.WriteLine("📈 MÉTRICAS GERAIS DO SISTEMA:\n");
    Console.WriteLine($"   Total de Eventos: {eventos.Count}");
    Console.WriteLine($"   Total de Inscrições: {todasInscricoes.Count}");
    Console.WriteLine($"   Total de Presenças: {todasPresencas.Count}");

    if (todasInscricoes.Count > 0)
    {
      double taxaPresencaGeral = (double)todasPresencas.Count / todasInscricoes.Count * 100;
      Console.WriteLine($"   Taxa de Presença Geral: {taxaPresencaGeral:F1}%");
    }

    Console.WriteLine("\n────────────────────────────────────────────────────────────────────────────");
    Console.WriteLine("\n📊 ENGAJAMENTO POR CATEGORIA:\n");

    var categorias = new[] { CategoriaEvento.Seminario, CategoriaEvento.Workshop, CategoriaEvento.Conferencia };

    foreach (var categoria in categorias)
    {
      var eventosCategoria = eventos.Where(e => e.GetCategoriaEvento().Equals(categoria)).ToList();

      if (eventosCategoria.Count > 0)
      {
        int inscricoesCategoria = 0;
        int presencasCategoria = 0;

        foreach (var evento in eventosCategoria)
        {
          var inscricoes = todasInscricoes.Where(i => i.GetEvento().GetId() == evento.GetId()).Count();
          var presencas = presencaService.ContarPresencasPorEvento(evento.GetId());

          inscricoesCategoria += inscricoes;
          presencasCategoria += presencas;
        }

        Console.WriteLine($"   {categoria}:");
        Console.WriteLine($"      Eventos: {eventosCategoria.Count}");
        Console.WriteLine($"      Inscrições: {inscricoesCategoria}");
        Console.WriteLine($"      Presenças: {presencasCategoria}");

        if (inscricoesCategoria > 0)
        {
          double taxaEngajamento = (double)presencasCategoria / inscricoesCategoria * 100;
          Console.WriteLine($"      Taxa de Engajamento: {taxaEngajamento:F1}%");
        }
        Console.WriteLine();
      }
    }

    Console.WriteLine("────────────────────────────────────────────────────────────────────────────");
    Console.WriteLine("\n🏆 TOP 5 EVENTOS COM MAIOR ENGAJAMENTO:\n");

    var eventosComEngajamento = eventos.Select(e => new
    {
      Evento = e,
      Inscricoes = todasInscricoes.Count(i => i.GetEvento().GetId() == e.GetId()),
      Presencas = presencaService.ContarPresencasPorEvento(e.GetId())
    })
    .Where(x => x.Inscricoes > 0)
    .Select(x => new
    {
      x.Evento,
      x.Inscricoes,
      x.Presencas,
      TaxaEngajamento = (double)x.Presencas / x.Inscricoes * 100
    })
    .OrderByDescending(x => x.TaxaEngajamento)
    .Take(5)
    .ToList();

    int posicao = 1;
    foreach (var item in eventosComEngajamento)
    {
      Console.WriteLine($"   {posicao}. {item.Evento.GetDescricao()}");
      Console.WriteLine($"      Data: {item.Evento.GetData().ToString("dd/MM/yyyy")}");
      Console.WriteLine($"      Inscrições: {item.Inscricoes} | Presenças: {item.Presencas}");
      Console.WriteLine($"      Taxa de Engajamento: {item.TaxaEngajamento:F1}%");
      Console.WriteLine();
      posicao++;
    }

    var eventosBaixoEngajamento = eventos.Select(e => new
    {
      Evento = e,
      Inscricoes = todasInscricoes.Count(i => i.GetEvento().GetId() == e.GetId()),
      Presencas = presencaService.ContarPresencasPorEvento(e.GetId())
    })
    .Where(x => x.Inscricoes > 0)
    .Select(x => new
    {
      x.Evento,
      x.Inscricoes,
      x.Presencas,
      TaxaEngajamento = (double)x.Presencas / x.Inscricoes * 100
    })
    .Where(x => x.TaxaEngajamento < 50)
    .ToList();

    if (eventosBaixoEngajamento.Count > 0)
    {
      Console.WriteLine("────────────────────────────────────────────────────────────────────────────");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"\n⚠️  ALERTAS - EVENTOS COM BAIXO ENGAJAMENTO (<50%):\n");
      Console.ResetColor();

      foreach (var item in eventosBaixoEngajamento)
      {
        Console.WriteLine($"   • {item.Evento.GetDescricao()} - {item.TaxaEngajamento:F1}%");
      }
      Console.WriteLine();
    }

    Console.WriteLine("════════════════════════════════════════════════════════════════════════════\n");
    Console.WriteLine("Pressione qualquer tecla para voltar...");
    Console.ReadKey();
  }
}

