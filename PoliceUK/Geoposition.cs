namespace PoliceUk
{
    public class Geoposition : IGeoposition
    {
        public double Latitude {get; private set;}

        public double Longitude {get; private set;}

        public Geoposition(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}