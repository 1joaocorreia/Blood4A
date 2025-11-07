using System.ComponentModel.DataAnnotations.Schema;

namespace Blood4A.Models;

public class AberturaFechamento
{
    public int Id { get; set; } // EF framework specific property

    // FK -----------------------------------------------   
    public int referente_a { get; set; }
    // FK -----------------------------------------------   

    public string Horario_Abertura { get; set; }

    public string Horario_Fechamento { get; set; }

    public string Dia_Da_Semana { get; set; }

    // FK -----------------------------------------------    
    [ForeignKey("referente_a")]
    public Clinicas obj_referente_a { get; set; }
    // FK -----------------------------------------------

}
