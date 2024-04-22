# Deploy Tutorial

In this tutorial we learn how to deploy the recording bot sample to a new AKS Cluster and set up a Recording Policy for all users within our Tenant.

## Prerequisites

- [Windows 11](https://www.microsoft.com/de-de/software-download/windows11)
- [Powershell 7 as administrator](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.4)
- [Git command line tool](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
- [AZ Azure command line tool](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows)
- [Helm command line tool](https://helm.sh/docs/intro/install/)
- [kubectl command line tool](https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/)
  - This tutorial also shows how to install kubectl with the azure command line tool
- [Microsoft Entra Id Tenant](https://learn.microsoft.com/en-us/entra/fundamentals/create-new-tenant) [with Microsoft Teams users](https://learn.microsoft.com/en-us/entra/fundamentals/license-users-groups)
- [Microsoft Azure Subscription](https://learn.microsoft.com/en-us/azure/cost-management-billing/manage/create-subscription)
  - The subscription in this tutorial is called `recordingbotsubscription`, also see [variables](#variables).
- Microsoft Entra Id adminstrator

The Microsoft Entra Id administrator is required to create recording policies and to approve application permissions of the app registration. This tutorial assumes we are a Microsoft Entra Id administrator and always log in as such unless the tutorial requires otherwise.

## Contents

1. [Deploy an AKS Cluster](./deploy/aks.md)
2. [Deploy an Azure Container Registry](./deploy/acr.md)
3. [Clone Code and build Docker Image](./deploy/build.md)
4. [Deploy and configure Bot Service](./deploy/bot-service.md)
5. [Deploy Recording Sample to AKS Cluster](./deploy/helm-deploy.md)
6. [Create and assign a Recording Policy](./deploy/policy.md)
7. [Verify functionality](./deploy/test.md)

## Variables

Throughout this tutorial we will create azure resources. The names we choose in this tutorial are:

| Ressource | Name |
| --------- | ---- |
| Ressource Group | `recordingbottutorial` |
| AKS Cluster | `recordingbotcluster` |
| Azure Container Registry | `recordingbotregistry` |
| App Registration | `recordingbotregistration` |
| Bot Service | `recordingbotservice` |
| Azure Subscription | `recordingbotsubscription` |

Variables that are used in this tutorial are:

| What? | Value |
| ----- | ----- |
| AKS DNS record | `recordingbottutorial`_.westeurope.cloudapp.azure.com_ |
| App Registration Id | `00000000-0000-0000-0000-000000000000` |  
| App Registration Secret | `abcdefghijklmnopqrstuvwxyz` |
| Recording Policy Name | `TutorialPolicy` |
| Recording Policy Application Instance UPN | `tutorialbot@lm-ag.de` |
| Recording Policy Application Instance Display Name | `Tutorial Bot` |
| Recording Policy Application Instance Object Id | `11111111-1111-1111-1111-111111111111` |
| Microsoft Entra Id Tenant Id | `99999999-9999-9999-9999-999999999999` |
| Kubernetes Recording Bot Deployment Name | `recordingbottutorial` |
| Kubernetes Recording Bot Namespace | `recordingbottutorial` |
| Kubernetes Cert-Manager Deployment Name | `cert-manager` |
| Kubernetes Cert-Manager Namespace | `cert-manager` |
| Bot Name within the application | `Tutorial Bot` |
| Let's Encrypt Email address | `tls-security@lm-ag.de` |
| Windows Nodepool | `win22` |
| Azure Subscription Id | `yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyyy` |
| Azure Region | `westeurope` |

> [!TIP]  
> Consider to define own variable values before we start. Keep in mind the Azure resources have limitations for naming, read [this](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/resource-name-rules) for reference. The app registration values, the object id of the application instance and the Microsoft Entra Id Tenant Id are automatically generated. Do not forget to replace the placeholders with the actual values.

Now let us start [deploy an aks cluster](./deploy/aks.md)