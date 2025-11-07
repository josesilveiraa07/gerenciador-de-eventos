Tela tela = new Tela();
LivroCRUD livroCRUD = new LivroCRUD(tela);
AlunoCRUD alunoCRUD = new AlunoCRUD(tela);
EmprestimoCRUD emprestimoCRUD = new EmprestimoCRUD(tela, livroCRUD, alunoCRUD);

string opcao;
List<string> opcoes = new List<string>();
opcoes.Add("     Menu      ");
opcoes.Add("1 - Emprestimos");
opcoes.Add("2 - Livros     ");
opcoes.Add("3 - Alunos     ");
opcoes.Add("0 - Sair       ");

while (true)
{
    tela.PrepararTela("Sistema de Biblioteca Super Legal");
    opcao = tela.MostrarMenu(opcoes, 2, 2);

    if (opcao == "0") break;
    else if (opcao == "1") emprestimoCRUD.ExecutarCRUD();
    else if (opcao == "2") livroCRUD.ExecutarCRUD();
    else if (opcao == "3") alunoCRUD.ExecutarCRUD();
    else
    {
        tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
        Console.ReadKey();
    }
}
