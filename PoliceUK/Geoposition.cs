namespace PoliceUk
{
    public class Geoposition : IGeoposition
    {
        public double Latitiude {get; private set;}

        public double Longitude {get; private set;}

        public Geoposition(double latitude, double longitude)
        {
            this.Latitiude = latitude;
            this.Longitude = longitude;
        }
    }
}