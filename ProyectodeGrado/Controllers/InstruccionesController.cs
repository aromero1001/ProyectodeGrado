using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectodeGrado.Models.SVModel;


namespace ProyectodeGrado.Controllers
{
    public class InstruccionesController : Controller
    {
        // GET: Instrucciones
        public ActionResult Instruccion()
        {
            if (CuentaController.variablesGlobales.id_rol == 3 || CuentaController.variablesGlobales.id_rol == 4)
            {
                ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            }
            return View();
        }

        public ActionResult verItems()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }
    }
}