using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace apiRESTCheckUsuario.Models
{
    public class clsUsuario
    {
        // Definición de atributos
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string ruta { get; set; }
        public int tipo { get; set; }

        // Cadena de conexión
        private readonly string cadConn = ConfigurationManager.ConnectionStrings["bd_control_acceso"].ConnectionString;

        // Constructor vacío
        public clsUsuario() { }

        // Constructor para validación de acceso
        public clsUsuario(string usuario, string contrasena)
        {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }

        // Constructor para insertar usuario
        public clsUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string usuario, string contrasena, string ruta, int tipo)
        {
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.ruta = ruta;
            this.tipo = tipo;
        }

        // Método para insertar un usuario
        public DataSet spInsUsuario()
        {
            string cadSql = "CALL spInsUsuario(@nombre, @apellidoPaterno, @apellidoMaterno, @usuario, @contrasena, @ruta, @tipo)";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlCommand cmd = new MySqlCommand(cadSql, cnn);
                cmd.Parameters.AddWithValue("@nombre", this.nombre);
                cmd.Parameters.AddWithValue("@apellidoPaterno", this.apellidoPaterno);
                cmd.Parameters.AddWithValue("@apellidoMaterno", this.apellidoMaterno);
                cmd.Parameters.AddWithValue("@usuario", this.usuario);
                cmd.Parameters.AddWithValue("@contrasena", this.contrasena);
                cmd.Parameters.AddWithValue("@ruta", this.ruta);
                cmd.Parameters.AddWithValue("@tipo", this.tipo);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "spInsUsuario");
                return ds;
            }
        }

        // Método para validar acceso
        public DataSet spValidarAcceso()
        {
            string cadSQL = "CALL spValidarAcceso(@usuario, @contrasena);";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlCommand cmd = new MySqlCommand(cadSQL, cnn);
                cmd.Parameters.AddWithValue("@usuario", this.usuario);
                cmd.Parameters.AddWithValue("@contrasena", this.contrasena);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "spValidarAcceso");
                return ds;
            }
        }

        // Método para obtener la lista de usuarios con filtro
        public DataSet vwRptUsuario(string filtro)
        {
            string cadSQL = "SELECT * FROM vwRptUsuario WHERE nombre LIKE CONCAT('%', @filtro, '%') OR usuario LIKE CONCAT('%', @filtro, '%')";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlCommand cmd = new MySqlCommand(cadSQL, cnn);
                cmd.Parameters.AddWithValue("@filtro", filtro);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "vwRptUsuario");
                return ds;
            }
        }

        // Método para obtener solo los tipos de usuario
        public DataSet vmwTipoUsuario()
        {
            string cadSQL = "SELECT Clave, Rol FROM vwRptUsuario";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlCommand cmd = new MySqlCommand(cadSQL, cnn);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "vwTipoUsuario");
                return ds;
            }
        }
    }
}
