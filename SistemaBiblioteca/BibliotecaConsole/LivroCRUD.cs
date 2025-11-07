public class LivroCRUD
{
    //
    // Propriedades
    //
    private List<Livro> livros;
    private Livro livro;
    private int posicao;
    private List<string> dados = new List<string>();
    private int coluna, linha, largura;
    private int larguraDados, colunaDados, linhaDados;
    private Tela tela;


    //
    // Métodos
    //
    public LivroCRUD(Tela tela)
    {
        // propriedades para o CRUD
        this.livros = new List<Livro>(); // inicializa a coleção de livros
        this.livro = new Livro();        // inicializa o objeto para manipulação dos dados de UM livro
        this.posicao = -1;               // inicializa o "ponteiro" da coleção de livros

        // inicializa o vetor com as pergunras de livro
        this.dados.Add("ISBN    : ");
        this.dados.Add("Título  : ");
        this.dados.Add("Autor   : ");
        this.dados.Add("Ano     : ");
        this.dados.Add("Páginas : ");

        // indica para LivroCRUD onde está o objeto tela
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
        this.livros.Add(new Livro("1", "Aprendendo C#", "João da Silva", 2020, 350));
        this.livros.Add(new Livro("2", "Programação Avançada", "Maria Oliveira", 2021, 500));
    }


    public void ExecutarCRUD()
    {
        string resp;

        // montar a janela do CRUD
        this.tela.MontarJanela("Cadastro de Livros", this.dados, this.coluna, this.linha, this.largura);

        // algoritmo CRUD
        this.EntrarDados(1);
        bool achou = this.ProcurarCodigo();
        if (!achou)
        {
            resp = this.tela.Perguntar("ISBN não encontrado. Deseja cadastrar (S/N) : ");
            if (resp.ToLower() == "s")
            {
                this.EntrarDados(2);
                resp = this.tela.Perguntar("Confirma cadastro (S/N) : ");
                if (resp.ToLower() == "s")
                {
                    this.livros.Add(
                        new Livro(this.livro.isbn, this.livro.titulo, this.livro.autor, this.livro.anoPublicacao, this.livro.paginas)
                    );
                }
            }
        }
        else // se achou // if (achou)
        {
            this.MostrarDados();
            resp = this.tela.Perguntar("Deseja alterar, excluir ou voltar (A/E/V) : ");
            if (resp.ToLower() == "a")
            {
                this.tela.MontarJanela("Alteração de Livro", this.dados, this.coluna, this.linha + this.dados.Count + 2, this.largura);
                this.tela.MostrarMensagem("Informe os novos dados");
                this.EntrarDados(2, true);
                resp = this.tela.Perguntar("Confirma alteração (S/N) : ");
                if (resp.ToLower() == "s")
                {
                    this.livros[this.posicao].titulo = this.livro.titulo;
                    this.livros[this.posicao].autor = this.livro.autor;
                    this.livros[this.posicao].anoPublicacao = this.livro.anoPublicacao;
                    this.livros[this.posicao].paginas = this.livro.paginas;
                }
            }
            else if (resp.ToLower() == "e")
            {
                resp = this.tela.Perguntar("Confirma exclusão (S/N) : ");
                if (resp.ToLower() == "s")
                {
                    this.livros.RemoveAt(this.posicao);
                }
            }
        }
    }


    public void EntrarDados(int qual, bool alteracao = false)
    {
        if (qual == 1)
        {
            Console.SetCursorPosition(this.colunaDados, this.linhaDados);
            this.livro.isbn = Console.ReadLine();
        }
        else
        {
            // se for alteração, desloca a linha para a "segunda tela"
            int deslocamentoLinha = alteracao ? this.dados.Count + 2 : 0; 
            
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 1);
            this.livro.titulo = Console.ReadLine();
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 2);
            this.livro.autor = Console.ReadLine();
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 3);
            this.livro.anoPublicacao = int.Parse(Console.ReadLine());
            Console.SetCursorPosition(this.colunaDados, this.linhaDados + deslocamentoLinha + 4);
            this.livro.paginas = int.Parse(Console.ReadLine());
        }
    }


    public bool ProcurarCodigo()
    {
        bool encontrei = false;
        for (int i = 0; i < this.livros.Count; i++)
        {
            if (this.livro.isbn == this.livros[i].isbn)
            {
                encontrei = true;
                this.posicao = i;
                break;
            }
        }
        return encontrei;
    }


    public string ObterTituloPorISBN(string isbn)
    {
        this.livro.isbn = isbn;
        bool achou = this.ProcurarCodigo();
        if (achou)
        {
            return this.livros[this.posicao].titulo;
        }
        else
        {
            return "";
        }
    }


    public void MostrarDados()
    {
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 1, this.livros[this.posicao].titulo);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 2, this.livros[this.posicao].autor);
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 3, this.livros[this.posicao].anoPublicacao.ToString());
        this.tela.MostrarMensagem(this.colunaDados, this.linhaDados + 4, this.livros[this.posicao].paginas.ToString());
    }

}

