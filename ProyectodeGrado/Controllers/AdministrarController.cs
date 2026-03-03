using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectodeGrado.Models.SVModel;
using System.Drawing.Imaging;

namespace ProyectodeGrado.Controllers
{
    public class AdministrarController : Controller
    {
        // GET: Administrar
        public ActionResult Elecciones()
        {
            using (SVContext db = new SVContext())
            {
                ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
                var ele = db.ELECCION.Single(e => e.ID_ELECCION == 1);
                if (ele.TITULO == "TITULO")
                {
                    ViewBag.Configurado = false;
                    return View();
                }
                else
                {
                    ViewBag.Exitoso = TempData["exitoso"];
                    ViewBag.Titulo = ele.TITULO;
                    ViewBag.FechaInicio = ele.FECHA_INICIO.ToShortDateString().ToString();
                    ViewBag.FechaFin = ele.FECHA_FIN.ToShortDateString().ToString();
                    ViewBag.Hora = ele.FECHA_FIN.ToShortTimeString();
                    ViewBag.TipoInstituto = ele.TIPO_INSTITUTO;
                    ViewBag.Descripcion = ele.DESCRIPCION;
                    ViewBag.Duracion = ele.DURACION_VOTO;
                    ViewBag.NombreInstituto = ele.NOMBRE_INSTITUTO;
                    if (ele.ID_TIPO_ELECCION == 1)
                        ViewBag.TipoEleccion = "Candidatos";
                    if (ele.ID_TIPO_ELECCION == 2)
                        ViewBag.TipoEleccion = "Partidos";
                    if (ele.ID_TIPO_ELECCION == 3)
                        ViewBag.TipoEleccion = "Propuesta";
                    return View();
                }
            }
        }

        public ActionResult ConfigurarElecciones()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        [HttpPost]
        public ActionResult ConfigurarElecciones(HttpPostedFileBase logo)
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                bool exitoso = false;
                string fecha_fin;
                int tipo_eleccion = 0;
                var ele = db.ELECCION.Single(e => e.ID_ELECCION == 1);
                try
                {
                    if (Request.Form["titulo"] == null || string.IsNullOrWhiteSpace(Request.Form["titulo"]))
                    {

                    }
                    else
                    {
                        ele.TITULO = Request.Form["titulo"].ToUpper();
                        db.SaveChanges();
                        exitoso = true;
                    }

                    if (Request.Form["fecha_inicio"] == null || string.IsNullOrWhiteSpace(Request.Form["fecha_inicio"]))
                    {

                    }
                    else
                    {
                        if (Convert.ToDateTime(Request.Form["fecha_inicio"]) < Convert.ToDateTime(Request.Form["fecha_fin"]))
                        {
                            ele.FECHA_INICIO = Convert.ToDateTime(Request.Form["fecha_inicio"]);
                            db.SaveChanges();
                            exitoso = true;
                        }
                        else
                        {
                            ViewBag.fecha = false;
                        }
                            
                    }

                    if (Request.Form["fecha_fin"] == null || string.IsNullOrWhiteSpace(Request.Form["fecha_fin"]))
                    {

                    }
                    else
                    {
                        fecha_fin = Request.Form["fecha_fin"];
                    }

                    if (Request.Form["hora"] == null || string.IsNullOrWhiteSpace(Request.Form["hora"]))
                    {

                    }
                    else
                    {
                        if (Convert.ToDateTime(Request.Form["fecha_inicio"]) < Convert.ToDateTime(Request.Form["fecha_fin"]))
                        {
                            fecha_fin = Request.Form["fecha_fin"];
                            DateTime date = Convert.ToDateTime(fecha_fin);
                            DateTime time = Convert.ToDateTime(Request.Form["hora"]);
                            DateTime CompleteDate = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                            ele.FECHA_FIN = CompleteDate;
                            db.SaveChanges();
                            exitoso = true;
                        }
                        else
                        {
                            ViewBag.fecha = false;
                        }
                    }

                    if (Request.Form["nombre_institucion"] == null || string.IsNullOrWhiteSpace(Request.Form["nombre_institucion"]))
                    {

                    }
                    else
                    {
                        ele.NOMBRE_INSTITUTO = Request.Form["nombre_institucion"].ToUpper();
                        db.SaveChanges();
                        exitoso = true;
                    }

                    if (Request.Form["nombre_institucion"] == null || string.IsNullOrWhiteSpace(Request.Form["nombre_institucion"]))
                    {

                    }
                    else
                    {
                        ele.NOMBRE_INSTITUTO = Request.Form["nombre_institucion"].ToUpper();
                        db.SaveChanges();
                        exitoso = true;
                    }

                    if (Request.Form["t_institucion"] == null || string.IsNullOrWhiteSpace(Request.Form["t_institucion"]))
                    {

                    }
                    else
                    {
                        ele.TIPO_INSTITUTO = Request.Form["t_institucion"].ToUpper();
                        db.SaveChanges();
                        exitoso = true;
                    }

                    if (Request.Form["descripcion"] == null || string.IsNullOrWhiteSpace(Request.Form["descripcion"]))
                    {

                    }
                    else
                    {
                        ele.DESCRIPCION = Request.Form["descripcion"];
                        db.SaveChanges();
                        exitoso = true;
                    }

                    if (Request.Form["duracion"] == null || string.IsNullOrWhiteSpace(Request.Form["duracion"]))
                    {

                    }
                    else
                    {
                        int duracion = Convert.ToInt32(Request.Form["duracion"]);
                        ele.DURACION_VOTO = duracion;
                        db.SaveChanges();
                        exitoso = true;
                    }
                    if (Request.Form["eleccion"] == null || string.IsNullOrWhiteSpace(Request.Form["eleccion"]))
                    {

                    }
                    else
                    {
                        if (Request.Form["eleccion"] == "Candidatos")
                            tipo_eleccion = 1;
                        if (Request.Form["eleccion"] == "Partidos")
                            tipo_eleccion = 2;
                        if (Request.Form["eleccion"] == "Propuesta")
                            tipo_eleccion = 3;

                        ele.ID_TIPO_ELECCION = tipo_eleccion;
                        db.SaveChanges();
                        exitoso = true;
                    }
                    if (exitoso == true)
                    {
                        TempData["exitoso"] = true;
                        return RedirectToAction("Elecciones", "Administrar");
                    }
                    else
                    {
                        ViewBag.Exitoso = false;
                        return View("ConfigurarElecciones");
                    }
                }
                catch
                {
                    ViewBag.Exitoso = false;
                    return View("ConfigurarElecciones");
                }
            }
        }

        public ActionResult AdministrarUsuarios()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        public ActionResult BuscarUsuario()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                string cedula = Request.Form["ced_buscar"];
                if (db.CEDULAS_APROBADAS.Any(c => c.CEDULA == cedula))
                {
                    ViewBag.Existe = true;
                    var usuario = from u in db.CEDULAS_APROBADAS
                                  where u.CEDULA == cedula
                                  select u;

                    ViewBag.Cedula = cedula;
                    var Nombre = usuario.SingleOrDefault().NOMBRE;
                    ViewBag.Nombre = Nombre;
                    var Apellido = usuario.SingleOrDefault().APELLIDO;
                    ViewBag.Apellido = Apellido;
                    if (usuario.SingleOrDefault().ID_ROL == 1 || usuario.SingleOrDefault().ID_ROL == 2)
                        ViewBag.TipoUsuario = "ADMINISTRADOR";
                    if (usuario.SingleOrDefault().ID_ROL == 3)
                        ViewBag.TipoUsuario = "VOTANTE";
                    if (usuario.SingleOrDefault().ID_ROL == 4)
                        ViewBag.TipoUsuario = "CANDIDATO";

                    return View("AdministrarUsuarios");
                }
                else
                {
                    ViewBag.Existe = false;
                    return View("AdministrarUsuarios");
                }
            }
        }

        public ActionResult ActualizarUsuarios()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            if (Request.Form["ced"] == null || string.IsNullOrWhiteSpace(Request.Form["ced"])
                || Request.Form["nombre"] == null || string.IsNullOrWhiteSpace(Request.Form["nombre"])
                || Request.Form["apellido"] == null || string.IsNullOrWhiteSpace(Request.Form["apellido"])
                || Request.Form["ced"] == null || string.IsNullOrWhiteSpace(Request.Form["tipo_usuario"]))
            {
                ViewBag.Lleno = false;
                return View("AdministrarUsuarios");
            }
            else
            {
                ViewBag.Lleno = true;
            }

            var cedula = Request.Form["ced"];
            var nombre = Request.Form["nombre"].ToUpper();
            var apellido = Request.Form["apellido"].ToUpper();
            int tipo_usuario = 0;
            if (Request.Form["tipo_usuario"] == "Candidato")
                tipo_usuario = 4;
            if (Request.Form["tipo_usuario"] == "Votante")
                tipo_usuario = 3;
            if (Request.Form["tipo_usuario"] == "Administrador")
                tipo_usuario = 2;

            using (SVContext db = new SVContext())
            {
                if (Request.Form["accion"] == "Agregar")
                {
                    CEDULAS_APROBADAS ca = new CEDULAS_APROBADAS
                    {
                        CEDULA = cedula,
                        NOMBRE = nombre,
                        APELLIDO = apellido,
                        ID_ROL = tipo_usuario
                    };

                    db.CEDULAS_APROBADAS.Add(ca);
                    try
                    {
                        db.SaveChanges();
                        return View("AdministrarUsuarios");
                    }
                    catch
                    {
                        return View("AdministrarUsuarios");
                    }
                }
                else
                {
                    if (Request.Form["accion"] == "Modificar")
                    {
                        if (db.CEDULAS_APROBADAS.Any(c => c.CEDULA == cedula))
                        {
                            var ca = db.CEDULAS_APROBADAS.Single(u => u.CEDULA == cedula);
                            ca.NOMBRE = nombre;
                            ca.APELLIDO = apellido;
                            ca.ID_ROL = tipo_usuario;
                            if (db.USUARIO.Any(u => u.CEDULA == cedula))
                            {
                                var usuario = db.USUARIO.Single(u => u.CEDULA == cedula);
                                usuario.ID_ROL = tipo_usuario;
                            }
                            db.SaveChanges();
                            return View("AdministrarUsuarios");
                        }
                        else
                        {
                            ViewBag.NoExiste = true;
                            return View("AdministrarUsuarios");
                        }
                    }
                    else
                    {
                        ViewBag.Opcion = false;
                        return View("AdministrarUsuarios");
                    }
                }
            }
        }

        public ActionResult EliminarUsuario()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                string cedula = Request.Form["ced_eliminar"];
                try
                {
                    if (db.CEDULAS_APROBADAS.Any(c => c.CEDULA == cedula && c.ID_ROL != 2 && c.ID_ROL != 1))
                    {
                        var usuario = (from u in db.CEDULAS_APROBADAS
                                       where u.CEDULA == cedula
                                       select u).FirstOrDefault();
                        db.CEDULAS_APROBADAS.Remove(usuario);
                        db.SaveChanges();
                        return View("AdministrarUsuarios");
                    }
                    else
                    {
                        ViewBag.Error = true;
                        return View("AdministrarUsuarios");
                    }
                }
                catch
                {
                    return View("AdministrarUsuarios");
                }
            }
        }

        public ActionResult VerCandidatos()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        public ActionResult AdministrarPartido()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        [HttpPost]
        public ActionResult ActualizarPartido(HttpPostedFileBase imagen)
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            if (Request.Form["partido"] == null || string.IsNullOrWhiteSpace(Request.Form["partido"])
                || Request.Form["descripcion"] == null || string.IsNullOrWhiteSpace(Request.Form["descripcion"]))
            {
                ViewBag.Lleno = false;
                return View("AdministrarPartido");
            }
            else
            {
                ViewBag.Lleno = true;
            }

            var partido = Request.Form["partido"].ToUpper();
            var descripcion = Request.Form["descripcion"];

            using (SVContext db = new SVContext())
            {
                PARTIDO p = new PARTIDO
                {
                    ID_TIPO_ELECCION = 2,
                    NOMBRE = partido,
                    DESCRIPCION = descripcion,
                    FOTO = null
                };

                db.PARTIDO.Add(p);
                try
                {
                    db.SaveChanges();
                    if (imagen != null)
                    {
                        string img = System.IO.Path.Combine(Server.MapPath("~/images"), "prueba");
                        imagen.SaveAs(img);
                    }
                    return View("AdministrarPartido");
                }
                catch
                {
                    return View("AdministrarPartido");
                }
            }
        }

        [HttpPost]
        public ActionResult ModificarPartido()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            if (Request.Form["partido_v"] == null || string.IsNullOrWhiteSpace(Request.Form["partido_v"]))
            {
                ViewBag.Lleno = false;
                return View("AdministrarPartido");
            }
            else
            {
                ViewBag.Lleno = true;
            }

            var partido_v = Request.Form["partido_v"].ToUpper();
            var partido_n = Request.Form["partido_n"].ToUpper();
            var descripcion = Request.Form["descripcion"];

            using (SVContext db = new SVContext())
            {
                if (db.PARTIDO.Any(p => p.NOMBRE == partido_v))
                {
                    var par = db.PARTIDO.Single(p => p.NOMBRE == partido_v);
                    if (Request.Form["partido_n"] == null || string.IsNullOrWhiteSpace(Request.Form["partido_n"]))
                    {

                    }
                    else
                        par.NOMBRE = partido_n;

                    if(Request.Form["descripcion"] == null || string.IsNullOrWhiteSpace(Request.Form["descripcion"]))
                    {

                    }
                    else
                        par.DESCRIPCION = descripcion;

                    db.SaveChanges();
                    return View("AdministrarPartido");
                }
                else
                {
                    ViewBag.NoExiste = true;
                    return View("AdministrarPartido");
                }
            }
        }

        [HttpPost]
        public ActionResult EliminarPartido()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                string partido = Request.Form["partido_eliminar"].ToUpper();
                try
                {
                    if (db.PARTIDO.Any(p => p.NOMBRE == partido))
                    {
                        var par = (from p in db.PARTIDO
                                   where p.NOMBRE == partido
                                   select p).FirstOrDefault();
                        db.PARTIDO.Remove(par);
                        db.SaveChanges();
                        return View("AdministrarPartido");
                    }
                    else
                    {
                        ViewBag.Error = true;
                        return View("AdministrarPartido");
                    }
                }
                catch
                {
                    return View("AdministrarPartido");
                }
            }
        }

        [HttpPost]
        public ActionResult BuscarPartido()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                string partido = Request.Form["partido_buscar"].ToUpper();
                if (db.PARTIDO.Any(p => p.NOMBRE == partido))
                {
                    ViewBag.Existe = true;
                    var par = from p in db.PARTIDO
                                  where p.NOMBRE == partido
                                  select p;

                    ViewBag.NombrePartido = partido;
                    var descripcion = par.SingleOrDefault().DESCRIPCION;
                    ViewBag.Descripcion = descripcion;

                    return View("AdministrarPartido");
                }
                else
                {
                    ViewBag.Existe = false;
                    return View("AdministrarPartido");
                }
            }
        }

        public ActionResult VerPartidos()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                return View();
            }
        }

        public ActionResult AdministrarPropuestas()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                try
                {
                    ViewBag.EliminarOpcion = TempData["EliminarOpcion"];
                    ViewBag.AgregarPropuesta = TempData["AgregarPropuesta"];
                    ViewBag.AgregarOpcion = TempData["AgregarOpcion"];
                    ViewBag.ModificarOpcion = TempData["ModificarOpcion"];
                    var propuesta = (from prop in db.PROPUESTA
                                     select prop).FirstOrDefault();
                    ViewBag.Propuesta = propuesta.TITULO;
                    return View();
                }
                catch
                {
                    return View("AgregarPropuesta");
                }
            }
        }

        public ActionResult AgregarPropuesta()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        [HttpPost]
        public ActionResult AgregarPropuesta(string propuesta)
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                var pro = Request.Form["propuesta"].ToUpper();
                string descripcion = Request.Form["descripcion"];
                try
                {
                    if (Request.Form["propuesta"] == null || string.IsNullOrWhiteSpace(Request.Form["propuesta"])
                        || Request.Form["descripcion"] == null || string.IsNullOrWhiteSpace(Request.Form["descripcion"])
                        || Request.Form["opcion1"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion1"])
                        || Request.Form["opcion2"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion2"]))
                    {
                        ViewBag.Completo = false;
                        return View();
                    }
                    else
                    {
                        PROPUESTA p = new PROPUESTA
                        {
                            ID_TIPO_ELECCION = 3,
                            TITULO = pro,
                            DESCRIPCION = descripcion
                        };
                        db.PROPUESTA.Add(p);
                        db.SaveChanges();

                        if (db.PROPUESTA.Any(pr => pr.TITULO == pro))
                        {
                            var propues = (from prop in db.PROPUESTA
                             where prop.TITULO == pro
                             select prop).FirstOrDefault();
                            int id_propuesta = propues.ID_PROPUESTA;

                            OPCION_PROPUESTA op1 = new OPCION_PROPUESTA
                            {
                                ID_PROPUESTA = id_propuesta,
                                OPCION = Request.Form["opcion1"]
                            };
                            db.OPCION_PROPUESTA.Add(op1);
                            db.SaveChanges();

                            OPCION_PROPUESTA op2 = new OPCION_PROPUESTA
                            {
                                ID_PROPUESTA = id_propuesta,
                                OPCION = Request.Form["opcion2"]
                            };
                            db.OPCION_PROPUESTA.Add(op2);
                            db.SaveChanges();

                            if (Request.Form["opcion3"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion3"]))
                            {

                            }
                            else
                            {
                                OPCION_PROPUESTA op3 = new OPCION_PROPUESTA
                                {
                                    ID_PROPUESTA = id_propuesta,
                                    OPCION = Request.Form["opcion3"]
                                };
                                db.OPCION_PROPUESTA.Add(op3);
                                db.SaveChanges();
                            }

                            if (Request.Form["opcion4"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion4"]))
                            {

                            }
                            else
                            {
                                OPCION_PROPUESTA op4 = new OPCION_PROPUESTA
                                {
                                    ID_PROPUESTA = id_propuesta,
                                    OPCION = Request.Form["opcion4"]
                                };
                                db.OPCION_PROPUESTA.Add(op4);
                                db.SaveChanges();
                            }

                            if (Request.Form["opcion5"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion5"]))
                            {

                            }
                            else
                            {
                                OPCION_PROPUESTA op5 = new OPCION_PROPUESTA
                                {
                                    ID_PROPUESTA = id_propuesta,
                                    OPCION = Request.Form["opcion5"]
                                };
                                db.OPCION_PROPUESTA.Add(op5);
                                db.SaveChanges();
                            }
                        }
                        TempData["AgregarPropuesta"] = true;
                        return RedirectToAction("AdministrarPropuestas", "Administrar");
                    }
                }
                catch
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult AgregarOpcion()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                int id_propuesta = 0;
                var id = (from prop in db.PROPUESTA
                              select prop).FirstOrDefault();
                id_propuesta = id.ID_PROPUESTA;

                if (Request.Form["opcion"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion"]))
                {
                    TempData["AgregarOpcion"] = false;
                    return RedirectToAction("AdministrarPropuestas", "Administrar");
                }
                else
                {
                    OPCION_PROPUESTA op = new OPCION_PROPUESTA
                    {
                        ID_PROPUESTA = id_propuesta,
                        OPCION = Request.Form["opcion"]
                    };
                    db.OPCION_PROPUESTA.Add(op);
                    db.SaveChanges();
                    TempData["AgregarOpcion"] = true;
                    return RedirectToAction("AdministrarPropuestas", "Administrar");
                }
            }
        }

        [HttpPost]
        public ActionResult ModificarOpcion()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                string opcion = Request.Form["opcion"];
                if (Request.Form["opcion"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion"])
                    || Request.Form["n_opcion"] == null || string.IsNullOrWhiteSpace(Request.Form["n_opcion"]))
                {
                    TempData["ModificarOpcion"] = false;
                    return RedirectToAction("AdministrarPropuestas", "Administrar");
                }
                else
                {
                    var op = db.OPCION_PROPUESTA.Single(o => o.OPCION == opcion);
                    op.OPCION = Request.Form["n_opcion"].ToUpper();
                    db.SaveChanges();
                    TempData["ModificarOpcion"] = true;
                    return RedirectToAction("AdministrarPropuestas", "Administrar");
                }
            }
        }

        [HttpPost]
        public ActionResult EliminarOpcion()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                string opcion = Request.Form["opcion"];
                if (Request.Form["opcion"] == null || string.IsNullOrWhiteSpace(Request.Form["opcion"]))
                {
                    TempData["EliminarOpcion"] = false;
                    return RedirectToAction("AdministrarPropuestas", "Administrar");
                }
                else
                {
                    var op = (from o in db.OPCION_PROPUESTA
                               where o.OPCION == opcion
                               select o).FirstOrDefault();

                    db.OPCION_PROPUESTA.Remove(op);
                    db.SaveChanges();
                    TempData["EliminarOpcion"] = true;
                    return RedirectToAction("AdministrarPropuestas", "Administrar");
                }
            }
        }

        public ActionResult EliminarPropuesta()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            using (SVContext db = new SVContext())
            {
                try
                {
                    var opcion = (from o in db.OPCION_PROPUESTA
                              select o).ToList();
                    foreach (var o in opcion)
                    {
                        db.OPCION_PROPUESTA.Remove(o);
                        db.SaveChanges();
                    }

                    var propuesta = (from p in db.PROPUESTA
                                     select p).FirstOrDefault();
                    db.PROPUESTA.Remove(propuesta);
                    db.SaveChanges();
                    ViewBag.EliminarPropuesta = true;
                    return RedirectToAction("AgregarPropuesta", "Administrar");

                }
                catch
                {
                    ViewBag.EliminarPropuesta = false;
                    return RedirectToAction("AgregarPropuesta", "Administrar");
                }
            }
        }

        [HttpPost]
        public ActionResult SubirFotoPartido(HttpPostedFileBase imagen)
        {
            var nombre = Request.Form["partido"];
            string id = "0";
            using (SVContext db = new SVContext())
            {
                if (imagen != null)
                {
                    var partido = (from p in db.PARTIDO
                                   where p.NOMBRE == nombre
                                   select p).FirstOrDefault();
                    id = partido.ID_PARTIDO.ToString();
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/images/"), id + ".jpg");
                    imagen.SaveAs(path);
                    return View("AdministrarPartido");
                }
                else
                {
                    return View("AdministrarPartido");
                }
            }
        }
    }
}