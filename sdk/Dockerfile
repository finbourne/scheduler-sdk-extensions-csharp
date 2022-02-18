FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN mkdir -p /usr/src
WORKDIR /usr/src

COPY Finbourne.Scheduler.Sdk.Extensions.sln /usr/src/
COPY Finbourne.Scheduler.Sdk.Extensions/Finbourne.Scheduler.Sdk.Extensions.csproj /usr/src/Finbourne.Scheduler.Sdk.Extensions/
COPY Finbourne.Scheduler.Sdk.Extensions.Tests/Finbourne.Scheduler.Sdk.Extensions.Tests.csproj /usr/src/Finbourne.Scheduler.Sdk.Extensions.Tests/
COPY Finbourne.Scheduler.Sdk.Extensions.Tutorials/Finbourne.Scheduler.Sdk.Extensions.Tutorials.csproj /usr/src/Finbourne.Scheduler.Sdk.Extensions.Tutorials/
RUN dotnet restore Finbourne.Scheduler.Sdk.Extensions

ENV FBN_SCHEDULER_API_URL ${FBN_SCHEDULER_API_URL}

# ENTRYPOINT [ "/bin/bash" ]
ENTRYPOINT dotnet test --configuration Release --verbosity normal Finbourne.Scheduler.Sdk.Extensions