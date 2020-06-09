using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EcoOplacementApi.Repository
{
    public interface IRepositoryGeneral
    {
        /// <summary>
        /// listo todos los registros de una tabla de forma asincrona por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<List<TEntity>> GetAll<TEntity>() where TEntity : class;
        /// <summary>
        /// traigo el primer registro de una consulta de una forma asincrona por Entity de acuerdo a las condiciones
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetFirst<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        /// <summary>
        /// para casos especiales donde no se cierre la conexion de inmediato traer un registro de acuerdo al predicado y de forma asincrona
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstEspecial<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        /// <summary>
        /// listo todos los registros de una tabla de forma asincrona de acuerdo al predicado y por Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetList<TEntity>(Expression<System.Func<TEntity, bool>> predicate) where TEntity : class;
        /// <summary>
        /// metodo para agregar un registro en la base
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task Add<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// eliminar un registro de la base de datos
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task Delete<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// actualizar un registro
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Update<TEntity>(TEntity entity, object id) where TEntity : class;
        /// <summary>
        /// obtener el registro de una tabla por el id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Find<TEntity>(object id) where TEntity : class;
        /// <summary>
        /// para casos especiales donde no se cierre la conexion de inmediato traer un registro por el id y de forma asincrona
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindEspecial<TEntity>(object id) where TEntity : class;
        /// <summary>
        /// obtener el ultimo registro de una tabla
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        TEntity GetLast<TEntity>() where TEntity : class;

        /// <summary>
        /// consulta generica para la base global que nos sirve para obtener un objeto datatable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DataTable> data(string query, SqlParameter[] parameters);

        /// <summary>
        /// consulta generica para la base global que nos sirve para obtener un objeto datatable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DataTable> dataOutPlacement(string query, SqlParameter[] parameters);

        /// <summary>
        /// consulta generica para la base global que nos sirve para obtener un objeto datatable sin parametros
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<DataTable> dataSinP(string query);

        /// <summary>
        /// Consulta para ejecutar un procedimiento almacenado que realiza una operacion ya sea de insercion, modificacion o eliminacion de registros en la base
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task Guardar(string query, SqlParameter[] parameters);
        /// <summary>
        /// ejecutar el procedimiento para traer la ot de mocksys
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DataTable> MocksysGuardar(string query, SqlParameter[] parameters);

        Task<DataTable> GetAllMocksysSinParametros(string query, int x);
        /// <summary>
        /// obtiene el registro de forma sincrona por el id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        TEntity ObtenerRegistro<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}
