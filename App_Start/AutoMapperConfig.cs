using AutoMapper;
using EcoOplacementApi.Models;
using EcoOplacementApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg => {
                //cfg.CreateMap<Users, UserViewModels>();


                cfg.CreateMap<DatosEducacionViewModels, HojaVidaInformacionEducativa>();
                //.ForMember(x => x.dicIntensidadEstudio, opt => opt.Ignore())
                //.ForMember(x => x.fechaActualizacion, opt => opt.Ignore()).ForMember(x => x.fechaCreacion, opt => opt.Ignore())
                //.ForMember(x => x.idInformacionEducativa, opt => opt.Ignore()).ForMember(x => x.otroEstudio, opt => opt.Ignore()).ForMember(x => x.tituloObtenido, opt => opt.Ignore())
                //.ForMember(x => x.vigente, opt => opt.Ignore());
                cfg.CreateMap<DatosEducacionComplementariosViewModels, HojaVidaInformacionEducativa>();
                cfg.CreateMap<DatosLaboralesViewModels, HojaVidaExperienciaLaboral>();
                cfg.CreateMap<HojaVidaDatosPersonales, entHojaVida> ();
                cfg.CreateMap<Usuario, UsuarioViewModels>();

                cfg.CreateMap<AgendamientoViewModels, Agenda>();
                cfg.CreateMap<Agenda, AgendamientoViewModels>();

                cfg.CreateMap<TipoPregunta, TipoPreguntaViewModels>();
                cfg.CreateMap<TipoPreguntaViewModels, TipoPregunta>();

                cfg.CreateMap<Prueba, PruebaViewModels>();
                cfg.CreateMap<PruebaViewModels, Prueba>();

                cfg.CreateMap<Pregunta, PreguntasViewModels>();
                cfg.CreateMap<PreguntasViewModels, Pregunta>();

                cfg.CreateMap<PreguntaRespuesta, PreguntaRespuestaViewModels>();
                cfg.CreateMap<PreguntaRespuestaViewModels, PreguntaRespuesta>();

                cfg.CreateMap<OfertasEmpleo, OfertaViewModels>();
                cfg.CreateMap<OfertaViewModels, OfertasEmpleo>();

                cfg.CreateMap<AgendamientosDataViewModels, Agenda>();
                cfg.CreateMap<Agenda, AgendamientosDataViewModels>();

                cfg.CreateMap<PruebaRespuesta, PruebaPreguntaViewModels>();
                cfg.CreateMap<PruebaPreguntaViewModels, PruebaRespuesta>();

                cfg.CreateMap<IttBrandingDigital, IttBrandingDigitalViewModels>();
                cfg.CreateMap<IttBrandingDigitalViewModels ,IttBrandingDigital>();

                cfg.CreateMap<IttEmpresa, IttEmpresaViewModels>();
                cfg.CreateMap<IttEmpresaViewModels, IttEmpresa>();

                cfg.CreateMap<IttEventosNetworking, IttEventosNetworkingViewModels>();
                cfg.CreateMap<IttEventosNetworkingViewModels, IttEventosNetworking>();

                cfg.CreateMap<IttIncripcionEvento, IttIncripcionEventoViewModels>();
                cfg.CreateMap<IttIncripcionEventoViewModels, IttIncripcionEvento>();

                cfg.CreateMap<Coworking, CoworkingViewModels>();
                cfg.CreateMap<CoworkingViewModels, Coworking>();

                cfg.CreateMap<AgendaTipo, AgendaTipoViewModels>();
                cfg.CreateMap<AgendaTipoViewModels, AgendaTipo>();

                cfg.CreateMap<AhorroDeudaViewModels, EjercicioAhorro>();
                cfg.CreateMap<AhorroMensualAnual, EjercicioAhorro>();
                cfg.CreateMap<AhorroanualViewModels, EjercicioAhorro>();
                cfg.CreateMap<InversionViewModels, EjercicioAhorro>();
            });

        }
    }
}