# Projeto de Consulta de APIs

## Descrição do projeto
Este projeto é um serviço em **C#** desenvolvido para realizar consultas periódicas às APIs do **IBGE** e do **Climatempo**. Os dados retornados por essas consultas são armazenados em um banco de dados **SQLite**. O projeto utiliza injeção de dependências e padrões de projeto como **Factory** e **Repository**, para garantir uma arquitetura limpa e modular. Além disso, o projeto posssui uma implementação de logging utilizando a biblioteca **Serilog**, os logs são gravados em um arquivo e também exibidos no console. 

## Instruções para configuração e Execução

### Pré-requisitos
- .NET SKD 6.0 ou superior instalado.
- Acesso à internet para consultar as APIs.
- SQLite para armazenar os dados localmente.

### Configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/debtil/VisualSoftwareTeste.git
   cd VisualSoftwareTeste

2. Restaure as dependências do projeto:
     ```bash
     dotnet restore

3. Compile o projeto:
   ```bash
   dotnet build

4. Execute o projeto:
   ```bash
   dotnet run

### Serviço Windows
Para executar o projeto por meio de um serviço windows, será necessário alguns passos a mais.

1. **Abrir o Agendador de Tarefas**:
   - Na barra de pesquisa do Windows, digite **Agendador de Tarefas** e pressione **Enter** para abrir a aplicação.
2. **Criar uma Nova Tarefa**:
   - No painel direito do Agendador de Tarefas, clique em **Criar Tarefa**.
   - Na janela que se abre, forneça um nome descritivo para a tarefa, como "Executar Serviço de Integração", e adicione uma descrição, por exemplo, "Esta tarefa executará a integração com diferentes APIs.
3. **Configurar Disparadores**:
   - Clique na aba **Disparadores** e, em seguida, clique em **Novo**.
   - No menu de configurações avançadas, marque a opção **Repetir a tarefa a cada:** e selecione o intervalo de tempo desejado.
   - Após ajustar as configurações, clique em **OK** para salvar o disparador.
4. **Definir Ações da Tarefa**:
   - Vá para a aba **Ações** e clique em **Novo**.
   - No campo **Programa/script**, navegue até o arquivo **.bat** contido neste repositório.
   - Depois de selecionar o arquivo, clique em **OK** para confirmar a ação.

5. **Executar a Tarefa**:
   - Na aba **Tarefas Ativas**, localize a tarefa que você acabou de criar. Clique duas vezes sobre ela com o botão esquerdo do mouse para abrir uma nova aba.
   - Em seguida, clique com o botão direito do mouse sobre a tarefa e selecione a opção **Executar**. Isso iniciará a execução da tarefa.

## Justificativa dos Padrões de Projeto Utilizados

### Factory:
É utilizado para encapsular a lógica de criação de objetos. No projeto ele é usado na classe ApiClientFactory, que centraliza a criação das instânicas do HttpClient para acessar as APIs.
Escolhi ele pois permite a criação isolada dos Http Clients, o que facilita uma futura manutenção e possíveis testes. Além disso, deste modo todas as instâncias ficam configuradas do mesmo jeito.

### Padrão Repository
É implementado nas classes da pasta repositories, abstraindo a lógica de acesso aos dados e fornecendo uma interface para interagir com o banco de dados.
Escolhi o Repository pois assim o código que manipula os dados fica separado do restante da lógica de negócio, facilitando a compreensão do fluxo da aplicação. Deste modo também facilita em futuras alterações e testes do código.

## Vídeo explicativo:
O vídeo onde explico o código desenvolvido pode ser encontrado no link a seguir: https://drive.google.com/file/d/1c2z0Ge1jczfTwo0tyW8CagN1XQ47c3YL/view?usp=sharing
