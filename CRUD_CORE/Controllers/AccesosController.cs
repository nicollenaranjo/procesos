using Microsoft.AspNetCore.Mvc;
using CRUD_CORE.Models;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CRUD_CORE.Controllers
{
    public class AccesosController : Controller
    {
        static string cadena = "Data Source=DESKTOP-AT13GP0\\MSSQLSERVER_1; initial Catalog = DBCRUDCORE; Integrated Security = true";
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registro(Usuario oUsuario)
        {
            bool registrado;
            string? mensaje;

            if (oUsuario.Clave == oUsuario.ConfirmarClave)
            {
                oUsuario.Clave = oUsuario.Clave;
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            ViewData["Mensaje"] = mensaje;
            if (registrado)
            {
                return RedirectToAction("Login", "Accesos");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario_1", cn);
                cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
               oUsuario.idUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }

            if (oUsuario.idUsuario != 0)
            {
                //Session["usuario"] = oUsuario;
                return RedirectToAction("Listar", "Mantenedor");
            }
            else {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }

            

        }
    }
}
