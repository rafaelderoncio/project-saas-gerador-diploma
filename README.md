# Project SaaS Certfy

API em .NET para emissão e validação de certificados acadêmicos em PDF.

## Sumário

1. [Visão Geral](#visão-geral)
2. [Arquitetura e Estrutura](#arquitetura-e-estrutura)
3. [Tecnologias](#tecnologias)
4. [Pré-requisitos](#pré-requisitos)
5. [Como Executar Localmente](#como-executar-localmente)
6. [Documentação da API (Swagger)](#documentação-da-api-swagger)
7. [Endpoints](#endpoints)
8. [Exemplo de Geração de Certificado](#exemplo-de-geração-de-certificado)
9. [Docker](#docker)
10. [Kubernetes](#kubernetes)
11. [Dados Iniciais](#dados-iniciais)
12. [Tratamento de Erros](#tratamento-de-erros)
13. [Testes](#testes)
14. [Troubleshooting](#troubleshooting)
15. [Contribuição](#contribuição)
16. [Licença](#licença)

## Visão Geral

O projeto expõe endpoints para:

- listar instituições, cursos e disciplinas;
- gerar certificado em PDF a partir de dados acadêmicos;
- validar certificado por código de autenticação.

A geração de PDF utiliza template HTML e renderização headless com `PuppeteerSharp`.

## Arquitetura e Estrutura

A solução está organizada por camadas:

- `src/Project.SaaS.Certfy.API`: entrada HTTP (Minimal API), DI, Swagger e configuração de pipeline.
- `src/Project.SaaS.Certfy.Core`: regras de negócio, serviços, validações, exceções, helpers e repositórios in-memory.
- `src/Project.SaaS.Certfy.Domain`: contratos públicos (requests/responses) e enums.
- `src/Project.SaaS.Certfy.Test`: projeto de testes.
- `deploy/`: artefatos de deploy (`Dockerfile` e manifesto Kubernetes).

Fluxo simplificado para emissão:

1. API recebe `CertificateRequest`.
2. Core valida payload + entidades (instituição/curso/disciplinas).
3. Core monta `CertificateModel` + hash de autenticação + QR Code.
4. Template HTML é renderizado e convertido para PDF via Puppeteer.
5. API retorna o arquivo `application/pdf`.

## Tecnologias

- .NET `10.0`
- ASP.NET Core Minimal API
- Swashbuckle (Swagger/OpenAPI)
- FluentValidation
- PuppeteerSharp
- QRCoder

## Pré-requisitos

- SDK .NET 10
- (Opcional) Docker 24+
- (Opcional) Kubernetes + `kubectl`

## Como Executar Localmente

No diretório raiz do repositório:

```bash
dotnet restore src/Project.SaaS.Certfy.slnx
dotnet run --project src/Project.SaaS.Certfy.API/Project.SaaS.Certfy.API.csproj
```

URLs de desenvolvimento (conforme `launchSettings.json`):

- `http://localhost:5164`
- `https://localhost:7244`

## Documentação da API (Swagger)

Com a API em execução:

- `http://localhost:5164/swagger`
- `https://localhost:7244/swagger`

A documentação OpenAPI é enriquecida por:

- metadados dos endpoints em `Program.cs` (`WithSummary`, `WithDescription`, `Produces`, `Accepts`);
- comentários XML (`/// <summary>`) em contratos públicos de `Domain` e `Core`.

## Endpoints

### Cursos

- `GET /api/courses/{courseId}`
- `GET /api/courses?page=1&size=10`

### Disciplinas

- `GET /api/disciplines/{disciplineId}`
- `GET /api/disciplines?page=1&size=10`

### Instituições

- `GET /institutions/{institutionId}`
- `GET /api/institutions?page=1&size=10`

### Certificados

- `POST /api/certificate/generate` (retorna PDF)
- `GET /api/certificate/validate?authentication={codigo}`

## Exemplo de Geração de Certificado

### Request

```http
POST /api/certificate/generate
Content-Type: application/json
```

```json
{
  "institutionId": "cc0bb7cb-1e49-49e7-9d33-22dedfa26344",
  "conclusionDate": "2026-02-16T00:00:00Z",
  "student": {
    "name": "Maria da Silva",
    "documentNumber": "12345678901",
    "documentType": "CPF",
    "registration": "202300123"
  },
  "course": {
    "courseId": "9f8bd499-152b-447a-afae-69ca805cebfd",
    "disciplines": [
      {
        "disciplineId": "5c06fc8f-c05d-4f30-909b-cd17928f8ee5",
        "period": "2025.1",
        "average": 8.5,
        "hasDispensed": false
      },
      {
        "disciplineId": "b914ab35-de20-4bf8-9da7-81c88141198e",
        "period": "2025.2",
        "average": 9.0,
        "hasDispensed": false
      }
    ]
  },
  "signature": {
    "deersPersonName": "Diretor Acadêmico",
    "administrativePersonName": "Secretário Geral"
  }
}
```

### Response

- `200 OK`
- `Content-Type: application/pdf`
- arquivo: `certificate{registration}.pdf`

## Docker

Build da imagem:

```bash
docker build -f deploy/Dockerfile -t certfy-api:latest .
```

Execução local:

```bash
docker run --rm -p 8080:8080 certfy-api:latest
```

Teste rápido:

```bash
curl http://localhost:8080/api/institutions?page=1\&size=10
```

## Kubernetes

Manifesto disponível em `deploy/kubernetes.yaml` com:

- `Deployment` (2 réplicas, probes e limites de recursos);
- `Service` (`ClusterIP`);
- `Ingress` (NGINX).

Antes de aplicar, ajuste:

- imagem: `seu-registro/certfy-api:latest`
- host: `certfy.exemplo.com`

Aplicação:

```bash
kubectl apply -f deploy/kubernetes.yaml
```

## Dados Iniciais

Os dados in-memory são carregados de:

- `src/Project.SaaS.Certfy.Core/Repositories/DataFiles/course.json`
- `src/Project.SaaS.Certfy.Core/Repositories/DataFiles/discipline.json`
- `src/Project.SaaS.Certfy.Core/Repositories/DataFiles/institution.json`

## Tratamento de Erros

A API usa:

- `GlobalExceptionHandler`
- `ProblemDetails`
- exceções de domínio (`BaseException`)

Isso padroniza respostas de erro para consumo por clientes HTTP.

## Testes

Executar testes:

```bash
dotnet test src/Project.SaaS.Certfy.Test/Project.SaaS.Certfy.Test.csproj
```

## Troubleshooting

- `dotnet build` falha no restore:
  - valide versão do SDK com `dotnet --info`;
  - limpe cache do NuGet: `dotnet nuget locals all --clear`;
  - restaure novamente: `dotnet restore src/Project.SaaS.Certfy.slnx`.
- erro de geração PDF em container:
  - confirme uso do `deploy/Dockerfile` (inclui Chromium e dependências).
- Swagger não aparece:
  - em produção, Swagger está desabilitado no `Program.cs` por padrão.

## Contribuição

1. Crie uma branch para sua feature/correção.
2. Faça commits pequenos e objetivos.
3. Execute build/testes localmente.
4. Abra PR com contexto técnico e evidências de teste.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo `LICENSE`.
