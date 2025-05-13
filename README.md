# 🎬 NovassatMovies

**NovassatMovies** é um aplicativo mobile multiplataforma desenvolvido em .NET MAUI como parte de um teste técnico para a empresa **Auvo Tecnologia**. O projeto tem como objetivo consumir a API do TMDb (The Movie Database), exibindo listas de filmes populares e recentes, além de permitir a visualização de detalhes completos de cada título.

---

## 📱 Tecnologias e Arquitetura

| Camada           | Tecnologia / Padrão                         |
|------------------|---------------------------------------------|
| UI               | .NET MAUI                                   |
| Arquitetura      | MVVM (Model-View-ViewModel) com CommunityToolkit |
| Navegação        | Shell Navigation (AppShell)                 |
| HTTP Client      | [Flurl.Http](https://flurl.dev)             |
| Armazenamento    | SQLite (persistência local)                 |
| Injeção de Dependência | .NET MAUI Extensions (ServiceCollection) |
| Estilo / Temas   | Estilos globais em `App.xaml`               |

---

## 🧩 Funcionalidades

- ✔️ Exibição de **filmes populares**
- 🔍 Busca de filmes por **título ou palavra-chave**
- 🎞️ Tela de **detalhes** com título, sinopse, imagem, nota e lançamento
- ⭐ Adicionar filmes aos **favoritos**
- 📂 Armazenamento local com **SQLite**
- 📱 Layout responsivo e adaptável

---

## 📁 Estrutura do Projeto

```bash
NovassatMovies/
├── Features/
│   └── Movies/               # Telas, ViewModels e lógica de filmes
├── Infrastructure/
│   ├── Services/             # Serviços (API, navegação, storage)
│   └── Repositories/         # Camada de abstração de dados
├── Models/                   # Modelos usados na aplicação
├── Resources/                # Imagens, fontes e estilos
├── Extenders/                # Helpers e métodos de extensão
├── AppShell.xaml             # Navegação via Shell
├── App.xaml                  # Tema e recursos globais
└── Program.cs                # Configuração de DI e boot do app
```

---

## 🧪 Como executar o projeto

> ⚠️ Pré-requisitos: [.NET 8 ou superior](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) e SDKs das plataformas desejadas (Android/iOS/Windows/macOS).

```bash
# Clone o repositório
git clone https://github.com/NovassatSystems/NovassatMovies.git
cd NovassatMovies

# Restaure os pacotes
dotnet restore

# Execute no emululador ou dispositivo conectado
dotnet build
dotnet run -f:net8.0-android
```

> ✅ A navegação inicial está configurada via Shell, com rotas declaradas em `AppShell.xaml`.

---

## 🔐 Chave da API TMDb

A aplicação consome a API pública do TMDb. Para rodar localmente, é necessário definir sua chave de API em:

```csharp
// Constants.cs
public const string TmdbApiKey = "SUA_CHAVE_AQUI";
```

Crie sua chave em: [https://www.themoviedb.org/settings/api](https://www.themoviedb.org/settings/api)

---

## 💡 Boas práticas aplicadas

- ✔️ Uso de `ObservableProperty` e `RelayCommand` via CommunityToolkit
- ✔️ Organização por feature (estrutura modular)
- ✔️ Navegação desacoplada por `INavigationService`
- ✔️ Camadas separadas de Serviço / Repositório
- ✔️ Requisições desacopladas usando Flurl
- ✔️ Armazenamento local com abstração via `IStorageService`

---

## 🧠 Motivação e Observações

Este projeto foi desenvolvido como parte de um teste técnico para a empresa **Auvo Tecnologia**, com foco em qualidade de código, boas práticas de arquitetura mobile e experiência do usuário.

---

## 📄 Licença

Este projeto é distribuído sob a licença MIT. Consulte o arquivo [LICENSE](./LICENSE) para mais detalhes.