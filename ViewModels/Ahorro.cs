namespace EcoOplacementApi.ViewModels
{
    public class AhorroDeudaViewModels
    {
        public string articulo { get; set; }
        public string plazo { get; set; }
        public double? valor { get; set; }
        public double? tasa_interes { get; set; }
        public int? idUsuario { get; set; }
    }

    public class AhorroMensualAnual
    {
        public double? ahorro_mensual { get; set; }
        public double? ahorro_anual { get; set; }
        public int? edad { get; set; }
        public double? tasa_interes { get; set; }
        public int? idUsuario { get; set; }
    }
    public class AhorroanualViewModels
    {
        public double? ahorro_anual { get; set; }
        public int? idUsuario { get; set; }
    }

    public class InversionViewModels
    {
        public double? ahorro_inversion { get; set; }
        public int? idUsuario { get; set; }
    }

    public class NotificacionViewModels
    {
        public int? idUsuario { get; set; }
        public string  email { get; set; }
        public string telefono { get; set; }
        public bool? habilitado { get; set; }
    }
}