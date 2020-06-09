using EcoOplacementApi.Filters.Security;
using EcoOplacementApi.Models;
using EcoOplacementApi.Repository;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace EcoOplacementApi.Controllers
{
    [Authorize]
    //[System.Web.Http.Cors.EnableCorrs(origins: "*", headers: "*", methods: "*")]
    public class InstanfitController : ApiController
    {
        private RepositoryGeneral _ir = new RepositoryGeneral();
        //private IRepositoryGeneral _ir;
        //public InstanfitController(IRepositoryGeneral ir)//inyectamos el objeto
        //{
        //    _ir = ir;
        //}
        /// <summary>
        /// crear al usuario en instafit
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/instanfit/InstanfitAutenticacion")]
        //public IHttpActionResult InstanfitAutenticacion(UserInstafitViewModels user)
        //{
        //    var _fun = new FuncionesViewModels();
        //    var respuesta = _fun.CrearUserInstafit(user);
        //    return Ok(respuesta);
        //}
        /// <summary>
        /// crear al usuario en instafit y redireccionarlo a la pagina de instanfit 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/instanfit/InstanfitAutenticacionLink")]
        public async Task<IHttpActionResult> InstanfitAutenticacionLink(UserInstafitViewModels user)
        {

            try
            {
                var idUser = JwtManager.getIdUserSession();
                var _fun = new FuncionesViewModels();
                string token = _fun.ValidarUserInstafit(user);
                string link = token;
                // Guardamos la url del usuairo en la tabla
                var updateUser = await _ir.GetFirst<Usuario>(z => z.idUsuario == idUser);
                if (updateUser != null)
                {
                    updateUser.instafitUrl = link;
                    await _ir.Update(updateUser, updateUser.idUsuario);
                }
                return Ok(link);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Solicitud de token a instafit
        /// </summary>
        /// <param name="re"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/instanfit/InstanfitRequest")]
        //public IHttpActionResult InstanfitRequest(RequestInstafitViewModels re)
        //{
        //    var _fun = new FuncionesViewModels();
        //    var respuesta = _fun.RequestInstafit(re);
        //    return Ok(respuesta);
        //}
        /// <summary>
        /// Canjeo de token en instafit
        /// </summary>
        /// <param name="re"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/instanfit/InstanfitCoupon")]
        //public IHttpActionResult InstanfitCoupon(CouponInstafitViewModels co)
        //{
        //    var _fun = new FuncionesViewModels();
        //    var respuesta = _fun.SuscripcionInstafit(co);
        //    return Ok(respuesta);
        //}
        /// <summary>
        /// redirecciona al usuario a la pagina de instanfit
        /// </summary>
        /// <param name="token_autenticacion"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("api/instanfit/InstanfitLink")]
        //public IHttpActionResult InstanfitLink(string token_autenticacion)
        //{
        //    var _fun = new FuncionesViewModels();
        //    return Ok(WebConfigurationManager.AppSettings["Link_instafit_base"].ToString() + token_autenticacion);
        //}
    }
}
