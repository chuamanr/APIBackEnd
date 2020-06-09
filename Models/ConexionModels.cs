namespace EcoOplacementApi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ConexionModels : DbContext
    {
        public ConexionModels()
            : base("name=ConexionModels")
        {
        }

        public virtual DbSet<Agenda> Agenda { get; set; }
        public virtual DbSet<AgendaTipo> AgendaTipo { get; set; }
        public virtual DbSet<AplicacionesOfertas> AplicacionesOfertas { get; set; }
        public virtual DbSet<AreaOferta> AreaOferta { get; set; }
        public virtual DbSet<CiudadCodigosDANE> CiudadCodigosDANE { get; set; }
        public virtual DbSet<CiudadOferta> CiudadOferta { get; set; }
        public virtual DbSet<Contacto> Contacto { get; set; }
        public virtual DbSet<Coworking> Coworking { get; set; }
        public virtual DbSet<DiagnosticoCredito> DiagnosticoCredito { get; set; }
        public virtual DbSet<DiccionarioDatos> DiccionarioDatos { get; set; }
        public virtual DbSet<EjercicioAhorro> EjercicioAhorro { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<EstadoOferta> EstadoOferta { get; set; }
        public virtual DbSet<ExperienciaOferta> ExperienciaOferta { get; set; }
        public virtual DbSet<FraCurso> FraCurso { get; set; }
        public virtual DbSet<FraNiveles> FraNiveles { get; set; }
        public virtual DbSet<FraProgreso> FraProgreso { get; set; }
        public virtual DbSet<HojaVidaDatosPersonales> HojaVidaDatosPersonales { get; set; }
        public virtual DbSet<HojaVidaExperienciaLaboral> HojaVidaExperienciaLaboral { get; set; }
        public virtual DbSet<HojaVidaInformacionEducativa> HojaVidaInformacionEducativa { get; set; }
        public virtual DbSet<IttBrandingDigital> IttBrandingDigital { get; set; }
        public virtual DbSet<IttEmpresa> IttEmpresa { get; set; }
        public virtual DbSet<IttEventosNetworking> IttEventosNetworking { get; set; }
        public virtual DbSet<IttIncripcionEvento> IttIncripcionEvento { get; set; }
        public virtual DbSet<LogActividad> LogActividad { get; set; }
        public virtual DbSet<logEnvioCorreoAgendamiento> logEnvioCorreoAgendamiento { get; set; }
        public virtual DbSet<logFoto> logFoto { get; set; }
        public virtual DbSet<LogRecuperacionContrasenaSMS> LogRecuperacionContrasenaSMS { get; set; }
        public virtual DbSet<Mensaje> Mensaje { get; set; }
        public virtual DbSet<MenuEcosistema> MenuEcosistema { get; set; }
        public virtual DbSet<MenuItemEcosistema> MenuItemEcosistema { get; set; }
        public virtual DbSet<OfertasEmpleo> OfertasEmpleo { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<PlantillaCorreo> PlantillaCorreo { get; set; }
        public virtual DbSet<PlantillaHojaDeVida> PlantillaHojaDeVida { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<PreguntaRespuesta> PreguntaRespuesta { get; set; }
        public virtual DbSet<Prueba> Prueba { get; set; }
        public virtual DbSet<PruebaRespuesta> PruebaRespuesta { get; set; }
        public virtual DbSet<RangoSalario> RangoSalario { get; set; }
        public virtual DbSet<SalarioCargo> SalarioCargo { get; set; }
        public virtual DbSet<SalarioCargoNivel> SalarioCargoNivel { get; set; }
        public virtual DbSet<SalarioCiudad> SalarioCiudad { get; set; }
        public virtual DbSet<SalarioEmpresaTamano> SalarioEmpresaTamano { get; set; }
        public virtual DbSet<SalarioSector> SalarioSector { get; set; }
        public virtual DbSet<SaludFinancieraPreguntas> SaludFinancieraPreguntas { get; set; }
        public virtual DbSet<SaludFinancieraRespuestas> SaludFinancieraRespuestas { get; set; }
        public virtual DbSet<SaludFinancieraTipo> SaludFinancieraTipo { get; set; }
        public virtual DbSet<SectorOferta> SectorOferta { get; set; }
        public virtual DbSet<SectorOfertaMP> SectorOfertaMP { get; set; }
        public virtual DbSet<TipoContratoOferta> TipoContratoOferta { get; set; }
        public virtual DbSet<TipoDiccionario> TipoDiccionario { get; set; }
        public virtual DbSet<TipoPregunta> TipoPregunta { get; set; }
        public virtual DbSet<TipoPreguntaInforme> TipoPreguntaInforme { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioProducto> UsuarioProducto { get; set; }
        public virtual DbSet<UsuarioSesion> UsuarioSesion { get; set; }
        public virtual DbSet<Videos> Videos { get; set; }
        public virtual DbSet<ContingenciaPrimaria> ContingenciaPrimaria { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agenda>()
                .Property(e => e.discriminadorAgendamiento)
                .IsUnicode(false);

            modelBuilder.Entity<Agenda>()
                .Property(e => e.contacto)
                .IsUnicode(false);

            modelBuilder.Entity<AgendaTipo>()
                .Property(e => e.glosaAgendaTipo)
                .IsUnicode(false);

            modelBuilder.Entity<AgendaTipo>()
                .Property(e => e.modulo)
                .IsUnicode(false);

            modelBuilder.Entity<AreaOferta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<CiudadCodigosDANE>()
                .Property(e => e.codigoNivel2)
                .IsUnicode(false);

            modelBuilder.Entity<CiudadCodigosDANE>()
                .Property(e => e.codigoNivel3)
                .IsUnicode(false);

            modelBuilder.Entity<CiudadCodigosDANE>()
                .Property(e => e.nombreNivel1)
                .IsUnicode(false);

            modelBuilder.Entity<CiudadCodigosDANE>()
                .Property(e => e.nombreNivel2)
                .IsUnicode(false);

            modelBuilder.Entity<CiudadCodigosDANE>()
                .Property(e => e.nombreNivel3)
                .IsUnicode(false);

            modelBuilder.Entity<CiudadCodigosDANE>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<CiudadCodigosDANE>()
                .HasMany(e => e.IttEventosNetworking)
                .WithOptional(e => e.CiudadCodigosDANE)
                .HasForeignKey(e => e.idCiudad);

            modelBuilder.Entity<CiudadOferta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Contacto>()
                .Property(e => e.tipoContacto)
                .IsUnicode(false);

            modelBuilder.Entity<Contacto>()
                .Property(e => e.contacto1)
                .IsUnicode(false);

            modelBuilder.Entity<Coworking>()
                .Property(e => e.horario)
                .IsUnicode(false);

            modelBuilder.Entity<DiagnosticoCredito>()
                .Property(e => e.RespustaDiagnostico)
                .IsUnicode(false);

            modelBuilder.Entity<DiagnosticoCredito>()
                .Property(e => e.LogConsumo)
                .IsUnicode(false);

            modelBuilder.Entity<DiccionarioDatos>()
                .Property(e => e.nombreDiccionario)
                .IsUnicode(false);

            modelBuilder.Entity<EjercicioAhorro>()
                .Property(e => e.articulo)
                .IsUnicode(false);

            modelBuilder.Entity<EjercicioAhorro>()
                .Property(e => e.plazo)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.evento)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.parametros)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.errorMensaje)
                .IsUnicode(false);

            modelBuilder.Entity<EstadoOferta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<ExperienciaOferta>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<FraCurso>()
                .Property(e => e.descripcionCurso)
                .IsUnicode(false);

            modelBuilder.Entity<FraCurso>()
                .Property(e => e.imagenCurso)
                .IsUnicode(false);

            modelBuilder.Entity<FraCurso>()
                .Property(e => e.descripcionCorta)
                .IsUnicode(false);

            modelBuilder.Entity<FraCurso>()
                .HasMany(e => e.FraProgreso)
                .WithRequired(e => e.FraCurso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FraNiveles>()
                .Property(e => e.contenidoTexto)
                .IsUnicode(false);

            modelBuilder.Entity<FraNiveles>()
                .Property(e => e.urlVideo)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.nombres)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.apellidoPaterno)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.apellidoMaterno)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.rutaFoto)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.correoElectronico)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.telefonoCelular)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.direccionResidencia)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.anosExperiencia)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.paisesTrabajar)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.redesSociales)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.descripcionPerfilProfesional)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.idImagen)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.genero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.movilidadLaboral)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.idiomaNivel)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.habilidades)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.tipoTextoHV)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDatosPersonales>()
                .Property(e => e.colorHV)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaExperienciaLaboral>()
                .Property(e => e.empresa)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaExperienciaLaboral>()
                .Property(e => e.cargo)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaExperienciaLaboral>()
                .Property(e => e.funcionesLogros)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaExperienciaLaboral>()
                .Property(e => e.trabajoActualmente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaInformacionEducativa>()
                .Property(e => e.institucionEducativa)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaInformacionEducativa>()
                .Property(e => e.tituloObtenido)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaInformacionEducativa>()
                .Property(e => e.estadoEstudio)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaInformacionEducativa>()
                .Property(e => e.otroEstudio)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaInformacionEducativa>()
                .Property(e => e.descripcionEducacion)
                .IsUnicode(false);

            modelBuilder.Entity<IttBrandingDigital>()
                .Property(e => e.descripcionServicios)
                .IsUnicode(false);

            modelBuilder.Entity<IttBrandingDigital>()
                .Property(e => e.identidadDigital)
                .IsUnicode(false);

            modelBuilder.Entity<IttEmpresa>()
                .Property(e => e.nombreSector)
                .IsUnicode(false);

            modelBuilder.Entity<IttEmpresa>()
                .Property(e => e.nombreEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<IttEmpresa>()
                .Property(e => e.descripcionEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<IttEmpresa>()
                .Property(e => e.mailEmpresarial)
                .IsUnicode(false);

            modelBuilder.Entity<IttEmpresa>()
                .Property(e => e.telefonoEmpresarial)
                .IsUnicode(false);

            modelBuilder.Entity<IttEmpresa>()
                .Property(e => e.paginaWeb)
                .IsUnicode(false);

            modelBuilder.Entity<IttEventosNetworking>()
                .Property(e => e.descripcionEvento)
                .IsUnicode(false);

            modelBuilder.Entity<IttEventosNetworking>()
                .Property(e => e.nombreConferencista)
                .IsUnicode(false);

            modelBuilder.Entity<IttEventosNetworking>()
                .Property(e => e.descripcionConferencista)
                .IsUnicode(false);

            modelBuilder.Entity<IttEventosNetworking>()
                .Property(e => e.lugarEvento)
                .IsUnicode(false);

            modelBuilder.Entity<LogActividad>()
                .Property(e => e.ipAcceso)
                .IsUnicode(false);

            modelBuilder.Entity<LogActividad>()
                .Property(e => e.consultaJSON)
                .IsUnicode(false);

            modelBuilder.Entity<logEnvioCorreoAgendamiento>()
                .Property(e => e.plantilla)
                .IsUnicode(false);

            modelBuilder.Entity<logEnvioCorreoAgendamiento>()
                .Property(e => e.fecha)
                .IsUnicode(false);

            modelBuilder.Entity<logEnvioCorreoAgendamiento>()
                .Property(e => e.hora)
                .IsUnicode(false);

            modelBuilder.Entity<logEnvioCorreoAgendamiento>()
                .Property(e => e.contacto)
                .IsUnicode(false);

            modelBuilder.Entity<logFoto>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<LogRecuperacionContrasenaSMS>()
                .Property(e => e.identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<LogRecuperacionContrasenaSMS>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<LogRecuperacionContrasenaSMS>()
                .Property(e => e.mensajeSMS)
                .IsUnicode(false);

            modelBuilder.Entity<LogRecuperacionContrasenaSMS>()
                .Property(e => e.response)
                .IsUnicode(false);

            modelBuilder.Entity<Mensaje>()
                .Property(e => e.textoMensaje)
                .IsUnicode(false);

            modelBuilder.Entity<MenuEcosistema>()
                .Property(e => e.textoMenu)
                .IsUnicode(false);

            modelBuilder.Entity<MenuEcosistema>()
                .Property(e => e.imagenRutaMenu)
                .IsUnicode(false);

            modelBuilder.Entity<MenuItemEcosistema>()
                .Property(e => e.textoMenuItem)
                .IsUnicode(false);

            modelBuilder.Entity<MenuItemEcosistema>()
                .Property(e => e.urlMenuItem)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.tituloOferta)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.descripcionOferta)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.salarioOferta)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.ciudadOferta)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.idOfertaProveedor)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.proveedorEmpleo)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.sector)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.tipoContrato)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.razonSocial)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.cantidadVacantes)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.requisitos)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.experienciaRequerida)
                .IsUnicode(false);

            modelBuilder.Entity<OfertasEmpleo>()
                .Property(e => e.area)
                .IsUnicode(false);

            modelBuilder.Entity<Pais>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.tipoParametro)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.parametroJSON)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.nombrePlantilla)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.htmlPlantilla)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.tagEstudiosPrincipales)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.tagEstudiosComplementarios)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.tagExperienciaLaboral)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.tagIdioma)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.tagRedes)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.tagMovilidadLaboral)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .Property(e => e.tagFuncionesLogros)
                .IsUnicode(false);

            modelBuilder.Entity<PlantillaHojaDeVida>()
                .HasMany(e => e.HojaVidaDatosPersonales)
                .WithOptional(e => e.PlantillaHojaDeVida)
                .HasForeignKey(e => e.idPlantillaAplicada);

            modelBuilder.Entity<Pregunta>()
                .Property(e => e.glosaPregunta)
                .IsUnicode(false);

            modelBuilder.Entity<Pregunta>()
                .Property(e => e.respuestaLibre)
                .IsUnicode(false);

            modelBuilder.Entity<PreguntaRespuesta>()
                .Property(e => e.glosaPreguntaRespuesta)
                .IsUnicode(false);

            modelBuilder.Entity<PruebaRespuesta>()
                .Property(e => e.respuestaLibre)
                .IsUnicode(false);

            modelBuilder.Entity<RangoSalario>()
                .Property(e => e.rango)
                .IsUnicode(false);

            modelBuilder.Entity<SalarioCargo>()
                .Property(e => e.idCargo)
                .IsUnicode(false);

            modelBuilder.Entity<SalarioCargo>()
                .Property(e => e.glosaCargo)
                .IsUnicode(false);

            modelBuilder.Entity<SalarioCargoNivel>()
                .Property(e => e.glosaCargoNivel)
                .IsUnicode(false);

            modelBuilder.Entity<SalarioCiudad>()
                .Property(e => e.glosaCiudad)
                .IsUnicode(false);

            modelBuilder.Entity<SalarioEmpresaTamano>()
                .Property(e => e.glosaEmpresaTamano)
                .IsUnicode(false);

            modelBuilder.Entity<SalarioSector>()
                .Property(e => e.glosaSector)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraPreguntas>()
                .Property(e => e.glosaPregunta)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraPreguntas>()
                .Property(e => e.campoRespuesta)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.estratoSocioeconomico)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.genero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo1)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo2)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo3)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo4)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo5)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo6)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo7)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo8)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo9)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo10)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo11)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo12)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo13)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo14)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo15)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo16)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo17)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo18)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo19)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo20)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo21)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo22)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo23)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo24)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo25)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo26)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo27)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo28)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo29)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo30)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo31)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo32)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo33)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo34)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo35)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo36)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo37)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo38)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo39)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo40)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo41)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo42)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo43)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo44)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo45)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo46)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo47)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo48)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo49)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo50)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo51)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo52)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo53)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo54)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo55)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo56)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo57)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo58)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo59)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo60)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo61)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo62)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo63)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo64)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo65)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo66)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo67)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo68)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo69)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo70)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo71)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo72)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo73)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo74)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo75)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo76)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo77)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo78)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo79)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo80)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo81)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo82)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo83)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo84)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo85)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo86)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo87)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo88)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo89)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo90)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo91)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo92)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo93)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo94)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraRespuestas>()
                .Property(e => e.campo95)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraTipo>()
                .Property(e => e.nombreSaludFinancieraTipo)
                .IsUnicode(false);

            modelBuilder.Entity<SaludFinancieraTipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<SectorOferta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<SectorOfertaMP>()
                .Property(e => e.Sector)
                .IsUnicode(false);

            modelBuilder.Entity<SectorOfertaMP>()
                .Property(e => e.SecTraducion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoContratoOferta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDiccionario>()
                .Property(e => e.nombreTipoDiccionario)
                .IsFixedLength();

            modelBuilder.Entity<TipoDiccionario>()
                .HasMany(e => e.DiccionarioDatos)
                .WithOptional(e => e.TipoDiccionario1)
                .HasForeignKey(e => e.tipoDiccionario);

            modelBuilder.Entity<TipoPregunta>()
                .Property(e => e.nombreTipoPregunta)
                .IsUnicode(false);

            modelBuilder.Entity<TipoPregunta>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoPregunta>()
                .Property(e => e.tipoEvaluacion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoPreguntaInforme>()
                .Property(e => e.textoInforme)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.nombres)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.apellidoPaterno)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.apellidoMaterno)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.correoElectronico)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.numeroCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.clave)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.responseJSON)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.responseDataExperian)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.idCoursera)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.dataCoursera)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.telefonoNotificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.correoNotificacion)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioProducto>()
                .Property(e => e.IdentificacionUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioProducto>()
                .Property(e => e.IdProducto)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioSesion>()
                .Property(e => e.token)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioSesion>()
                .Property(e => e.ip)
                .IsUnicode(false);

            modelBuilder.Entity<Videos>()
                .Property(e => e.titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Videos>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Videos>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<Videos>()
                .Property(e => e.nivel)
                .IsUnicode(false);

            modelBuilder.Entity<Videos>()
                .Property(e => e.tiempo)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.tipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.numeroIdentificacion)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.numeroTelefono)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.numeroTelefono2)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.numeroCelular)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.correoElectronico)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.nombreUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.producto)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.cobertura)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.cobertura2)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.cobertura3)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.fechaSolicitud)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.codigoSubSocio)
                .IsUnicode(false);

            modelBuilder.Entity<ContingenciaPrimaria>()
                .Property(e => e.nombreSubsocio)
                .IsUnicode(false);
        }
    }
}
