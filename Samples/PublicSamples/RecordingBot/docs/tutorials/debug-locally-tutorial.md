# Debug Locally Tutorial

In this tutorial we will learn how we can run the recording bot sample locally to allow
debugging the recording bot sample in our IDE.

## Prerequisites

- [Windows 11](https://www.microsoft.com/software-download/windows11)
- [Powershell 7 as administrator](https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-windows)
- [Git command line tool](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
- [AZ Azure command line tool](https://learn.microsoft.com/cli/azure/install-azure-cli-windows)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) (Community, Professional and Enterprise Edition work for this Tutorial)
- [ngrok](https://ngrok.com/docs/guides/device-gateway/windows/)
- [Microsoft Entra Id Tenant](https://learn.microsoft.com/entra/fundamentals/create-new-tenant) [with Microsoft Teams users](https://learn.microsoft.com/entra/fundamentals/license-users-groups)
- [Microsoft Azure Subscription](https://learn.microsoft.com/azure/cost-management-billing/manage/create-subscription)
  - The subscription in this tutorial is called `recordingbotsubscription`, also see [variables](#variables).
- [ACME Implementation: Certbot](https://certbot.eff.org/instructions?ws=other&os=windows)
- A DNS TXT entry
  - we will create/edit the entry in this tutorial, to do so you should know how to do this with your DNS provider
- A DNS CName entry
  - we will create/edit the CName in this tutorial, to do so you should know how to do this with your DNS provider
- Microsoft Entra Id adminstrator

The Microsoft Entra Id administrator is required to approve application permissions of an app
registration, we also use the administrator account to create the app registration and a Azure Bot
Service.

## Contents

1. [Create ngrok tunnels](./debug/1-ngrok.md)
2. [Retreive Certificate](./debug/2-certificate.md)
3. [Create Bot Service](./debug/3-bot-service.md)
4. [Clone Code](./debug/4-clone.md)
5. [Build and Run](./debug/5-build-run.md)
6. [Validate that the Bot works](./debug/6-validate.md)

## Variables

Throughout this tutorial we will create some azure resources. We also have some variables that we
create during the tutorial, eg. ngrok creates a unique fully qualified domain name for us. In the
tutorial, note the placeholders and replace them with your own values.

|    Variable Name    |   Value in the tutorial    |
| ------------------- | -------------------------- |
| Azure Subsciription | `recordingbotsubscription` |
| ngrok authtoken     | _ajnbkwaoawerfavauhniluhn_ |

If you encounter any problems during the tutorial, please feel free to create an [issue](https://github.com/lm-development/aks-sample/issues).
This means that the tutorial can be improved continously.

Now let us start to [create the ngrok tunnels](./debug/1-ngrok.md)