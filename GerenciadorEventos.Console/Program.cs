Tela tela = new Tela();
EventosService eventosService = new EventosService();
EventoCRUD eventoCRUD = new EventoCRUD(tela, eventosService);

string opcao;
List<string> opcoes = new List<string>();
opcoes.Add("        Menu         ");
opcoes.Add("1 - Cadastrar Evento ");
opcoes.Add("2 - Listar Eventos   ");
opcoes.Add("0 - Sair             ");

while (true)
{
  tela.PrepararTela("Sistema de Gerenciamento de Eventos");
  opcao = tela.MostrarMenu(opcoes, 2, 2);

  if (opcao == "0") break;
  else if (opcao == "1") eventoCRUD.ExecutarCRUD();
  else if (opcao == "2") eventoCRUD.ListarEventos();
  else
  {
    tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
    Console.ReadKey();
  }
}
