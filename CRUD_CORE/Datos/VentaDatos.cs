using CRUD_CORE.Models;
using System.Data.SqlClient;
using System.Data;

namespace CRUD_CORE.Datos
{
    public class VentaDatos
    {
        public List<VentaModel> Listar()
        {
            var oLista = new List<VentaModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_Listar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new VentaModel()
                        {
                            idVenta = Convert.ToInt32(dr["idVenta"]),
                            Nombre = Convert.ToString(dr["Nombre"]),
                            Precio = Convert.ToInt32(dr["Precio"]),
                            DiaVenta = Convert.ToDateTime(dr["DiaVenta"])
                    });
                    }
                }
            }
            return oLista;
        }

        public VentaModel Obtener(int idVenta)
        {
            var oVenta = new VentaModel();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_Obtener", conexion);
                cmd.Parameters.AddWithValue("idVenta", idVenta);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oVenta.idVenta = Convert.ToInt32(dr["idVenta"]);
                        oVenta.Nombre = Convert.ToString(dr["Nombre"]);
                        oVenta.Precio = Convert.ToInt32(dr["Precio"]);
                        oVenta.DiaVenta = Convert.ToDateTime(dr["DiaVenta"]);
                    }
                }
            }
            return oVenta;
        }

        public bool Guardar(VentaModel oventa)
        {
            bool rpta;
            try
            {

                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Guardar", conexion);
                    cmd.Parameters.AddWithValue("Nombre", oventa.Nombre);
                    cmd.Parameters.AddWithValue("Precio", oventa.Precio);
                    cmd.Parameters.AddWithValue("DiaVenta", oventa.DiaVenta);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                rpta = true;
            }
            catch (Exception e)
            {
                string errro = e.Message;
                rpta = false;
            }
            return rpta;
        }
        public bool Eliminar(int idVenta)
        {
            bool rpta;
            try
            {

                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Eliminar", conexion);
                    cmd.Parameters.AddWithValue("idVenta", idVenta);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                rpta = true;
            }
            catch (Exception e)
            {
                string errro = e.Message;
                rpta = false;
            }
            return rpta;
        }
    }
}
