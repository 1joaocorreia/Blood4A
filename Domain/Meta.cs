using System.ComponentModel.DataAnnotations.Schema;

public class Meta<T, TOBJ>
{
    [System.ComponentModel.DataAnnotations.Key]
    public required int id_meta {get; set;}
    // FK ------------------------------------
    public required T referente_a {get; set;}
    // FK ------------------------------------

    public required string condicao {get; set;}

    // FK ------------------------------------
    [ForeignKey("referente_a")]
    public TOBJ? obj_referente_a {get; set;}
    // FK ------------------------------------
}