FROM mcr.microsoft.com/dotnet/sdk:5.0 AS Builder
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore 
COPY . ./
RUN dotnet publish -c release -o out


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=Builder /app/out .
ENTRYPOINT ["dotnet","DockerVolumeConceptProject.dll"]