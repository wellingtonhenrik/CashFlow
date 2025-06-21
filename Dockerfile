
##Baixando a imagem do dotnet## 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

##Criando a pasta app e entrando nela##
WORKDIR /app
##Expondo em qual porta##
##EXPOSE 4000##
##Copiando tudo quu estra dentro da pasta src e colocando na pasta app que é o diretorio atual 
COPY src/ .


##pasta da statup  do projeto, como a pasta já existe ao executar WORKDIR apenas vai entrar nela##
WORKDIR /app/CashFlow.Api

##restaurando dependencias de todos os projetos 
RUN dotnet restore

##Publicando api em release e colocando o publish nas pasta out##
##Como esta executando em release precisa criar a appsettings.Production
RUN dotnet publish -c Release -o /app/out

##Acessando a pasta app com outra image (aspnet)##
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

##Copiandor os arquivos da primeira imagem##
COPY --from=build-env /app/out .

##Executar a api##
##Setando a porta que vai rodar
##ENV ASPNETCORE_URLS=http://+:4000##
ENTRYPOINT ["dotnet", "CashFlow.Api.dll"]

   
