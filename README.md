# BlazorWasmSuperHeroApi

Prática das páginas 8 a 37 do material **13.net_blazor_vscode.pdf**, implementada em Blazor WebAssembly com .NET 10.

## O que foi solicitado

| Páginas | Atividade e implementação |
| --- | --- |
| 8 e 9 | Criar o projeto Blazor WebAssembly com o nome BlazorWasmSuperHeroApi. Os arquivos estão diretamente na raiz solicitada. |
| 10 e 11 | Obter um token de acesso à SuperHero API. Esta versão usa o token de exemplo do material, validado durante a implementação. |
| 12 a 22 | Criar as classes que representam os dados retornados, incluindo biografia, aparência, atributos, trabalho, conexões e imagem. |
| 23 a 25 | Implementar a busca na Home, com campo de nome, botão Pesquisar, carregamento, resultados e mensagens de erro. |
| 26 a 33 | Executar o projeto e diagnosticar falhas de requisição no navegador. |
| 34 a 37 | Usar a URL corrigida com www e api.php e exibir os resultados em cartões. |

Os cartões apresentam imagem, nome, nome completo, editora, primeira aparição e os seis atributos de Powerstats. Inteligência, força, velocidade e combate correspondem aos campos mostrados no exemplo. Resistência e poder completam os atributos do modelo. A busca funciona pelo botão e pela tecla Enter.

A criação e publicação no GitHub e a postagem no Google Classroom ficam para entrega manual.

## Como executar

É necessário ter o SDK do .NET 10 e acesso à internet para restaurar os pacotes, consultar a API e carregar as imagens.

Abra o terminal na pasta do projeto e execute:

```powershell
cd "C:\Users\lucas\OneDrive\Documentos\vs code uni\pratica-heroiapi"
dotnet run
```

Abra [http://localhost:5008](http://localhost:5008). O endereço está definido em `Properties/launchSettings.json`. Para encerrar, pressione `Ctrl+C`.

Para verificar a compilação, execute `dotnet build`. Para usar outra porta, execute `dotnet run --urls http://localhost:5018` e abra o endereço exibido no terminal.

## Configuração do token

O token fica na propriedade `SuperHeroApi:AccessToken` do arquivo `wwwroot/appsettings.json`. Ele foi extraído do exemplo da página 11 do material.

Para usar um token próprio, acesse a [SuperHero API](https://superheroapi.com/), faça login pela opção GitHub, copie o token fornecido e substitua apenas o valor de `AccessToken`. Salve e recarregue a aplicação. Esse login serve para obter acesso à API.

Como o projeto roda em WebAssembly no navegador, essa configuração e o token usado nas consultas são visíveis ao visitante. O token de exemplo depende da disponibilidade e validade mantidas pelo serviço.

## Organização e funcionamento

- `Program.cs` inicia o Blazor e registra o HttpClient com limite de 30 segundos por consulta e o serviço SuperHeroService.
- `Services/SuperHeroService.cs` lê o token da configuração, codifica o nome digitado para uso na URL e consulta `https://www.superheroapi.com/api.php/{token}/search/{nome}`. A resposta é desserializada com `GetFromJsonAsync`.
- `Models/SuperHeroResponse.cs` representa o envelope da pesquisa, com `response`, `results-for`, `results` e `error`.
- `Models/SuperHero.cs` representa cada personagem. Os outros sete arquivos de modelos representam `Powerstats`, `Biography`, `Appearance`, `Work`, `Connections` e `ImageInfo`, além do envelope já citado.
- Os atributos `JsonPropertyName` ligam propriedades C# aos nomes que contêm hífen no JSON, como `full-name`, `first-appearance` e `group-affiliation`. Os atributos de poder são textos porque a API também pode devolver o texto `"null"`.
- `Pages/Home.razor` mantém o nome digitado, o termo da última pesquisa, a lista de resultados e o estado de carregamento. As chamadas usam `async/await`. Durante a consulta, os controles ficam desabilitados para evitar requisições sobrepostas.
- `Components/HeroCard.razor` exibe cada personagem. A imagem vem primeiro de `image.url`. Quando o servidor original bloqueia o carregamento, o serviço consulta o mesmo ID no catálogo público de akabab, confere o ID e o nome e tenta a URL da imagem retornada. Se ambas as fontes falharem, exibe “Imagem indisponível”. Campos ausentes recebem a indicação “Não informado”.
- `wwwroot/css/app.css` organiza os cartões em uma, duas ou três colunas conforme a largura da tela.

## Tratamento de erros

Uma busca em branco fica desabilitada. Quando a API informa que o nome não foi encontrado, a página mostra a mensagem de ausência de resultados. Falhas HTTP, demora na resposta, JSON inválido e token recusado recebem mensagens próprias e opção de tentar novamente. O estado de carregamento sempre é encerrado em `finally`.

O endereço com `www.superheroapi.com/api.php` aplica a correção demonstrada no PDF. Redirecionamentos e permissões de CORS dependem do servidor da API. Para diagnosticar uma falha, abra as ferramentas do navegador com F12 e consulte a aba Network/Rede.

## Como conferir o resultado

1. Pesquise `Batman`. A API deve listar as versões disponíveis do personagem, com nome completo, editora, imagem, atributos e primeira aparição.
2. Pesquise `Superman` pressionando Enter.
3. Digite um nome inexistente e confirme a mensagem de ausência de resultados.
4. Confira a página em uma janela estreita para verificar a organização dos cartões.
5. Para simular uma falha de conexão, use o modo Offline das ferramentas de desenvolvimento, faça uma busca, retorne ao modo Online e clique em Tentar novamente.

Os resultados e as imagens podem mudar conforme o conteúdo e a disponibilidade do serviço externo. A referência da integração está na [documentação oficial da SuperHero API](https://superheroapi.com/).

Durante a implementação, o servidor das imagens originais retornou HTTP 403. Por isso, foi acrescentada a tentativa pelo [catálogo público de akabab](https://akabab.github.io/superhero-api/), derivado da SuperHero API. Essa adaptação consulta `https://akabab.github.io/superhero-api/api/id/{id}.json` exclusivamente para obter uma imagem alternativa. As pesquisas e os dados dos cartões continuam vindo do endpoint apresentado no PDF.
