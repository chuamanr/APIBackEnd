namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SaludFinancieraRespuestas
    {
        public int id { get; set; }

        public int? idUsuario { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaNacimiento { get; set; }

        public int? ciudadResidencia { get; set; }

        public int? estadoCivil { get; set; }

        [StringLength(4)]
        public string estratoSocioeconomico { get; set; }

        [StringLength(1)]
        public string genero { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public bool? vigente { get; set; }

        [StringLength(5000)]
        public string campo1 { get; set; }

        [StringLength(5000)]
        public string campo2 { get; set; }

        [StringLength(5000)]
        public string campo3 { get; set; }

        [StringLength(5000)]
        public string campo4 { get; set; }

        [StringLength(5000)]
        public string campo5 { get; set; }

        [StringLength(5000)]
        public string campo6 { get; set; }

        [StringLength(5000)]
        public string campo7 { get; set; }

        [StringLength(5000)]
        public string campo8 { get; set; }

        [StringLength(5000)]
        public string campo9 { get; set; }

        [StringLength(5000)]
        public string campo10 { get; set; }

        [StringLength(5000)]
        public string campo11 { get; set; }

        [StringLength(5000)]
        public string campo12 { get; set; }

        [StringLength(5000)]
        public string campo13 { get; set; }

        [StringLength(5000)]
        public string campo14 { get; set; }

        [StringLength(5000)]
        public string campo15 { get; set; }

        [StringLength(5000)]
        public string campo16 { get; set; }

        [StringLength(5000)]
        public string campo17 { get; set; }

        [StringLength(5000)]
        public string campo18 { get; set; }

        [StringLength(5000)]
        public string campo19 { get; set; }

        [StringLength(5000)]
        public string campo20 { get; set; }

        [StringLength(5000)]
        public string campo21 { get; set; }

        [StringLength(5000)]
        public string campo22 { get; set; }

        [StringLength(5000)]
        public string campo23 { get; set; }

        [StringLength(5000)]
        public string campo24 { get; set; }

        [StringLength(5000)]
        public string campo25 { get; set; }

        [StringLength(5000)]
        public string campo26 { get; set; }

        [StringLength(5000)]
        public string campo27 { get; set; }

        [StringLength(5000)]
        public string campo28 { get; set; }

        [StringLength(5000)]
        public string campo29 { get; set; }

        [StringLength(5000)]
        public string campo30 { get; set; }

        [StringLength(5000)]
        public string campo31 { get; set; }

        [StringLength(5000)]
        public string campo32 { get; set; }

        [StringLength(5000)]
        public string campo33 { get; set; }

        [StringLength(5000)]
        public string campo34 { get; set; }

        [StringLength(5000)]
        public string campo35 { get; set; }

        [StringLength(5000)]
        public string campo36 { get; set; }

        [StringLength(5000)]
        public string campo37 { get; set; }

        [StringLength(5000)]
        public string campo38 { get; set; }

        [StringLength(5000)]
        public string campo39 { get; set; }

        [StringLength(5000)]
        public string campo40 { get; set; }

        [StringLength(5000)]
        public string campo41 { get; set; }

        [StringLength(5000)]
        public string campo42 { get; set; }

        [StringLength(5000)]
        public string campo43 { get; set; }

        [StringLength(5000)]
        public string campo44 { get; set; }

        [StringLength(5000)]
        public string campo45 { get; set; }

        [StringLength(5000)]
        public string campo46 { get; set; }

        [StringLength(5000)]
        public string campo47 { get; set; }

        [StringLength(5000)]
        public string campo48 { get; set; }

        [StringLength(5000)]
        public string campo49 { get; set; }

        [StringLength(5000)]
        public string campo50 { get; set; }

        [StringLength(5000)]
        public string campo51 { get; set; }

        [StringLength(5000)]
        public string campo52 { get; set; }

        [StringLength(5000)]
        public string campo53 { get; set; }

        [StringLength(5000)]
        public string campo54 { get; set; }

        [StringLength(5000)]
        public string campo55 { get; set; }

        [StringLength(5000)]
        public string campo56 { get; set; }

        [StringLength(5000)]
        public string campo57 { get; set; }

        [StringLength(5000)]
        public string campo58 { get; set; }

        [StringLength(5000)]
        public string campo59 { get; set; }

        [StringLength(5000)]
        public string campo60 { get; set; }

        [StringLength(5000)]
        public string campo61 { get; set; }

        [StringLength(5000)]
        public string campo62 { get; set; }

        [StringLength(5000)]
        public string campo63 { get; set; }

        [StringLength(5000)]
        public string campo64 { get; set; }

        [StringLength(5000)]
        public string campo65 { get; set; }

        [StringLength(5000)]
        public string campo66 { get; set; }

        [StringLength(5000)]
        public string campo67 { get; set; }

        [StringLength(5000)]
        public string campo68 { get; set; }

        [StringLength(5000)]
        public string campo69 { get; set; }

        [StringLength(5000)]
        public string campo70 { get; set; }

        [StringLength(5000)]
        public string campo71 { get; set; }

        [StringLength(5000)]
        public string campo72 { get; set; }

        [StringLength(5000)]
        public string campo73 { get; set; }

        [StringLength(5000)]
        public string campo74 { get; set; }

        [StringLength(5000)]
        public string campo75 { get; set; }

        [StringLength(5000)]
        public string campo76 { get; set; }

        [StringLength(5000)]
        public string campo77 { get; set; }

        [StringLength(5000)]
        public string campo78 { get; set; }

        [StringLength(5000)]
        public string campo79 { get; set; }

        [StringLength(5000)]
        public string campo80 { get; set; }

        [StringLength(5000)]
        public string campo81 { get; set; }

        [StringLength(5000)]
        public string campo82 { get; set; }

        [StringLength(5000)]
        public string campo83 { get; set; }

        [StringLength(5000)]
        public string campo84 { get; set; }

        [StringLength(5000)]
        public string campo85 { get; set; }

        [StringLength(5000)]
        public string campo86 { get; set; }

        [StringLength(5000)]
        public string campo87 { get; set; }

        [StringLength(5000)]
        public string campo88 { get; set; }

        [StringLength(5000)]
        public string campo89 { get; set; }

        [StringLength(5000)]
        public string campo90 { get; set; }

        [StringLength(5000)]
        public string campo91 { get; set; }

        [StringLength(5000)]
        public string campo92 { get; set; }

        [StringLength(5000)]
        public string campo93 { get; set; }

        [StringLength(5000)]
        public string campo94 { get; set; }

        [StringLength(5000)]
        public string campo95 { get; set; }

        public int? kn1Division { get; set; }

        public int? kn2ValDinTiempo { get; set; }

        public int? kn3InterPagado { get; set; }

        public int? kn4CalInteresSimple { get; set; }

        public int? kn5CalcInteresCompuesto { get; set; }

        public int? kn4_5CalcInteresCompuesto { get; set; }

        public int? kn6RiesEInversion { get; set; }

        public int? kn7ConInflacion { get; set; }

        public int? kn8Diversificacion { get; set; }

        public int? knTotal { get; set; }

        public int? knTotalMay5 { get; set; }

        public int? decide { get; set; }

        public int? hacePresupuesto { get; set; }

        public int? nivelDetallePresupuesto { get; set; }

        public int? nivelUtilizaPresuuesto { get; set; }

        public int? ScorePresupuesto { get; set; }

        public int? culturaFinanciera1 { get; set; }

        public int? culturaFinanciera2 { get; set; }

        public int? culturaFinanciera3 { get; set; }

        public int? culturaFinanciera4 { get; set; }

        public int? culturaFinanciera5 { get; set; }

        public int? culturaFinanciera6 { get; set; }

        public int? culturaFinanciera7 { get; set; }

        public int? culturaFinanciera8 { get; set; }

        public int? culturaFinanciera9 { get; set; }

        public int? culturaFinanciera10 { get; set; }

        public int? culturaFinanciera11 { get; set; }

        public int? culturaFinanciera12 { get; set; }

        public int? culturaFinanciera13 { get; set; }

        public int? culturaFinanciera14 { get; set; }

        public int? culturaFinanciera15 { get; set; }

        public int? culturaFinanciera16 { get; set; }

        public int? culturaFinanciera17 { get; set; }

        public int? culturaFinanciera18 { get; set; }

        public int? culturaFinanciera19 { get; set; }

        public int? culturaFinanciera20 { get; set; }

        public int? culturaFinanciera21 { get; set; }

        public int? scoreCulturaFinaciera { get; set; }

        public int? IngresosYMitigarGastos { get; set; }

        public int? frecuenciaAhorro { get; set; }

        public int? nivelAhorro { get; set; }

        public int? ahorro1 { get; set; }

        public int? ahorro2 { get; set; }

        public int? ahorro3 { get; set; }

        public int? ahorro4 { get; set; }

        public int? ahorro5 { get; set; }

        public int? totalFormaAhorro { get; set; }

        public int? anosAhorro { get; set; }

        public double? ahorroImprevistoEmergencia { get; set; }

        public int? capacidadFaltaIngresos { get; set; }

        public double? puntajeAhorro { get; set; }

        public double? muerteIncapacidad1 { get; set; }

        public double? muerteIncapacidad2 { get; set; }

        public double? muerteIncapacidad3 { get; set; }

        public double? muerteIncapacidad4 { get; set; }

        public double? muerteIncapacidad5 { get; set; }

        public double? puntajeMuerteIncapacidad { get; set; }

        public int? planJubilacion { get; set; }

        public int? nivelJubilacion { get; set; }

        public int? puntajeJubilacion { get; set; }

        public int? comoPagas1 { get; set; }

        public int? comoPagas2 { get; set; }

        public int? comoPagas3 { get; set; }

        public int? comoPagas4 { get; set; }

        public int? comoPagas5 { get; set; }

        public int? comoPagas6 { get; set; }

        public int? comoPagas7 { get; set; }

        public int? comoPagas8 { get; set; }

        public int? comoPagas9 { get; set; }

        public int? comoPagas10 { get; set; }

        public int? comoPagas11 { get; set; }

        public int? comoPagas12 { get; set; }

        public int? comoPagas13 { get; set; }

        public int? comoPagas14 { get; set; }

        public int? comoPagas15 { get; set; }

        public int? comoPagas16 { get; set; }

        public int? comoPagas17 { get; set; }

        public int? comoPagas18 { get; set; }

        public int? comoPagas19 { get; set; }

        public int? comoPagas20 { get; set; }

        public int? comoPagas21 { get; set; }

        public int? CostoCredito { get; set; }

        public int? nivelImpulsivilidad { get; set; }

        public int? estrategiaCompra1 { get; set; }

        public int? estrategiaCompra2 { get; set; }

        public int? estrategiaCompra3 { get; set; }

        public int? estrategiaCompra4 { get; set; }

        public int? estrategiaCompra5 { get; set; }

        public int? estrategiaCompra6 { get; set; }

        public int? estrategiaCompra7 { get; set; }

        public int? estrategiaCompra8 { get; set; }

        public int? puntajeComportCompra { get; set; }

        public int? tienePrestamo { get; set; }

        public int? tipoDeuda1 { get; set; }

        public int? tipoDeuda2 { get; set; }

        public int? tipoDeuda3 { get; set; }

        public int? tipoDeuda4 { get; set; }

        public int? tipoDeuda5 { get; set; }

        public int? tipoDeuda6 { get; set; }

        public int? tipoDeuda7 { get; set; }

        public int? tipoDeuda8 { get; set; }

        public int? tipoDeuda9 { get; set; }

        public int? tipoDeuda10 { get; set; }

        public int? puntajeDeuda1 { get; set; }

        public int? puntajeDeuda2 { get; set; }

        public int? puntajeDeuda3 { get; set; }

        public int? puntajeDeuda4 { get; set; }

        public int? puntajeDeuda5 { get; set; }

        public int? puntajeDeuda6 { get; set; }

        public int? puntajeDeuda7 { get; set; }

        public int? puntajeDeuda8 { get; set; }

        public int? puntajeDeuda9 { get; set; }

        public int? puntajeDeuda10 { get; set; }

        public bool? indicadorTerminadoConocimientos { get; set; }

        public bool? indicadorTerminadoPresupuesto { get; set; }

        public bool? indicadorTerminadoCulturaFinanciera { get; set; }

        public bool? indicadorTerminadoAhorro { get; set; }

        public bool? indicadorTerminadoJubilacion { get; set; }

        public bool? indicadorTerminadoDar { get; set; }

        public int? puntajeDar { get; set; }

        public bool? indicadorTerminadoCapacidades { get; set; }

        public int? puntajeConocimiento { get; set; }

        public bool? indicadorTerminadoActitudCompras { get; set; }

        public int? puntajeActitudCompras { get; set; }

        public bool? indicadorTerminadoEndeudamiento { get; set; }

        public double? puntajeEndeudamiento { get; set; }

        public bool? indicadorTerminadoMuerte { get; set; }

        public double? puntajeIncapacidadMuerte { get; set; }
    }
}
