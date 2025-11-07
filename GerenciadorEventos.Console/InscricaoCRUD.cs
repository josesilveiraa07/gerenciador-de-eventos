public class InscricaoCRUD
{
  private InscricoesService inscricoesService;
  private EventosService eventosService;
  private List<string> dados = new List<string>();
  private int coluna, linha, largura;
  private int larguraDados, colunaDados, linhaDados;
  private Tela tela;

  private string participante = "";
  private int eventoId;
  private DateTime dataInscricao;
  private DateTime dataReferencia;


  public InscricaoCRUD(Tela tela, InscricoesService inscricoesService, EventosService eventosService)
  {
    this.inscricoesService = inscricoesService;
    this.eventosService = eventosService;

    this.dados.Add("Participante           : ");
    this.dados.Add("ID do Evento           : ");
    this.dados.Add("Data Inscrição (dd/MM/yyyy): ");
    this.dados.Add("Data Referência (dd/MM/yyyy): ");

    this.tela = tela;

    this.coluna = 10;
    this.linha = 4;
    this.largura = 60;

    this.larguraDados = this.largura - dados[0].Length - 2;
    this.colunaDados = this.coluna + dados[0].Length + 1;
    this.linhaDados = this.linha + 2;
  }


  public void ExecutarCRUD()
  {
    string opcao;
    List<string> opcoes = new List<string>();
    opcoes.Add("     Menu Inscrições     ");
    opcoes.Add("1 - Registrar Inscrição  ");
    opcoes.Add("2 - Listar Inscrições    ");
    opcoes.Add("3 - Lista de Espera      ");
    opcoes.Add("4 - Confirmar Inscrição  ");
    opcoes.Add("0 - Voltar               ");

    while (true)
    {
      this.tela.PrepararTela("Gerenciamento de Inscrições");
      opcao = this.tela.MostrarMenu(opcoes, 2, 4);

      if (opcao == "0") break;
      else if (opcao == "1") this.RegistrarInscricao();
      else if (opcao == "2") this.ListarInscricoes();
      else if (opcao == "3") this.ListarListaEspera();
      else if (opcao == "4") this.ConfirmarInscricao();
      else
      {
        this.tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
        Console.ReadKey();
      }
    }
  }


  private void RegistrarInscricao()
  {
    string resp;

    this.tela.PrepararTela("Registrar Inscrição");

    this.MostrarEventosDisponiveis();

    this.tela.MontarJanela("Dados da Inscrição", this.dados, this.coluna, this.linha + 12, this.largura);

    this.tela.MostrarMensagem("Digite os dados da inscrição");
    this.EntrarDados();

    resp = this.tela.Perguntar("Confirma inscrição (S/N) : ");
    if (resp.ToLower() == "s")
    {
      try
      {
        List<Evento> eventos = this.eventosService.ObterTodosEventos();
        Evento? evento = eventos.FirstOrDefault(e => e.GetId() == this.eventoId);

        if (evento == null)
        {
          this.tela.MostrarMensagem("Erro: Evento não encontrado! Pressione uma tecla...");
          Console.ReadKey();
          return;
        }

        this.inscricoesService.RegistrarInscricao(
            this.participante,
            evento,
            this.dataInscricao,
            this.dataReferencia
        );

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
      Console.SetCursorPosition(this.colunaDados, this.linhaDados + 12);
      this.participante = Console.ReadLine() ?? "";

      Console.SetCursorPosition(this.colunaDados, this.linhaDados + 13);
      string? eventoIdStr = Console.ReadLine();
      this.eventoId = int.Parse(eventoIdStr ?? "0");

      string dataInscricaoStr = this.LerComDica(this.colunaDados, this.linhaDados + 14, "dd/MM/yyyy");
      this.dataInscricao = DateTime.ParseExact(dataInscricaoStr, "dd/MM/yyyy", null);

      string dataReferenciaStr = this.LerComDica(this.colunaDados, this.linhaDados + 15, "dd/MM/yyyy");
      this.dataReferencia = DateTime.ParseExact(dataReferenciaStr, "dd/MM/yyyy", null);
    }
    catch (Exception ex)
    {
      this.tela.MostrarMensagem($"Erro ao processar dados: {ex.Message}");
      throw;
    }
  }


  private void MostrarEventosDisponiveis()
  {
    List<Evento> eventos = this.eventosService.ObterTodosEventos();

    if (eventos.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhum evento disponível.");
      return;
    }

    int linhaAtual = 4;
    int colunaInicial = 2;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("Eventos Disponíveis:");
    linhaAtual += 2;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"{"ID",-6} {"Descrição",-25} {"Data",-12} {"Categoria",-12}");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("─────────────────────────────────────────────────────────────────");
    linhaAtual++;

    int count = 0;
    foreach (var evento in eventos)
    {
      if (count >= 5) break;

      string descricaoTruncada = evento.GetDescricao().Length > 25
          ? evento.GetDescricao().Substring(0, 22) + "..."
          : evento.GetDescricao();

      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"{evento.GetId(),-6} {descricaoTruncada,-25} {evento.GetData().ToString("dd/MM/yyyy"),-12} {evento.GetCategoriaEvento().ToString(),-12}");
      linhaAtual++;
      count++;
    }
  }


  private void ListarInscricoes()
  {
    this.tela.PrepararTela("Lista de Inscrições");

    List<Inscricao> inscricoes = this.inscricoesService.ListarInscricoes();

    if (inscricoes.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhuma inscrição registrada. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    int linhaAtual = 4;
    int colunaInicial = 2;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"{"ID",-6} {"Participante",-20} {"Evento",-20} {"Data Insc.",-12} {"Espera",-8}");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    foreach (var inscricao in inscricoes)
    {
      if (linhaAtual >= 22)
      {
        this.tela.MostrarMensagem("Pressione uma tecla para ver mais inscrições...");
        Console.ReadKey();
        this.tela.PrepararTela("Lista de Inscrições");
        linhaAtual = 4;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write($"{"ID",-6} {"Participante",-20} {"Evento",-20} {"Data Insc.",-12} {"Espera",-8}");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;
      }

      string participanteTruncado = inscricao.GetParticipante().Length > 20
          ? inscricao.GetParticipante().Substring(0, 17) + "..."
          : inscricao.GetParticipante();

      string eventoTruncado = inscricao.GetEvento().GetDescricao().Length > 20
          ? inscricao.GetEvento().GetDescricao().Substring(0, 17) + "..."
          : inscricao.GetEvento().GetDescricao();

      string estaEmEspera = inscricao.EstaEmListaEspera() ? "Sim" : "Não";

      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"{inscricao.GetId(),-6} {participanteTruncado,-20} {eventoTruncado,-20} {inscricao.GetDataInscricao().ToString("dd/MM/yyyy"),-12} {estaEmEspera,-8}");
      linhaAtual++;
    }

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"Total de inscrições: {inscricoes.Count}");

    this.tela.MostrarMensagem("Pressione D para ver detalhes ou qualquer tecla para voltar...");
    var tecla = Console.ReadKey();

    if (tecla.Key == ConsoleKey.D)
    {
      this.MostrarDetalhesInscricao();
    }
  }


  private void ListarListaEspera()
  {
    this.tela.PrepararTela("Lista de Espera");

    List<Inscricao> listaEspera = this.inscricoesService.ListarListaEspera();

    if (listaEspera.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhuma inscrição na lista de espera. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    int linhaAtual = 4;
    int colunaInicial = 2;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"{"ID",-6} {"Participante",-25} {"Evento",-25} {"Data Insc.",-12}");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    foreach (var inscricao in listaEspera)
    {
      if (linhaAtual >= 22)
      {
        this.tela.MostrarMensagem("Pressione uma tecla para ver mais...");
        Console.ReadKey();
        this.tela.PrepararTela("Lista de Espera");
        linhaAtual = 4;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write($"{"ID",-6} {"Participante",-25} {"Evento",-25} {"Data Insc.",-12}");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;
      }

      string participanteTruncado = inscricao.GetParticipante().Length > 25
          ? inscricao.GetParticipante().Substring(0, 22) + "..."
          : inscricao.GetParticipante();

      string eventoTruncado = inscricao.GetEvento().GetDescricao().Length > 25
          ? inscricao.GetEvento().GetDescricao().Substring(0, 22) + "..."
          : inscricao.GetEvento().GetDescricao();

      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"{inscricao.GetId(),-6} {participanteTruncado,-25} {eventoTruncado,-25} {inscricao.GetDataInscricao().ToString("dd/MM/yyyy"),-12}");
      linhaAtual++;
    }

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"Total na lista de espera: {listaEspera.Count}");

    this.tela.MostrarMensagem("Pressione uma tecla para voltar...");
    Console.ReadKey();
  }


  private void ConfirmarInscricao()
  {
    this.tela.PrepararTela("Confirmar Inscrição");

    string idStr = this.tela.Perguntar("Digite o ID da inscrição: ");

    if (!int.TryParse(idStr, out int id))
    {
      this.tela.MostrarMensagem("ID inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    this.inscricoesService.ConfirmarInscricao(id);

    this.tela.MostrarMensagem("Pressione uma tecla para continuar...");
    Console.ReadKey();
  }


  private void MostrarDetalhesInscricao()
  {
    this.tela.PrepararTela("Detalhes da Inscrição");

    string idStr = this.tela.Perguntar("Digite o ID da inscrição: ");

    if (!int.TryParse(idStr, out int id))
    {
      this.tela.MostrarMensagem("ID inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    List<Inscricao> inscricoes = this.inscricoesService.ListarInscricoes();
    Inscricao? inscricaoEncontrada = inscricoes.FirstOrDefault(i => i.GetId() == id);

    if (inscricaoEncontrada == null)
    {
      this.tela.MostrarMensagem("Inscrição não encontrada. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    int linhaAtual = 4;
    int colunaInicial = 10;

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"ID: {inscricaoEncontrada.GetId()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Participante: {inscricaoEncontrada.GetParticipante()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Evento: {inscricaoEncontrada.GetEvento().GetDescricao()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"ID do Evento: {inscricaoEncontrada.GetEvento().GetId()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Data da Inscrição: {inscricaoEncontrada.GetDataInscricao().ToString("dd/MM/yyyy")}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Data de Referência: {inscricaoEncontrada.GetDataReferencia().ToString("dd/MM/yyyy")}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Em Lista de Espera: {(inscricaoEncontrada.EstaEmListaEspera() ? "Sim" : "Não")}");

    linhaAtual++;
    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write("--- Detalhes do Evento ---");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Data do Evento: {inscricaoEncontrada.GetEvento().GetData().ToString("dd/MM/yyyy")}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Local: {inscricaoEncontrada.GetEvento().GetLocal()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Categoria: {inscricaoEncontrada.GetEvento().GetCategoriaEvento()}");

    this.tela.MostrarMensagem("Pressione uma tecla para voltar...");
    Console.ReadKey();
  }

}

