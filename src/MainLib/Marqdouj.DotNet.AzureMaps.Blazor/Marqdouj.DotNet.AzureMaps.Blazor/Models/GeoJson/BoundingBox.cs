using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// A GeoJSON BoundingBox. Supports exactly 4 (2D) or 6 (3D including altitude) doubles.
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7946#section-5"/>.
    /// </summary>
    /// <remarks>
    /// [west, south, east, north] or [west, south, elevation1, east, north, elevation2]
    /// </remarks>
    public class BoundingBox : List<double>, ICloneable
    {
        [JsonConstructor]
        public BoundingBox() { }

        //[JsonConstructor]
        public BoundingBox(IList<double> values) : base(ValidateValues(values)) {}

        public BoundingBox(Position southwest, Position northeast) : base(ConvertToList(southwest, northeast)) { }

        public BoundingBox(double west, double south, double east, double north) 
            :base (ValidateValues([west, south, east, north]))
        {
        }

        public BoundingBox(double west, double south, double elevation1, double east, double north, double elevation2)
            : base(ValidateValues([west, south, elevation1, east, north, elevation2]))
        {
        }

        private static IList<double> ValidateValues(IList<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            if (values.Count != 4 && values.Count != 6)
                throw new ArgumentException("BoundingBox must contain exactly 4 (2D) or 6 (3D) numbers.", nameof(values));
            return values;
        }

        public static IList<double> ConvertToList(Position southwest, Position northeast)
        {
            var hasSWZ = southwest.HasElevation;
            var hasNEZ = northeast.HasElevation;
            var values = new List<double>();

            if (hasSWZ && hasNEZ)
            {
                values.Add(southwest.Longitude);
                values.Add(southwest.Latitude);
                values.Add(southwest.Elevation!.Value);
                values.Add(northeast.Longitude);
                values.Add(northeast.Latitude);
                values.Add(northeast.Elevation!.Value);
            }
            else if (!hasSWZ && !hasNEZ)
            {
                values.Add(southwest.Longitude);
                values.Add(southwest.Latitude);
                values.Add(northeast.Longitude);
                values.Add(northeast.Latitude);
            }
            else if (hasSWZ)
            {
                throw new ArgumentException("Southwest position has an elevation and Northeast does not.");
            }
            else if (hasNEZ)
            {
                throw new ArgumentException("Northeast position has an elevation and Southwest does not.");
            }

            return ValidateValues(values);
        }

        #region Non-GeoJSON Spec
        //Not part of the GeoJSON spec but useful

        [JsonIgnore] public bool Is2D => Count == 4;
        [JsonIgnore] public bool Is3D => Count == 6;
        [JsonIgnore] public bool IsValid => Is2D || Is3D;

        [JsonIgnore]
        public Position? SouthWest
        {
            get
            {
                if (IsValid)
                    return Is2D ? new Position(this[0], this[1]) : new Position(this[0], this[1], this[2]);

                return null;
            }
        }

        [JsonIgnore]
        public Position? NorthEast
        {
            get
            {
                if (IsValid)
                    return Is2D ? new Position(this[2], this[3]) : new Position(this[3], this[4], this[5]);

                return null;
            }
        }

        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return ToString();

            return IsValid ? "[" + string.Join(", ", this.Select(e => e.ToString(format))) + "]" : "Invalid";
        }

        public override string ToString()
        {
            return IsValid ? "[" + string.Join(", ", this) + "]" : "Invalid";
        }
    }
}
