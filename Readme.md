# Proyecto DVP.Tasks

Este proyecto es una aplicación basada en .NET 8.0 que implementa una arquitectura de Clean Code con el patrón CQRS (Command Query Responsibility Segregation) para una aplicación que cumple con el requerimiento: La empresa DVP necesita un sistema para gestionar las tareas de sus colaboradores. El sistema debe permitir la autenticación de usuarios y tener un sistema de roles que determine las funcionalidades accesibles para cada tipo de usuario (Administrador, Supervisor y Empleado).

## Arquitectura y Patrones

### Clean Code

El proyecto sigue los principios de Clean Code para asegurar un código limpio, legible y mantenible. Esto incluye el uso de nombres descriptivos, funciones pequeñas y la eliminación de código duplicado.

### CQRS

Se utiliza el patrón CQRS para separar las operaciones de lectura y escritura. Los comandos (Commands) manejan las operaciones de escritura, mientras que las consultas (Queries) manejan las operaciones de lectura.

### Singleton

El patrón Singleton se utiliza para garantizar que solo haya una instancia de las clases responsables del acceso a la base de datos, asegurando que el acceso a los recursos se maneje de manera eficiente.

## Arquitectura Hexagonal

En el proyecto se ha implementado la Arquitectura Hexagonal, también conocida como Arquitectura de Puertos y Adaptadores. Este patrón de diseño permite un desacoplamiento efectivo entre el núcleo de la aplicación y las interfaces externas, como bases de datos, servicios web y otras dependencias. A continuación, se describen los componentes clave y cómo se estructuran en nuestra aplicación:

### Componentes Clave

1. **Núcleo de la Aplicación (Core)**
   - **Dominios y Entidades**: Contiene las entidades del dominio y la lógica de negocio central. Aquí es donde se definen las reglas y comportamientos que rigen la aplicación.
   - **Casos de Uso**: Representa las operaciones específicas que el sistema debe realizar, a menudo implementadas como comandos o consultas. 

2. **Puertos**
   - **Puertos de Entrada**: Interfaces que definen los casos de uso que el núcleo de la aplicación expone. Estos puertos son implementados por los servicios del dominio.
   - **Puertos de Salida**: Interfaces que definen las interacciones con sistemas externos, como bases de datos o servicios web. Estos puertos permiten al núcleo de la aplicación interactuar con el mundo exterior sin conocer detalles específicos de las implementaciones.

3. **Adaptadores**
   - **Adaptadores de Entrada**: Implementaciones de los puertos de entrada, tales como controladores de API o interfaces de usuario, que traducen las solicitudes externas en comandos o consultas que el núcleo de la aplicación puede procesar.
   - **Adaptadores de Salida**: Implementaciones de los puertos de salida, como repositorios o clientes de servicios externos, que traducen las solicitudes del núcleo de la aplicación en llamadas a sistemas externos.

### Beneficios de la Arquitectura Hexagonal

- **Desacoplamiento**: El núcleo de la aplicación está separado de las tecnologías y frameworks específicos. Esto facilita la prueba y el mantenimiento del código, así como la posibilidad de cambiar o actualizar las dependencias externas sin afectar la lógica de negocio central.
- **Flexibilidad**: Permite que diferentes adaptadores se conecten al núcleo de la aplicación sin necesidad de modificar el código central. Esto es útil para soportar múltiples interfaces o fuentes de datos.
- **Pruebas Simples**: Facilita la realización de pruebas unitarias en el núcleo de la aplicación, ya que se puede simular fácilmente los puertos de entrada y salida.

### Estructura del Proyecto

- **`DVP.Tasks.Domain`**: Contiene las entidades y casos de uso del dominio. Aquí reside la lógica de negocio principal y las interfaces de puertos.
- **`DVP.Tasks.Infrastructure`**: Implementa los adaptadores de salida y la persistencia de datos. Contiene implementaciones específicas de los puertos de salida.
- **`DVP.Tasks.Api`**: Implementa los adaptadores de entrada, como controladores de API y otros puntos de entrada para la aplicación.

Este enfoque asegura que la aplicación pueda evolucionar y adaptarse a nuevas necesidades sin comprometer la integridad del núcleo de la lógica de negocio.

## Descripcion general del proyecto
Este proyecto es una aplicación web en .NET 8 que utiliza Azure Active Directory (AAD) para la autenticación de usuarios. Los usuarios, una vez creados y habilitados en AAD, pueden obtener un Bearer Token proporcionando su correo electrónico y contraseña registrados en AAD. Este token les permite acceder a las API seguras de la aplicación.

El backend está conectado a una instancia de SQL Server alojada en Azure, y utiliza Entity Framework Core con un enfoque de Code-First para el modelado y gestión de la base de datos

### Características
Autenticación con Azure Active Directory (AAD):

Los usuarios se crean y gestionan dentro de Azure Active Directory.
Los usuarios autenticados pueden obtener un Bearer Token enviando sus credenciales (correo electrónico y contraseña) al endpoint de token.
El token se usa para acceder de forma segura a los endpoints de la API.
Entity Framework Core con SQL Server en Azure:

La aplicación utiliza Entity Framework Core para conectarse a una base de datos SQL en Azure.
Se implementa un enfoque de Code-First para definir los modelos de la base de datos y gestionar las migraciones.

### Tecnologías utilizadas
- **.NET 8**
- **Azure Active Directory (AAD)** para la autenticación
Entity Framework Core para acceso a datos
- **SQL Server** alojado en Azure para la gestión de la base de datos
- **Migraciones Code-First** para la gestión de esquemas

## Pasos para Levantar el Proyecto Localmente

1. **Clonar el Repositorio**

   Clona el repositorio desde GitHub:

   ```bash
   git clone https://github.com/celopez74/DVPTest.git

2. **Restaurar dependencias**
   Navega al directorio del proyecto y restaura las dependencias:
   ```bash
   cd DVP.Tasks
   dotnet restore

3. **Ejecutar el proyecto**
   Ejecuta la aplicación:
   ```bash
   dotnet run --project dotnet run --project DVP.Tasks.Api/DVP.Tasks.Api.csproj

4. **Ejecutar las pruebas unitarias**
   Para ejecutar las pruebas unitarias, utiliza el siguiente comando:
   ```bash
   dotnet test
   ```
   

## Información Adicional
   * **Documentación del API**: La documentación del API está disponible en Swagger UI, accesible en http://localhost:5000/swagger cuando la aplicación esté en ejecución en ambiente Local (ejecutada desde VS), cuando se ejecute desde dockerfile o docker-compose la url disponible es: http://localhost:5002/swagger.

   Con el swagger se pueden obtener los endpoints disponibles en la aplicación, sin embargo la ejecución de los mismos requieren token de autenticación, el único endpoint que no requiere autenticación es el endpoint de Login con el cual se puede conseguir el token para acceder a los demás servicios.

   http://localhost:5000/api/ms-DVP/v1/Login

   ```bash
      curl --location 'http://localhost:5000/api/ms-DVP/v1/Login' \
      --header 'Content-Type: application/json' \
      --data-raw '{
         "email": "danileo@yopmail.com",
         "password": "MyPassword$$$$"
      }'
   ```
   ó
   ```bash
      curl --location 'http://localhost:5002/api/ms-DVP/v1/Login' \
      --header 'Content-Type: application/json' \
      --data-raw '{
         "email": "danileo@yopmail.com",
         "password": "MyPassword$$$$"
      }'
   ```

   En el momento en que se crea un nuevo usuario ya como es dado de alta en Azure Active Directory, también es posible obtener sus propios token's con las credenciales de los usuarios creados desde el endpoint de creacion de usuario.

   * **Dockerfile**: Se realiza la creación de un archivo dockerfile que permite la ejecución de la aplicación en un contenedor, para probarlo de esta manera se hace neceario tener instaldo Docker Desktop.  
   Teniendo Docker desktop instalado ejecutar en la raíz del proyecto:
      ```bash
      docker build -t dvp_test .  
     ```
      Una vez creado el contendor, en donde si es la primera vez que se ejecuta puede tardar algo de tiempo ya que se usan imagenes públicas y su descarga puede demorar, posteriormente procedemos a ejecutar:
       ```bash
      docker run -d -p 5002:5002 dvp_test
      ```
   * **docker-compose.yaml** por otra parte se adjunta un Docker compose que permite levantar el servicio de sql server y la aplicación conjuntamente, de nuevo, la imagen del sql server pública puede tomar un buen tiempo en descartar. Para ejecutar el docker compose:
      ```bash
      docker-compose up --build
      ```
   
## Nota Adicional

Dado a que se utilizan servicios en Azure que pueden generar costos, el archivo de configuración cargado en el repositorio no cuenta con las claves necesarias para utilizar los recursos desplegados, en este caso, se enviará por email un par de archivos llamados appsettings.Local.json y appsettings.Development.json con las claves correctas, dichos archivos entregados por email se deben reemplazar por el de la ruta:  /DVP.Tasks.Api/Configuracion.

el archivo de configuración del environment Local se debe utilizar para probar localmente ejecutando la aplicación con dotnet run, o para levantar el servicio con docker, esta configuración apunta a la base de datos SQL en Azure 

Vale la pena anotar servicios de SQL server Azure se encuentran detenidos ya que consumen solo por estar encendidos, por lo que si se quiere probar utilizando esta base de datos se deberia notificar al creador para que inicie el servicio, también puede ser necesario contar con la ip pública de donde se va a acceder a la base de datos ya que hay que agregarla como excepción en el firewall del sql para que permita su conexión.

Si se desea utilizar la configuración del docker compose, se tomará como base el archivo de configuración del ambiente development el cual apunta a la base de datos levantada en el compose.

si al iniciar el docker compose puede ser necesario tener que detener el compose y volverlo a ejecutar ya que en la primera carga es posigle que los servicios de sql se estén actualizando y la aplicación no logre conectarse a la base de datos.

```bash
   docker-compose down
   docker-compse up -d 
```

