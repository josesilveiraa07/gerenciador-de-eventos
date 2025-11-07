public class Emprestimo
{
    public int id;
    public string isbn;
    public string matricula;
    public DateTime DataEmprestimo;
    public DateTime DataDevolucao;
    public DateTime? DataDevolvido;




    public Emprestimo(int id, string isbn, string matricula, DateTime dataEmprestimo, DateTime dataDevolucao, DateTime? dataDevolvido)
    {
        this.id = id;
        this.isbn = isbn;
        this.matricula = matricula;
        this.DataEmprestimo = dataEmprestimo;
        this.DataDevolucao = dataDevolucao;
        this.DataDevolvido = dataDevolvido;
    }



    public Emprestimo()
    {
        this.id = 0;
        this.isbn = "";
        this.matricula = "";
        this.DataEmprestimo = DateTime.MinValue;
        this.DataDevolucao = DateTime.MinValue;
        this.DataDevolvido = DateTime.MinValue;
    }
}