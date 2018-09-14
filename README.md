---
services: active-directory
platforms: dotnet
author: dstrockis
---

# Build a multi-tenant SaaS web application using Azure AD & OpenID Connect

This sample shows how to build a multi-tenant .Net MVC web application that uses OpenID Connect to sign up and sign in users from any Azure Active Directory tenant, using the ASP.Net OpenID Connect OWIN middleware and the Active Directory Authentication Library (ADAL) for .NET.

For more information about how the protocols work in this scenario and other scenarios, see the [Authentication Scenarios for Azure AD](https://azure.microsoft.com/documentation/articles/active-directory-authentication-scenarios/) document.

## How To Run This Sample

Getting started is simple!  To run this sample you will need:
- Visual Studio 2013
- An Internet connection
- An Azure subscription (a free trial is sufficient)

Every Azure subscription has an associated Azure Active Directory tenant. You can easily create new tenants within your subscription for testing purposes   If you don't already have an Azure subscription, you can get a free subscription by signing up at [https://azure.microsoft.com](https://azure.microsoft.com).  All of the Azure AD features used by this sample are available free of charge.

### Step 1:  Clone or download this repository

From your shell or command line:

`git clone https://github.com/Azure-Samples/active-directory-dotnet-webapp-multitenant-openidconnect.git`

### Step 2:  Create a user account in your Azure Active Directory tenant

If you already have a user account in your Azure Active Directory tenant, you can skip to the next step.  This sample will not work with a Microsoft account, so if you signed in to the Azure portal with a Microsoft account and have never created a user account in your directory before, you need to do that now. You can find instructions to do that [here](http://www.cloudidentity.com/blog/2013/12/11/setting-up-an-asp-net-project-with-organizational-authentication-requires-an-organizational-account/).  If you create an account and want to use it to sign-in to the Azure portal, don't forget to add the user account as a co-administrator of your Azure subscription.

### Step 3:  Register the sample with your Azure Active Directory tenant

1. Sign in to the [Azure management portal](https://manage.windowsazure.com).
2. Click on Active Directory in the left hand nav.
3. Click the directory tenant where you wish to register the sample application.
4. Click the Applications tab.
5. In the drawer, click Add.
6. Click "Add an application my organization is developing".
7. Enter a friendly name for the application, for example "TodoListWebApp_MT", select "Web Application and/or Web API", and click next.
8. For the sign-on URL, enter the base URL for the sample, which is by default `https://localhost:44302/`.
9. For the App ID URI, enter `https://<your_tenant_domain>/TodoListWebApp_MT`, replacing `<your_tenant_domain>` with the domain of your Azure AD tenant (either in the form `<tenant_name>.onmicrosoft.com` or your own custom domain if you registered it in Azure Active Directory).
10. Click the Finish button on the lower right corner. Your application is now provisioned.
11. Click on the Configure tab of your application
12. Find "the application is multi-tenant" switch and flip it to yes. Hit the Save button from the command bar.
12. Scroll down to the bottom of the page, to the section Permissions to other applications". On the Azure Active Directory row, open the Delegated Permissions combo box and select "Enable sign-on and read users' profiles". Hit Save again.

Don't close the browser yet, as we will still need to work with the portal for few more steps. 

### Step 4:  Provision a key for your app in your Azure Active Directory tenant

The new customer onboarding process implemented by the sample requires the application to perform an OAuth2 request, which in turn requires to associate a key to the app in your tenant.
 
1. Click the Configure tab of your application.
2. Find the Keys section. In the Select Duration dropdown, pick a value.
3. Hit the Save button on the bottom command bar.
4. Once the save operation terminates, the value of the key appears. Copy the key to the clipboard. **Important: this is the only opportunity you have to access the value of the key, if you don't use it now you'll have to create a new one.**

Leave the browser open to this page. 

### Step 5:  Configure the sample to use your Azure Active Directory tenant

At this point we are ready to paste into the VS project the settings that will tie it to its entry in your Azure AD tenant. 

1. Open the solution in Visual Studio 2013.
2. Open the `web.config` file.
3. Find the app key `ida:Password` and replace the value you copied in step 4.
4. Go back to the portal, find the CLIENT ID field and copy its content to the clipboard
5. Find the app key `ida:ClientId` and replace the value with the CLIENT ID from the Azure portal.

### Step 6:  [optional] Create an Azure Active Directory test tenant 

This sample shows how to take advantage of the consent model in Azure AD to make an application available to any user from any organization with a tenant in Azure AD. To see that part of the sample in action, you need to have access to user accounts from a tenant that is different from the one you used for developing the application. The simplest way of doing that is to create a new directory tenant in your Azure subscription (just navigate to the main Active Directory page in the portal and click Add) and add test users.
This step is optional as you can also use accounts from the same directory, but if you do you will not see the consent prompts as the app is already approved. 

### Step 5:  Run the sample

The sample implements two distinct tasks: the onboarding of a new customer and regular sign in & use of the application.
####  Sign up
Start the application. Click on Sign Up.
You will be presented with a form that simulates an onboarding process. Here you can choose if you want to follow the "admin consent" flow (the app gets provisioned for all the users in one organization - requires you to sign up using an administrator) or the "user consent" flow (the app gets provisioned for your user only).
Click the SignUp button. You'll be transferred to the Azure AD portal. Sign in as the user you want to use for consenting. If the user is from a tenant that is different from the one where the app was developed, you will be presented with a consent page. Click OK. You will be transported back to the app, where your registration will be finalized.
####  Sign in
Once you signed up, you can either click on the Todo tab or the sign in link to gain access to the application. Note that if you are doing this in the same session in which you signed up, you will automatically sign in with the same account you used for signing up. If you are signing in during a new session, you will be presented with Azure AD's credentials prompt: sign in using an account compatible with the sign up option you chose earlier (the exact same account if you used user consent, any user form the same tenant if you used admin consent). 

## How To Deploy This Sample to Azure

Coming soon.

## About The Code

The application is subdivided in three main functional areas:

1. Common assets
2. Sign up
3. Todo editor

Let's briefly list the noteworthy elements in each area. For mroe details please refer to the comments in the code.

### Common assets

The application relies on models defined in Models/AppModels.cs, stored via entities as described by the context and initializer classes in the DAL folder.
The Home controller provides the basis for the main experience, listing all the actions the user can perform and providing conditional UI elements for explicit sign in and sign out (driven by the Account controller).

### Sign Up

The sign up operations are handled by the Onboarding controller.
The SignUp action and corresponding view simulate a simple onboarding experience, which results in an OAuth2 code grant request that triggers the consent flow.
The ProcessCode action receives authorization codes from Azure AD and, if they appear valid (see the code comments for details) it creates entries in the application store for the new customer organization/user.

### Todo editor

This is the application proper.
Its core resource is the Todo controller, a CRUD editor which leverages claims and the entity framework to manage a personalized list of Todo items for the currently signed in user.
The Todo controller is secured via OpenId Connect, according to the logic in App_Start/Startup.Auth.cs.
Notable code:

    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
    {
       ValidateIssuer = false,
    }

That code turns off the default Issuer validation, given that in the multitenant case the list of acceptable issuer values is dynamic and cannot be acquired via metadata (as it is instead the case for the single organization case). 

    RedirectToIdentityProvider = (context) =>
    {
       string appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;
       context.ProtocolMessage.Redirect_Uri = appBaseUrl;
       context.ProtocolMessage.Post_Logout_Redirect_Uri = appBaseUrl;
       return Task.FromResult(0);
    }

That handler for `RedirectToIdentityProvider` assigns to the `Redirect_Uri` and `Post_Logout_Redirect_Uri` (properties used for sign in and sign out locations) URLs that reflect the current address of the application. This allows you to deploy the app to Azure Web Sites or any other location without having to change hardcoded address settings. Note that you do need to add the intended addresses to the Azure AD entry for your application.

Finally: the implementation of `SecurityTokenValidated` contains the custom caller validation logic, comparing the incoming token with the database of trusted tenants and registered users and interrupting the authentication sequence if a match is not found.


All of the OWIN middleware in this project is created as a part of the open source [Katana project](http://katanaproject.codeplex.com).  You can read more about OWIN [here](http://owin.org).
