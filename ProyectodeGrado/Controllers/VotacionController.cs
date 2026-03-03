using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectodeGrado.Models.SVModel;
using static ProyectodeGrado.Controllers.CuentaController;


namespace ProyectodeGrado.Controllers
{
    public class VotacionController : Controller
    {
        //Creacion de instancia de la base de datos

        private SVContext db = new SVContext();

        public ActionResult Seleccion()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            //Consultar los datos de la configuracion de la eleccion
            var eleccion = from t in db.ELECCION
                           select t;
            //Seleccionar el primer registro de la configuracion de la eleccion 
            var TipoEleccion = eleccion.First();
            //Consultar el la durecion del voto y guardarlo en una variable temporal para usarlo en cada tipo de eleccion 
            TempData["ducacion"] = TipoEleccion.DURACION_VOTO.ToString();

            //Si existe un id usuario en tabla usuario que sea igual al del usuario logeado no te permite votar 
            if (db.AUDITORIA.Any(u => u.ID_USUARIO == variablesGlobales.id_usuario))
            {
                //Retorna un mensaje que ya ejercistes tu voto 
                return View("VotoEjercido");
            }
            else
            {
                //Guardame los registro de la tabala eleccion
                var tipoeleccion = from t in db.ELECCION select t;
                //Guardame el primer regisro de la tabal eleccion
                var primTipoEleccion = tipoeleccion.First();

                TempData["ducacion"]=(primTipoEleccion.DURACION_VOTO * 60).ToString();
               
                if (primTipoEleccion.ID_TIPO_ELECCION == 1)
                {
                    return RedirectToAction("Candidato");
                }
                if (primTipoEleccion.ID_TIPO_ELECCION == 2)
                {
                    return RedirectToAction("Partido");
                }
                if (primTipoEleccion.ID_TIPO_ELECCION == 3)
                {
                   return RedirectToAction("Propuesta");
                }
            }

            return View("error");
        }


        



        //CANDIDATO---------------------------------------------------------------------------------------------
        public ActionResult Candidato()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            ViewBag.segundos = TempData["ducacion"];
            //ViewBag.segundos = TempData["ducacion"];
            var cANDIDATO = db.CANDIDATO.Include(c => c.PARTIDO).Include(c => c.USUARIO);
            return View(cANDIDATO.ToList());
        }
    


        public ActionResult VotarCandidato(int? id)
        {

            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CANDIDATO cANDIDATO = db.CANDIDATO.Find(id);

            if (cANDIDATO == null)
            {
                return HttpNotFound();
            }
           
            return View(cANDIDATO);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VotarCandidato(int id)
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            SVContext db = new SVContext();
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            var usuario = from u in db.USUARIO
                          where u.ID_USUARIO == variablesGlobales.id_usuario
                          select u;

            string genero = usuario.FirstOrDefault().GENERO;
            string nivelEstudio = usuario.FirstOrDefault().NIVEL_ESTUDIO;
            string facultad = usuario.FirstOrDefault().FACULTAD_SECCION;
            var fechaactual = DateTime.Today;
            string fechanacimiento = usuario.FirstOrDefault().FECHA_NACIMIENTO.Year.ToString();
            int edad = Convert.ToInt32(fechaactual.Year) - Convert.ToInt32(fechanacimiento);
            var anioactual = DateTime.Today;

            //AQUÍ PONES LA CONDICIÓN SI FUE VÁLIDO O NO. POR AHORA PONEMOS TRUE PARA PROBAR
            bool valido = true;
            var eleccion = from t in db.ELECCION
                           select t;

            var TipoEleccion = eleccion.First();
            var idtipo = TipoEleccion.ID_TIPO_ELECCION;
            if (idtipo == 1)
            { 

                VOTOS voto = new VOTOS
                {
                    ESTADO = valido,
                    ID_ELECCION =Convert.ToInt32(idtipo),
                    ID_SELECCIONADO = id,
                    ID_PROPUESTA = 0,
                    FECHA_HORA = DateTime.Now,
                    GENERO = genero,
                    FACULTAD = facultad,
                    NIVEL_ESTUDIO = nivelEstudio,
                    EDAD = edad
                };

                db.VOTOS.Add(voto);
                db.SaveChanges();
            }


            //Acuérdate que hay que guardar también el voto en la tabla de auditoría
            //En el LinQ de abajo tomo el ID del último voto que se hizo. 
            var usvoto = (from v in db.VOTOS
                          select v).OrderByDescending(o => o.ID_VOTO);
            int idvoto = usvoto.FirstOrDefault().ID_VOTO;
            AUDITORIA auditoria = new AUDITORIA
            {
                ID_USUARIO = variablesGlobales.id_usuario,
                ESTATUS_VOTO = valido,
                ID_VOTO = idvoto,
                FECHA_HORA = DateTime.Now
            };
            db.AUDITORIA.Add(auditoria);
            db.SaveChanges();
            return RedirectToAction("FinalizarVoto");

        }

        //PARTIDO-------------------------------------------------------------------------------------------------------------

        public ActionResult Partido()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            ViewBag.segundos = TempData["ducacion"];
            var pARTIDO = db.PARTIDO.Include(p => p.TIPO_ELECCION);
            return View(pARTIDO.ToList());
        }

        //Aqui busca el partido seleccionado
        public ActionResult VotarPartido(int? id)
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PARTIDO pARTIDO = db.PARTIDO.Find(id);
            if (pARTIDO == null)
            {
                return HttpNotFound();
            }
          
            return View(pARTIDO);
        }



        //Aqui guarda el voto de ese partido seleccionado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VotarPartido(int id)
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            SVContext db = new SVContext();

            var usuario = from u in db.USUARIO
                          where u.ID_USUARIO == variablesGlobales.id_usuario
                          select u;

            string genero = usuario.FirstOrDefault().GENERO;
            string nivelEstudio = usuario.FirstOrDefault().NIVEL_ESTUDIO;
            string facultad = usuario.FirstOrDefault().FACULTAD_SECCION;
            var fechaactual = DateTime.Today;
            string fechanacimiento = usuario.FirstOrDefault().FECHA_NACIMIENTO.Year.ToString();
            int edad = Convert.ToInt32(fechaactual.Year) - Convert.ToInt32(fechanacimiento);
            var anioactual = DateTime.Today;

            //AQUÍ PONES LA CONDICIÓN SI FUE VÁLIDO O NO. POR AHORA PONEMOS TRUE PARA PROBAR
            bool valido = true;
            var eleccion = from t in db.ELECCION
                           select t;

            var TipoEleccion = eleccion.First();
            var idtipo = TipoEleccion.ID_TIPO_ELECCION;
            int idtip = Convert.ToInt32(idtipo);
            if (idtipo == 2)
            {
              


                VOTOS voto = new VOTOS
                {
                    ESTADO = valido,
                    ID_ELECCION = idtip,
                    ID_SELECCIONADO = id,
                    ID_PROPUESTA = 0,
                    FECHA_HORA = DateTime.Now,
                    GENERO = genero,
                    FACULTAD = facultad,
                    NIVEL_ESTUDIO = nivelEstudio,
                    EDAD = edad
                };

                db.VOTOS.Add(voto);
                db.SaveChanges();
            }


            //Acuérdate que hay que guardar también el voto en la tabla de auditoría
            //En el LinQ de abajo tomo el ID del último voto que se hizo. 
            var usvoto = (from v in db.VOTOS
                          select v).OrderByDescending(o => o.ID_VOTO);
            int idvoto = usvoto.FirstOrDefault().ID_VOTO;
            AUDITORIA auditoria = new AUDITORIA
            {
                ID_USUARIO = variablesGlobales.id_usuario,
                ESTATUS_VOTO = valido,
                ID_VOTO = idvoto,
                FECHA_HORA = DateTime.Now
            };
            db.AUDITORIA.Add(auditoria);
            db.SaveChanges();
            return RedirectToAction("FinalizarVoto");
        }

        
        //PROPUESTA------------------------------------------------------------------------------------------------------------

        public ActionResult Propuesta()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            ViewBag.segundos = TempData["ducacion"];
            var oPCION_PROPUESTA = db.OPCION_PROPUESTA.Include(o => o.PROPUESTA);
            return View(oPCION_PROPUESTA.ToList());
        }
        

        //Aqui muestra las opciones de la votacion por propuesta
        public ActionResult Votar(int? id)
        {  //Guardame los registro de la tabala eleccion
            var tipoeleccion = from t in db.ELECCION select t;
            //Guardame el primer regisro de la tabal eleccion
            var primTipoEleccion = tipoeleccion.First();

            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (primTipoEleccion.ID_TIPO_ELECCION == 3)
            {


                OPCION_PROPUESTA oPCION_PROPUESTA = db.OPCION_PROPUESTA.Find(id);

                if (oPCION_PROPUESTA == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ID_PROPUESTA = new SelectList(db.PROPUESTA, "ID_PROPUESTA", "TITULO", oPCION_PROPUESTA.ID_PROPUESTA);
                return View(oPCION_PROPUESTA);

            }
            return View("error");
        }


        //Este action es para guardar el voto de la opcion elegida de la propuesta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Votar(int id)
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            SVContext db = new SVContext();

            var usuario = from u in db.USUARIO
                          where u.ID_USUARIO == variablesGlobales.id_usuario
                          select u;

            string genero = usuario.FirstOrDefault().GENERO;
            string nivelEstudio = usuario.FirstOrDefault().NIVEL_ESTUDIO;
            string facultad = usuario.FirstOrDefault().FACULTAD_SECCION;
            var fechaactual = DateTime.Today;
            string fechanacimiento = usuario.FirstOrDefault().FECHA_NACIMIENTO.Year.ToString();
            int edad = Convert.ToInt32(fechaactual.Year) - Convert.ToInt32(fechanacimiento);
            var anioactual = DateTime.Today;

            //AQUÍ PONES LA CONDICIÓN SI FUE VÁLIDO O NO. POR AHORA PONEMOS TRUE PARA PROBAR
            bool valido = true;
                            var eleccion = from t in db.ELECCION
                                           select t;

                            var TipoEleccion = eleccion.First();
                            int idtipo = Convert.ToInt32(TipoEleccion.ID_TIPO_ELECCION);
            if (idtipo == 3)
            {
                var propuesta = from p in db.PROPUESTA
                                select p;
                int idpropuesta = propuesta.FirstOrDefault().ID_PROPUESTA;


            
            
                VOTOS voto = new VOTOS
                {
                    ESTADO = valido,
                    ID_ELECCION = Convert.ToInt32(idtipo),
                    ID_SELECCIONADO = id,
                    ID_PROPUESTA = idpropuesta,
                    FECHA_HORA = DateTime.Now,
                    GENERO = genero,
                    FACULTAD = facultad,
                    NIVEL_ESTUDIO = nivelEstudio,
                    EDAD = edad

                };

                db.VOTOS.Add(voto);
                db.SaveChanges();
            }
     

            //Acuérdate que hay que guardar también el voto en la tabla de auditoría
            //En el LinQ de abajo tomo el ID del último voto que se hizo. 
            var usvoto = (from v in db.VOTOS
                         select v).OrderByDescending(o => o.ID_VOTO);
            int idvoto = usvoto.FirstOrDefault().ID_VOTO;
            AUDITORIA auditoria = new AUDITORIA
            {
                ID_USUARIO = variablesGlobales.id_usuario,
                ESTATUS_VOTO = valido,
                ID_VOTO = idvoto,
                FECHA_HORA = DateTime.Now
            };
            db.AUDITORIA.Add(auditoria);
            db.SaveChanges();
            return RedirectToAction("FinalizarVoto");     
        }


        //NULO-----------------------------------------------------------------------------------------------------------

        //Si se pasa el tiempo se guarda la votacion pero nula
        public ActionResult VotoNulo()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            //Guardame los registro de la tabala eleccion
            var tipoeleccion = from t in db.ELECCION select t;
            //Guardame el primer regisro de la tabal eleccion
            var primTipoEleccion = tipoeleccion.First();


            if (primTipoEleccion.ID_TIPO_ELECCION == 1)
            {

                SVContext db = new SVContext();

                var usuario = from u in db.USUARIO
                              where u.ID_USUARIO == variablesGlobales.id_usuario
                              select u;

                string genero = usuario.FirstOrDefault().GENERO;
                string nivelEstudio = usuario.FirstOrDefault().NIVEL_ESTUDIO;
                string facultad = usuario.FirstOrDefault().FACULTAD_SECCION;
                var fechaactual = DateTime.Today;
                string fechanacimiento = usuario.FirstOrDefault().FECHA_NACIMIENTO.Year.ToString();
                int edad = Convert.ToInt32(fechaactual.Year) - Convert.ToInt32(fechanacimiento);
                var anioactual = DateTime.Today;


                var eleccion = from t in db.ELECCION
                               select t;

                var TipoEleccion = eleccion.First();
                int idtipo = Convert.ToInt32(TipoEleccion.ID_TIPO_ELECCION);


                VOTOS voto2 = new VOTOS
                {
                    ESTADO = false,
                    ID_ELECCION = Convert.ToInt32(idtipo),
                    ID_SELECCIONADO = 0,
                    ID_PROPUESTA = 0,
                    FECHA_HORA = DateTime.Now,
                    GENERO = genero,
                    FACULTAD = facultad,
                    NIVEL_ESTUDIO = nivelEstudio,
                    EDAD = edad

                };

                db.VOTOS.Add(voto2);
                db.SaveChanges();


                //Acuérdate que hay que guardar también el voto en la tabla de auditoría
                //En el LinQ de abajo tomo el ID del último voto que se hizo. 
                var usvoto = (from v in db.VOTOS
                              select v).OrderByDescending(o => o.ID_VOTO);
                int idvoto = usvoto.FirstOrDefault().ID_VOTO;
                AUDITORIA auditoria = new AUDITORIA
                {
                    ID_USUARIO = variablesGlobales.id_usuario,
                    ESTATUS_VOTO = false,
                    ID_VOTO = idvoto,
                    FECHA_HORA = DateTime.Now
                };
                db.AUDITORIA.Add(auditoria);
                db.SaveChanges();
                return View();


            }
                if (primTipoEleccion.ID_TIPO_ELECCION ==2)
                {
                SVContext db = new SVContext();

                var usuario = from u in db.USUARIO
                              where u.ID_USUARIO == variablesGlobales.id_usuario
                              select u;

                string genero = usuario.FirstOrDefault().GENERO;
                string nivelEstudio = usuario.FirstOrDefault().NIVEL_ESTUDIO;
                string facultad = usuario.FirstOrDefault().FACULTAD_SECCION;
                var fechaactual = DateTime.Today;
                string fechanacimiento = usuario.FirstOrDefault().FECHA_NACIMIENTO.Year.ToString();
                int edad = Convert.ToInt32(fechaactual.Year) - Convert.ToInt32(fechanacimiento);
                var anioactual = DateTime.Today;


                var eleccion = from t in db.ELECCION
                               select t;

                var TipoEleccion = eleccion.First();
                int idtipo = Convert.ToInt32(TipoEleccion.ID_TIPO_ELECCION);
            

                VOTOS voto2 = new VOTOS
                {
                    ESTADO = false,
                    ID_ELECCION = Convert.ToInt32(idtipo),
                    ID_SELECCIONADO = 0,
                    ID_PROPUESTA = 0,
                    FECHA_HORA = anioactual,
                    GENERO = genero,
                    FACULTAD = facultad,
                    NIVEL_ESTUDIO = nivelEstudio,
                    EDAD = edad

                };

                db.VOTOS.Add(voto2);
                db.SaveChanges();


                //Acuérdate que hay que guardar también el voto en la tabla de auditoría
                //En el LinQ de abajo tomo el ID del último voto que se hizo. 
                var usvoto = (from v in db.VOTOS
                              select v).OrderByDescending(o => o.ID_VOTO);
                int idvoto = usvoto.FirstOrDefault().ID_VOTO;
                AUDITORIA auditoria = new AUDITORIA
                {
                    ID_USUARIO = variablesGlobales.id_usuario,
                    ESTATUS_VOTO = false,
                    ID_VOTO = idvoto,
                    FECHA_HORA = fechaactual
                };
                db.AUDITORIA.Add(auditoria);
                db.SaveChanges();
                return View();
            }

               if (primTipoEleccion.ID_TIPO_ELECCION == 3)
            {
                SVContext db = new SVContext();

                var usuario = from u in db.USUARIO
                              where u.ID_USUARIO == variablesGlobales.id_usuario
                              select u;

                string genero = usuario.FirstOrDefault().GENERO;
                string nivelEstudio = usuario.FirstOrDefault().NIVEL_ESTUDIO;
                string facultad = usuario.FirstOrDefault().FACULTAD_SECCION;
                var fechaactual = DateTime.Today;
                string fechanacimiento = usuario.FirstOrDefault().FECHA_NACIMIENTO.Year.ToString();
                int edad = Convert.ToInt32(fechaactual.Year) - Convert.ToInt32(fechanacimiento);
                var anioactual = DateTime.Today;

                var propuesta = from p in db.PROPUESTA
                                select p;

                var eleccion = from t in db.ELECCION
                               select t;

                var TipoEleccion = eleccion.First();
                int idtipo = Convert.ToInt32(TipoEleccion.ID_TIPO_ELECCION);
                int idpropuesta = propuesta.FirstOrDefault().ID_PROPUESTA;

                ITEMS_ELECCION itemEleccion = new ITEMS_ELECCION
                {
                    ID_TIPO_ELECCION = idtipo,
                    ID_ITEM = 0,
                    ID_PROPUESTA = idpropuesta
                    
                };
                db.ITEMS_ELECCION.Add(itemEleccion);
                db.SaveChanges();

                VOTOS voto2 = new VOTOS
                {
                    ESTADO = false,
                    ID_ELECCION = Convert.ToInt32(idtipo),
                    ID_SELECCIONADO = 0,
                    ID_PROPUESTA = idpropuesta,
                    FECHA_HORA = anioactual,
                    GENERO = genero,
                    FACULTAD = facultad,
                    NIVEL_ESTUDIO = nivelEstudio,
                    EDAD = edad

                };

                db.VOTOS.Add(voto2);
                db.SaveChanges();

                //Acuérdate que hay que guardar también el voto en la tabla de auditoría
                //En el LinQ de abajo tomo el ID del último voto que se hizo. 
                var usvoto = (from v in db.VOTOS
                              select v).OrderByDescending(o => o.ID_VOTO);
                int idvoto = usvoto.FirstOrDefault().ID_VOTO;
                AUDITORIA auditoria = new AUDITORIA
                {
                    ID_USUARIO = variablesGlobales.id_usuario,
                    ESTATUS_VOTO = false,
                    ID_VOTO = idvoto,
                    FECHA_HORA = fechaactual
                };
                db.AUDITORIA.Add(auditoria);
                db.SaveChanges();
                return View();
            }
            return View("error");
        }

        //Aqui direcciona a un voto exitoso
        public ActionResult FinalizarVoto()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();

        }

        //Aqui te direcciona a un voto nulo
        public ActionResult Tiempo()
        {
            ViewBag.Nombre = CuentaController.variablesGlobales.nombre;
            return View();
        }
    }
}
  


   
