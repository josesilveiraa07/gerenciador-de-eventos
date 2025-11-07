public class InscricoesService
{
    private readonly List<Inscricao> inscricoes = [];
    private readonly List<Inscricao> listaEspera = [];

    public void RegistrarInscricao(string participante, Evento evento, DateTime dataInscricao, DateTime dataReferencia)
    {
        if (!ValidarInscricao(participante, evento, dataInscricao, dataReferencia))
        {
            return;
        }

        Inscricao novaInscricao = new Inscricao(participante, evento, dataInscricao, dataReferencia);

        int vagasDisponiveis = CalcularVagasDisponiveis(evento);

        if (vagasDisponiveis > 0)
        {
            inscricoes.Add(novaInscricao);
            Console.WriteLine($"Inscrição confirmada para {participante} no evento {evento.GetDescricao()}");
        }
        else
        {
            novaInscricao.SetListaEspera(true);
            listaEspera.Add(novaInscricao);
            Console.WriteLine($"Evento lotado. {participante} adicionado à lista de espera.");
        }
    }

    private bool ValidarInscricao(string participante, Evento evento, DateTime dataInscricao, DateTime dataReferencia)
    {
        if (dataInscricao > DateTime.Now)
        {
            Console.WriteLine("Erro: Data de inscrição não pode ser superior à data atual.");
            return false;
        }

        int vagasDisponiveis = CalcularVagasDisponiveis(evento);
        
        if (vagasDisponiveis == 0 && listaEspera.Count(i => i.GetEvento().GetId() == evento.GetId()) >= evento.GetCapacidadeMaxima())
        {
            Console.WriteLine("Erro: Evento lotado e lista de espera cheia.");
            return false;
        }

        if (!VerificarEventosCompativeis(participante, evento))
        {
            Console.WriteLine("Erro: Inscrição múltipla só é permitida para eventos compatíveis.");
            return false;
        }

        return true;
    }

    private int CalcularVagasDisponiveis(Evento evento)
    {
        int inscricoesConfirmadas = inscricoes.Count(i => i.GetEvento().GetId() == evento.GetId());
        return evento.GetCapacidadeMaxima() - inscricoesConfirmadas;
    }

    private bool VerificarEventosCompativeis(string participante, Evento novoEvento)
    {
        var inscricoesParticipante = inscricoes.Where(i => i.GetParticipante() == participante).ToList();

        foreach (var inscricao in inscricoesParticipante)
        {
            Evento eventoExistente = inscricao.GetEvento();
            
            if (eventoExistente.GetData().Date == novoEvento.GetData().Date)
            {
                return false;
            }

            if (eventoExistente.GetCategoriaEvento().Equals(novoEvento.GetCategoriaEvento()) && 
                eventoExistente.GetData().Date == novoEvento.GetData().Date)
            {
                return false;
            }
        }

        return true;
    }

    public void ConfirmarInscricao(int inscricaoId)
    {
        Inscricao? inscricao = inscricoes.FirstOrDefault(i => i.GetId() == inscricaoId);

        if (inscricao == null)
        {
            Console.WriteLine("Inscrição não encontrada.");
            return;
        }

        Evento evento = inscricao.GetEvento();
        int vagasAtualizadas = CalcularVagasDisponiveis(evento);
        
        Console.WriteLine($"Inscrição confirmada. Vagas disponíveis atualizadas: {vagasAtualizadas}");
    }

    public List<Inscricao> ListarInscricoes()
    {
        return inscricoes;
    }

    public List<Inscricao> ListarListaEspera()
    {
        return listaEspera;
    }
}
