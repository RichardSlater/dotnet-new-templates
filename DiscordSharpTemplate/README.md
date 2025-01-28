## Introduction

## Configuration

Configuration can be provided in one of three primary ways:

1. Through the `config.json` file next to the executable.
2. Through environment variables.
3. Through command line parameters.

Technically [User Secrets][msft-user-secrets] can also be used to provide configuration options, although this is
normally reserved for a Local Development scenario.

Additional mechanisms, such as Azure KeyVault can be added, as the template uses the well known
`ConfigurationBuildier()` interface.

### Supported Configuration Providers

#### JSON Configuration

Secrets should never be placed in JSON Configuration next to the deployment as it is trivially accessed from the file
system. For production environments, use a Key Vault.

```json
{
  "Bot": {
    "Discord": {
      "Prefixes": [
        "!",
        "$"
      ]
    }
  }
}
```

#### Environment Variables

Environment variables are the "least bad place" to put secrets, they are still trivially accessible from anything that
can dump the process or docker address space so doesn't provide any meaningful security. Use a Key Vault to store
production secrets.

```bash
EXPORT ConfigurationRoot__Discord__Prefixes__0="!"
EXPORT ConfigurationRoot__Discord__Prefixes__1="*"
```

**Note: it's important to include the indexer on the end, it's not simply enough to pass an array as an environment
variable.**

#### Command Line Arguments

Command Line Arguments are generally accessible by simply looking at the process list so are an inappropriate place to
store secrets. For production purposes use a Key Vault or similar.

```bash
./discord-bot --ConfigurationRoot:Discord:Prefixes:0 "!" --ConfigurationRoot:Discord:Prefixes:1 "*"
```

### Configuration Options

#### Prefixes (String[])

Prefixes are the starts of commands, i.e. the prefix for the `!hello` command is `!`. The prefix is not included in the
class derived from `BaseCommandModule` so can be changed on a deployment to deployment basis:

```csharp
[Command("hello")]
```

Do not include the prefix in the `CommandAttribute`!

#### BotToken

Taken from the [Discord Developer site](https://discord.com/developers/applications), this is the actual token use to connect to a Discord Application, it should be
kept secret:

```json
{
  "ConfigurationRoot": {
    "Discord": {
      "BotToken": "MT...MA"
    }
  }
}
```

For simple cases you could inject this via an Environment Variable, however for production environments I strongly
recommend using some kind of Key Vault:

- [Azure Key Vault][msft-keyvault]
- [AWS Key Management Service][aws-kms]
- [Google Cloud Key Management Service][gcp-kms]
- [Hashicorp Vault][hashicorp-vault]
- [Infisical][infisical]
- [OpenBao][openbao]
- [Kubernetes Secrets][k8s-secrets]

[msft-user-secrets]: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows

[msft-keyvault]: https://azure.microsoft.com/en-gb/products/key-vault

[aws-kms]: https://docs.aws.amazon.com/kms/latest/developerguide/overview.html

[gcp-kms]: https://cloud.google.com/kms/docs/key-management-service

[hashicorp-vault]: https://www.vaultproject.io/

[infisical]: https://infisical.com/

[openbao]: https://openbao.org/

[k8s-secrets]: https://kubernetes.io/docs/concepts/configuration/secret/