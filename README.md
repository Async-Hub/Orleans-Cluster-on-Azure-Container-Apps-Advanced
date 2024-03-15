## Microsoft Orleans cluster on Azure-Container Apps advanced

This is the modernized and up to date version of  sample provided by Microsoft ([Orleans Cluster on Azure Container Apps](https://github.com/Azure-Samples/Orleans-Cluster-on-Azure-Container-Apps)). It uses:

* Fully automated Azure Pipelines to deploy from code to cloud with minimal effort
* Blazor Server model which works on .NET 8 and Microsoft Orleans 8
* Scalable server-side Blazor app on Azure Container Apps
* Azure SignalR Service, Azure Key Vault, Azure Storage Account, Azure Application Insights, Azure Load Testing, Microsoft Playwright for E2E Tests, Azure DevOps and many more

What is required for this solution?  
> - Microsoft Azure Subscription, [you can create a free account](https://azure.microsoft.com/en-us/free/) if you don't have any.

### 1. Create an Azure DevOps project for the solution.

1. Open Azure DevOps portal : https://dev.azure.com/
2. Create new organization/use an existing organization. A sample name: **orleans-on-ctap1**
3. Create a new public project. A sample name: **ShoppingApp**. For a public project Microsoft provides 
[unlimited restriction for parallel jobs](https://learn.microsoft.com/en-us/azure/devops/pipelines/licensing/concurrent-jobs) (by default up to 25 parallel jobs) in case you use self-hosted agents.  
4. Import existing repository: https://github.com/Async-Hub/Orleans-Cluster-on-Azure-Container-Apps-Advanced.git
5. Create YAML pipeline by using exsisting "Azure-Pipelines.yml" from imporeted sources. A sample name: **Default CI and CD** pipeline. After creation just only save it, don't run it.
   
### 2. Install Docker Desktop on your machine.

Install [Docker Desktop](https://docs.docker.com/desktop/install/windows-install/) on operation system which you are using.  

>Attention! The steps outlined below, up to section 3, are optional

If you are using Windows 11 or Windows 10 it is more appropriate to use WSL 2 and install Docker Desktop on Ubuntu-22.04. Here 
we have two options:

- The first option is to use WSL 2 on the host operationg system.
- The second option is to keep the host operating system clean and create a Windows 10/11 Virtual Machine with Hyper-V using [Nested Virtualization](https://learn.microsoft.com/en-us/virtualization/hyper-v-on-windows/user-guide/nested-virtualization). What are required for *Nested Virtualization*?  
> - [Windows machine with virtualization technology (AMD-V / Intel VT-x)](https://learn.microsoft.com/en-us/virtualization/hyper-v-on-windows/user-guide/nested-virtualization)
>   - Windows Server 2016/Windows 10 or greater for Intel processor with VT-x
>   - Windows Server 2022/Windows 11 or greater AMD EPYC/Ryzen processor
> - Enable [nested virtualization](https://learn.microsoft.com/en-us/windows/wsl/faq#can-i-run-wsl-2-in-a-virtual-machine-) on the Virtual Machine

Regardless of the option you chose above, you need to open a terminal on the host (for the first option) or on the VM (for the second option). Then enable WSL 2 and install Ubuntu 22.04 with the following command, typing it in the terminal window:
```
wsl --install -d Ubuntu-22.04
```
Install [Docker Engine](https://docs.docker.com/engine/install/ubuntu/) on Ubuntu.
```
$ sudo apt-get update
$ sudo apt-get install ca-certificates curl
$ sudo install -m 0755 -d /etc/apt/keyrings
$ sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
$ sudo chmod a+r /etc/apt/keyrings/docker.asc
$ echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
$ sudo apt-get update
$ sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```
Start Docker service.
```
sudo service docker start
sudo service docker status
```
You can clone the solution you have imported https://dev.azure.com/orleans-on-ctap1/_git/ShoppingApp into C:\ drive on your Windows machine ([Git CLI](https://git-scm.com/download/win) is required)
```
mkdir C:\Repos
cd C:\Repos
git clone https://dev.azure.com/orleans-on-ctap1/_git/ShoppingApp
```
and interop with it from Ubuntu-22.04 by the following way:
```
$ cd /mnt/c/Repos/ShoppingApp/build/azure-pipelines-agents/debian-12.2/
$ sudo docker build -t azure-pipelines-agents-debian-12.2:15032024 .
```

### 3. Create a self-hosted agents pool for the Azure DevOps organization.

Build an agent docker image by using files from "build\azure-pipelines-agents" based on Debian image
```
sudo docker build -t azure-pipelines-agents-debian-12.2:15032024 .
```
or on Ubuntu image.
```
sudo docker build -t azure-pipelines-agents-ubuntu-20.04:15032024 .
```
Also create Playwright image
```
sudo docker build -t azure-pipelines-agents-playwright-1.42.0:15032024 .
```
Create [Azure DevOps personal access token (PAT token)](https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate). For the scope select: Agent Pools (read, manage), Deployment group (read, manage).  
Run Debian or Ubuntu based Azure Pipelines agent by using the following command:
```
sudo docker run --rm -v /var/run/docker.sock:/var/run/docker.sock \
    -e AZP_URL=https://dev.azure.com/orleans-on-ctap1 \
    -e AZP_TOKEN=<PAT token> -e AZP_AGENT_NAME=01_debian-12.2 \
    -e AZP_POOL=Default -e AZP_WORK=_work --name 01_debian-12.2 azure-pipelines-agents-debian-12.2:15032024
```
The syntax above uses Bash. If you use PowerShell shell, just replace "\\" (backslash) with "`" (backtick).  
  
>Warning! Doing Docker within a Docker by using Docker socket has serious security implications. The code inside the container can now run as root on your Docker host. Please be very careful.

### 4. Create a service connection for the Azure DevOps project.

Use your existing Microsoft Azure Subscription, [you can create a free account](https://azure.microsoft.com/en-us/free/) if you don't have any.  
By using Azure CLI create a service principal and configure its access to Azure resources. To retrieve current subscription ID, run:  
```
az account show --query id --output tsv
```
Configure its access to Azure subscription:
```
az ad sp create-for-rbac --name Orleans-On-Ctap1 --role Owner --scopes /subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```
Cretae a new [Azure Resource Manager service connection](https://learn.microsoft.com/en-us/azure/devops/pipelines/library/connect-to-azure?view=azure-devops#create-an-azure-resource-manager-service-connection-with-an-existing-service-principal) with an existing service principal (orleans-on-ctap1), required name: **DefaultAzureServiceConnection**. Choose **Verify connection** to validate the settings you entered
Install [GitTools](https://marketplace.visualstudio.com/items?itemName=gittools.gittools) for the current Azure DevOps Organization. 
Install [Azure Load Testing](https://marketplace.visualstudio.com/items?itemName=AzloadTest.AzloadTesting) for the current Azure DevOps Organization.
Disable [shallow fetch](https://learn.microsoft.com/en-us/azure/devops/pipelines/yaml-schema/steps-checkout?view=azure-pipelines#shallow-fetch) for the Azure Pipeline. It's required for GitTools.

### 5. Run the Azure Pipeline.

Run the Azure Pipeline, wait till it completely deploy the solution and enjoy it.