# Introduction 
This project aims to provide automated end to end tests for a compliance recording bot between two or more users participating in a call.  
It's purpose is to ensure that a compliance recording bot joins a call and the call participants receives a visual notification that a records has started.  

## End-To-End Framework
As a end-to-end testing framework playwright.net is used. (Read more [Playwright](https://playwright.dev/dotnet/))

## Framework
As a framework to write and execute tests, nUnit is used. (Read  more [NUnit](https://nunit.org/))

## Otp
Otp.Net is used to generate one time passwords and optain a code for login. (Read more [Otp.Net](https://www.nuget.org/packages/Otp.NET))


# Getting Started
To start using this project, you need to provide at least two users that are registered for using teams.
Therefore you need to provide the following information in the .runsettings file:
```json
{
		"RunConfiguration": {
      "EnvironmentVariables": {
        "UserA_Username": "",
        "UserA_UserPassword": "",
        "UserA_UserSeed": "",
        "UserB_Username": "",
        "UserB_UserPassword": "",
        "UserB_UserSeed": "",
        "UserC_Username": "",
        "UserC_UserPassword": "",
        "UserC_UserSeed": ""
      }
    }
}
```

Furthermore you should adjust the launch options to your needs in the .runsettings file. 
Locally its a good idea execute the tests not in headless mode to see the test running, but if you consider to run the tests in a pipeline you should keep it headless:
```json
{
  "RunConfiguration": {
      "EnvironmentVariables": {
        "LaunchOptions": {
          "headless": false,
          "slowMo": 0
        }
      }
    }
}
```

# Contribute
TODO: 
Currently there is a missing locator for the compliance recording bot toast (Locator: CallComplianceToast). This needs to be added to the project.
Furthermore the login uses xPath to find the elements. This should be changed to use the id or data-tid once it is provided.