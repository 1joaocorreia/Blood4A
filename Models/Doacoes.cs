using System.ComponentModel.DataAnnotations.Schema;

namespace Blood4A.Models;

public class Doacoes
{
    [System.ComponentModel.DataAnnotations.Key]
    public int Id_Doacao { get; set; }

    // FK -----------------------------------------------
    public int id_agente { get; set; }

    public int id_doador { get; set; }

    public int id_clinica { get; set; }
    // FK -----------------------------------------------

    public string Data_Doacao { get; set; }

    public string Hora_Doacao { get; set; }

    // FK -----------------------------------------------
    [ForeignKey("id_agente")]
    public Agentes obj_id_agente { get; set; }

    [ForeignKey("id_doador")]
    public Doadores obj_id_doador { get; set; }

    [ForeignKey("id_clinica")]
    public Clinicas obj_id_clinica { get; set; }
    // FK -----------------------------------------------

}