using HoloLab.PositioningTools.GeographicCoordinate;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ImmersalGeoLocationUpdater
{
    internal static class GeoConverter
    {
        public static EcefPosition ConvertToEcef(double latitude, double longitude, double ellipsoidalHeight)
        {
            var geodeticPosition = new GeodeticPosition(latitude, longitude, ellipsoidalHeight);
            var ecefPosition = GeographicCoordinateConversion.GeodeticToEcef(geodeticPosition);

            return ecefPosition;
        }

        public static (double X, double Y, double Z, double W) ConvertToEcefRotation(
            double rotationX, double rotationY, double rotationZ, double rotationW,
            GeodeticPosition origin)
        {
            var rotation = new Quaternion((float)rotationY, (float)rotationZ, (float)rotationX, (float)rotationW);
            rotation = Quaternion.Normalize(rotation);

            var westEnuPosition = new EnuPosition(-1, 0, 0);
            var upEnuPosition = new EnuPosition(0, 0, 1);
            var northEnuPosition = new EnuPosition(0, 1, 0);

            var westInEcef = EnuVectorToEcefVector(westEnuPosition, origin);
            var upInEcef = EnuVectorToEcefVector(upEnuPosition, origin);
            var northInEcef = EnuVectorToEcefVector(northEnuPosition, origin);

            var matrix = Matrix4x4.Identity;
            matrix.M11 = westInEcef.X;
            matrix.M12 = westInEcef.Y;
            matrix.M13 = westInEcef.Z;

            matrix.M21 = upInEcef.X;
            matrix.M22 = upInEcef.Y;
            matrix.M23 = upInEcef.Z;

            matrix.M31 = northInEcef.X;
            matrix.M32 = northInEcef.Y;
            matrix.M33 = northInEcef.Z;

            var nwuToEcefMatrix = Quaternion.CreateFromRotationMatrix(matrix);

            var ecefRotation = Quaternion.Concatenate(rotation, nwuToEcefMatrix);
            return (ecefRotation.X, ecefRotation.Y, ecefRotation.Z, ecefRotation.W);
        }

        private static Vector3 EnuVectorToEcefVector(EnuPosition enuVector, GeodeticPosition origin)
        {
            var ecefOrigin = GeographicCoordinateConversion.GeodeticToEcef(origin);
            var ecefVector = GeographicCoordinateConversion.EnuToEcef(enuVector, origin);

            return new Vector3(
                (float)(ecefVector.X - ecefOrigin.X),
                (float)(ecefVector.Y - ecefOrigin.Y),
                (float)(ecefVector.Z - ecefOrigin.Z)
                );
        }
    }
}
