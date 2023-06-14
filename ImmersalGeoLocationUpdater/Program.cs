using HoloLab.PositioningTools.GeographicCoordinate;

namespace ImmersalGeoLocationUpdater
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;

            if (args.Length != 9)
            {
                Console.WriteLine($"Usage: {appName} <Immersal developer token> <map id> <latitude> <longitude> <ellipsoidal height> <nwu rotation x> <nwu rotation y> <nwu rotation z> <nwu rotation w>");
                return;
            }

            var token = args[0];

            if (int.TryParse(args[1], out var mapId) == false)
            {
                Console.WriteLine($"Map id is wrong: ${mapId}");
                return;
            }

            try
            {
                var latitude = double.Parse(args[2]);

                var longitude = double.Parse(args[3]);
                var ellipsoidalHeight = double.Parse(args[4]);

                var rotationX = double.Parse(args[5]);
                var rotationY = double.Parse(args[6]);
                var rotationZ = double.Parse(args[7]);
                var rotationW = double.Parse(args[8]);

                await UpdateMapMetadataAsync(
                                    token, mapId,
                                    latitude, longitude, ellipsoidalHeight,
                                    rotationX, rotationY, rotationZ, rotationW);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static async Task UpdateMapMetadataAsync(
            string token, int mapId,
            double latitude, double longitude, double ellipsoidalHeight,
            double rotationX, double rotationY, double rotationZ, double rotationW)
        {
            var ecefPosition = GeoConverter.ConvertToEcef(latitude, longitude, ellipsoidalHeight);

            var origin = new GeodeticPosition(latitude, longitude, ellipsoidalHeight);
            var ecefRotation = GeoConverter.ConvertToEcefRotation(rotationX, rotationY, rotationZ, rotationW, origin);

            var immersalClient = new ImmersalClient();
            var request = new SetMapMetadataRequest()
            {
                token = token,
                id = mapId,
                latitude = latitude,
                longitude = longitude,
                altitude = ellipsoidalHeight,
                tx = ecefPosition.X,
                ty = ecefPosition.Y,
                tz = ecefPosition.Z,
                qx = ecefRotation.X,
                qy = ecefRotation.Y,
                qz = ecefRotation.Z,
                qw = ecefRotation.W
            };

            await immersalClient.SetMapMetadataAsync(request);
        }
    }
}
