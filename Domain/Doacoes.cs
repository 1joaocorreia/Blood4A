using System.ComponentModel.DataAnnotations.Schema;


namespace Blood4A.Domain;

public class Doacoes
{
    [System.ComponentModel.DataAnnotations.Key]
    public int id_doacao { get; set; }

    // FK -----------------------------------------------
    public required int id_agente { get; set; }

    public required int id_doador { get; set; }

    public required int id_clinica { get; set; }
    // FK -----------------------------------------------

    public required string data_doacao { get; set; }

    public required string hora_doacao { get; set; }

    // FK -----------------------------------------------
    [ForeignKey("id_agente")]
    public Agentes? obj_id_agente { get; set; }

    [ForeignKey("id_doador")]
    public Doadores? obj_id_doador { get; set; }

    [ForeignKey("id_clinica")]
    public Clinicas? obj_id_clinica { get; set; }
    // FK -----------------------------------------------

}