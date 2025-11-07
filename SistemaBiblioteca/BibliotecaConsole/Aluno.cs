public class Aluno
{
    // propriedades
    public string matricula;
    public string nome;
    public string email;
    public string telefone;
    public string curso;


    // construtor
    public Aluno()
    {
        this.matricula = "";
        this.nome = "";
        this.email = "";
        this.telefone = "";
        this.curso = "";
    }


    public Aluno(string matricula, string nome, string email, string telefone, string curso)
    {
        this.matricula = matricula;
        this.nome = nome;
        this.email = email;
        this.telefone = telefone;
        this.curso = curso;
    }
}