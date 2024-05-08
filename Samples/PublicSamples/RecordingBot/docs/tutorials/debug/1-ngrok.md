# Create ngrok tunnels

In this step we will create the ngrok tunnels and create a CName entry on our Domain.

## Authenticate ngrok client

First off we need to authenticate our ngrok client to do so, we need an authtoken from the [ngrok website](https://dashboard.ngrok.com/get-started/your-authtoken).
The web page should look similar to the example below:

![ngrok authtoken web page](../../images/screenshot-ngrok-authtoken.png)

In the top of the page we can copy our authtoken, in the example the authtoken is hidden, so let us
say we have there a value of `ajnbkwaoawerfavauhniluhn`. We copy the value in the field on the page,
and continue in a terminal with the following command:

``` powershell
ngrok authtoken ajnbkwaoawerfavauhniluhn
```

The resulting output should show:

``` text
Authtoken saved to configuration file: C:\Users\User\AppData\Local/ngrok/ngrok.yml
```

## Create a ngrok configuration file

Now let us create a ngrok configuration file that defines our tunnels.

We can create the configuration file at whatever directory with whatever name we like, in the examples `C:\Users\User\.ngrok\recrdingtunnels.yaml` is our configuration file.

So now let us create the file and fill it with the following content.

```yaml
tunnels:
  notifications:
    proto: http
    addr: 9442
  media:
    proto: tcp
    addr: 8445
```

Save the file and continue with starting the configuration.

## Start ngrok

## Configure CName Entry
