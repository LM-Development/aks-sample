# Deploying with a Custom Domain

## Prerequisites

- A custom domain
- Access to your DNS provider to create a CNAME record
- Azure CLI installed
- Helm installed
- Kubernetes cluster as created by [deploy tutorial](../tutorials/deploy-tutorial.md)

## Steps

### 1. Create a CNAME Record

Create a CNAME record in your DNS provider that points your custom domain to the built-in domain of the load balancer IP (xxxx.{region}.cloudapp.azure.com).

### 2. Provide the Custom Domain During Chart Deployment

When deploying the chart, provide the custom domain for the FQDN. This is done by setting the `host` parameter.

Example:
```sh
helm install teams-recording-bot ./deploy/teams-recording-bot \
    --namespace teams-recording-bot \
    --create-namespace \
    --set host="custom.domain.com" \
    --set public.ip=STATIC_IP_ADDRESS \
    --set image.domain=YOUR_ACR_DOMAIN
```

### 3. Update Bot Channel Registration

Update the Bot Channel Registration in the Azure Bot Resource to use the new custom domain in the Teams Calling URI.

1. Go to the Azure portal and navigate to your Bot Channels Registration.
2. Update the Messaging endpoint to use your custom domain, e.g., `https://custom.domain.com/api/messages`.
3. Save the changes.

### 4. Verify the Steps

Verify that the steps are sufficient to get the custom domain to work by testing the bot in a Teams meeting.

1. Create a Teams meeting.
2. Invite the bot to the meeting using the custom domain.
3. Verify that the bot joins the meeting and functions as expected.

