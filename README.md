<p align="center">
    <img src="wwwroot/Cover.png" alt="Cover">
</p>

# Sobre la aplicación

Esta aplicación backend, desarrollada con C# y ASP.NET Web API, es una plataforma de blogging que permite a los usuarios 
autenticarse utilizando JSON Web Tokens (JWT) y ASP.NET Identity. A través de esta autenticación basada en tokens, los 
usuarios pueden gestionar sus cuentas, crear publicaciones y editar sus perfiles, accediendo a los servicios básicos 
para la administración de contenido y cuentas dentro de la plataforma.

## Características principales

- **Autenticación de usuarios**: Registro y autenticación mediante JWT e Identity, asegurando seguridad y
  control de acceso.
- **Gestión de perfiles**: Edición y eliminación de perfiles, incluyendo información personal
  y profesional.
- **Publicaciones de usuario**: CRUD completo para la gestión de publicaciones. Los usuarios
  pueden crear contenido y participar en interacciones mediante comentarios y reacciones.

## Propósito y objetivo del proyecto

Este proyecto es una iniciativa personal con el objetivo de aplicar y reforzar nuevos conocimientos
en desarrollo backend utilizando C# y ASP.NET Web API. A través de su implementación, busco mejorar
mis habilidades en autenticación de usuarios, gestión de datos y arquitectura de software,
permitiéndome profundizar en las mejores prácticas del desarrollo de APIs.

Además, este proyecto me sirve como un espacio de aprendizaje continuo, explorando **conceptos** como
seguridad, optimización de código y diseño de sistemas escalables.

## Tecnologías utilizadas

- **Lenguaje de programación**: C#
- **Framework**: ASP.NET Web API
- **Base de datos**: SQL Server
- **ORM**: Entity Framework Core
- **Autenticación**: JWT (JSON Web Tokens)
- **Gestión de usuarios**: ASP.NET Identity

# Estructura del proyecto

```plaintext
BlogApp
├───Controllers
├───Data
├───DTOs
├───Entities
├───Filters
├───Helpers
├───Repositories
├───Responses
```

<table>
  <thead>
    <tr>
      <th>Carpeta</th>
      <th>Descripción</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Controllers</td>
      <td>
        En esta carpeta existen 2 controladores:
        <ul>
          <li><strong>AuthController</strong>: Controlador encargado de la autenticación de usuarios.</li>
          <li><strong>FeedController</strong>: Controlador encargado de la interacción de los posts de los usuarios.</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td>Data</td>
      <td>Este directorio contiene el <strong>ApplicationDbContext</strong>
      para la configuración de la base de datos, así como las migraciones.
      </td>
    </tr>
    <tr>
      <td>DTOs</td>
      <td>
        En esta carpeta se encuentran los <strong>Data Transfer Objects</strong> (DTOs) utilizados para la
        transferencia de datos entre el cliente y el servidor. Estos objetos son utilizados para
        validar y estructurar la información que se envía y recibe a través de la API.
      </td>
    </tr>
    <tr>
      <td>Entities</td>
      <td>
        En esta carpeta se encuentran las entidades que representan las tablas de la base de datos.
        Estas entidades son utilizadas por el ORM (Entity Framework Core) para mapear los datos
        entre la base de datos y el código.
      </td>
    </tr>
    <tr>
      <td>Filters</td>
      <td>
        En esta carpeta se encuentran los filtros utilizados para la validación de datos y la
        autenticación de usuarios. Estos filtros son utilizados para validar la información que se
        envía a través de la API y para autenticar a los usuarios que intentan acceder a los
        recursos protegidos.
      </td>
    </tr>
    <tr>
      <td>Helpers</td>
      <td>
        En esta carpeta se encuentran las clases de ayuda que se utilizan en toda la aplicación.
        Estas clases son utilizadas para realizar tareas comunes y definir constantes.
      </td>
    </tr>
    <tr>
    <td>Repositories</td>
      <td>
        Esta carpeta contiene los repositorios encargados de acceder a los datos de la 
        BD. A través de ellos, se realizan operaciones CRUD, consultas y otras 
        interacciones con la información almacenada. Aquí se encuentran las implementaciones 
        necesarias para procesar las solicitudes de los clientes.
      </td>
    </tr>
    <tr>
      <td>Responses</td>
      <td>
        En esta carpeta se encuentran las clases de respuesta que se utilizan para enviar
        información al cliente. Estas clases son utilizadas para estructurar la información que
        se envía a través de la API y para manejar los errores que se producen en la aplicación.
      </td>
    </tr>
  </tbody>
</table>

# Autenticación y seguridad
> 📝 **Nota:** Para la verificación del correo electrónico, se utiliza un servicio llamado Ethereal Email, que permite enviar correos electrónicos de prueba sin necesidad de utilizar un servidor SMTP real. Te proporciono las credenciales para que puedas probarlo:
> - **Sitio Web**: [Ethereal Email](https://ethereal.email/).
> - **Email**: `deshaun.cole@ethereal.email`
> - **Contraseña**: `3T6xQMPufreBw2arD7`
> 
> De esta forma, podras recibir el codigo de verificación para tu cuenta.

La autenticación de usuarios se realiza mediante JSON Web Tokens (JWT) y ASP.NET Identity.
Con base en los siguientes diagramas, se puede observar el flujo de autenticación y autorización:

### Digarama de registro
**Paso 1:** El usuario envía una solicitud de registro con su correo y contraseña, y el servidor le manda una correo para confirmar su cuenta.

<img src="wwwroot/RegisterDiagram.png" alt="register">

### Diagrama de inicio de sesión
**Paso 2:** El usuario envía una solicitud de inicio de sesión con su correo y contraseña, y el servidor le responde con un token JWT.

<img src="wwwroot/LoginDiagram.png" alt="login">

### Diagrama de generar nuevo token
El token JWT tiene una duración de 30 minutos. Si el usuario desea generar un nuevo token, debe enviar una solicitud
con el **Refresh Token** y el servidor le responderá con un nuevo token.

<img src="wwwroot/RefreshTokenDiagram.png" alt="refresh">

> 📝 **Nota:** El **Refresh Token** es creado y almacenado la primera vez que el usuario inicia sesión.
> Si el usuario no tiene un **Refresh Token**, deberá iniciar sesión nuevamente. De este modo, se previene el uso no autorizado de la cuenta.

### Diagrama de autorización
El usuario envía una solicitud a un recurso protegido, y el servidor verifica el token JWT. Si es válido, se le permite el acceso al recurso.

<img src="wwwroot/AuthorizationDiagram.png" alt="authorization">

> 📝 **Nota:** En cada solicitud, el token **JWT** se envía en la cabecera de autorización como un Bearer Token.

# Endpoints de la API
Url base: `https://demoblogapp-b2h0d9bngkeub4hp.westus2-01.azurewebsites.net/api`
## 1. Autenticación de usuarios

### 1.1 Registro de usuario
- **URL**: `/auth/register`
- **Método**: `POST`
- **Descripción**: Registra un nuevo usuario en la aplicación.
- **Cuerpo de la solicitud**:
```json
{
  "email": "string",
  "password": "string"
}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "Account created! Thank you for your registration, kindly check your email for confirmation code.",
  "statusCode": 201
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos.
- - `409 Conflict`: El correo electrónico ya está en uso.

### 1.2 Confirmación de cuenta (verificación de correo)
- **URL**: `/api/Authentication/confirm-email/{email}/{code}`
- **Método**: `POST`
- **Descripción**: Confirma la cuenta del usuario mediante un código de verificación enviado por correo electrónico.
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "Email confirmed successfully",
  "statusCode": 200
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos.
- - `404 Not Found`: El correo electrónico no existe o el código de confirmación es inválido.

### 1.3 Reenvío de correo de confirmación
- **URL**: `/api/Authentication/confirm-email/{email}/{code}`
- **Método**: `POST`
- **Descripción**: Reenvía un correo de confirmación al usuario.
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "Email confirmed successfully",
  "statusCode": 200
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos.
- - `404 Not Found`: El correo electrónico no existe o el código de confirmación es inválido.
- - `409 Conflict`: El correo electrónico ya está verificado.

### 1.4 Inicio de sesión
- **URL**: `/api/Authentication/login`
- **Método**: `POST`
- **Descripción**: Inicia sesión en la aplicación y devuelve un token JWT.
- **Cuerpo de la solicitud**:
```json
{
  "email": "string",
  "password": "string"
}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "Login Successfully!",
  "statusCode": 200,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhODYxODcwOS03MWE3LTRmZTctOWRkYi1mNWVlYzk3MzA4MGYiLCJuYW1lIjoidXNlcjVAZXhhbXBsZS5jb20iLCJlbWFpbCI6InVzZXI1QGV4YW1wbGUuY29tIiwiZXhwIjoxNzQ2NDg5NjA4LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTUxIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE1MSJ9.P_O3_v9DlyNfUjJolHNLqbB7870FoD2sd9KhYxJmwI4",
    "refreshToken": "9DkdkTDavkM3L09V86wxF998QYLqYDUx/HMVb+R1a9y6elOVSYPUyeBWVT9nnThNY20JFd7rTX/T5D8MZt8cmw==",
    "emailConfirmed": true
  }
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos.
- - `404 Not Found`: El usuario no existe.
- - `403 Forbidden`: El email del usuario no ha sido confirmado.

### 1.5 Obtener información del usuario
- **URL**: `/api/Authentication/user`
- **Método**: `GET`
- **Descripción**: Obtiene la información del usuario autenticado.
- **Cuerpo de la solicitud**:
```json
{
  "refreshToken": "string"
}
```
- **Cabecera**:
```plaintext
Authorization: Bearer {token}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "User found.",
  "statusCode": 200,
  "data": {
    "id": "a8618709-71a7-4fe7-9ddb-f5eec973080f",
    "userName": "user5@example.com",
    "email": "user5@example.com",
    "emailConfirmed": true,
    "profileComplete": false,
    "....": "...."
  }
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos.
- - `404 Not Found`: El usuario no existe.

### 1.6 Generar nuevo token
- **URL**: `/api/Authentication/refresh-token`
- **Método**: `POST`
- **Descripción**: Genera un nuevo token JWT utilizando el refresh token almacenado en la base de datos.
- **Cabecera**:
```plaintext
Authorization: Bearer {token}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "Token refreshed successfully!",
  "statusCode": 200,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhODYxODcwOS03MWE3LTRmZTctOWRkYi1mNWVlYzk3MzA4MGYiLCJuYW1lIjoidXNlcjVAZXhhbXBsZS5jb20iLCJlbWFpbCI6InVzZXI1QGV4YW1wbGUuY29tIiwiZXhwIjoxNzQ2NDkwMjYwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTUxIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE1MSJ9.Cm55gpZ8BfcLOgnp-5GRFtP87_WT_AgnOEP-ZNOQYJQ",
    "refreshToken": "kwI/Yg6400SEFyXQMx6qIw0+ieRwSTR6A9dcoGA+gOSYSWyjvymBFh5RevxZUBeLx2VBp0lXgeCcKOIf1f4D4A==",
    "emailConfirmed": true
  }
}
```
- **Errores posibles**:
- - `400 Bad Request`: El refresh token no existe o es inválido || El token no existe o es inválido.
- - `404 Not Found`: El refresh token podría no haber sido generado por que el usuario no existe o no a iniciado sesión.

## 2. Feed

### 2.1 Obtener publicaciones(Posts)
- **URL**: `/posts/feed`
- **Método**: `GET`
- **Descripción**: Obtiene todas las publicaciones de los usuarios.
- **Cabecera**:
```plaintext
Authorization: Bearer {token}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 200,
  "data": [
    {
      "id": 0,
      "content": "string",
      "picture": "string",
      "creationDate": "2025-05-06T18:19:29.133Z",
      "updatedDate": "2025-05-06T18:19:29.133Z",
      "user": {
        "id": "string",
        "userName": "string",
        "email": "string",
        "emailConfirmed": true,
        "headline": "string",
        "profilePicture": "string",
        "firstName": "string",
        "lastName": "string",
        "company": "string",
        "position": "string",
        "country": "string",
        "city": "string",
        "website": "string",
        "coverImage": "string",
        "about": "string",
        "birthday": "2025-05-06",
        "profileComplete": true
      },
      "comments": [
        {
          "id": 0,
          "content": "string",
          "creationDate": "2025-05-06T18:19:29.133Z",
          "updatedDate": "2025-05-06T18:19:29.133Z",
          "userId": "string",
          "user": {
            "id": "string",
            "userName": "string",
            "email": "string",
            "emailConfirmed": true,
            "headline": "string",
            "profilePicture": "string",
            "firstName": "string",
            "lastName": "string",
            "company": "string",
            "position": "string",
            "country": "string",
            "city": "string",
            "website": "string",
            "coverImage": "string",
            "about": "string",
            "birthday": "2025-05-06",
            "profileComplete": true
          },
          "postId": 0,
          "post": "string"
        }
      ],
      "postLikes": [
        {
          "postId": 0,
          "post": {
            "id": 0,
            "content": "string",
            "picture": "string",
            "creationDate": "2025-05-06T18:19:29.134Z",
            "updatedDate": "2025-05-06T18:19:29.134Z",
            "isActive": true,
            "userId": "string",
            "user": {
              "id": "string",
              "userName": "string",
              "normalizedUserName": "string",
              "email": "string",
              "normalizedEmail": "string",
              "emailConfirmed": true,
              "passwordHash": "string",
              "securityStamp": "string",
              "concurrencyStamp": "string",
              "phoneNumber": "string",
              "phoneNumberConfirmed": true,
              "twoFactorEnabled": true,
              "lockoutEnd": "2025-05-06T18:19:29.134Z",
              "lockoutEnabled": true,
              "accessFailedCount": 0,
              "firstName": "string",
              "lastName": "string",
              "headline": "string",
              "company": "string",
              "position": "string",
              "country": "string",
              "city": "string",
              "website": "string",
              "profileComplete": true,
              "profilePicture": "string",
              "coverImage": "string",
              "about": "string",
              "birthday": "2025-05-06",
              "isActive": true,
              "posts": [
                "string"
              ],
              "comments": [
                {
                  "id": 0,
                  "content": "string",
                  "creationDate": "2025-05-06T18:19:29.134Z",
                  "updateDate": "2025-05-06T18:19:29.134Z",
                  "isActive": true,
                  "postId": 0,
                  "post": "string",
                  "userId": "string",
                  "user": "string"
                }
              ],
              "postLikes": [
                "string"
              ]
            },
            "comments": [
              {
                "id": 0,
                "content": "string",
                "creationDate": "2025-05-06T18:19:29.134Z",
                "updateDate": "2025-05-06T18:19:29.134Z",
                "isActive": true,
                "postId": 0,
                "post": "string",
                "userId": "string",
                "user": "string"
              }
            ],
            "postLikes": [
              "string"
            ]
          },
          "userId": "string",
          "user": {
            "id": "string",
            "userName": "string",
            "normalizedUserName": "string",
            "email": "string",
            "normalizedEmail": "string",
            "emailConfirmed": true,
            "passwordHash": "string",
            "securityStamp": "string",
            "concurrencyStamp": "string",
            "phoneNumber": "string",
            "phoneNumberConfirmed": true,
            "twoFactorEnabled": true,
            "lockoutEnd": "2025-05-06T18:19:29.134Z",
            "lockoutEnabled": true,
            "accessFailedCount": 0,
            "firstName": "string",
            "lastName": "string",
            "headline": "string",
            "company": "string",
            "position": "string",
            "country": "string",
            "city": "string",
            "website": "string",
            "profileComplete": true,
            "profilePicture": "string",
            "coverImage": "string",
            "about": "string",
            "birthday": "2025-05-06",
            "isActive": true,
            "posts": [
              "string"
            ],
            "comments": [
              {
                "id": 0,
                "content": "string",
                "creationDate": "2025-05-06T18:19:29.134Z",
                "updateDate": "2025-05-06T18:19:29.134Z",
                "isActive": true,
                "postId": 0,
                "post": "string",
                "userId": "string",
                "user": "string"
              }
            ],
            "postLikes": [
              "string"
            ]
          },
          "likedAt": "2025-05-06T18:19:29.134Z"
        }
      ],
      "commentsCount": 0,
      "likesCount": 0
    }
  ]
}
```
- **Errores posibles**:
- - `404 Not Found`: No se encontraron publicaciones.

### 2.2 Obtener publicaciones por ID.
- **URL**: `/posts/{postId}`
- **Método**: `GET`
- **Descripción**: Obtiene una publicación específica por su ID.

### 2.3 Crear publicación
- **URL**: `/posts`
- **Método**: `POST`
- **Descripción**: Crea una nueva publicación.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
{
  "content": "string",
  "picture": "string"
}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 201,
  "data": "string"
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos.
- - `404 Not Found`: Usuario no encontrado. El usuario podría no existir o el token es inválido.

### 2.2 Obtener publicaciones por ID.
- **URL**: `/posts/{postId}`
- **Método**: `GET`
- **Descripción**: Obtiene una publicación específica por su ID.
- - - **Cabecera**:
```plaintext
Authorization: Bearer {token}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 200,
  "data": [
    {
      "id": 0,
      "content": "string",
      "picture": "string",
      "creationDate": "2025-05-06T18:23:40.203Z",
      "updatedDate": "2025-05-06T18:23:40.203Z",
      "user": {
        "id": "string",
        "userName": "string",
        "email": "string",
        "emailConfirmed": true,
        "headline": "string",
        "profilePicture": "string",
        "firstName": "string",
        "lastName": "string",
        "company": "string",
        "position": "string",
        "country": "string",
        "city": "string",
        "website": "string",
        "coverImage": "string",
        "about": "string",
        "birthday": "2025-05-06",
        "profileComplete": true
      },
      "comments": [
        {
          "id": 0,
          "content": "string",
          "creationDate": "2025-05-06T18:23:40.203Z",
          "updatedDate": "2025-05-06T18:23:40.203Z",
          "userId": "string",
          "user": {
            "id": "string",
            "userName": "string",
            "email": "string",
            "emailConfirmed": true,
            "headline": "string",
            "profilePicture": "string",
            "firstName": "string",
            "lastName": "string",
            "company": "string",
            "position": "string",
            "country": "string",
            "city": "string",
            "website": "string",
            "coverImage": "string",
            "about": "string",
            "birthday": "2025-05-06",
            "profileComplete": true
          },
          "postId": 0,
          "post": "string"
        }
      ],
      "postLikes": [
        {
          "postId": 0,
          "post": {
            "id": 0,
            "content": "string",
            "picture": "string",
            "creationDate": "2025-05-06T18:23:40.203Z",
            "updatedDate": "2025-05-06T18:23:40.203Z",
            "isActive": true,
            "userId": "string",
            "user": {
              "id": "string",
              "userName": "string",
              "normalizedUserName": "string",
              "email": "string",
              "normalizedEmail": "string",
              "emailConfirmed": true,
              "passwordHash": "string",
              "securityStamp": "string",
              "concurrencyStamp": "string",
              "phoneNumber": "string",
              "phoneNumberConfirmed": true,
              "twoFactorEnabled": true,
              "lockoutEnd": "2025-05-06T18:23:40.203Z",
              "lockoutEnabled": true,
              "accessFailedCount": 0,
              "firstName": "string",
              "lastName": "string",
              "headline": "string",
              "company": "string",
              "position": "string",
              "country": "string",
              "city": "string",
              "website": "string",
              "profileComplete": true,
              "profilePicture": "string",
              "coverImage": "string",
              "about": "string",
              "birthday": "2025-05-06",
              "isActive": true,
              "posts": [
                "string"
              ],
              "comments": [
                {
                  "id": 0,
                  "content": "string",
                  "creationDate": "2025-05-06T18:23:40.203Z",
                  "updateDate": "2025-05-06T18:23:40.203Z",
                  "isActive": true,
                  "postId": 0,
                  "post": "string",
                  "userId": "string",
                  "user": "string"
                }
              ],
              "postLikes": [
                "string"
              ]
            },
            "comments": [
              {
                "id": 0,
                "content": "string",
                "creationDate": "2025-05-06T18:23:40.203Z",
                "updateDate": "2025-05-06T18:23:40.203Z",
                "isActive": true,
                "postId": 0,
                "post": "string",
                "userId": "string",
                "user": "string"
              }
            ],
            "postLikes": [
              "string"
            ]
          },
          "userId": "string",
          "user": {
            "id": "string",
            "userName": "string",
            "normalizedUserName": "string",
            "email": "string",
            "normalizedEmail": "string",
            "emailConfirmed": true,
            "passwordHash": "string",
            "securityStamp": "string",
            "concurrencyStamp": "string",
            "phoneNumber": "string",
            "phoneNumberConfirmed": true,
            "twoFactorEnabled": true,
            "lockoutEnd": "2025-05-06T18:23:40.203Z",
            "lockoutEnabled": true,
            "accessFailedCount": 0,
            "firstName": "string",
            "lastName": "string",
            "headline": "string",
            "company": "string",
            "position": "string",
            "country": "string",
            "city": "string",
            "website": "string",
            "profileComplete": true,
            "profilePicture": "string",
            "coverImage": "string",
            "about": "string",
            "birthday": "2025-05-06",
            "isActive": true,
            "posts": [
              "string"
            ],
            "comments": [
              {
                "id": 0,
                "content": "string",
                "creationDate": "2025-05-06T18:23:40.203Z",
                "updateDate": "2025-05-06T18:23:40.203Z",
                "isActive": true,
                "postId": 0,
                "post": "string",
                "userId": "string",
                "user": "string"
              }
            ],
            "postLikes": [
              "string"
            ]
          },
          "likedAt": "2025-05-06T18:23:40.203Z"
        }
      ],
      "commentsCount": 0,
      "likesCount": 0
    }
  ]
}
```
- **Errores posibles**:
- - `404 Not Found`: No se encontró la publicación.


### 2.4 Actualizar publicación
- **URL**: `/posts/{postId}`
- **Método**: `PUT`
- **Descripción**: Actualiza una publicación existente mientras el usuario sea el autor de la publicación.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
{
  "content": "string",
  "picture": "string"
}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 200,
  "data": "string"
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos. El token del usuario podría no ser válido.
- - `403 Unauthorized`: Acceso denegado para editar la publicación. El usuario no es el autor de la publicación.
- - `404 Not Found`: Usuario o publicación no encontrados.

### 2.5 Eliminar publicación
- **URL**: `/posts/{postId}`
- **Método**: `DELETE`
- **Descripción**: Elimina una publicación existente mientras el usuario sea el autor de la publicación.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 200,
  "data": "string"
}
```
- **Errores posibles**:
- - `400 Bad Request`: Datos de entrada inválidos. El token del usuario podría no ser válido.
- - `403 Unauthorized`: Acceso denegado para eliminar la publicación. El usuario no es el autor de la publicación.
- - `404 Not Found`: Usuario o publicación no encontrados.

### 2.6 Reaccionar a una publicación
- **URL**: `/posts/{postId}/like`
- **Método**: `PUT`
- **Descripción**: Reacciona a una publicación existente mediante el id de la publicación.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 200,
  "data": "string"
}
```
- **Errores posibles**:
- - `404 Not Found`: Usuario o publicación no encontrados.

### 2.7 Obtener reacciones de una publicación
- **URL**: `/posts/{postId}/likes`
- **Método**: `GET`
- **Descripción**: Obtiene todas las reacciones de una publicación existente mediante el id de la publicación.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 200,
  "data": {
    "postId": 0,
    "user": [
      {
        "id": "string",
        "userName": "string",
        "email": "string",
        "emailConfirmed": true,
        "headline": "string",
        "profilePicture": "string",
        "firstName": "string",
        "lastName": "string",
        "company": "string",
        "position": "string",
        "country": "string",
        "city": "string",
        "website": "string",
        "coverImage": "string",
        "about": "string",
        "birthday": "2025-05-06",
        "profileComplete": true
      }
    ]
  }
}
```
- **Errores posibles**:
- - `404 Not Found`: Publicación no encontrada.

## 3. Comentarios

### 3.1 Añadir un comentario
- **URL**: `/posts/{postId}/comments`
- **Método**: `POST`
- **Descripción**: Añade un nuevo comentario a una publicación existente.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
{
  "content": "string"
}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 201,
  "data": "string"
}
```
- **Errores posibles**:
- - `404 Not Found`: Publicación no encontrada.

### 3.2 Actualizar un comentario
- **URL**: `/comments/{commentrId}`
- **Método**: `PUT`
- **Descripción**: Actualiza un comentario existente mientras el usuario sea el autor del comentario.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
{
  "content": "string"
}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 201,
  "data": "string"
}
```
- **Errores posibles**:
- - `404 Not Found`: Comentario no encontrada.

### 3.4 Eliminar un comentario
- **URL**: `/comments/{commentId}`
- **Método**: `DELETE`
- **Descripción**: Elimina un comentario existente mientras el usuario sea el autor del comentario.
- - **Cabecera**:
```plaintext
Authorization: Bearer {token}
{
  "content": "string"
}
```
- **Respuesta exitosa**:
```json
{
  "isSuccessful": true,
  "message": "string",
  "statusCode": 200,
  "data": "string"
}
```
- **Errores posibles**:
- - `403 Unauthorized`: Acceso denegado para eliminar el comentario. El usuario no es el autor del comentario.
- - `404 Not Found`: Comentario o usuario no encontrados.