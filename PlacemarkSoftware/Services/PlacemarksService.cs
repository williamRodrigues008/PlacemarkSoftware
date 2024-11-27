using PlacemarkSoftware.API.Helpers;
using PlacemarkSoftware.API.Interfaces;
using PlacemarkSoftware.API.Models;

namespace PlacemarkSoftware.API.Services
{
    public class PlacemarksService : IReadPlacemarks
    {
        public IEnumerable<Placemark> ExportPlacemarks(IEnumerable<Placemark> placemarksParams)
        {
            var context = new Context();
            var placemarksList = context.ExportFileKML(placemarksParams);



            return placemarksList;
        }

        public IEnumerable<Placemark> GetAllPlacemarks()
        {
            var context = new Context();
            return context.GetPlacemarkAll();
        }

        public IEnumerable<Placemark> GetPlacemark(string typeSearch, Placemark placemarks)
        {
            var context = new Context();
            return context.GetPlacemark(typeSearch.ToLower(), placemarks);
        }
    }
}
