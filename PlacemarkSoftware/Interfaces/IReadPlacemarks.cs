using PlacemarkSoftware.API.Models;

namespace PlacemarkSoftware.API.Interfaces
{
    public interface IReadPlacemarks
    {
        IEnumerable<Placemark> ExportPlacemarks(IEnumerable<Placemark> placemarks);
        IEnumerable<Placemark> GetAllPlacemarks();
        IEnumerable<Placemark> GetPlacemark(string typeSearch, Placemark placemarks);
    }
}
