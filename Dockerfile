ARG VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

ENV PATH="${PATH}:/root/.dotnet/tools"

ADD . .

RUN dotnet restore *.sln

RUN dotnet publish \
  -c Release \
  -o ./out \
  --no-restore

#-----------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

ENV APP_HOME=/app
WORKDIR $APP_HOME

RUN adduser --disabled-password --gecos "" appcore -u 1000
ENV ASPNETCORE_URLS=http://*:5002
ENV COMPlus_EnableDiagnostics=0

ARG ENV
ENV ASPNETCORE_ENVIRONMENT ${ENV:-Local}
COPY --from=build-env --chown=app:app /app/out $APP_HOME
COPY --from=build-env --chown=app:app /app/DVP.Tasks.Api/Configuration $APP_HOME/Configuration

USER 1000
EXPOSE 5002 5002
ENTRYPOINT ["dotnet", "DVP.Tasks.Api.dll"]
