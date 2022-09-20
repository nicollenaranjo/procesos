using CRUD_CORE.Models;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Azure;

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

        public DataTable dtVenta()
        {
            DataTable dt = new DataTable();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_Listar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                adapter.Dispose();
            }
            return dt;
        }
        public List<VentaModel> ListarDia()
        {
            var oLista = new List<VentaModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("VentaDia1", conexion);
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
                string error = e.Message;
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
                string error = e.Message;
                rpta = false;
            }
            return rpta;
        }

        public bool Editar(VentaModel oventa)
        {
            bool rpta;
            try
            {

                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Editar1", conexion);
                    cmd.Parameters.AddWithValue("idVenta", oventa.idVenta);
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
                string error = e.Message;
                rpta = false;
            }
            return rpta;
        }
        public void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Document document = new Document();
            dt = dtVenta();
            if (dt.Rows.Count > 0)
            {
                document.Open();
                Font fontTitle = FontFactory.GetFont(FontFactory.COURIER_BOLD, 25);
                Font font9 = FontFactory.GetFont(FontFactory.TIMES, 9);

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                document.Add(new Paragraph(20, "Reporte del Ventas del Dia", fontTitle));
                document.Add(new Chunk("\n"));
                float[] widths = new float[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                    widths[i] = 4f;

                table.SetWidths(widths);
                table.WidthPercentage = 90;

                PdfPCell cell = new PdfPCell(new Phrase("columns"));
                cell.Colspan = dt.Columns.Count;

                foreach (DataColumn c in dt.Columns)
                {
                    table.AddCell(new Phrase(c.ColumnName, font9));
                }

                foreach (DataRow r in dt.Rows)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int h = 0; h < dt.Columns.Count; h++)
                        {
                            table.AddCell(new Phrase(r[h].ToString(), font9));
                        }
                    }
                }
                document.Add(table);
            }
            document.Close();
        }
    }
}
