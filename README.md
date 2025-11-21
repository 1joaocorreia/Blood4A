College stuff. Just ignore it ☺️

# Como executar a aplicação corretamente
Como esse programa tem enorme dependência por dados, você precisa primeiro criar um banco de dados e inicializa-lo.

## Criando e Carregando o Banco de Dados
1. Para começar, entre na pasta "Database/" e crie um arquivo SQLite com o exato nome "database.db" (ou o nome que você desejar, mas terá que indicar esse nome no appsettings.json em "ConnectionStrings", "DefaultConnection")
   
2. Em seguida, execute `sqlite3 database.db` para entrar começar a interagir com o banco de dados diretamente e use o comando `.read bd__bloodf4A.txt` para carregar o schema das tabelas.

3. Finalmente, use o utilitário localizado em "Utilitary/DatabaseLoader" para popular o banco de dados.

| Dentro de Utilitary/DatabaseLoader, execute:

- `go build`
- `./database_loader -sqlite ../../Database/database.db`

Após conclusão, o banco de dados estará criado e com dados fictícios carregados.

Abaixo temos disponível um tutorial simples em vídeo de como fazer tudo isso:

[Click Here](Data/como_carregar_o_banco_de_dados.mp4)

## Executando
Vá para a pasta root do projeto (Blood4A/) e execute:

- `dotnet restore` para instalar as dependencias necessárias

Em seguida, execute:
- `dotnet run` para rodar o projeto

Você talvez receberá algumas mensagens de aviso, mas ignore-as.

Agora, basta visitar https://localhost:7298 e avaliar o projeto!
