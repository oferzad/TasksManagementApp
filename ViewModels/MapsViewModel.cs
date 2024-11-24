using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Models;
using TasksManagementApp.Services;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace TasksManagementApp.ViewModels
{
    public class MapsViewModel:ViewModelBase
    {
        #region Origin

        //Define a property to hold the origin entry text
        private string origin;
        public string Origin
        {
            get => this.origin;
            set
            {
                this.origin = value;
                if (SelectedOrigin == null || this.origin != SelectedOrigin.Description)
                    PopulateOrigins();
                else
                    Origins.Clear();
                OnPropertyChanged("Origin");
            }
        }
        
        //Define a property to hol`qd the selected google auto complete prediction
        private GooglePlaceAutoCompletePrediction selectedOrigin;
        public GooglePlaceAutoCompletePrediction SelectedOrigin
        {
            get => this.selectedOrigin;
            set
            {
                this.selectedOrigin = value;
                OnPropertyChanged();
            }
        }

        //Define a command for origin selection changed!
        public ICommand OriginSelection => new Command(OnOriginSelection);
        private async void OnOriginSelection()
        {
            if (SelectedOrigin != null)
            {
                Origin = SelectedOrigin.Description;
                GooglePlace p = await mapsService.GetPlaceDetails(SelectedOrigin.PlaceId);
                RouteOrigin = p;
            }

        }

        //Define an observacbleCollection of all google auto complete predictions
        private ObservableCollection<GooglePlaceAutoCompletePrediction> origins;
        public ObservableCollection<GooglePlaceAutoCompletePrediction> Origins
        {
            get => this.origins;
            set
            {
                this.origins = value;
                OnPropertyChanged();
            }
        }
        //Define a method that populates all google auto complete predictions for the origin
        private async void PopulateOrigins()
        {
            //find auto complete places first for origin 
            GooglePlaceAutoCompleteResult originPlaces = await mapsService.GetPlaces(Origin);
            Origins.Clear();
            foreach (GooglePlaceAutoCompletePrediction place in originPlaces.AutoCompletePlaces)
            {
                Origins.Add(place);
            }

        }

        
        //Define a property to hold the Origin Google PLace that was selected!
        public GooglePlace RouteOrigin { get; private set; }



        #endregion

        #region Destination

        //Define a property to hold the destination entry text
        private string destination;
        public string Destination
        {
            get => this.destination;
            set
            {
                this.destination = value;
                if (SelectedDestination == null || this.destination != SelectedDestination.Description)
                    PopulateDestinations();
                else
                    Destinations.Clear();
                OnPropertyChanged();
            }
        }

        //Define a property to hold the selected google auto complete prediction
        private GooglePlaceAutoCompletePrediction selectedDestination;
        public GooglePlaceAutoCompletePrediction SelectedDestination
        {
            get => this.selectedDestination;
            set
            {
                this.selectedDestination = value;
                OnPropertyChanged();
            }
        }

        //Define a command for destination selection changed!
        public ICommand DestinationSelection => new Command(OnDestinationSelection);
        private async void OnDestinationSelection()
        {
            if (SelectedDestination != null)
            {
                Destination = SelectedDestination.Description;
                GooglePlace p = await mapsService.GetPlaceDetails(SelectedDestination.PlaceId);
                RouteDestination = p;
            }

        }

        //Define an observacbleCollection of all google auto complete predictions
        private ObservableCollection<GooglePlaceAutoCompletePrediction> destinations;
        public ObservableCollection<GooglePlaceAutoCompletePrediction> Destinations
        {
            get => this.destinations;
            set
            {
                this.destinations = value;
                OnPropertyChanged();
            }
        }
        //Define a method that populates all google auto complete predictions for the destination
        private async void PopulateDestinations()
        {
            //find auto complete places first for destination 
            GooglePlaceAutoCompleteResult destinationPlaces = await mapsService.GetPlaces(Destination);
            Destinations.Clear();
            foreach (GooglePlaceAutoCompletePrediction place in destinationPlaces.AutoCompletePlaces)
            {
                Destinations.Add(place);
            }
        }

        //Define a property to hold the Destination Google PLace that was selected!
        public GooglePlace RouteDestination { get; private set; }



        #endregion

        private GoogleMapsApiService mapsService;
        public MapsViewModel(GoogleMapsApiService mapsService)
        {
            this.mapsService = mapsService;
            this.Destinations = new ObservableCollection<GooglePlaceAutoCompletePrediction>();
            this.Origins = new ObservableCollection<GooglePlaceAutoCompletePrediction>();
        }
        public GoogleDirection RouteDirections { get; private set; }


        public ICommand Go => new Command(OnGo);
        public async void OnGo()
        {
            try
            {
                if (RouteOrigin == null || RouteDestination == null)
                {
                    return;
                }

                //get directions to move from origin to destination
                RouteDirections = await mapsService.GetDirections($"{RouteOrigin.Latitude}", $"{RouteOrigin.Longitude}", $"{RouteDestination.Latitude}", $"{RouteDestination.Longitude}");
                //Update the actual map on the view
                if (OnUpdateMapEvent != null)
                    OnUpdateMapEvent();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public event Action OnUpdateMapEvent;



    }
}
