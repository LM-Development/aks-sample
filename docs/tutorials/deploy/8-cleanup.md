# Clean up Azure Resources

In this section of the tutorial, we will delete all the Azure resources we created during the
tutorial. And remove all things we created for the Compliance Recording Policy.

## Remove Compliance Recording Policy

At first we will first undo everything to create a recording policy.

### Unassign Recording Policy

Unassigning the recording policy is done very similar to assigning it:

``` pwsh
Grant-CsTeamsComplianceRecordingPolicy `
      -Global `
      -PolicyName $null
```

The command should complete without any output to the terminal window.

### Delete the Recording Application

We continue with deleting the recording application that links the recording policy to the
application instance:

``` pwsh
Remove-CsTeamsComplianceRecordingApplication `
    -Identity 'Tag:TutorialPolicy/11111111-1111-1111-1111-111111111111'
```

The `Identity` Argument is a combination from the recording policy name and the object-id of the
application instance, it uses the pattern:

`Tag:` + name of the recording policy + `/` + the object-id of the application instance

After running the powershell command it should complete without any output.

### Delete the Recording Policy

After decouplic the application instance and the recording policy we can delete the recording
policy:

``` pwsh
Remove-CsTeamsComplianceRecordingPolicy `
    -Identity 'TutorialPolicy'
```

The command should complete successful without any further output.

### Delete the Application Instance

To delete the application instance we need to delete the user principal of the application instance
in the entra id:

``` pwsh
az ad user delete --id tutorialbot@lm-ag.de
```

The command should also complete without any output.

## Delete Resource Group

With deleting the Resource Group we created at the start of this tutorial, we will recursively
delete all the resources within the resource group. To delete the resource group we run:

``` pwsh
az group delete --name recordingbottutorial
```

After confirming the operation with _y_ for yes, the execution of the command takes some time and
should successfully finish without any further output.

## Delete App Registration

Since App Registrations are created within a Microsoft Entra Id Tenant and not within a
Resource Group. The deletion of the App Registration, we created during the Tutorial, needs to be
done seperately. To do so we run:

``` pwsh
az ad app delete --id cccccccc-cccc-cccc-cccc-cccccccccccc
```

If the command ran successfully it should finish without any output. The app registration can then still
be found in the deleted applications view of the [Microsoft Entra Admin Center](https://entra.microsoft.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade/quickStartType~/null/sourceType/Microsoft_AAD_IAM),
the app registration can be restored there within the next 30 days.

And that is it, we deleted all Azure resources we created during the tutorial.
