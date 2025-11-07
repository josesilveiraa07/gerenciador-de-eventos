public class Livro
{
    // propriedades
    public string titulo;
    public string autor;
    public string isbn;
    public int anoPublicacao;
    public int paginas;


    //
    // métodos
    //
    public Livro(string cod, string tit, string aut, int ano, int pag)
    {
        isbn = cod;
        titulo = tit;
        autor = aut;
        anoPublicacao = ano;
        paginas = pag;
    }

    public Livro()
    {
        this.isbn = "";
        this.titulo = "";
        this.autor = "";
        this.anoPublicacao = 0;
        this.paginas = 0;
    }


    public void ImprimirDados()
    {
        Console.WriteLine("Dados do livro:");
        Console.WriteLine($"ISBN        : {isbn}");
        Console.WriteLine($"Título      : {titulo}");
        Console.WriteLine($"Autor       : {autor}");
        Console.WriteLine($"Ano de Pub. : {anoPublicacao}");
        Console.WriteLine($"Num de pág. : {paginas}");
        Console.WriteLine("");
    }

}