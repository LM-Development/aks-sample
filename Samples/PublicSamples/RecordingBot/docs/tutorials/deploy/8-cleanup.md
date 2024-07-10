# Clean up Azure Resources

In this section of the tutorial, we will delete all the Azure resources we created during the tutorial.

## Delete Resource Group

With deleting the Resource Group we created at the start of this tutorial, we will recursively delete
all the resources within the resource group. To delete the resource group we run:

```pwsh
az group delete --name recordingbottutorial
```

After confirming the operation with _y_ for yes, the execution of the command takes some time and
should successfully finish without any further output.

## Delete App Registration

Since App Registrations are created within a Microsoft Entra Id Tenant and not within a
Resource Group. The deletion of the App Registration, we created during the Tutorial, needs to be
done seperately. To do so we run:

```pwsh
az ad app delete --id cccccccc-cccc-cccc-cccc-cccccccccccc
```

If the command ran successfully it should finish without any output. The app registration can then still
be found in the deleted applications view of the [Microsoft Entra Admin Center](https://entra.microsoft.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade/quickStartType~/null/sourceType/Microsoft_AAD_IAM),
the app registration can be restored there within the next 30 days.

And that is it, we deleted all Azure resources we created during the tutorial.
