# The image is based on Ubuntu 24.04
FROM mcr.microsoft.com/playwright/python:v1.50.0-noble

RUN DEBIAN_FRONTEND=noninteractive apt-get update
RUN DEBIAN_FRONTEND=noninteractive apt-get upgrade -y

RUN DEBIAN_FRONTEND=noninteractive apt-get install -y -qq --no-install-recommends \
    apt-transport-https apt-utils ca-certificates \
    curl git iputils-ping jq lsb-release openssh-client \
    software-properties-common

# Install the .NET SDK 9.0
RUN add-apt-repository ppa:dotnet/backports
RUN apt-get update && apt-get install -y dotnet-sdk-9.0

# Can be 'linux-x64', 'linux-arm64', 'linux-arm', 'rhel.6-x64'.
ENV TARGETARCH=linux-x64
ENV RUN_AGENT_ONCE=False

WORKDIR /azp

# Run a self-hosted Azure Pipelines agent in Docker
COPY ./start.sh .
RUN chmod +x start.sh

# Create agent user and set up home directory
RUN useradd -m -d /home/agent agent
RUN chown -R agent:agent /azp /home/agent

ENTRYPOINT [ "./start.sh" ]