using DefaultNamespace;

public class EventoCRUD
{
  //
  // Propriedades
  //
  private EventosService eventosService;
  private Evento? evento;
  private List<string> dados = new List<string>();
  private int coluna, linha, largura;
  private int larguraDados, colunaDados, linhaDados;
  private Tela tela;

  // Variáveis temporárias para armazenar os dados do evento
  private string descricao = "";
  private DateTime data;
  private CategoriaEvento? categoriaEvento;
  private int capacidadeMaxima;
  private StatusEvento? status;
  private string local = "";
  private DateTime dataHoraInicio;
  private DateTime dataHoraFim;


  //
  // Métodos
  //
  public EventoCRUD(Tela tela, EventosService eventosService)
  {
    // propriedades para o CRUD
    this.eventosService = eventosService;

    // inicializa o vetor com as perguntas de evento
    this.dados.Add("ID               : ");
    this.dados.Add("Descrição        : ");
    this.dados.Add("Data (dd/MM/yyyy): ");
    this.dados.Add("Categoria        : ");
    this.dados.Add("Capacidade Máxima: ");
    this.dados.Add("Status           : ");
    this.dados.Add("Local            : ");
    this.dados.Add("Início (dd/MM/yyyy HH:mm): ");
    this.dados.Add("Fim (dd/MM/yyyy HH:mm)   : ");

    // indica para EventoCRUD onde está o objeto tela
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
    string resp;

    // montar a janela do CRUD
    this.tela.MontarJanela("Cadastro de Eventos", this.dados, this.coluna, this.linha, this.largura);

    // algoritmo CRUD - apenas cadastro neste caso
    this.tela.MostrarMensagem("Digite os dados do novo evento");
    this.EntrarDados();

    resp = this.tela.Perguntar("Confirma cadastro (S/N) : ");
    if (resp.ToLower() == "s")
    {
      try
      {
        // Validar campos obrigatórios
        if (this.categoriaEvento == null || this.status == null)
        {
          this.tela.MostrarMensagem("Erro: Categoria e Status são obrigatórios! Pressione uma tecla...");
          Console.ReadKey();
          return;
        }

        // Criar o evento
        this.evento = new Evento(
            this.descricao,
            this.data,
            this.categoriaEvento,
            this.capacidadeMaxima,
            this.status,
            this.local,
            this.dataHoraInicio,
            this.dataHoraFim
        );

        // Adicionar através do service
        this.eventosService.AdicionarEvento(this.evento);
        this.tela.MostrarMensagem("Evento cadastrado com sucesso! Pressione uma tecla...");
        Console.ReadKey();
      }
      catch (Exception ex)
      {
        this.tela.MostrarMensagem($"Erro ao cadastrar: {ex.Message}. Pressione uma tecla...");
        Console.ReadKey();
      }
    }
  }


  private string LerComDica(int coluna, int linha, string dica)
  {
    string input = "";

    // Função auxiliar para mostrar a dica
    void MostrarDica()
    {
      Console.SetCursorPosition(coluna, linha);
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.Write(dica);
      Console.ResetColor();
      Console.SetCursorPosition(coluna, linha);
    }

    // Função auxiliar para limpar a linha
    void LimparLinha()
    {
      Console.SetCursorPosition(coluna, linha);
      Console.Write(new string(' ', this.larguraDados));
      Console.SetCursorPosition(coluna, linha);
    }

    // Mostra a dica inicialmente
    MostrarDica();

    while (true)
    {
      ConsoleKeyInfo tecla = Console.ReadKey(true);

      // Enter finaliza a entrada
      if (tecla.Key == ConsoleKey.Enter)
      {
        break;
      }

      // Backspace remove o último caractere
      if (tecla.Key == ConsoleKey.Backspace)
      {
        if (input.Length > 0)
        {
          input = input.Substring(0, input.Length - 1);
          LimparLinha();

          // Se ficou vazio, mostra a dica novamente
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

      // Ignora teclas de controle
      if (char.IsControl(tecla.KeyChar))
      {
        continue;
      }

      // Se é o primeiro caractere, apaga a dica
      if (input.Length == 0)
      {
        LimparLinha();
      }

      // Adiciona o caractere ao input
      input += tecla.KeyChar;
      Console.Write(tecla.KeyChar);
    }

    // Limpa a dica se o usuário não digitou nada
    if (input.Length == 0)
    {
      LimparLinha();
    }

    return input;
  }


  public void EntrarDados()
  {
    try
    {
      // ID (apenas informativo, será gerado automaticamente)
      Console.SetCursorPosition(this.colunaDados, this.linhaDados);
      Console.Write("(gerado automaticamente)");

      // Descrição
      Console.SetCursorPosition(this.colunaDados, this.linhaDados + 1);
      this.descricao = Console.ReadLine() ?? "";

      // Data
      string dataStr = this.LerComDica(this.colunaDados, this.linhaDados + 2, "dd/MM/yyyy");
      this.data = DateTime.ParseExact(dataStr, "dd/MM/yyyy", null);

      // Categoria
      string categoriaStr = this.LerComDica(this.colunaDados, this.linhaDados + 3, "Seminario/Workshop/Conferencia");
      this.categoriaEvento = CategoriaEvento.FromString(categoriaStr);

      // Capacidade Máxima
      Console.SetCursorPosition(this.colunaDados, this.linhaDados + 4);
      this.capacidadeMaxima = int.Parse(Console.ReadLine() ?? "0");

      // Status
      string statusStr = this.LerComDica(this.colunaDados, this.linhaDados + 5, "Aberto/EmAndamento/Concluido");
      this.status = StatusEvento.FromString(statusStr);

      // Local
      Console.SetCursorPosition(this.colunaDados, this.linhaDados + 6);
      this.local = Console.ReadLine() ?? "";

      // Data/Hora Início
      string inicioStr = this.LerComDica(this.colunaDados, this.linhaDados + 7, "dd/MM/yyyy HH:mm");
      this.dataHoraInicio = DateTime.ParseExact(inicioStr, "dd/MM/yyyy HH:mm", null);

      // Data/Hora Fim
      string fimStr = this.LerComDica(this.colunaDados, this.linhaDados + 8, "dd/MM/yyyy HH:mm");
      this.dataHoraFim = DateTime.ParseExact(fimStr, "dd/MM/yyyy HH:mm", null);
    }
    catch (Exception ex)
    {
      this.tela.MostrarMensagem($"Erro ao processar dados: {ex.Message}");
      throw;
    }
  }


  public void ListarEventos()
  {
    this.tela.PrepararTela("Lista de Eventos");

    List<Evento> eventos = this.eventosService.ObterTodosEventos();

    if (eventos.Count == 0)
    {
      this.tela.MostrarMensagem("Nenhum evento cadastrado. Pressione uma tecla...");
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
    Console.Write($"{"ID",-6} {"Descrição",-20} {"Data",-12} {"Categoria",-12} {"Status",-15}");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    // Listagem dos eventos
    foreach (var evento in eventos)
    {
      if (linhaAtual >= 22) // Limite da tela
      {
        this.tela.MostrarMensagem("Pressione uma tecla para ver mais eventos...");
        Console.ReadKey();
        this.tela.PrepararTela("Lista de Eventos");
        linhaAtual = 4;

        // Redesenhar cabeçalho
        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write($"{"ID",-6} {"Descrição",-20} {"Data",-12} {"Categoria",-12} {"Status",-15}");
        linhaAtual++;

        Console.SetCursorPosition(colunaInicial, linhaAtual);
        Console.Write("═══════════════════════════════════════════════════════════════════════════════");
        linhaAtual++;
      }

      string descricaoTruncada = evento.GetDescricao().Length > 20
          ? evento.GetDescricao().Substring(0, 17) + "..."
          : evento.GetDescricao();

      Console.SetCursorPosition(colunaInicial, linhaAtual);
      Console.Write($"{evento.GetId(),-6} {descricaoTruncada,-20} {evento.GetData().ToString("dd/MM/yyyy"),-12} {evento.GetCategoriaEvento().ToString(),-12} {evento.GetStatus().ToString(),-15}");
      linhaAtual++;
    }

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write("═══════════════════════════════════════════════════════════════════════════════");
    linhaAtual++;

    Console.SetCursorPosition(colunaInicial, linhaAtual);
    Console.Write($"Total de eventos: {eventos.Count}");

    this.tela.MostrarMensagem("Pressione D para ver detalhes de um evento ou qualquer tecla para voltar...");
    var tecla = Console.ReadKey();

    if (tecla.Key == ConsoleKey.D)
    {
      this.MostrarDetalhesEvento();
    }
  }


  private void MostrarDetalhesEvento()
  {
    this.tela.PrepararTela("Detalhes do Evento");

    string idStr = this.tela.Perguntar("Digite o ID do evento: ");

    if (!int.TryParse(idStr, out int id))
    {
      this.tela.MostrarMensagem("ID inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    List<Evento> eventos = this.eventosService.ObterTodosEventos();
    Evento? eventoEncontrado = eventos.FirstOrDefault(e => e.GetId() == id);

    if (eventoEncontrado == null)
    {
      this.tela.MostrarMensagem("Evento não encontrado. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    // Exibir detalhes do evento
    int linhaAtual = 4;
    int colunaInicial = 10;

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"ID: {eventoEncontrado.GetId()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Descrição: {eventoEncontrado.GetDescricao()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Data: {eventoEncontrado.GetData().ToString("dd/MM/yyyy")}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Categoria: {eventoEncontrado.GetCategoriaEvento()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Capacidade Máxima: {eventoEncontrado.GetCapacidadeMaxima()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Status: {eventoEncontrado.GetStatus()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Local: {eventoEncontrado.GetLocal()}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Início: {eventoEncontrado.GetDataHoraInicio().ToString("dd/MM/yyyy HH:mm")}");

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write($"Fim: {eventoEncontrado.GetDataHoraFim().ToString("dd/MM/yyyy HH:mm")}");

    this.tela.MostrarMensagem("Pressione uma tecla para voltar...");
    Console.ReadKey();
  }

}

