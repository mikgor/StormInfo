using StormInfo.StormApi;

namespace StormInfo.Services
{
    static class StormService 
    {
        static serwerSOAPPortClient _stormAPI;
        static string _keyAPI;
        static bool _connected;
        public static MyComplexTypeMiejscowosc Location;

        private static void Connect()
        {
            _stormAPI.Open();
            _connected = true;
        }

        public static void Disconnect()
        {
            if (_connected)
                _stormAPI.Close();
        }

        public static void Initialize(string name)
        {
            _stormAPI = new serwerSOAPPortClient();
            _keyAPI = "";
            Connect();
            Location = _stormAPI.miejscowosc(name, _keyAPI);
        }

        public static bool Initialized
            => Location.x != 0 && Location.y != 0;

        public static string GetLocationsByName(string name)
            => _stormAPI.miejscowosci_lista(name, "", _keyAPI);

        public static MyComplexTypeBurza GetStormInfo(int radius)
            => _stormAPI.szukaj_burzy(Location.y.ToString(), Location.x.ToString(), radius, _keyAPI);
    }
}
