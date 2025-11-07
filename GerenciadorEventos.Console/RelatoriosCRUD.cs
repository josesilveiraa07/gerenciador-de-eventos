public class RelatoriosCRUD
{
  private NotificacoesService notificacoesService;
  private Tela tela;

  public RelatoriosCRUD(Tela tela, NotificacoesService notificacoesService)
  {
    this.tela = tela;
    this.notificacoesService = notificacoesService;
  }

  public void ExecutarCRUD()
  {
    string opcao;
    List<string> opcoes = new List<string>();
    opcoes.Add("  Menu Relatórios/Alertas  ");
    opcoes.Add("1 - Participação Mensal    ");
    opcoes.Add("2 - Taxa de Engajamento    ");
    opcoes.Add("3 - Ver Alertas            ");
    opcoes.Add("0 - Voltar                 ");

    while (true)
    {
      this.tela.PrepararTela("Relatórios e Notificações");
      opcao = this.tela.MostrarMenu(opcoes, 2, 4);

      if (opcao == "0") break;
      else if (opcao == "1") this.GerarRelatorioParticipacaoMensal();
      else if (opcao == "2") this.GerarRelatorioTaxaEngajamento();
      else if (opcao == "3") this.VerAlertas();
      else
      {
        this.tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
        Console.ReadKey();
      }
    }
  }

  private void GerarRelatorioParticipacaoMensal()
  {
    this.tela.PrepararTela("Relatório de Participação Mensal");

    int linhaAtual = 4;
    int colunaInicial = 10;

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write("Digite o mês (1-12): ");
    string? mesStr = Console.ReadLine();

    Console.SetCursorPosition(colunaInicial, linhaAtual++);
    Console.Write("Digite o ano (ex: 2024): ");
    string? anoStr = Console.ReadLine();

    if (!int.TryParse(mesStr, out int mes) || mes < 1 || mes > 12)
    {
      this.tela.MostrarMensagem("Mês inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    if (!int.TryParse(anoStr, out int ano) || ano < 2000 || ano > 2100)
    {
      this.tela.MostrarMensagem("Ano inválido. Pressione uma tecla...");
      Console.ReadKey();
      return;
    }

    this.notificacoesService.GerarRelatorioParticipacaoMensal(mes, ano);
  }

  private void GerarRelatorioTaxaEngajamento()
  {
    this.notificacoesService.GerarRelatorioTaxaEngajamento();
  }

  private void VerAlertas()
  {
    this.notificacoesService.VerificarAlertasAutomaticos();
  }
}

