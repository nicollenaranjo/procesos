using Microsoft.AspNetCore.Mvc;

using CRUD_CORE.Datos;
using CRUD_CORE.Models;
using Rotativa.AspNetCore;

namespace CRUD_CORE.Controllers
{
    public class MantenedorController : Controller
    {
        VentaDatos _VentaDatos = new VentaDatos();
        public IActionResult Listar()
        {
            // La vista mostrara una lista de contactos
            var oLista = _VentaDatos.Listar();
            return View(oLista);
        }
        public IActionResult ListarDia() {
            var oLista = _VentaDatos.ListarDia();
            return View(oLista);
        }

        public IActionResult Guardar()
        {
            // Metodo solo devuelve la vista

            return View();
        }
        [HttpPost]
        public IActionResult Guardar(VentaModel oVenta)
        {
            // Metodo recibe el objeto para guardarlo en BD
            if (!ModelState.IsValid)
                return View();

            var respuesta = _VentaDatos.Guardar(oVenta);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Eliminar(int idVenta)
        {
            var oventa= _VentaDatos.Obtener(idVenta);
            return View(oventa);
        }


            [HttpPost]
        public IActionResult Eliminar(VentaModel oVenta)
        {
            var rpt = _VentaDatos.Eliminar(oVenta.idVenta);
            if (rpt)
                return RedirectToAction("Listar");
            else
                return View();
        
        }
        public IActionResult Editar(int idVenta)
        {
            var oventa = _VentaDatos.Obtener(idVenta);
            return View(oventa);
        }


        [HttpPost]
        public IActionResult Editar(VentaModel oVenta)
        {
            var rpt = _VentaDatos.Editar(oVenta);
            if (rpt)
                return RedirectToAction("Listar");
            else
                return View();

        }
    }
}
