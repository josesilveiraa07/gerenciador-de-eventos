public class EmprestimoCRUD
{
    public List<Emprestimo> emprestimos;
    public Emprestimo emprestimo;
    public int posicao;
    private List<string> dados;
    private int coluna, linha, largura;
    private int larguraDados, colunaDados, linhaDados;
    private Tela tela;
    private LivroCRUD livroCRUD;
    private AlunoCRUD alunoCRUD;




    public EmprestimoCRUD(Tela tela, LivroCRUD livroCRUD, AlunoCRUD alunoCRUD)
    {
        this.emprestimos = new List<Emprestimo>();
        this.emprestimo = new Emprestimo();
        this.posicao = -1;

        this.dados = new List<string>();
        this.dados.Add("ID              : ");
        this.dados.Add("ISBN            : ");
        this.dados.Add("Matrícula       : ");
        this.dados.Add("Data Empréstimo : ");
        this.dados.Add("Data Devolver   : ");
        this.dados.Add("Data Devolvido  : ");

        this.tela = tela;
        this.livroCRUD = livroCRUD;
        this.alunoCRUD = alunoCRUD;

        this.coluna = 10;
        this.linha = 5;
        this.largura = 55;

        this.larguraDados = this.largura - dados[0].Length - 2;
        this.colunaDados = this.coluna + dados[0].Length + 1;
        this.linhaDados = this.linha + 2;

        this.emprestimos.Add(new Emprestimo(1, "1", "2025001", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(5), null));
        this.emprestimos.Add(new Emprestimo(2, "2", "2025002", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(10), null));
    }



    public void ExecutarCRUD()
    {
        string opcao, resp;
        List<string> opcoesEmp = new List<string>();
        opcoesEmp.Add(" Empréstimos ");
        opcoesEmp.Add("1 - Emprestar");
        opcoesEmp.Add("2 - Devolver ");
        opcoesEmp.Add("3 - Listar   ");
        opcoesEmp.Add("0 - Sair     ");
        while (true)
        {
            opcao = tela.MostrarMenu(opcoesEmp, coluna, linha);
            if (opcao == "0") break;
            else if (opcao == "1" || opcao == "2")
            {
                this.coluna += 10;
                this.linha += 2;
                this.colunaDados += 10;
                this.linhaDados += 2;

                string titulo = (opcao == "1") ? "Registrar Empréstimo" : "Registrar Devolução";
                this.tela.MontarJanela(titulo, this.dados, this.coluna, this.linha, this.largura);
                this.EntrarDados(1);
                bool achou = this.ProcurarCodigo();

                if (opcao == "1" && achou)
                {
                    this.MostrarDados();
                    this.tela.MostrarMensagem("Empréstimo já existe. Pressione uma tecla para continuar...");
                    Console.ReadKey();
                    this.tela.MostrarMensagem("");
                }
                else if (opcao == "1" && !achou)
                {
                    bool dadosValidos = this.EntrarDados(2);
                    if (!dadosValidos) break;
                    resp = this.tela.Perguntar("Confirma cadastro (S/N) : ");
                    if (resp.ToLower() == "s")
                    {
                        this.emprestimos.Add(new Emprestimo(this.emprestimo.id, this.emprestimo.isbn, this.emprestimo.matricula, this.emprestimo.DataEmprestimo, this.emprestimo.DataDevolucao, null));
                        this.tela.MostrarMensagem("Empréstimo registrado com sucesso! Pressione uma tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else if (opcao == "2" && achou)
                {
                    this.emprestimos[this.posicao].DataDevolvido = DateTime.Now;
                    this.MostrarDados();
                    this.tela.MostrarMensagem("Devolução registrada com sucesso! Pressione uma tecla para continuar...");
                    Console.ReadKey();
                }
                this.tela.ApagarArea(this.coluna, this.linha, this.coluna + this.largura, this.linha + this.dados.Count + 2);

                this.coluna -= 10;
                this.linha -= 2;
                this.colunaDados -= 10;
                this.linhaDados -= 2;

            }
            else if (opcao == "3")
            {
                this.ListarEmprestimos();
                break;
            }
            else
            {
                tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
                Console.ReadKey();
            }
        }
    }


    public bool EntrarDados(int qual, bool alteracao = false)
    {
        if (qual == 1)
        {
            Console.SetCursorPosition(this.colunaDados, this.linhaDados);
            this.emprestimo.id = int.Parse(Console.ReadLine());
        }
        else
        {
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + 1);
            this.emprestimo.isbn = Console.ReadLine();
            string tituloLivro = this.livroCRUD.ObterTituloPorISBN(this.emprestimo.isbn);
            Console.SetCursorPosition(this.colunaDados + 4, this.linhaDados + 1);
            Console.Write(tituloLivro);

            if (string.IsNullOrWhiteSpace(tituloLivro))
            {
                this.tela.MostrarMensagem("Livro não encontrado. Pressione uma tecla para continuar...");
                Console.ReadKey();
                return false;
            }

            Console.SetCursorPosition(this.colunaDados, this.linhaDados + 2);
            this.emprestimo.matricula = Console.ReadLine();
            string nomeAluno = this.alunoCRUD.ObterNomePorMatricula(this.emprestimo.matricula);
            Console.SetCursorPosition(this.colunaDados + 4, this.linhaDados + 2);
            Console.Write(nomeAluno);
            if (string.IsNullOrWhiteSpace(nomeAluno))
            {
                this.tela.MostrarMensagem("Aluno não encontrado. Pressione uma tecla para continuar...");
                Console.ReadKey();
                return false;
            }

            Console.SetCursorPosition(this.colunaDados, this.linhaDados + 3);
            this.emprestimo.DataEmprestimo = DateTime.Parse(Console.ReadLine());
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + 4);
            this.emprestimo.DataDevolucao = DateTime.Parse(Console.ReadLine());
            this.emprestimo.DataDevolvido = null;
        }
        return true;
    }



    public void ListarEmprestimos()
    {
        this.tela.PrepararTela("Listagem de Empréstimos");
        this.tela.MostrarMensagem(1, 3, "ID   | ISBN     | Matrícula | Data Empréstimo | Data Devolução | Data Devolvido");
        this.tela.MostrarMensagem(1, 4, "-----+----------+-----------+-----------------+----------------+---------------");
        int linha =  5;
        for (int i = 0; i < this.emprestimos.Count; i++)
        {
            string dataDevolvidoStr = this.emprestimos[i].DataDevolvido.HasValue ? this.emprestimos[i].DataDevolvido.Value.ToString("dd/MM/yyyy") : "";
            Console.SetCursorPosition(1, linha);
            Console.Write(this.emprestimos[i].id);
            Console.SetCursorPosition(8, linha);
            Console.Write(this.emprestimos[i].isbn);
            Console.SetCursorPosition(19, linha);
            Console.Write(this.emprestimos[i].matricula);
            Console.SetCursorPosition(31, linha);
            Console.Write(this.emprestimos[i].DataEmprestimo.ToString("dd/MM/yyyy"));
            Console.SetCursorPosition(49, linha);   
            Console.Write(this.emprestimos[i].DataDevolucao.ToString("dd/MM/yyyy"));
            Console.SetCursorPosition(66, linha);
            Console.Write(dataDevolvidoStr);
            linha++;
        }
        this.tela.MostrarMensagem("");
        this.tela.MostrarMensagem("Pressione uma tecla para continuar...");
        Console.ReadKey();  
    }



    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.emprestimos.Count; i++)
        {
            if (this.emprestimo.id == this.emprestimos[i].id)
            {
                encontrei = true;
                this.posicao = i;
                break;
            }
        }
        return encontrei;
    }


    public void MostrarDados()
    {
        string livro = this.emprestimos[this.posicao].isbn + " - " + this.livroCRUD.ObterTituloPorISBN(this.emprestimos[this.posicao].isbn);
        string aluno = this.emprestimos[this.posicao].matricula + " - " + this.alunoCRUD.ObterNomePorMatricula(this.emprestimos[this.posicao].matricula);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 1, livro);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 2, aluno);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 3, this.emprestimos[this.posicao].DataEmprestimo.ToString("dd/MM/yyyy"));
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 4, this.emprestimos[this.posicao].DataDevolucao.ToString("dd/MM/yyyy"));
        string dataDevolvidoStr = this.emprestimos[this.posicao].DataDevolvido.HasValue ? this.emprestimos[this.posicao].DataDevolvido.Value.ToString("dd/MM/yyyy") : "";
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 5, dataDevolvidoStr);
    }


}
