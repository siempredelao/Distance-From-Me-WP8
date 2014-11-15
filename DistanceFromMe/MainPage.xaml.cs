using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DistanceFromMe.Resources;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Toolkit;
using System.Device.Location;

namespace DistanceFromMe
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
        }

        // Código de ejemplo para compilar una ApplicationBar traducida
        private void BuildLocalizedApplicationBar()
        {
            // Establecer ApplicationBar de la página en una nueva instancia de ApplicationBar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 0.5;

            // Crear un nuevo botón y establecer el valor de texto en la cadena traducida de AppResources.
            // Search button
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.search.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarSearchButtonText;
            appBarButton.Click += ToggleSearch;
            ApplicationBar.Buttons.Add(appBarButton);

            // Location button
            appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/location.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarLocationButtonText;
            appBarButton.Click += ToggleLocation;
            ApplicationBar.Buttons.Add(appBarButton);

            // Crear un nuevo elemento de menú con la cadena traducida de AppResources.
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarAboutMenuItemText);
            appBarMenuItem.Click += appBarAboutMenuItemClick;
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void appBarAboutMenuItemClick(object sender, EventArgs e)
        {
            
        }

        private async void ToggleLocation(object sender, EventArgs e)
        {
            Geolocator geo = new Geolocator();
            geo.DesiredAccuracy = PositionAccuracy.High;
            Geoposition position = null;

            // Se intenta ubicar la posición de nuestro teléfono
            try
            {
                position = await geo.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Ubicación está desactivada en tu dispositivo");
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    MyCoordinate = new GeoCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude);
                    myMap.SetView(MyCoordinate, 15);
                    var children = MapExtensions.GetChildren(map);
                    var ob = children.Where(x => x.GetType() == typeof(UserLocationMarker)).FirstOrDefault();
                    UserLocationMarker marker = (UserLocationMarker)ob;

                    marker.GeoCoordinate = coordinate;
                    marker.Visibility = System.Windows.Visibility.Visible;
                }
                catch (Exception)
                {
                    //      MessageBox.Show("No podemos obtener lugares cercanos a tu locacíon ,verifica tu conexión a datos");

                }


            });

            // Se mueve el centro del mapa hacia nuestra ubicación
            this.map.Center.Latitude = position.Coordinate.Latitude;
            this.map.Center.Longitude = position.Coordinate.Longitude;
            this.map.ZoomLevel = 15;
        }

        private void ToggleSearch(object sender, EventArgs e)
        {
            
        }
    }
}