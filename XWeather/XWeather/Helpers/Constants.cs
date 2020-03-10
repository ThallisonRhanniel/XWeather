using System;
using System.Collections.Generic;
using System.Text;

namespace XWeather.Helpers
{
    public static class EndPoints
    {
        public const string BaseUrl = "https://api.darksky.net/forecast";
    }

    public static class Animation
    {
        public const string FilePermission = "permission_location.json";
        public const string TextPermissionGps = "Olá! precisamos da sua permissão. \r\n       Por favor, ative o GPS.";
        public const string FileWifi = "wifi_off.json";
        public const string WithoutConnection = "Sem Internet...";
        public const string LoadingFile = "loading.json";
        public const string TextLoading = "Carregando...";

    }
}
