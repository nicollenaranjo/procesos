﻿using Microsoft.AspNetCore.Mvc;
using CRUD_CORE.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;




namespace CRUD_CORE.Controllers
{
    public class AccesosController : Controller
    {
        static string cadena = "Data Source=DESKTOP-AT13GP0\\MSSQLSERVER_1; initial Catalog = DBCRUDCORE; Integrated Security = true";

        public static string ConvertirClave(string txt) {

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(txt));

                foreach(byte b in result)
                    Sb.Append(b.ToString("x2"));
            
            }
            return Sb.ToString();
        }
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
            bool rpta;
            bool registrado;
            string? mensaje;
            try
            {
                if (oUsuario.Clave == oUsuario.ConfirmarClave)
                {
                    oUsuario.Clave = ConvertirClave(oUsuario.Clave);
                    
                }
                else
                {
                    ViewData["Mensaje"] = "Las contraseñas no coinciden";
                    return View();
                }

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario_3", cn);
                    cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                    cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                    cmd.Parameters.AddWithValue("idRol", 2);
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
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
            catch (Exception e)
            {
                string error = e.Message;
                rpta = false;
            }
      
            return View();

        }
        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            bool rpta;
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_ValidarUsuario_3", cn);
                    cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("Clave", ConvertirClave(oUsuario.Clave));
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    oUsuario.idRol = (Roles)Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                rpta = false;
                if (oUsuario.idRol != 0)
                {
                    
                    return RedirectToAction("Listar", "Mantenedor");
                }
                else
                {
                    ViewData["Mensaje"] = "Usuario no encontrado";
                    return View();
                }
            }
            catch (Exception e) {
                string error = e.Message;
                rpta = false;
            }


            return View();
        }
    }
}
