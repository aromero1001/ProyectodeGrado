using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectodeGrado.Models.SVModel;
using CaptchaMvc.HtmlHelpers;

namespace ProyectodeGrado.Controllers
{

    public class RegistroController : Controller
    {

        private SVContext db = new SVContext();
        /// <summary>
        /// Este Action va realizar la validacion si la cedula se encuentra en cedulas aprobadas 
        /// </summary>
        /// <param name="cedula">Este parametro tiene que ver con la cedula ingresada en la ventana modal cuando deseas registrarte</param>
        /// <returns></returns>
        public ActionResult Validacion(string cedula)
        {
            //Esta consulta se encarga de recoger las cedulas que estan en las cedulas aprobadas
                var cedulasEncontradas = from p in db.CEDULAS_APROBADAS
                                  .Where(p => p.CEDULA == cedula)
                                         select p.CEDULA;
            //Esta consulta se encarga de recoger las cedulas que estan en usuarios, quiere dercir , que esta registrado
                var cedulasEncontradas2 = from t in db.USUARIO
                                          .Where(t => t.CEDULA == cedula)
                                          select t.CEDULA;
            var id_rol = from i in db.CEDULAS_APROBADAS
                         .Where(i => i.CEDULA == cedula)
                         select i.ID_ROL;
            //Este if valida que si la cedula tiene un registro en cedulas aprobadas y mas de uno en usuario 
                if ((cedulasEncontradas.Count() == 0) || (cedulasEncontradas2.Count() > 0))
                {
                //Mensaje de que el usuario existe en el sistema o no esta autorizado para registrarse
                    return View("ErrorValidacion");
                }
                else
                {
                //Redirecciona al controlador de registro y guarda la cedula que se valida
                TempData["cedula"] = cedula;
                TempData["id_rol"] = id_rol;
                return RedirectToAction("Registro");
                }
            

        }
        public ActionResult Registro()
        {
           
            //La cedula que esta en el dato temporal se guarda en el viewbag para ser usada en el campo de ceedula del registro
            ViewBag.CEDULA = new SelectList(db.CEDULAS_APROBADAS, "CEDULA", "CEDULA");
            ViewBag.cedula = TempData["cedula"].ToString();
            ViewBag.id_rol = TempData["id_rol"];
            //Redirecciona a la vista donde esta el registro
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro([Bind(Include = "ID_USUARIO,ID_ESTATUS,CEDULA,NOMBRE_USUARIO,CLAVE,CORREO,FOTO_PERFIL,DESCRIPCION,NOMBRE,APELLIDO,TIPO_ESTUDIO,NIVEL_ESTUDIO,FACULTAD_SECCION,GENERO,FECHA_NACIMIENTO,ID_ROL,INTENTOS")] USUARIO uSUARIO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Si el modelo esta bien agrega lo guardado en el objeto y guardalo en la tabla y guardas el id  para ser usado en el resgistro
                    //de preguntas
                    db.USUARIO.Add(uSUARIO);
                    db.SaveChanges();
                    TempData["idusuario"] = uSUARIO.ID_USUARIO;
                    //Redirecciona a las preguntas de seguridad, al action
                    return RedirectToAction("PreguntasSeguridad1");
                }

                //Si no quiere decir que el modelo esta presentando problemas
                return View("ErrorRegistro");
            }
            catch
            {
                return View("ErrorRegistro");
            }
        }

        public ActionResult PreguntasSeguridad()
        {
            //Guardas el ID en un viewbag para ser usado en el campo de ID de la tabla preguntas de seguridad y asi poder ser relacionado
            ViewBag.Captcha = TempData["Captcha"];
            ViewBag.idusuario = TempData["idusuario"].ToString();
            TempData["idusuario2"] = TempData["idusuario"];
            return View("PreguntasSeguridad");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreguntasSeguridad([Bind(Include = "ID_USUARIO,N_PREGUNTA,PREGUNTA,RESPUESTA")] PREGUNTAS_SEGURIDAD pREGUNTAS_SEGURIDAD)
        {

            while (ModelState.IsValid)
            {
                TempData["idusuario"] = TempData["idusuario2"];
                //Si el modelo esta bien entonces guardas las preguntas
                //Redirecciona al action de gfinalizar registro
                if (this.IsCaptchaValid("Captcha is not valid"))
                {
                    db.PREGUNTAS_SEGURIDAD.Add(pREGUNTAS_SEGURIDAD);
                    db.SaveChanges();
                    return RedirectToAction("FinalizarRegistro");
                    //Puedes registrate
                }
                else
                {
                    //Existe un error en el captcha 
                    TempData["Captcha"] = false;
                    ViewBag.ErrMessage = "Error: El captcha no es válido.";
                    return RedirectToAction("PreguntasSeguridad");
                }
                //return RedirectToAction("FinalizarRegistro");

            }
            //if (this.IsCaptchaValid("Captcha is not valid"))
            //{
            //    //Puedes registrate
            //}
            //else
            //{
            //    //Existe un error en el captcha 
            //    ViewBag.ErrMessage = "Error: captcha is not valid.";
            //}
            //return View("Error");
            return RedirectToAction("PreguntasSeguridad");
        }

        //PRIMERA PREGUNTA
        public ActionResult PreguntasSeguridad1()
        {
            //Guardas el ID en un viewbag para ser usado en el campo de ID de la tabla preguntas de seguridad y asi poder ser relacionado
            ViewBag.idusuario = TempData["idusuario"].ToString();
            TempData["idusuario2"] = TempData["idusuario"];
            return View("PreguntasSeguridad1");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreguntasSeguridad1([Bind(Include = "ID_USUARIO,N_PREGUNTA,PREGUNTA,RESPUESTA")] PREGUNTAS_SEGURIDAD pREGUNTAS_SEGURIDAD)
        {

            while (ModelState.IsValid)
            {
                TempData["idusuario"] = TempData["idusuario2"];
                db.PREGUNTAS_SEGURIDAD.Add(pREGUNTAS_SEGURIDAD);
                db.SaveChanges();
                return RedirectToAction("PreguntasSeguridad2");

            }
            return RedirectToAction("PreguntasSeguridad2");
        }
        //FIN PRIMERA PREGUNTA

        //COMIENZO DE SEGUNDA PREGUNTA
        public ActionResult PreguntasSeguridad2()
        {
            //Guardas el ID en un viewbag para ser usado en el campo de ID de la tabla preguntas de seguridad y asi poder ser relacionado
            ViewBag.idusuario = TempData["idusuario"].ToString();
            TempData["idusuario2"] = TempData["idusuario"];
            return View("PreguntasSeguridad2");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreguntasSeguridad2([Bind(Include = "ID_USUARIO,N_PREGUNTA,PREGUNTA,RESPUESTA")] PREGUNTAS_SEGURIDAD pREGUNTAS_SEGURIDAD)
        {

            while (ModelState.IsValid)
            {
                TempData["idusuario"] = TempData["idusuario2"];
                db.PREGUNTAS_SEGURIDAD.Add(pREGUNTAS_SEGURIDAD);
                db.SaveChanges();
                return RedirectToAction("PreguntasSeguridad");

            }
            return RedirectToAction("PreguntasSeguridad");
        }
        // FIN DE SEGUNDA PREGUNTA
        public ActionResult FinalizarRegistro()
        {
            //Muestra un mensaje de que el registro finalizo con exito
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}
