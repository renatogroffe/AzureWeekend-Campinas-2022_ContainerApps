using Dapper.Contrib.Extensions;

namespace WorkerQuestao.Data;

[Table("dbo.HistoricoVotacao")]
public class VotoTecnologia
{
    [Key]
    public int Id { get; set; }
    public DateTime? DataProcessamento { get; set; }
    public string? Topico { get; set; }
    public int Particao { get; set; }
    public string? Producer { get; set; }
    public string? Consumer { get; set; }
    public string? ConsumerGroup { get; set; }
    public string? HorarioVoto { get; set; }
    public string? IdVoto { get; set; }
    public string? Tecnologia { get; set; }
}