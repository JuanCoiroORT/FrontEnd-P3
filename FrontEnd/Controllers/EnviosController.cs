using Compartido.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MVC.Models;
using LogicaNegocio.EntidadesNegocio;
using LogicaAccesoDatos.Repositorios;
using LogicaAplicacion.Interfaces.UsuarioInterfaces;
using LogicaNegocio.ExcepcionesEntidades;
using System.Security.Claims;
using LogicaAplicacion.Interfaces.AgenciaInterfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using LogicaAplicacion.AplicacionCasosUso.UsuarioCU;
using LogicaAplicacion.Interfaces.EnvioInterfaces;
using LogicaAplicacion.AplicacionCasosUso.EnvioCU;

namespace MVC.Controllers
{
    public class EnviosController : Controller
    {
        private IGetAllComunes _getAllComunes;
        private IGetAllUrgentes _getAllUrgentes;
        private ICrearUrgente _crearUrgente;
        private ICrearComun _crearComun;
        private IGetUrgenteById _getUrgenteById;
        private IGetComunById _getComunById;
        private IUpdateEnvio _updateEnvio;
        private IGetUserById _getUserById;
        private IGetAgencias _getAgencias;
        private IGetClientes _getClientes;
        private IGetAgenciaById _getAgenciaById;
        private IDeleteUrgente _deleteUrgente;
        private IDeleteComun _deleteComun;
        private IExisteNumTracking _existeNumTracking;

        public EnviosController(IGetAllComunes getAllComunes, IGetAllUrgentes getAllUrgentes,
            ICrearUrgente crearUrgente, ICrearComun crearComun,
            IGetUserById getUserById, IGetAgencias getAgencias, IGetClientes getClientes, IGetAgenciaById getAgenciaById,
            IGetUrgenteById getUrgenteById, IUpdateEnvio updateEnvio, IGetComunById getComunById, IDeleteUrgente deleteUrgente,
            IDeleteComun deleteComun, IExisteNumTracking existeNumTracking)
        {
            _getAllComunes = getAllComunes;
            _getAllUrgentes = getAllUrgentes;
            _crearUrgente = crearUrgente;
            _crearComun = crearComun;
            _getAgenciaById = getAgenciaById;
            _getComunById = getComunById;
            _getUrgenteById = getUrgenteById;
            _updateEnvio = updateEnvio;
            _getUserById = getUserById;
            _getAgencias = getAgencias;
            _getClientes = getClientes;
            _deleteUrgente = deleteUrgente;
            _deleteComun = deleteComun;
            _existeNumTracking = existeNumTracking;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            IEnumerable<ComunDTO> comunesDTO = _getAllComunes.Execute();
            IEnumerable<UrgenteDTO> urgentesDTO = _getAllUrgentes.Execute();
            ViewModelEnviosIndex envios = new ViewModelEnviosIndex(urgentesDTO, comunesDTO);
            return View(envios);
        }

        [HttpGet]
        public IActionResult CreateUrgente()
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            // Traer a los clientes para select de email
            IEnumerable<UsuarioDTO> clientesDTO = _getClientes.Execute();
            var clientesSelect = clientesDTO.Select(c => new SelectListItem
            {
                Text = c.Email.Valor,
                Value = c.Id.ToString()
            }).ToList();

            //Armar viewmodel
            var vm = new ViewModelEnvioUrgente
            {
                urgenteDTO = new UrgenteDTO(),
                Clientes = clientesSelect
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult CreateUrgente(ViewModelEnvioUrgente vm)
        {
            try
            {
                UrgenteDTO urgenteDTO = vm.urgenteDTO;

                // Buscar el empleado que dio de alta el envio, es decir el que esta logueado
                int empleadoId = (int)HttpContext.Session.GetInt32("IdLogueado");
                UsuarioDTO empleadoDTO = _getUserById.Execute(empleadoId);

                // Buscar cliente por id
                int clienteId = urgenteDTO.ClienteId;
                UsuarioDTO clienteDTO = _getUserById.Execute(clienteId);

                // Asignar propiedades
                urgenteDTO.ClienteId = clienteDTO.Id;
                urgenteDTO.EmpleadoId = empleadoDTO.Id;
                urgenteDTO.Estado = "EN_PROCESO";
                //Asignar numero de tracking unico
                double numTracking;
                do
                {
                    Random random = new Random();
                    numTracking = Math.Round(random.NextDouble() * 900 + 100, 0);
                } while (_existeNumTracking.Execute(numTracking));
                urgenteDTO.NumTracking = numTracking;


                // Crear el envio
                _crearUrgente.Execute(urgenteDTO);

                //Rederigir al index
                return RedirectToAction("Index");
            }
            catch(EnvioException ex) 
            {
                //Mensaje con el error
                ViewBag.Message = ex.Message;

                // Volver a cargar el vm por si hay error al enviar formulario
                vm.Clientes = _getClientes.Execute().Select(c => new SelectListItem
                {
                    Text = c.Email.Valor,
                    Value = c.Id.ToString()
                }).ToList();

                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult CreateComun()
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            //Traer agencias para select
            IEnumerable<AgenciaDTO> agenciasDTOs = _getAgencias.Execute();

            var agenciasSelect = agenciasDTOs.Select(a => new SelectListItem
            {
                Text = a.Nombre,       
                Value = a.Id.ToString() 
            }).ToList();

            // Traer a los clientes para select de email
            IEnumerable<UsuarioDTO> clientesDTO = _getClientes.Execute();
            var clientesSelect = clientesDTO.Select(c => new SelectListItem
            {
                Text = c.Email.Valor,
                Value = c.Id.ToString()
            }).ToList();

            //Armar viewmodel
            var vm = new ViewModelEnvioComun
            {
                comunDTO = new ComunDTO(),
                Agencias = agenciasSelect,
                Clientes = clientesSelect
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult CreateComun(ViewModelEnvioComun vm)
        {
            try
            {
                ComunDTO comunDTO = vm.comunDTO;

                // Buscar el empleado que dio de alta el envio, es decir el que esta logueado
                int empleadoId = (int)HttpContext.Session.GetInt32("IdLogueado");
                UsuarioDTO empleadoDTO = _getUserById.Execute(empleadoId);

                // Buscar cliente por id
                int clienteId = comunDTO.ClienteId;
                UsuarioDTO clienteDTO = _getUserById.Execute(clienteId);

                // Buscar agencia por id
                int agenciaId = comunDTO.AgenciaId;
                AgenciaDTO agenciaDTO = _getAgenciaById.Execute(agenciaId);

                // Asignar propiedades
                comunDTO.ClienteId = clienteDTO.Id;
                comunDTO.EmpleadoId = empleadoDTO.Id;
                comunDTO.AgenciaId = agenciaDTO.Id;
                comunDTO.Estado = "EN_PROCESO";
                //Asignar numero de tracking unico
                double numTracking;
                do
                {
                    Random random = new Random();
                    numTracking = Math.Round(random.NextDouble() * 900 + 100, 0);
                } while (_existeNumTracking.Execute(numTracking));
                comunDTO.NumTracking = numTracking;
                
                // Crear el envio
                _crearComun.Execute(comunDTO);

                // Redirigir al index
                return RedirectToAction("Index");
            }
            catch (EnvioException ex)
            {
                //Mensaje con el error
                ViewBag.Message = ex.Message;

                // Volver a cargar la info en caso de error en el envio
                vm.Agencias = _getAgencias.Execute().Select(a => new SelectListItem
                {
                    Text = a.Nombre,
                    Value = a.Id.ToString()
                }).ToList();

                vm.Clientes = _getClientes.Execute().Select(c => new SelectListItem
                {
                    Text = c.Email.Valor,
                    Value = c.Id.ToString()
                }).ToList();

                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult EditComun(int id)
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ComunDTO comunDTO = _getComunById.Execute(id);
            return View(comunDTO);
        }

        [HttpPost]
        public IActionResult EditComun(int id, string comentario)
        {
            try
            {
                //Obtener envio general
                ComunDTO comunDTO = _getComunById.Execute(id);

                //Crear nuevo seguimineto
                Seguimiento nuevoSeguimineto = new Seguimiento
                {
                    Fecha = DateTime.Now,
                    Comentario = comentario,
                    EmpleadoId = comunDTO.EmpleadoId
                };

                //Agregar seguimineto al envio
                comunDTO.Seguimientos.Add(nuevoSeguimineto);

                // Actualizar Comun
                _updateEnvio.Execute(id, comunDTO);

                //Redirigir a index
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Mensaje con el error
                ViewBag.Message = ex.Message;

                //Obtener envio general para cargar la vista nuevamente
                ComunDTO comunDTO = _getComunById.Execute(id);
                return View(comunDTO);
            }
        }

        [HttpGet]
        public IActionResult EditUrgente(int id)
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            UrgenteDTO urgenteDTO = _getUrgenteById.Execute(id);
            return View(urgenteDTO);
        }

        [HttpPost]
        public IActionResult EditUrgente(int id,  string comentario)
        {
            try
            {
                //Obtener envio general
                UrgenteDTO urgenteDTO = _getUrgenteById.Execute(id);

                //Crear nuevo seguimiento
                Seguimiento nuevoSeguimiento = new Seguimiento
                {
                    Fecha = DateTime.Now,
                    Comentario = comentario,
                    EmpleadoId = urgenteDTO.EmpleadoId
                };

                //Agregar seguimineto al envio
                urgenteDTO.Seguimientos.Add(nuevoSeguimiento);

                // Actualizar Comun
                _updateEnvio.Execute(id, urgenteDTO);

                //Redirigir a index
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Mensaje con el error
                ViewBag.Message = ex.Message;

                //Obtener envio general para cargar la vista nuevamente
                UrgenteDTO urgenteDTO = _getUrgenteById.Execute(id);
                return View(urgenteDTO);
            }
        }


        [HttpGet]
        public IActionResult DeleteUrgente(int id)
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            UrgenteDTO urgenteDTO = _getUrgenteById.Execute(id);
            return View(urgenteDTO);
        }

        [HttpPost]
        public IActionResult DeleteUrgente(UrgenteDTO urgenteDTO)
        {
            try
            {
                _deleteUrgente.Execute(urgenteDTO.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Mensaje con el error
                ViewBag.Message = ex.Message;

                //Volver a mostrar envio
                UrgenteDTO nuevoUrgenteDTO = _getUrgenteById.Execute(urgenteDTO.Id);
                return View(nuevoUrgenteDTO);
            }
        }


        [HttpGet]
        public IActionResult DeleteComun(int id)
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ComunDTO comunDTO = _getComunById.Execute(id);
            return View(comunDTO);
        }

        [HttpPost]
        public IActionResult DeleteComun(ComunDTO comunDTO)
        {
            if (HttpContext.Session.GetInt32("IdLogueado") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                _deleteComun.Execute(comunDTO.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Mensaje con el error
                ViewBag.Message = ex.Message;

                //Volver a mostrar envio
                ComunDTO nuevoComunDTO = _getComunById.Execute(comunDTO.Id);
                return View(nuevoComunDTO);
            }
        }
    }
}
