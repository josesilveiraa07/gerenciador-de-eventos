public class PresencaCRUD
{
  //
  // Propriedades
  //
  private PresencaService presencaService;
  private InscricoesService inscricoesService;
  private EventosService eventosService;
  private List<string> dados = new List<string>();
  private int coluna, linha, largura;
  private int larguraDados, colunaDados, linhaDados;
  private Tela tela;

  // Variáveis temporárias
  private int inscricaoId;
  private DateTime horarioChegada;


  //
  // Métodos
  //
  public PresencaCRUD(Tela tela, PresencaService presencaService, InscricoesService inscricoesService, EventosService eventosService)
  {
    this.presencaService = presencaService;
    this.inscricoesService = inscricoesService;
    this.eventosService = eventosService;

    // inicializa o vetor com as perguntas
    this.dados.Add("ID da Inscrição        : ");
    this.dados.Add("Horário Chegada (dd/MM/yyyy HH:mm): ");

    // indica onde está o objeto tela
    this.tela = tela;

    // define a posição e largura da janela
    this.coluna = 10;
    this.linha = 4;
    this.largura = 60;

    // calcula a área dos dados
    this.larguraDados = this.largura - dados[0].Length - 2;
    this.colunaDados = this.coluna + dados[0].Length + 1;
    this.linhaDados = this.linha + 2;
  }


  public void ExecutarCRUD()
  {
    string opcao;
    List<string> opcoes = new List<string>();
    opcoes.Add("     Menu Presenças      ");
    opcoes.Add("1 - Registrar Presença   ");
    opcoes.Add("2 - Listar Presenças     ");
    opcoes.Add("3 - Presenças por Evento ");
    opcoes.Add("4 - Registrar Feedback   ");
    opcoes.Add("0 - Voltar               ");

    while (true)
    {
      this.tela.PrepararTela("Gerenciamento de Presenças");
      opcao = this.tela.MostrarMenu(opcoes, 2, 4);

      if (opcao == "0") break;
      else if (opcao == "1") this.RegistrarPresenca();
      else if (opcao == "2") this.ListarPresencas();
      else if (opcao == "3") this.ListarPresencasPorEvento();
      else if (opcao == "4") this.RegistrarFeedback();
      else
      {
        this.tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
        Console.ReadKey();
      }
    }
  }


  private void RegistrarPresenca()
  {
    string resp;

    // Limpar a tela e preparar nova tela
    this.tela.PrepararTela("Registrar Presença");

    // Mostrar inscrições disponíveis
    this.MostrarInscricoesDisponiveis();

    // montar a janela do CRUD
    this.tela.MontarJanela("Dados da Presença", this.dados, this.coluna, this.linha + 12, this.largura);

    // Entrar dados
    this.tela.MostrarMensagem("Digite os dados da presença");
    this.EntrarDados();

    resp = this.tela.Perguntar("Confirma registro de presença (S/N) : ");
    if (resp.ToLower() == "s")
    {
      try
      {
        // Buscar a inscrição
        List<Inscricao> inscricoes = this.inscricoesService.ListarInscricoes();
        Inscricao? inscricao = inscricoes.FirstOrDefault(i => i.GetId() == this.inscricaoId);

        if (inscricao == null)
        {
          this.tela.MostrarMensagem("Erro: Inscrição não encontrada! Pressione uma tecla...");
          Console.ReadKey();
          return;
        }

        // Verificar se já tem presença registrada
        if (this.presencaService.VerificarPresencaRegistrada(inscricao.GetId()))
        {
          this.tela.MostrarMensagem("Erro: Presença já registrada para esta inscrição! Pressione uma tecla...");
          Console.ReadKey();
          return;
        }

        // Registrar através do service
        bool sucesso = this.presencaService.RegistrarPresenca(inscricao, this.horarioChegada);

        this.tela.MostrarMensagem("Pressione uma tecla para continuar...");
        Console.ReadKey();
      }
      catch (Exception ex)
      {
        this.tela.MostrarMensagem($"Erro ao registrar: {ex.Message}. Pressione uma tecla...");
        Console.ReadKey();
      }
    }
  }


  private string LerComDica(int coluna, int linha, string dica)
  {
    string input = "";

    void MostrarDica()
    {
      Console.SetCursorPosition(coluna, linha);
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.Write(dica);
      Console.ResetColor();
      Console.SetCursorPosition(coluna, linha);
    }

    void LimparLinha()
    {
      Console.SetCursorPosition(coluna, linha);
      Console.Write(new string(' ', this.larguraDados));
      Console.SetCursorPosition(coluna, linha);
    }

    MostrarDica();

    while (true)
    {
      ConsoleKeyInfo tecla = Console.ReadKey(true);

      if (tecla.Key == ConsoleKey.Enter)
      {
        break;
      }

      if (tecla.Key == ConsoleKey.Backspace)
      {
        if (input.Length > 0)
        {
          input = input.Substring(0, input.Length - 1);
          LimparLinha();

          if (input.Length == 0)
          {
            MostrarDica();
          }
          else
          {
            Console.Write(input);
          }
        }
        continue;
      }

      if (char.IsControl(tecla.KeyChar))
      {
        continue;
      }

      if (input.Length == 0)
      {
        LimparLinha();
      }

      input += tecla.KeyChar;
      Console.Write(tecla.KeyChar);
    }

    if (input.Length == 0)
    {
      LimparLinha();
    }

    return input;
  }


  private void EntrarDados()
  {
    try
    {
      // ID da Inscrição
      Console.SetCursorPosition(this.colunaDados, this.linhaDados + 12);
      string? inscricaoIdStr = Console.ReadLine();
      this.inscricaoId = int.Parse(inscricaoIdStr ?? "0");

      // Horário de Chegada
      string horarioStr = this.LerComDica(this.colunaDados, this.linhaDados + 13, "dd/MM/yyyy HH:mm");
      this.horarioChegada = DateTime.ParseExact(horarioStr, "dd/MM/yyyy HH:mm", null);
    }
    catch (Exception ex)
    {
      this.tela.MostrarMensagem($"Erro ao processar dados: {ex.Message}");
      throw;
    }
  }


  private void MostrarInscricoesDisponiveis()
  {
    List<Inscricao> inscricoes = this.inscricoesService.ListarInscricoes();

    if (inscricoes.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhuma inscrição disponível.");
      return;
    }

    int linhaAtual = 4;
    int colunaInicial = 2;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("Inscrições Disponíveis:");
    linhaAtual += 2;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"{"ID",-6} {"Participante",-20} {"Evento",-20} {"Presença",-10}");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("─────────────────────────────────────────────────────────────────");
    linhaAtual++;

    int count = 0;
    foreach (var inscricao in inscricoes)
    {
      if (count >= 5) break;

      string participanteTruncado = inscricao.GetParticipante().Length > 20
          ? inscricao.GetParticipante().Substring(0, 17) + "..."
          : inscricao.GetParticipante();

      string eventoTruncado = inscricao.GetEvento().GetDescricao().Length > 20
          ? inscricao.GetEvento().GetDescricao().Substring(0, 17) + "..."
          : inscricao.GetEvento().GetDescricao();

      bool temPresenca = this.presencaService.VerificarPresencaRegistrada(inscricao.GetId());
      string statusPresenca = temPresenca ? "Sim" : "Não";

      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"{inscricao.GetId(),-6} {participanteTruncado,-20} {eventoTruncado,-20} {statusPresenca,-10}");
      linhaAtual++;
      count++;
    }
  }


  private void ListarPresencas()
  {
    this.tela.PrepararTela("Lista de Presenças");

    List<Presenca> presencas = this.presencaService.ObterTodasPresencas();

    if (presencas.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhuma presença registrada. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    int linhaAtual = 4;
    int colunaInicial = 2;

    // Cabeçalho
    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"{"ID",-6} {"Participante",-20} {"Evento",-20} {"Horário",-18} {"Feed",-5}");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    // Listagem das presenças
    foreach (var presenca in presencas)
    {
      if (linhaAtual >= 22)
      {
        this.tela.MostrarMensagem("Pressione uma tecla para ver mais presenças...");
        Console.ReadKey();
        this.tela.PrepararTela("Lista de Presenças");
        linhaAtual = 4;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write($"{"ID",-6} {"Participante",-20} {"Evento",-20} {"Horário",-18} {"Feed",-5}");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;
      }

      string participanteTruncado = presenca.GetInscricao().GetParticipante().Length > 20
          ? presenca.GetInscricao().GetParticipante().Substring(0, 17) + "..."
          : presenca.GetInscricao().GetParticipante();

      string eventoTruncado = presenca.GetInscricao().GetEvento().GetDescricao().Length > 20
          ? presenca.GetInscricao().GetEvento().GetDescricao().Substring(0, 17) + "..."
          : presenca.GetInscricao().GetEvento().GetDescricao();

      string temFeedback = presenca.TemFeedback() ? "Sim" : "Não";

      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"{presenca.GetId(),-6} {participanteTruncado,-20} {eventoTruncado,-20} {presenca.GetHorarioChegada().ToString("dd/MM/yyyy HH:mm"),-18} {temFeedback,-5}");
      linhaAtual++;
    }

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"Total de presenças: {presencas.Count}");

    this.tela.MostrarMensagem("Pressione D para ver detalhes ou qualquer tecla para voltar...");
    var tecla = Console.ReadKey();

    if (tecla.Key == ConsoleKey.D)
    {
      this.MostrarDetalhesPresenca();
    }
  }


  private void ListarPresencasPorEvento()
  {
    this.tela.PrepararTela("Presenças por Evento");

    // Mostrar eventos disponíveis
    List<Evento> eventos = this.eventosService.ObterTodosEventos();

    if (eventos.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhum evento cadastrado. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    int linhaAtual = 4;
    int colunaInicial = 2;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("Eventos Disponíveis:");
    linhaAtual += 2;

    foreach (var evento in eventos.Take(8))
    {
      int presencasCount = this.presencaService.ContarPresencasPorEvento(evento.GetId());
      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"ID: {evento.GetId()} - {evento.GetDescricao()} ({presencasCount}/{evento.GetCapacidadeMaxima()})");
      linhaAtual++;
    }

    string eventoIdStr = this.tela.Perguntar("Digite o ID do evento: ");

    if (!int.TryParse(eventoIdStr, out int eventoId))
    {
      this.tela.MostrarMensagem("ID inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    List<Presenca> presencas = this.presencaService.ObterPresencasPorEvento(eventoId);

    if (presencas.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhuma presença registrada para este evento. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    this.tela.PrepararTela($"Presenças - Evento {eventoId}");
    linhaAtual = 4;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"{"ID",-6} {"Participante",-30} {"Horário Chegada",-20}");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    foreach (var presenca in presencas)
    {
      string participanteTruncado = presenca.GetInscricao().GetParticipante().Length > 30
          ? presenca.GetInscricao().GetParticipante().Substring(0, 27) + "..."
          : presenca.GetInscricao().GetParticipante();

      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"{presenca.GetId(),-6} {participanteTruncado,-30} {presenca.GetHorarioChegada().ToString("dd/MM/yyyy HH:mm"),-20}");
      linhaAtual++;
    }

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");

    this.tela.MostrarMensagem($"Total: {presencas.Count} presenças. Pressione uma tecla...");
    Console.ReadKey();
  }


  private void RegistrarFeedback()
  {
    this.tela.PrepararTela("Registrar Feedback");

    string presencaIdStr = this.tela.Perguntar("Digite o ID da presença: ");

    if (!int.TryParse(presencaIdStr, out int presencaId))
    {
      this.tela.MostrarMensagem("ID inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    List<Presenca> presencas = this.presencaService.ObterTodasPresencas();
    Presenca? presenca = presencas.FirstOrDefault(p => p.GetId() == presencaId);

    if (presenca == null)
    {
      this.tela.MostrarMensagem("Presença não encontrada. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    // RN-005: Verificar se o evento está concluído
    Evento evento = presenca.GetInscricao().GetEvento();
    if (!evento.GetStatus().Equals(StatusEvento.Concluido))
    {
      this.tela.MostrarMensagem("Erro: Feedback só pode ser registrado para eventos concluídos! Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    // Mostrar informações da presença
    int linhaAtual = 4;
    int colunaInicial = 10;

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Participante: {presenca.GetInscricao().GetParticipante()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Evento: {evento.GetDescricao()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Horário Chegada: {presenca.GetHorarioChegada().ToString("dd/MM/yyyy HH:mm")}");

    linhaAtual++;

    if (presenca.TemFeedback())
    {
      Console.SetCursorPosition(colunaInicial, linhaAtual++);
      Console.Write($"Feedback atual: {presenca.GetFeedback()}");
      linhaAtual++;
    }

    string feedback = this.tela.Perguntar("Digite o feedback: ");

    if (string.IsNullOrWhiteSpace(feedback))
    {
      this.tela.MostrarMensagem("Feedback não pode ser vazio. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    presenca.SetFeedback(feedback);
    this.tela.MostrarMensagem("Feedback registrado com sucesso! Pressione uma tecla...");
    Console.ReadKey();
  }


  private void MostrarDetalhesPresenca()
  {
    this.tela.PrepararTela("Detalhes da Presença");

    string idStr = this.tela.Perguntar("Digite o ID da presença: ");

    if (!int.TryParse(idStr, out int id))
    {
      this.tela.MostrarMensagem("ID inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    List<Presenca> presencas = this.presencaService.ObterTodasPresencas();
    Presenca? presencaEncontrada = presencas.FirstOrDefault(p => p.GetId() == id);

    if (presencaEncontrada == null)
    {
      this.tela.MostrarMensagem("Presença não encontrada. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    // Exibir detalhes da presença
    int linhaAtual = 4;
    int colunaInicial = 10;

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"ID da Presença: {presencaEncontrada.GetId()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Participante: {presencaEncontrada.GetInscricao().GetParticipante()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Horário de Chegada: {presencaEncontrada.GetHorarioChegada().ToString("dd/MM/yyyy HH:mm")}");

    linhaAtual++;
    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write("--- Dados da Inscrição ---");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"ID Inscrição: {presencaEncontrada.GetInscricao().GetId()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Data Inscrição: {presencaEncontrada.GetInscricao().GetDataInscricao().ToString("dd/MM/yyyy")}");

    linhaAtual++;
    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write("--- Dados do Evento ---");

    Evento evento = presencaEncontrada.GetInscricao().GetEvento();
    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Evento: {evento.GetDescricao()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Data: {evento.GetData().ToString("dd/MM/yyyy")}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Local: {evento.GetLocal()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Status: {evento.GetStatus()}");

    if (presencaEncontrada.TemFeedback())
    {
      linhaAtual++;
      Console.SetCursorPosition(colunaInicial, linhaAtual++);
      Console.Write("--- Feedback ---");
      Console.SetCursorPosition(colunaInicial, linhaAtual++);
      Console.Write($"{presencaEncontrada.GetFeedback()}");
    }

    this.tela.MostrarMensagem("Pressione uma tecla para voltar...");
    Console.ReadKey();
  }

}

