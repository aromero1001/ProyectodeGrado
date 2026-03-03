using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectodeGrado.Controllers
{
    public class ResultadosController : Controller
    {
        // GET: Resultados
        public ActionResult ResultadoPartidos()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        public ActionResult ResultadosPartidosAdmin()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        public ActionResult ResultadoCandidatos()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        public ActionResult ResultadoCandidatosAdmin()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        public ActionResult ResultadoPropuesta()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }

        public ActionResult ResultadoPropuestaAdmin()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }
    }
}