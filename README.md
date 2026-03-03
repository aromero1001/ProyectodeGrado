# Sistema de Votación Electrónica 🗳️

Este proyecto es un **Sistema de Votación Electrónica** desarrollado para proporcionar una plataforma segura, transparente y eficiente para la gestión de procesos electorales. El sistema permite el registro de votantes, la gestión de candidatos y partidos, y la realización de votaciones con validación en tiempo real.

## 🚀 Tecnologías y Versiones

El sistema está construido sobre el stack de tecnología de Microsoft, utilizando el patrón arquitectónico **ASP.NET MVC**.

### Backend
- **Framework:** ASP.NET MVC 5.2.3
- **Runtime:** .NET Framework 4.5.2
- **ORM:** Entity Framework 6.1.3
- **Seguridad:** Microsoft ASP.NET Identity 2.2.1
- **Autenticación:** OWIN 3.0.1 (Google, Facebook, Microsoft Account, Twitter)
- **Serialización:** Newtonsoft.Json 6.0.4

### Frontend
- **Diseño:** Bootstrap 3.3.7
- **Iconografía:** Font Awesome 4.6.1
- **Motor de Plantillas:** Razor (Microsoft.AspNet.Razor 3.2.3)
- **Scripts:** 
  - jQuery 1.10.2
  - jQuery Validation 1.11.1
  - Modernizr 2.6.2

### Herramientas y Otros
- **Optimización Web:** Microsoft.AspNet.Web.Optimization 1.1.3
- **Captchas:** CaptchaMvc.Mvc5 1.5.0
- **Telemetría:** Microsoft Application Insights 1.2.3

---

## 📋 Casos de Uso

El sistema contempla diversos roles y funcionalidades clave para asegurar un proceso electoral completo.

![Casos de Uso](file:///c:/Users/dahel/OneDrive%20-%20Cardinal/Documents/ProyectodeGrado/ProyectodeGrado/ProyectodeGrado/Content/images/Casos%20de%20uso.png)

### Resumen de Funcionalidades:
- **Registro y Autenticación:** Los usuarios pueden registrarse y acceder al sistema de forma segura.
- **Gestión de Candidatos:** Administración de los perfiles que participan en la elección.
- **Proceso de Votación:** Interfaz intuitiva para que el elector seleccione su preferencia (Candidato, Partido o Propuesta).
- **Control de Tiempo:** Mecanismos para gestionar la duración de la sesión de voto.
- **Resultados:** Visualización de los resultados finales del proceso.

---

## 📊 Diagrama de Flujo de Datos (DFD) - Registro

El siguiente diagrama describe el flujo de información durante el proceso de registro en el sistema de votación, asegurando que los datos del ciudadano sean validados correctamente.

![DFD Sistema de Votación REGISTRO](file:///c:/Users/dahel/OneDrive%20-%20Cardinal/Documents/ProyectodeGrado/ProyectodeGrado/ProyectodeGrado/Content/images/DFD%20Sistema%20de%20votaci%C3%B3n%20REGISTRO.png)

### Descripción del Flujo:
1. El ciudadano ingresa sus datos personales.
2. El sistema valida la información contra la base de datos de ciudadanos.
3. Si los datos son correctos y el ciudadano no está registrado previamente, se crea la cuenta.
4. Se confirma el registro exitoso al usuario.

---

## 🛠️ Instalación y Configuración

Para ejecutar este proyecto localmente, sigue estos pasos:

1. **Clonar el repositorio.**
2. **Abrir la solución** `ProyectodeGrado.sln` en Visual Studio (se recomienda 2015 o superior).
3. **Restaurar paquetes NuGet:** El sistema debería descargar automáticamente las dependencias especificadas en `packages.config`.
4. **Configurar la Base de Datos:** Asegúrate de tener una instancia de SQL Server disponible y actualiza la cadena de conexión en el `Web.config` si es necesario.
5. **Compilar y Ejecutar:** Presiona `F5` para iniciar el servidor de desarrollo IIS Express.

---

> [!NOTE]
> Este proyecto fue desarrollado como Proyecto de Grado, enfocándose en la integridad del voto y la facilidad de uso para el elector.
