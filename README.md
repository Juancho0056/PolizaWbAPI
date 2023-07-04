# PolizaWebAPI
<br/>
Este repositorio contiene una API web desarrollada con ASP.NET Core 6 y MongoDB como base de datos. Proporciona un conjunto de endpoints para administrar pólizas de seguros.

<br/>

## Usuario JWT Prueba
usuario: administrator@localhost
password: Password@123


## Tecnologias utilizadas

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [AspNetCore Identity MongoDbCore](https://github.com/alexandre-spieser/AspNetCore.Identity.MongoDbCore)
* [AspNetCore Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [Log4Net](https://logging.apache.org/log4net/)
* [FluentValidation](https://fluentvalidation.net/)
* [XUnit](https://xunit.net/), [Moq](https://github.com/moq)

## Inicio

### Configuracion Base de datos
"MongoConfiguration": {
  "ConnectionString": "mongodb://localhost",
  "DatabaseName": "gestion"
}
Verificar la clave **MongoConfiguration** en el archivo **appsettings.json** para configurar la conexion a MongoDb.

### Archivo de configuración log4net
Se utiliza Log4Net para el registro de eventos. Puedes encontrar la configuración en el archivo log4net.config.

<log4net>
  <!-- Configuración del registro de eventos -->
</log4net>

### Domain

Este proyecto contiene todas las entidades y lógica específicos de la capa de dominio.

### Application

Este proyecto contiene toda la lógica de la aplicación. Depende del proyecto `Domain`, pero no depende de ninguna otra capa o proyecto. Este proyecto define interfaces que son implementadas por proyectos externas. Por ejemplo, si la aplicación necesita acceder a un servicio de notificación, se agregaría una nueva interfaz a la aplicación y se crearía una implementación dentro de la infraestructura.

### Infrastructure

Este proyecto contiene clases para acceder a recursos de base de datos y es donde se ubican las implementaciones de las interfaces creadas en `Applicacion`.

### WebUI

Este proyecto depende de los proyectos `Application` e `Infrastructure`, sin embargo, la dependencia de la `Infrastructure` es solo para utilizar la inyección de dependencia. Por lo tanto, solo *Program.cs* debe hacer referencia a `Infrastructure`.

### Restaurar paquetes
`dotnet restore`


### Compilar
`dotnet publish -c Release`

### Estructura

src
│   ├───Application
│   │   ├───Common
│   │   │   ├───Behaviours
│   │   │   ├───Exceptions
│   │   │   ├───Interfaces
│   │   │   ├───Mappings
│   │   │   ├───Models
│   │   │   └───Security
│   │   ├───Features
│   │   │   └───Polizas
│   │   │       ├───Commands
│   │   │       │   └───Create
│   │   │       └───Queries
│   │   │           └───Get
│   │   └───WeatherForecasts
│   │       └───Queries
│   │           └───GetWeatherForecasts
│   ├───Domain
│   │   ├───Common
│   │   ├───Exceptions
│   ├───Infrastructure
│   │   ├───Common
│   │   ├───Identity
│   │   └───Services
│   └───WebUI
│       ├───ClientApp
│       │   └───src
│       │       └───app
│       ├───Controllers
│       ├───Filters
│       ├───Models
│       ├───Properties
│       ├───Services
│       ├───Swagger
│       │   └───Filters
│       └───wwwroot
│           └───api
└───tests
    └───ApplicationTest
