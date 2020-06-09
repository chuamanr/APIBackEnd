using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace EcoOplacementApi.Mockys_Conexion
{
    public class ConexionMocksys
    {
        private SqlConnection con;
        private SqlCommand cmd = new SqlCommand();
        private string CadenaConexion(int IdBase)
        {
            string stringConexion;
            stringConexion = "";
            switch (IdBase)
            {
                case 1: // "mocksy"
                    {
                        stringConexion = WebConfigurationManager.ConnectionStrings["Plataforma"].ConnectionString.ToString();
                        break;
                    }
                case 2: // "EcoOutPlacement"
                    {
                        stringConexion = WebConfigurationManager.ConnectionStrings["ConexionOutPlacement"].ConnectionString.ToString();
                        break;
                    }
                    //case 3: // "VENTAS_CALL"
                    //    {
                    //        stringConexion = "Data Source=10.15.23.15;Initial Catalog=VENTAS_CALL;Persist Security Info=True;User ID=USU_CALLPE;Password=123";
                    //        break;
                    //    }

            }
            return stringConexion;
        }
        //conectarse a la base
        private bool conectar(int x)
        {
            try
            {
                con = new SqlConnection(CadenaConexion(x));
                con.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //desconectarse de la base
        private void desconectar()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// ejecutar una consulta para escribir en la base
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public async Task Ejecutar(string query, SqlParameter[] parameters, int x)
        {
            try
            {
                if (conectar(x) == true)
                {
                    cmd.Parameters.Clear();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = query;
                    cmd.Parameters.AddRange(parameters);
                    await cmd.ExecuteNonQueryAsync();
                    desconectar();
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }

        }
        /// <summary>
        /// trae los datos en forma de datatable de forma asincrona sin pasarle parametros
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public async Task<DataTable> TraeDatosSinP(string sql, int x)
        {
            DataTable Tabla = new DataTable();

            if (conectar(x) == true)
            {
                cmd.Parameters.Clear();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Tabla.Load(reader);
                }
                desconectar();
            }
            return Tabla;
        }
        /// <summary>
        /// trae los datos en forma de datatable de forma asincrona pasandole parametros
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sql"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public async Task<DataTable> TraeDatosConP(SqlParameter[] parameters, string sql, int x)
        {
            DataTable Tabla = new DataTable();
            string mensaje = "";
            try
            {
                if (conectar(x) == true)
                {
                    cmd.Parameters.Clear();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 1000;
                    cmd.Parameters.AddRange(parameters);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Tabla.Load(reader);
                    }
                    desconectar();
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Tabla;
        }
    }
}