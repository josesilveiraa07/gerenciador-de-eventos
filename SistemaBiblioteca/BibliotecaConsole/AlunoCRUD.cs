public class AlunoCRUD
{
    //
    // Propriedades
    //
    private List<Aluno> alunos;
    private Aluno aluno;
    private int posicao;
    private List<string> dados = new List<string>();
    private int coluna, linha, largura;
    private int larguraDados, colunaDados, linhaDados;
    private Tela tela;


    //
    // Métodos
    //
    public AlunoCRUD(Tela tela)
    {
        // propriedades para o CRUD
        this.alunos = new List<Aluno>(); // inicializa a coleção de alunos
        this.aluno = new Aluno();        // inicializa o objeto para manipulação dos dados de UM aluno
        this.posicao = -1;               // inicializa o "ponteiro" da coleção de alunos

        // inicializa o vetor com as pergunras de aluno
        this.dados.Add("Matrícula : ");
        this.dados.Add("Nome      : ");
        this.dados.Add("E-mail    : ");
        this.dados.Add("Telefone  : ");
        this.dados.Add("Curso     : ");

        // indica para AlunoCRUD onde está o objeto tela
        this.tela = tela;

        // define a posição e largura da janela
        this.coluna = 15;
        this.linha = 5;
        this.largura = 50;

        // calcula a área dos dados
        this.larguraDados = this.largura - dados[0].Length - 2;
        this.colunaDados = this.coluna + dados[0].Length + 1;
        this.linhaDados = this.linha + 2;


        // inclusão de dados iniciais para teste
        this.alunos.Add(new Aluno("2025001", "Ana Souza", "ana@gmail.com", "123", "Engenharia"));
        this.alunos.Add(new Aluno("2025002", "Bruno Lima", "bruno@uol.com", "456", "Psicologia"));
    }



    public void ExecutarCRUD()
    {
        string resp;

        // montar a janela do CRUD
        this.tela.MontarJanela("Cadastro de Alunos", this.dados, this.coluna, this.linha, this.largura);

        // algoritmo CRUD
        this.EntrarDados(1);
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            resp = this.tela.Perguntar("Matrícula não encontrada. Deseja cadastrar (S/N) : ");
            if (resp.ToLower() == "s")
            {
                this.EntrarDados(2);
                resp = this.tela.Perguntar("Confirma cadastro (S/N) : ");
                if (resp.ToLower() == "s")
                {
                    this.alunos.Add(
                        new Aluno(this.aluno.matricula, this.aluno.nome, this.aluno.email, this.aluno.telefone, this.aluno.curso)
                    );
                }
            }
        }
        else // if (achou)
        {
            this.MostrarDados();
            resp = this.tela.Perguntar("Deseja alterar, excluir ou voltar (A/E/V) : ");
            if (resp.ToLower() == "a")
            {
                this.tela.MontarJanela("Alteração de Aluno", this.dados, this.coluna, this.linha + this.dados.Count + 2, this.largura);
                this.tela.MostrarMensagem("Informe os novos dados");
                this.EntrarDados(2, true);
                resp = this.tela.Perguntar("Confirma alteração (S/N) : ");
                if (resp.ToLower() == "s")
                {
                    this.alunos[this.posicao] = this.aluno;
                    this.tela.MostrarMensagem("Aluno alterado com sucesso! Pressione uma tecla para continuar...");
                    Console.ReadKey();
                }
            }
            else if (resp.ToLower() == "e")
            {
                resp = this.tela.Perguntar("Confirma exclusão (S/N) : ");
                if (resp.ToLower() == "s")
                {
                    this.alunos.RemoveAt(this.posicao);
                    this.tela.MostrarMensagem("Aluno excluído com sucesso! Pressione uma tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }


    public void EntrarDados(int qual, bool alteracao = false)
    {
        if (qual == 1)
        {
            Console.SetCursorPosition(this.colunaDados, this.linhaDados);
            this.aluno.matricula = Console.ReadLine();
        }
        else
        {
            // se for alteração, desloca a linha para a "segunda tela"
            int deslocamentoLinha = alteracao ? this.dados.Count + 2 : 0; 
            
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 1);
            this.aluno.nome = Console.ReadLine();
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 2);
            this.aluno.email = Console.ReadLine();
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 3);
            this.aluno.telefone = Console.ReadLine();
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 4);
            this.aluno.curso = Console.ReadLine();
        }
    }


    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.alunos.Count; i++)
        {
            if (this.aluno.matricula == this.alunos[i].matricula)
            {
                encontrei = true;
                this.posicao = i;
                break;
            }
        }
        return encontrei;
    }



    public string ObterNomePorMatricula(string matricula)
    {
        this.aluno.matricula = matricula;
        bool achou = this.ProcurarCodigo();
        if (achou)
        {
            return this.alunos[this.posicao].nome;
        }
        else
        {
            return "";
        }
    }


    public void MostrarDados()
    {
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 1, this.alunos[this.posicao].nome);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 2, this.alunos[this.posicao].email);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 3, this.alunos[this.posicao].telefone);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 4, this.alunos[this.posicao].curso);
    }

}