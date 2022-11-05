using Microsoft.Data.SqlClient;
using Dapper.Contrib.Extensions;
using WorkerQuestao.Models;

namespace WorkerQuestao.Data;

public class VotacaoRepository
{
    private readonly IConfiguration _configuration;

    public VotacaoRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Save(Voto voto, int partition)
    {
        using var conexao = new SqlConnection(
            _configuration.GetConnectionString("BaseVotacao"));
        conexao.Insert<VotoTecnologia>(new()
        {
            DataProcessamento = DateTime.UtcNow.AddHours(-3),
            Topico = _configuration["ApacheKafka:Topic"],
            Particao = partition,
            Producer = voto.Producer,
            Consumer = Environment.MachineName,
            ConsumerGroup = _configuration["ApacheKafka:GroupId"],
            HorarioVoto = voto.Horario,
            IdVoto = voto.IdVoto,
            Tecnologia = voto.Tecnologia
        });
    }
}