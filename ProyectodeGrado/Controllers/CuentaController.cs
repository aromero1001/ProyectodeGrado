using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectodeGrado.Models.SVModel;

namespace ProyectodeGrado.Controllers
{
    public class CuentaController : Controller
    {
        //Las variables globales para cualquier uso.
        public static class variablesGlobales
        {
            public static string nom_usuario { get; set; }

            public static int id_usuario { get; set; }

            public static int id_rol { get; set; }

            public static string nombre { get; set; }
        }

        // GET: Cuenta
        //Este es el Action que redirecciona a la vista de Login
        public ActionResult Login()
        {
            return View();
        }

        //Este es el action que valida los datos del login
        [HttpPost]
        public ActionResult Login(string nombre_usuario)
        {
            //La variable clv es para tomar el valor de la contraseña de la vista
            string clv = Request.Form["clave"];
            string nu_mayuscula = nombre_usuario.ToUpper();
            variablesGlobales.nom_usuario = nu_mayuscula;

            //Utilizo la base de datos para hacer el proceso de login
            using (SVContext db = new SVContext())
            {
                if(db.USUARIO.Any(c => c.CEDULA == nombre_usuario))
                {
                    var nmbr = from n in db.USUARIO
                               where n.CEDULA == nombre_usuario
                               select n;
                    variablesGlobales.nom_usuario = nmbr.FirstOrDefault().NOMBRE_USUARIO;
                }
                //Valida si los datos son correctos. Si son correctos, entonces se va a la vista de loggeado
                if (db.USUARIO.Any(u => (u.NOMBRE_USUARIO == nu_mayuscula || u.CEDULA == nu_mayuscula) && u.CLAVE == clv && u.INTENTOS < 3))
                {
                    var resetearIntentos = db.USUARIO.Single(x => x.NOMBRE_USUARIO == nu_mayuscula || x.CEDULA == nu_mayuscula);
                    resetearIntentos.INTENTOS = 0;
                    db.SaveChanges();
                    return RedirectToAction("Loggedin", "Cuenta");
                }
                else
                {
                    //Si el usuario se equivoca, se le suma 1 a su numeros de intentos fallidos y se bloquea al usuario
                    if (db.USUARIO.Any(a => (a.NOMBRE_USUARIO == nombre_usuario || a.CEDULA == nu_mayuscula) && a.INTENTOS >= 3))
                    {
                        var user = from us in db.USUARIO
                                   where us.NOMBRE_USUARIO == variablesGlobales.nom_usuario ||
                                   us.CEDULA == nu_mayuscula
                                   select us;
                        variablesGlobales.id_usuario = user.FirstOrDefault().ID_USUARIO;
                        variablesGlobales.id_rol = user.FirstOrDefault().ID_ROL;

                        var sumarIntento = db.USUARIO.Single(i => i.NOMBRE_USUARIO == nu_mayuscula);
                        sumarIntento.INTENTOS = sumarIntento.INTENTOS + 1;
                        sumarIntento.ID_ESTATUS = 2;
                        db.SaveChanges();
                        ViewBag.TheResult = 1;
                        return View();
                    }
                    else
                    {
                        try
                        {
                            //Aquí solo se le indica al usuario que se equivocó, ya que no está bloqueado
                            var sumar = db.USUARIO.Single(y => y.NOMBRE_USUARIO == nu_mayuscula ||
                            y.CEDULA == nu_mayuscula);
                            sumar.INTENTOS = sumar.INTENTOS + 1;
                            db.SaveChanges();
                            ViewBag.TheResult = 2;
                            return View();
                        }
                        catch
                        {
                            bool NnotEmpty = false;
                            bool CnotEmpty = false;

                            //Validaciones para que el usuario no deje los campos en blanco.
                            if (Request.Form["nombre_usuario"] == null || string.IsNullOrWhiteSpace(Request.Form["nombre_usuario"]))
                            {
                                ViewBag.Nmb = 1;
                            }
                            else
                            {
                                NnotEmpty = true;
                            }

                            if (Request.Form["clave"] == null || string.IsNullOrWhiteSpace(Request.Form["clave"]))
                            {
                                ViewBag.Clv = 1;
                            }
                            else
                            {
                                CnotEmpty = true;
                            }

                            if (NnotEmpty == true && CnotEmpty == true)
                            {
                                ViewBag.TheResult = 2;
                            }
                            return View();
                        }
                    }
                }
            }
        }

        //Este action es en donde ya muestra la vista del sistema, una vez que haya iniciado sesión.
        public ActionResult Loggedin()
        {
            using (SVContext db = new SVContext())
            {
                try
                {
                    //Toma los datos del usuario que ha ingresado al sistema, para posteriores funciones
                    var nom = from us in db.USUARIO
                              where us.NOMBRE_USUARIO == variablesGlobales.nom_usuario
                              select us;
                    variablesGlobales.id_usuario = nom.FirstOrDefault().ID_USUARIO;
                    variablesGlobales.id_rol = nom.FirstOrDefault().ID_ROL;
                    string nombre = nom.FirstOrDefault().NOMBRE;
                    ViewBag.Bienvenida = "Bienvenido(a) al sistema, " + nombre + " " + nom.FirstOrDefault().APELLIDO + ".";
                    variablesGlobales.nombre = nombre.First().ToString().ToUpper() + nombre.Substring(1).ToLower();
                    ViewBag.Nombre = variablesGlobales.nombre;

                    //Valida si el usuario es candidato o votante. Para mostrarlo cuando inicie sesión.
                    if (variablesGlobales.id_rol == 3 || variablesGlobales.id_rol == 4)
                    {
                        if (variablesGlobales.id_rol == 4)
                        {
                            ViewBag.EsCandidato = true;
                        }
                        else
                        {
                            ViewBag.EsCandidato = false;
                        }
                        return View("Loggedin");
                    }
                    else
                    {
                        ViewBag.EsAdmin = true;
                        return View("LoggedAdmin");
                    }
                }
                catch
                {
                    return View("Login");
                }
            }
        }

        //Desbloquear usuario (por más de tres intentos fallidos de inicio de sesión)
        public ActionResult Desbloquear()
        {
            using (SVContext db = new SVContext())
            {
                //Se muestran las preguntas que el usuario necesita responder para desbloquear la cuenta
                ViewBag.NombreUsuario = variablesGlobales.nom_usuario;
                var preg = from p in db.PREGUNTAS_SEGURIDAD
                           where p.ID_USUARIO == variablesGlobales.id_usuario
                           select p;

                var preg1 = preg.Where(p => p.N_PREGUNTA == 1);
                var preg2 = preg.Where(p => p.N_PREGUNTA == 2);
                var preg3 = preg.Where(p => p.N_PREGUNTA == 3);

                ViewBag.Pregunta1 = preg1.FirstOrDefault().PREGUNTA;
                ViewBag.Pregunta2 = preg2.FirstOrDefault().PREGUNTA;
                ViewBag.Pregunta3 = preg3.FirstOrDefault().PREGUNTA;

                return View();
            }
        }

        //Complementa al action desbloquear y olvidoclave
        [HttpPost]
        public ActionResult CambiarClave()
        {
            using (SVContext db = new SVContext())
            {
                //Valida si la persona respondió bien las preguntas de seguridad,
                //Si respondió bien, se redirige a la siguiente action para que cambie su clave.
                var preg = from p in db.PREGUNTAS_SEGURIDAD
                           where p.ID_USUARIO == variablesGlobales.id_usuario
                           select p;

                var datos = from d in db.USUARIO
                            where d.ID_USUARIO == variablesGlobales.id_usuario
                            select d;
                var preg1 = preg.Where(p => p.N_PREGUNTA == 1);
                var preg2 = preg.Where(p => p.N_PREGUNTA == 2);
                var preg3 = preg.Where(p => p.N_PREGUNTA == 3);
                var respuesta1 = preg1.FirstOrDefault().RESPUESTA;
                var respuesta2 = preg2.FirstOrDefault().RESPUESTA;
                var respuesta3 = preg3.FirstOrDefault().RESPUESTA;
                var r1 = Request.Form["respuesta1"].ToUpper();
                var r2 = Request.Form["respuesta2"].ToUpper();
                var r3 = Request.Form["respuesta3"].ToUpper();
                var ced = Request.Form["cedula"].ToUpper();
                var correo = Request.Form["correo"].ToUpper();

                if ((ced == datos.FirstOrDefault().CEDULA) && (correo.ToUpper() == datos.FirstOrDefault().CORREO) && (respuesta1 == r1) && (respuesta2 == r2) && (respuesta3 == r3))
                {
                    //Si responde bien las preguntas, se redirige a esta vista.
                    return View("CambiarClave");
                }
                else
                {
                    //Si no responde bien, se devuelve a la misma vista de desbloquear, indicando cuál fue el error.
                    ViewBag.Pregunta1 = preg1.FirstOrDefault().PREGUNTA;
                    ViewBag.Pregunta2 = preg2.FirstOrDefault().PREGUNTA;
                    ViewBag.Pregunta3 = preg3.FirstOrDefault().PREGUNTA;
                    ViewBag.NombreUsuario = variablesGlobales.nom_usuario;
                    ViewBag.Respuestas = false;
                    return View("Desbloquear");
                }
            }
        }

        //Complementa al action desbloquear y olvidoclave
        [HttpPost]
        public ActionResult ClaveCambiada()
        {
            using (SVContext db = new SVContext())
            {
                try
                {
                    //Valida si las contraseñas nuevas que el usuario ingresa coinciden 
                    //Si coinciden, se cambia la clave en la base de datos
                    if (Request.Form["clave_nueva"] == Request.Form["clave_confirmar"])
                    {
                        var user = db.USUARIO.Single(i => i.ID_USUARIO == variablesGlobales.id_usuario);
                        user.CLAVE = Request.Form["clave_nueva"];
                        user.INTENTOS = 0;
                        user.ID_ESTATUS = 1;
                        db.SaveChanges();
                        ViewBag.Coinciden = true;
                        variablesGlobales.id_usuario = 0;
                        variablesGlobales.id_rol = 0;
                        variablesGlobales.nom_usuario = "";
                        return View("Login");
                    }
                    else
                    {
                        //Si no coincide, se devuelve a la misma vista para indicar el error
                        ViewBag.Coinciden = false;
                        return View("CambiarClave");
                    }
                }
                catch
                {
                    //Si no coincide, se devuelve a la misma vista para indicar el error
                    ViewBag.Formato = false;
                    return View("CambiarClave");
                }
            }
        }

        //Action para mostrar la pantalla en donde se recupera la cuenta, por clave olvidada.
        public ActionResult OlvidoClave()
        {
            return View();
        }

        //Action para recuperar la cuenta, en caso de que se le olvidó la clave.
        [HttpPost]
        public ActionResult OlvidoClave(string user_name)
        {
            string user = user_name.ToUpper();
            variablesGlobales.nom_usuario = user;

            using (SVContext db = new SVContext())
            {
                try
                {
                    if (db.USUARIO.Any(n => n.NOMBRE_USUARIO == user))
                    {
                        variablesGlobales.nom_usuario = user;
                        var nom = from n in db.USUARIO
                                  where n.NOMBRE_USUARIO == variablesGlobales.nom_usuario
                                  select n;
                        variablesGlobales.id_usuario = nom.FirstOrDefault().ID_USUARIO;
                        ViewBag.Existe = true;
                        return RedirectToAction("Desbloquear", "Cuenta");
                    }
                    else
                    {
                        ViewBag.Existe = false;
                        return View();
                    }
                }
                catch
                {
                    ViewBag.Existe = false;
                    return View();
                }
            }
        }

        //Action para ver la información actual del perfil que tiene cada usuario
        public ActionResult VerPerfil()
        {
            ViewBag.CambioExitoso = TempData["CambioExitoso"];
            ViewBag.Nombre = variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                ViewBag.NombreUsuario = variablesGlobales.nom_usuario;
                var nom = from n in db.USUARIO
                          where n.NOMBRE_USUARIO == variablesGlobales.nom_usuario
                          select n;
                int id_partido = 0;
                if (db.CANDIDATO.Any(c => c.ID_USUARIO == variablesGlobales.id_usuario))
                {
                    var can = from c in db.CANDIDATO
                              where c.ID_USUARIO == variablesGlobales.id_usuario
                              select c;
                    id_partido = Convert.ToInt32(can.SingleOrDefault().ID_PARTIDO);

                    var par = from p in db.PARTIDO
                              where p.ID_PARTIDO == id_partido
                              select p;
                    ViewBag.Partido = par.SingleOrDefault().NOMBRE;

                }
                if (nom.FirstOrDefault().ID_ROL == 4)
                {
                    ViewBag.Candidato = true;
                }

                ViewBag.Cedula = nom.FirstOrDefault().CEDULA;
                if (nom.FirstOrDefault().ID_ROL == 3)
                {
                    ViewBag.TipoUsuario = "VOTANTE";
                }
                if (nom.FirstOrDefault().ID_ROL == 4)
                {
                    ViewBag.TipoUsuario = "CANDIDATO";
                }
                if (nom.FirstOrDefault().ID_ROL == 2 || nom.FirstOrDefault().ID_ROL == 1)
                {
                    ViewBag.TipoUsuario = "ADMINISTRADOR";
                }

                ViewBag.Descripcion = nom.FirstOrDefault().DESCRIPCION;
                ViewBag.FechaNacimiento = nom.FirstOrDefault().FECHA_NACIMIENTO;
                ViewBag.TipoEstudio = nom.FirstOrDefault().TIPO_ESTUDIO;
                ViewBag.NivelEstudio = nom.FirstOrDefault().NIVEL_ESTUDIO;
                ViewBag.Facultad = nom.FirstOrDefault().FACULTAD_SECCION;
                if (nom.FirstOrDefault().GENERO == "F")
                    ViewBag.genero = "Femenino";
                else
                    ViewBag.genero = "Masculino";

                if (nom.FirstOrDefault().ID_ROL == 2 || nom.FirstOrDefault().ID_ROL == 1)
                    return View("VerPerfilAdmin");
                else
                    return View();
            }
        }

        //Action para mostrar la vista de editar perfil
        public ActionResult EditarPerfil()
        {
            ViewBag.Nombre = variablesGlobales.nombre;
            ViewBag.CambioExitoso = TempData["CambioExitoso"];
            using (SVContext db = new SVContext())
            {
                ViewBag.NombreUsuario = variablesGlobales.nom_usuario;
                var nom = from n in db.USUARIO
                          where n.NOMBRE_USUARIO == variablesGlobales.nom_usuario
                          select n;
                if (nom.FirstOrDefault().ID_ROL == 4)
                {
                    ViewBag.Candidato = true;
                }

                ViewBag.Cedula = nom.FirstOrDefault().CEDULA;
                if (nom.FirstOrDefault().ID_ROL == 2 || nom.FirstOrDefault().ID_ROL == 1)
                {
                    ViewBag.TipoUsuario = "ADMINISTRADOR";
                }
                if (nom.FirstOrDefault().ID_ROL == 3)
                {
                    ViewBag.TipoUsuario = "VOTANTE";
                }
                if (nom.FirstOrDefault().ID_ROL == 4)
                {
                    ViewBag.TipoUsuario = "CANDIDATO";
                }

                if (nom.FirstOrDefault().ID_ROL == 2 || nom.FirstOrDefault().ID_ROL == 1)
                    return View("EditarPerfilAdmin");
                else
                    return View();
            }
        }

        //Action para actualizar el perfil
        [HttpPost]
        public ActionResult EditarPerfil(HttpPostedFileBase file)
        {
            ViewBag.Nombre = variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                try
                {
                    var user = db.USUARIO.Single(i => i.ID_USUARIO == variablesGlobales.id_usuario);
                    int id_partido = 0;
                    //if (variablesGlobales.id_rol == 4)
                    //{
                    //    var par = Request.Form["partido"];
                    //    var partido = from p in db.PARTIDO
                    //                  where p.NOMBRE == par
                    //                  select p;
                    //    id_partido = partido.SingleOrDefault().ID_PARTIDO;
                    //}
                    if (Request.Form["fecha_nacimiento"] == null || string.IsNullOrWhiteSpace(Request.Form["fecha_nacimiento"]))
                    {

                    }
                    else
                    {
                        user.FECHA_NACIMIENTO = Convert.ToDateTime(Request.Form["fecha_nacimiento"]);
                        db.SaveChanges();
                    }
                    if (Request.Form["tipo_estudio"] == null || string.IsNullOrWhiteSpace(Request.Form["tipo_estudio"]))
                    {

                    }
                    else
                    {
                        user.TIPO_ESTUDIO = Request.Form["tipo_estudio"].ToUpper();
                        db.SaveChanges();
                    }
                    if (Request.Form["nivel_estudio"] == null || string.IsNullOrWhiteSpace(Request.Form["nivel_estudio"]))
                    {

                    }
                    else
                    {
                        user.NIVEL_ESTUDIO = Request.Form["nivel_estudio"].ToUpper();
                        db.SaveChanges();
                    }
                    if (Request.Form["facultad"] == null || string.IsNullOrWhiteSpace(Request.Form["facultad"]))
                    {

                    }
                    else
                    {
                        user.FACULTAD_SECCION = Request.Form["facultad"].ToUpper();
                        db.SaveChanges();
                    }
                    if (Request.Form["genero"] == null || string.IsNullOrWhiteSpace(Request.Form["genero"]))
                    {

                    }
                    else
                    {
                        if (Request.Form["genero"] == "Femenino" || Request.Form["genero"] == "FEMENINO")
                        {
                            user.GENERO = "F";
                            db.SaveChanges();
                        }
                        else
                        {
                            user.GENERO = "M";
                            db.SaveChanges();
                        }
                    }
                    if (Request.Form["descripcion"] == null || string.IsNullOrWhiteSpace(Request.Form["descripcion"]))
                    {

                    }
                    else
                    {

                        user.DESCRIPCION = Request.Form["descripcion"];
                        db.SaveChanges();
                    }
                    if (Request.Form["partido"] == null || string.IsNullOrWhiteSpace(Request.Form["partido"]))
                    {

                    }
                    else
                    {
                        if (variablesGlobales.id_rol == 4)
                        {
                            var par = Request.Form["partido"];
                            var partido = from p in db.PARTIDO
                                          where p.NOMBRE == par
                                          select p;
                            id_partido = partido.SingleOrDefault().ID_PARTIDO;
                        }
                        //int id_partido = partido.SingleOrDefault().ID_PARTIDO;
                        if (db.CANDIDATO.Any(c => c.ID_USUARIO == variablesGlobales.id_usuario))
                        {
                            var candidato = db.CANDIDATO.Single(c => c.ID_USUARIO == variablesGlobales.id_usuario);
                            candidato.ID_PARTIDO = id_partido;
                            db.SaveChanges();
                        }
                        else
                        {
                            CANDIDATO c = new CANDIDATO
                            {
                                ID_USUARIO = variablesGlobales.id_usuario,
                                ID_PARTIDO = id_partido
                            };
                            db.CANDIDATO.Add(c);
                            db.SaveChanges();
                        }
                    }
                    TempData["CambioExitoso"] = true;
                    return RedirectToAction("VerPerfil", "Cuenta");
                }
                catch
                {
                    ViewBag.Exitoso = false;
                    return RedirectToAction("EditarPerfil", "Cuenta");
                }
            }
        }

        //Vista para ver la configuración actual del usuario
        public ActionResult Configuracion()
        {
            ViewBag.Nombre = variablesGlobales.nombre;
            ViewBag.CambioExitoso = TempData["CambioExitoso"];
            using (SVContext db = new SVContext())
            {
                var user = from u in db.USUARIO
                           where u.ID_USUARIO == variablesGlobales.id_usuario
                           select u;
                ViewBag.NombreUsuario = variablesGlobales.nom_usuario;
                ViewBag.Cedula = user.FirstOrDefault().CEDULA;
                ViewBag.Correo = user.FirstOrDefault().CORREO;
                if (user.FirstOrDefault().ID_ROL == 3)
                {
                    ViewBag.TipoUsuario = "VOTANTE";
                }
                else
                {
                    if (user.FirstOrDefault().ID_ROL == 4)
                    {
                        ViewBag.TipoUsuario = "CANDIDATO";
                    }
                    else
                    {
                        ViewBag.TipoUsuario = "ADMINISTRADOR";
                    }
                }

                var preg = from p in db.PREGUNTAS_SEGURIDAD
                           where p.ID_USUARIO == variablesGlobales.id_usuario
                           select p;

                var preg1 = preg.Where(p => p.N_PREGUNTA == 1);
                var preg2 = preg.Where(p => p.N_PREGUNTA == 2);
                var preg3 = preg.Where(p => p.N_PREGUNTA == 3);

                ViewBag.Pregunta1 = preg1.FirstOrDefault().PREGUNTA;
                ViewBag.Pregunta2 = preg2.FirstOrDefault().PREGUNTA;
                ViewBag.Pregunta3 = preg3.FirstOrDefault().PREGUNTA;
                if (user.FirstOrDefault().ID_ROL == 1 || user.FirstOrDefault().ID_ROL == 2)
                    return View("ConfiguracionAdmin");
                else
                    return View();
            }
        }

        //Action para mostrar el formulario, para que el usuario pueda actualizar sus datos
        //(Correo, preguntas de seguridad y contraseña).
        public ActionResult Configurar()
        {
            ViewBag.Nombre = variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                var user = from u in db.USUARIO
                           where u.ID_USUARIO == variablesGlobales.id_usuario
                           select u;
                ViewBag.NombreUsuario = variablesGlobales.nom_usuario;
                ViewBag.Cedula = user.FirstOrDefault().CEDULA;

                if (user.FirstOrDefault().ID_ROL == 3)
                {
                    ViewBag.TipoUsuario = "VOTANTE";
                }
                else
                {
                    if (user.FirstOrDefault().ID_ROL == 4)
                    {
                        ViewBag.TipoUsuario = "CANDIDATO";
                    }
                    else
                    {
                        ViewBag.TipoUsuario = "ADMINISTRADOR";
                    }
                }
                if (user.FirstOrDefault().ID_ROL == 1 || user.FirstOrDefault().ID_ROL == 2)
                    return View("ConfigurarAdmin");
                else
                    return View();
            }
        }

        //Action para actualizar el correo en la vista de configuración
        [HttpPost]
        public ActionResult ActualizarEmail()
        {
            using (SVContext db = new SVContext())
            {
                try
                {
                    if (Request.Form["correo"] == null || string.IsNullOrWhiteSpace(Request.Form["correo"]))
                    {
                        ViewBag.CambioCorreo = false;
                        return RedirectToAction("Configurar", "Cuenta");
                    }
                    else
                    {
                        var user = db.USUARIO.Single(i => i.ID_USUARIO == variablesGlobales.id_usuario);
                        user.CORREO = Request.Form["correo"].ToUpper();
                        db.SaveChanges();
                        TempData["CambioExitoso"] = true;
                        ViewBag.CambioCorreo = true;
                        return RedirectToAction("Configuracion", "Cuenta");
                    }
                }
                catch
                {
                    ViewBag.CambioCorreo = false;
                    return RedirectToAction("Configurar", "Cuenta");
                }
            }
        }

        //Action para actualizar las preguntas de seguridad en la vista de configuración.
        [HttpPost]
        public ActionResult ActualizarPreguntas()
        {
            ViewBag.Nombre = TempData["Nombre"];
            bool p1;
            bool p2;
            bool p3;
            bool r1;
            bool r2;
            bool r3;
            bool cambio1, cambio2, cambio3;

            using (SVContext db = new SVContext())
            {
                try
                {
                    var preg = from p in db.PREGUNTAS_SEGURIDAD
                               where p.ID_USUARIO == variablesGlobales.id_usuario
                               select p;

                    var preg1 = preg.Single(p => p.N_PREGUNTA == 1);
                    var preg2 = preg.Single(p => p.N_PREGUNTA == 2);
                    var preg3 = preg.Single(p => p.N_PREGUNTA == 3);

                    if (Request.Form["pregunta1"] == null || string.IsNullOrWhiteSpace(Request.Form["pregunta1"]))
                        p1 = false;
                    else
                        p1 = true;
                    if (Request.Form["respuesta1"] == null || string.IsNullOrWhiteSpace(Request.Form["respuesta1"]))
                        r1 = false;
                    else
                        r1 = true;
                    if (Request.Form["pregunta2"] == null || string.IsNullOrWhiteSpace(Request.Form["pregunta2"]))
                        p2 = false;
                    else
                        p2 = true;
                    if (Request.Form["respuesta2"] == null || string.IsNullOrWhiteSpace(Request.Form["respuesta2"]))
                        r2 = false;
                    else
                        r2 = true;
                    if (Request.Form["pregunta3"] == null || string.IsNullOrWhiteSpace(Request.Form["pregunta3"]))
                        p3 = false;
                    else
                        p3 = true;
                    if (Request.Form["respuesta3"] == null || string.IsNullOrWhiteSpace(Request.Form["respuesta3"]))
                        r3 = false;
                    else
                        r3 = true;

                    if ((p1 == true) && (r1 == true))
                    {
                        preg1.PREGUNTA = Request.Form["pregunta1"].ToUpper();
                        preg1.RESPUESTA = Request.Form["respuesta1"].ToUpper();
                        db.SaveChanges();
                        cambio1 = true;
                    }
                    else
                    {
                        cambio1 = false;
                    }

                    if ((p2 == true) && (r2 == true))
                    {
                        preg2.PREGUNTA = Request.Form["pregunta2"].ToUpper();
                        preg2.RESPUESTA = Request.Form["respuesta2"].ToUpper();
                        db.SaveChanges();
                        cambio2 = true;
                    }
                    else
                    {
                        cambio2 = false;
                    }

                    if ((p3 == true) && (r3 == true))
                    {
                        preg3.PREGUNTA = Request.Form["pregunta3"].ToUpper();
                        preg3.RESPUESTA = Request.Form["respuesta3"].ToUpper();
                        db.SaveChanges();
                        cambio3 = true;
                    }
                    else
                    {
                        cambio3 = false;
                    }

                    if ((cambio1 == true) || (cambio2 == true) || (cambio3 == true))
                    {
                        ViewBag.CambioPreguntas = true;
                        TempData["CambioExitoso"] = true;
                        return RedirectToAction("Configuracion", "Cuenta");
                    }
                    else
                    {
                        ViewBag.CambioPreguntas = false;
                        return RedirectToAction("Configurar", "Cuenta");
                    }
                }
                catch
                {
                    ViewBag.CambioPreguntas = false;
                    return RedirectToAction("Configurar", "Cuenta");
                }
            }
        }


        //Action para actualizar la clave en  la vista de configuración.
        [HttpPost]
        public ActionResult ActualizarClave()
        {
            bool claveActual;
            bool claveConfirmado;
            using (SVContext db = new SVContext())
            {
                try
                {
                    if (Request.Form["clave_vieja"] == null || string.IsNullOrWhiteSpace(Request.Form["clave_vieja"])
                    || Request.Form["clave_nueva"] == null || string.IsNullOrWhiteSpace(Request.Form["clave_nueva"])
                    || Request.Form["clave_confirmar"] == null || string.IsNullOrWhiteSpace(Request.Form["clave_confirmar"]))
                    {
                        ViewBag.Vacio = true;
                        return RedirectToAction("Configurar", "Cuenta");
                    }
                    else
                    {
                        ViewBag.Vacio = false;
                        var clv = db.USUARIO.Single(c => c.ID_USUARIO == variablesGlobales.id_usuario);
                        if (clv.CLAVE == Request.Form["clave_vieja"])
                        {
                            ViewBag.ClaveActual = false;
                            claveActual = true;
                        }
                        else
                        {
                            claveActual = false;
                            ViewBag.ClaveActual = false;
                        }

                        if (Request.Form["clave_nueva"] == Request.Form["clave_confirmar"])
                        {
                            ViewBag.ClaveCoinciden = true; ;
                            claveConfirmado = true;
                        }
                        else
                        {
                            claveConfirmado = false;
                            ViewBag.ClaveCoinciden = false;
                        }

                        if ((claveActual == true) && (claveConfirmado == true))
                        {
                            clv.CLAVE = Request.Form["clave_nueva"];
                            db.SaveChanges();
                            TempData["CambioExitoso"] = true;
                            return RedirectToAction("Configuracion", "Cuenta");
                        }
                        else
                        {
                            return RedirectToAction("Configurar", "Cuenta");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Configurar", "Cuenta");
                }

            }
        }

        //Action para cerrar sesión. Se inician de nuevo las variables globales.
        public ActionResult LogOut()
        {
            variablesGlobales.nom_usuario = "";
            variablesGlobales.nombre = "";
            variablesGlobales.id_rol = 0;
            variablesGlobales.id_usuario = 0;
            ViewBag.Logout = true;
            return View("Login");
        }
    }
}