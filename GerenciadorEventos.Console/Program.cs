Tela tela = new Tela();
EventosService eventosService = new EventosService();
InscricoesService inscricoesService = new InscricoesService();
PresencaService presencaService = new PresencaService();
NotificacoesService notificacoesService = new NotificacoesService(eventosService, inscricoesService, presencaService);

EventoCRUD eventoCRUD = new EventoCRUD(tela, eventosService);
InscricaoCRUD inscricaoCRUD = new InscricaoCRUD(tela, inscricoesService, eventosService);
PresencaCRUD presencaCRUD = new PresencaCRUD(tela, presencaService, inscricoesService, eventosService);
RelatoriosCRUD relatoriosCRUD = new RelatoriosCRUD(tela, notificacoesService);

notificacoesService.VerificarAlertasAutomaticos();

string opcao;
List<string> opcoes = new List<string>();
opcoes.Add("        Menu         ");
opcoes.Add("1 - Cadastrar Evento ");
opcoes.Add("2 - Listar Eventos   ");
opcoes.Add("3 - Inscrições       ");
opcoes.Add("4 - Presenças        ");
opcoes.Add("5 - Relatórios       ");
opcoes.Add("0 - Sair             ");

while (true)
{
  tela.PrepararTela("Sistema de Gerenciamento de Eventos");
  opcao = tela.MostrarMenu(opcoes, 2, 2);

  if (opcao == "0") break;
  else if (opcao == "1") eventoCRUD.ExecutarCRUD();
  else if (opcao == "2") eventoCRUD.ListarEventos();
  else if (opcao == "3") inscricaoCRUD.ExecutarCRUD();
  else if (opcao == "4") presencaCRUD.ExecutarCRUD();
  else if (opcao == "5") relatoriosCRUD.ExecutarCRUD();
  else
  {
    tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
    Console.ReadKey();
  }
}
