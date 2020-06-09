using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Mockys_Conexion;
using EcoOplacementApi.Models;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EcoOplacementApi.Repository
{
    public class RepositoryGeneral : IRepositoryGeneral
    {
        private ConexionModels _db = new ConexionModels();
        /// <summary>
        /// metodo para agregar un registro en la base
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Add<TEntity>(TEntity entity) where TEntity : class
        {
            using (var _con = new ConexionModels())
            {
                _con.Set<TEntity>().Add(entity);
                await _con.SaveChangesAsync();
            }
        }
        /// <summary>
        /// consulta generica para la base global que nos sirve para obtener un objeto datatable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> data(string query, SqlParameter[] parameters)
        {
            using (var _con = new ConexionModels())
            {
                var dt = new DataTable();
                var conn = _con.Database.Connection;
                var connectionState = conn.State;
                try
                {
                    if (connectionState != ConnectionState.Open) await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // error handling
                    throw;
                }
                finally
                {
                    if (connectionState != ConnectionState.Closed) conn.Close();
                }
                return dt;
            }
        }

        /// <summary>
        /// consulta generica para la base global que nos sirve para obtener un objeto datatable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> dataOutPlacement(string query, SqlParameter[] parameters)
        {
            var _cls = new ConexionMocksys();
            return await _cls.TraeDatosConP(parameters, query, 2);
        }


        /// <summary>
        /// consulta generica para la base global que nos sirve para obtener un objeto datatable sin parametros
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DataTable> dataSinP(string query)
        {
            using (var _con = new ConexionModels())
            {
                var dt = new DataTable();
                var conn = _con.Database.Connection;
                var connectionState = conn.State;
                try
                {
                    if (connectionState != ConnectionState.Open) await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // error handling
                    throw;
                }
                finally
                {
                    if (connectionState != ConnectionState.Closed) conn.Close();
                }
                return dt;
            }
        }
        /// <summary>
        /// eliminar un registro de la base de datos
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Delete<TEntity>(TEntity entity) where TEntity : class
        {
            using (var _con = new ConexionModels())
            {
                _con.Set<TEntity>().Remove(entity);
                await _con.SaveChangesAsync();
            }
        }
        /// <summary>
        /// obtener el registro de una tabla por el id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> Find<TEntity>(object id) where TEntity : class
        {
            using (var _con = new ConexionModels())
            {
                var entidad = await _db.Set<TEntity>().FindAsync(id);
                return entidad;
            }
        }

        /// <summary>
        /// listo todos los registros de una tabla usando la conexion de mocksys
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<DataTable> GetAllMocksysSinParametros(string query, int x)
        {
            var _cls = new ConexionMocksys();
            return await _cls.TraeDatosSinP(query, x);
        }

        /// <summary>
        /// listo todos los registros de una tabla de forma asincrona por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            using (var _con = new ConexionModels())
            {
                var lista = await _con.Set<TEntity>().ToListAsync();
                return lista;
            }
        }
        /// <summary>
        /// traigo el primer registro de una consulta de una forma asincrona por Entity de acuerdo a las condiciones
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TEntity> GetFirst<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            try
            {
                using (var _con = new ConexionModels())
                {
                    var entidad = await _db.Set<TEntity>().FirstOrDefaultAsync(predicate);
                    return entidad;
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }
            
        }
        /// <summary>
        /// obtener el ultimo registro de una tabla
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity GetLast<TEntity>() where TEntity : class
        {
            var entidad = _db.Set<TEntity>().ToList().LastOrDefault();
            return entidad;
        }
        /// <summary>
        /// listo todos los registros de una tabla de forma asincrona de acuerdo al predicado y por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            using (var _con = new ConexionModels())
            {
                var lista = await _con.Set<TEntity>().Where(predicate).ToListAsync();
                return lista;
            }
        }
        /// <summary>
        /// Consulta para ejecutar un procedimiento almacenado que realiza una operacion ya sea de insercion, modificacion o eliminacion de registros en la base
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task Guardar(string query, SqlParameter[] parameters)
        {
            using (var _con = new ConexionModels())
            {
                await _con.Database.ExecuteSqlCommandAsync(query, parameters);
            }
        }

        /// <summary>
        /// actualizar un registro
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Update<TEntity>(TEntity entity, object id) where TEntity : class
        {
            using (var _con = new ConexionModels())
            {
                var entidad = await _con.Set<TEntity>().FindAsync(id);
                if (entidad != null)
                {
                    _con.Entry(entidad).CurrentValues.SetValues(entity);
                    await _con.SaveChangesAsync();
                }
            }
        }
        /// <summary>
        /// ejecutar el procedimiento para traer la ot de mocksys
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> MocksysGuardar(string query, SqlParameter[] parameters)
        {
            var _cls = new ConexionMocksys();
            return await _cls.TraeDatosConP(parameters, query, 1);
        }

        public void Dispose()//metodo para cerrar la conexion a la base de datos
        {
            if (null != _db)
            {
                _db.Dispose();
            }
        }
        /// <summary>
        /// para casos especiales donde no se cierre la conexion de inmediato traer un registro de acuerdo al predicado y de forma asincrona
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TEntity> GetFirstEspecial<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entidad = await _db.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return entidad;
        }
        /// <summary>
        /// para casos especiales donde no se cierre la conexion de inmediato traer un registro por el id y de forma asincrona
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> FindEspecial<TEntity>(object id) where TEntity : class
        {
            var entidad = await _db.Set<TEntity>().FindAsync(id);
            return entidad;
        }
        /// <summary>
        /// obtiene el registro de forma sincrona por el id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ObtenerRegistro<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entidades = _db.Set<TEntity>().Where(predicate).ToList();
            var registro = entidades.LastOrDefault();
            return registro;
            //return null;
        }
    }

}
