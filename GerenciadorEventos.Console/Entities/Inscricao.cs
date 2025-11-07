public class Inscricao
{
    private int id;
    private string participante;
    private Evento evento;
    private DateTime dataInscricao;
    private DateTime dataReferencia;
    private bool estaEmListaEspera;

    public Inscricao(string participante, Evento evento, DateTime dataInscricao, DateTime dataReferencia)
    {
        this.id = new Random().Next(1, 10000);
        this.participante = participante;
        this.evento = evento;
        this.dataInscricao = dataInscricao;
        this.dataReferencia = dataReferencia;
        this.estaEmListaEspera = false;
    }

    public int GetId()
    {
        return id;
    }

    public string GetParticipante()
    {
        return participante;
    }

    public Evento GetEvento()
    {
        return evento;
    }

    public DateTime GetDataInscricao()
    {
        return dataInscricao;
    }

    public DateTime GetDataReferencia()
    {
        return dataReferencia;
    }

    public bool EstaEmListaEspera()
    {
        return estaEmListaEspera;
    }

    public void SetListaEspera(bool valor)
    {
        this.estaEmListaEspera = valor;
    }
}
