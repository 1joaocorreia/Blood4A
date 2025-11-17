using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blood4A.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agentes",
                columns: table => new
                {
                    Id_Agente = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome_Agente = table.Column<string>(type: "TEXT", nullable: false),
                    Cnpj_Agente = table.Column<string>(type: "TEXT", nullable: false),
                    Email_Agente = table.Column<string>(type: "TEXT", nullable: false),
                    Senha_Agente = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agentes", x => x.Id_Agente);
                });

            migrationBuilder.CreateTable(
                name: "Escolaridade",
                columns: table => new
                {
                    Id_Escolaridade = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Grau_Escolaridade = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao_Escolaridade = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolaridade", x => x.Id_Escolaridade);
                });

            migrationBuilder.CreateTable(
                name: "Localizacoes",
                columns: table => new
                {
                    Cep = table.Column<string>(type: "TEXT", nullable: false),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    Uf = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizacoes", x => x.Cep);
                });

            migrationBuilder.CreateTable(
                name: "Clinicas",
                columns: table => new
                {
                    Id_Clinica = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome_Clinica = table.Column<string>(type: "TEXT", nullable: false),
                    Cnpj_Clinica = table.Column<string>(type: "TEXT", nullable: false),
                    cep_location = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinicas", x => x.Id_Clinica);
                    table.ForeignKey(
                        name: "FK_Clinicas_Localizacoes_cep_location",
                        column: x => x.cep_location,
                        principalTable: "Localizacoes",
                        principalColumn: "Cep",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doadores",
                columns: table => new
                {
                    Id_Doador = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome_Doador = table.Column<string>(type: "TEXT", nullable: false),
                    Data_Nascimento_Doador = table.Column<string>(type: "TEXT", nullable: false),
                    Cpf_Doador = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone_Doador = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo_Sanguineo_Doador = table.Column<string>(type: "TEXT", nullable: false),
                    cep_location = table.Column<string>(type: "TEXT", nullable: false),
                    id_escolaridade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadores", x => x.Id_Doador);
                    table.ForeignKey(
                        name: "FK_Doadores_Escolaridade_id_escolaridade",
                        column: x => x.id_escolaridade,
                        principalTable: "Escolaridade",
                        principalColumn: "Id_Escolaridade",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doadores_Localizacoes_cep_location",
                        column: x => x.cep_location,
                        principalTable: "Localizacoes",
                        principalColumn: "Cep",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AberturaFechamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    referente_a = table.Column<int>(type: "INTEGER", nullable: false),
                    Horario_Abertura = table.Column<string>(type: "TEXT", nullable: false),
                    Horario_Fechamento = table.Column<string>(type: "TEXT", nullable: false),
                    Dia_Da_Semana = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AberturaFechamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AberturaFechamento_Clinicas_referente_a",
                        column: x => x.referente_a,
                        principalTable: "Clinicas",
                        principalColumn: "Id_Clinica",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doacoes",
                columns: table => new
                {
                    Id_Doacao = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_agente = table.Column<int>(type: "INTEGER", nullable: false),
                    id_doador = table.Column<int>(type: "INTEGER", nullable: false),
                    id_clinica = table.Column<int>(type: "INTEGER", nullable: false),
                    Data_Doacao = table.Column<string>(type: "TEXT", nullable: false),
                    Hora_Doacao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doacoes", x => x.Id_Doacao);
                    table.ForeignKey(
                        name: "FK_Doacoes_Agentes_id_agente",
                        column: x => x.id_agente,
                        principalTable: "Agentes",
                        principalColumn: "Id_Agente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doacoes_Clinicas_id_clinica",
                        column: x => x.id_clinica,
                        principalTable: "Clinicas",
                        principalColumn: "Id_Clinica",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doacoes_Doadores_id_doador",
                        column: x => x.id_doador,
                        principalTable: "Doadores",
                        principalColumn: "Id_Doador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AberturaFechamento_referente_a",
                table: "AberturaFechamento",
                column: "referente_a");

            migrationBuilder.CreateIndex(
                name: "IX_Clinicas_cep_location",
                table: "Clinicas",
                column: "cep_location");

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_id_agente",
                table: "Doacoes",
                column: "id_agente");

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_id_clinica",
                table: "Doacoes",
                column: "id_clinica");

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_id_doador",
                table: "Doacoes",
                column: "id_doador");

            migrationBuilder.CreateIndex(
                name: "IX_Doadores_cep_location",
                table: "Doadores",
                column: "cep_location");

            migrationBuilder.CreateIndex(
                name: "IX_Doadores_id_escolaridade",
                table: "Doadores",
                column: "id_escolaridade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AberturaFechamento");

            migrationBuilder.DropTable(
                name: "Doacoes");

            migrationBuilder.DropTable(
                name: "Agentes");

            migrationBuilder.DropTable(
                name: "Clinicas");

            migrationBuilder.DropTable(
                name: "Doadores");

            migrationBuilder.DropTable(
                name: "Escolaridade");

            migrationBuilder.DropTable(
                name: "Localizacoes");
        }
    }
}
