using AirbnbApp.Messanging;
using AirbnbApp.Services;
using AirbnbApp.Validations;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
using Models;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace AirbnbApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AddPublicationVM : ViewModelBase, IDataErrorInfo
    {
        public static Action<string> action;
        private IObjectSender objectSender;
        private Account account;
        public Messenger MyMessenger { get; set; }
        public Location CurrLoc { get; set; } = new Location();
        public CityInformation cityInformation { get; set; }
        private Home home;
        private Publication publication;
        private string latitude = "32.2797022146049";
        private string longitude = "85.78125";
        private string name;
        public string Address { get; set; }
        public string TheAnswer { get; set; }
        public string ButtonPic { get; set; }
        public string ApartmentType { get; set; }
        public string Name { get => name; set => Set(ref name, value); }
        public string Latitude
        {
            get => latitude1; set
            {
                latitude1 = value;
                CurrLoc = new Location(double.Parse(Latitude), double.Parse(Longitude));

            }
        }

        public string Longitude
        {
            get => longitude;
            set
            {
                longitude = value;
                CurrLoc.Longitude = double.Parse(Longitude);
            }
        }
        private Messenger messenger;


        public List<City> Cities { get; set; } = new List<City>();

        public List<Country> Countries { get; set; }

        public City SelectedCity
        {
            get => selectedCity; set
            {
                Set(ref selectedCity, value);
                NextButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public Country SelectedCountry
        {
            get => selectedCountry;
            set
            {
                Set(ref selectedCountry, value);
                Cities = value.Cities;
            }
        }



        private int selectedIndex = -1;
        public string Price
        {
            get => price; set
            {
                if (regex.IsMatch(value.ToString()))
                {
                    Set(ref price,value);
                    NextButtonCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public int NumberOfAdult { get; set; }
        public int NumberOfChild { get; set; }
        public int NumberOfInfant { get; set; }
        public int NumberOfBedroom { get; set; }
        public int NumberOfBathroom { get; set; }
        public string Number { get => number; set
            {
                if (regex.IsMatch(value))
                {
                    Set(ref number,value);
                }
            }
        }

        public string CityName { get; set; }

        public bool TV { get; set; }
        public bool WIFI { get; set; }
        public bool Hangers { get; set; }
        public bool Shampoo { get; set; }
        public bool Hairdryer { get; set; }
        public bool Essentials { get; set; }
        public bool CookingBasics { get; set; }
        public bool Deskworkspace { get; set; }
        public bool IronHousehold { get; set; }
        public bool FirstRadioButton { get; set; }
        public bool SecondRadioButton { get; set; }
        public bool LocationWithTextBoxRadioButton { get; set; }


        private ObservableCollection<Amenity> addedAmenities;
        private RelayCommand<Amenity> removeAmenityCommand;
        private List<Amenity> amenities;
        private Amenity selectedAmenity;
        private RelayCommand addAmenityCommand;
        private Country selectedCountry;
        private RelayCommand<ImageOfHome> deletePhoto;
        private RelayCommand nextButtonCommand;

        public RelayCommand AdultMinusButton { get; set; }
        public RelayCommand AdultPilusButton { get; set; }

        public RelayCommand ChildMinusButton { get; set; }
        public RelayCommand ChildPilusButton { get; set; }

        public RelayCommand InfantMinusButton { get; set; }
        public RelayCommand InfantPilusButton { get; set; }


        public RelayCommand BedroomMinusButton { get; set; }
        public RelayCommand BedroomPilusButton { get; set; }
        public RelayCommand BathroomMinusButton { get; set; }
        public RelayCommand BathroomPilusButton { get; set; }
        Regex regex = new Regex("[0-9]$");
        private City selectedCity;
        private HomeType homeTypeComboBox;
        private string latitude1 = "32.2797022146049";
        private string price;
        private string number;

        public RelayCommand NextButtonCommand => nextButtonCommand ?? (nextButtonCommand = new RelayCommand(() =>
        {
            home = new Home();
            home.Price = int.Parse(Price);
            foreach (var item in AddedAmenities)
            {
                home.Amenities.Add(item);
            }
            home.Address = Address;
            home.BathrommsCount = NumberOfBathroom;
            home.BedrommsCount = NumberOfBedroom;
            home.AdultsCount = NumberOfAdult;
            home.ChildrenCount = NumberOfChild;
            home.InfantCount = NumberOfInfant;
            home.HomeType = HomeTypeComboBox;
            home.PlaceType = PlaceType;
            home.Images = ImageToByte();
            home.lan = Latitude;
            home.lon = Longitude;
            home.City = SelectedCity;
            publication = new Publication()
            {
                Home = home,
                Number = Number,
                AccountMail = account.Email,
                AccountId = account.Id
            };

            objectSender.SendObjectPorstURi(publication, ProcessTypes.AddPublication);
            MessageBox.Show("Your Publication added!!!");
            messenger.Send<ViewChange>(new ViewChange() { ViewModel = App.Container.GetInstance<HomeVM>() });
        }, () =>
        {
            return Price != null
                   && SelectedCity != null;

        }));

        public HomeType HomeTypeComboBox { get => homeTypeComboBox; set => Set(ref homeTypeComboBox, value); }
        public TypeOfPlace PlaceType { get; set; }
        public ObservableCollection<ImageOfHome> Collection { get; set; }
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                if (value != -1) ButtonPic = Collection[selectedIndex].ImagePath;
            }
        }


        public ObservableCollection<Amenity> AddedAmenities { get => addedAmenities; set => Set(ref addedAmenities, value); }

        public RelayCommand<Amenity> RemoveAmenityCommand => removeAmenityCommand ?? (removeAmenityCommand = new RelayCommand<Amenity>((x) =>
        {
            if (x != null)
            {
                AddedAmenities.Remove(x);
            }
        }));

        public RelayCommand<ImageOfHome> DeletePhoto => deletePhoto ?? (deletePhoto = new RelayCommand<ImageOfHome>((x) =>
        {
            if (x != null)
            {
                Collection.Remove(x);
            }

        }));

        public List<Amenity> Amenities { get => amenities; set => Set(ref amenities, value); }

        public Amenity SelectedAmenity
        {
            get => selectedAmenity;
            set
            {
                Set(ref selectedAmenity, value);
                AddAmenityCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddAmenityCommand => addAmenityCommand ?? (addAmenityCommand = new RelayCommand(() =>
        {
            if (!AddedAmenities.Any(x => x.Name == SelectedAmenity.Name))
            {
                AddedAmenities.Add(SelectedAmenity);
                SelectedAmenity = null;
            }
            else MessageBox.Show("Amenity allready added");
        }, () => SelectedAmenity != null));


        public AddPublicationVM()
        {
            messenger = App.Container.GetInstance<Messenger>();
            this.objectSender = App.Container.GetInstance<IObjectSender>();
            Collection = new ObservableCollection<ImageOfHome>();
            AdultMinusButton = new RelayCommand(() => { NumberOfAdult = DecreasGivenNumber(NumberOfAdult); });
            AdultPilusButton = new RelayCommand(() => NumberOfAdult++);
            ChildMinusButton = new RelayCommand(() => { NumberOfChild = DecreasGivenNumber(NumberOfChild); });
            ChildPilusButton = new RelayCommand(() => NumberOfChild++);
            InfantMinusButton = new RelayCommand(() => { NumberOfInfant = DecreasGivenNumber(NumberOfInfant); });
            InfantPilusButton = new RelayCommand(() => NumberOfInfant++);
            BedroomMinusButton = new RelayCommand(() => { NumberOfBedroom = DecreasGivenNumber(NumberOfBedroom); });
            BedroomPilusButton = new RelayCommand(() => NumberOfBedroom++);
            BathroomMinusButton = new RelayCommand(() => { NumberOfBathroom = DecreasGivenNumber(NumberOfBathroom); });
            BathroomPilusButton = new RelayCommand(() => NumberOfBathroom++);
            action = DragAndPutPicture;


            AddedAmenities = new ObservableCollection<Amenity>();
            Amenities = new List<Amenity>();
            Amenities.Add(new Amenity() { Icon = "Wifi", Name = "Wifi" });
            Amenities.Add(new Amenity() { Icon = "Dishwasher", Name = "Dishwasher" });
            Amenities.Add(new Amenity() { Icon = "PaperRoll", Name = "PaperRoll" });
            Amenities.Add(new Amenity() { Icon = "Television", Name = "Television" });

            MyMessenger = App.Container.GetInstance<Messenger>();

            MyMessenger.Register<AccountMessage>(this, x =>
            {
                account = x.Account;
            });

            MyMessenger.Register<LocationMessage>(this, message =>
            {
                Latitude = message.Latitude.ToString();
                Longitude = message.Longitude.ToString();
            });



            Task.Run(async () =>
            {
                //var str2 = await objectSender.SendObjectPorstURi(null, ProcessTypes.GetAmenities);
                var str = await objectSender.SendObjectPorstURi(null, ProcessTypes.GetCountry);
                Countries = JsonConvert.DeserializeObject<List<Country>>(str);
                //Amenities = JsonConvert.DeserializeObject<List<Amenity>>(str2);
            });
        }
        public List<Image> ImageToByte()
        {
            List<Image> images = new List<Image>();
            foreach (var item in Collection)
            {
                var byteArr = ConvertImageToByte.GetPhoto(item.ImagePath);
                Image keep = new Image() { Photos = byteArr };
                images.Add(keep);
            }
            return images;
        }
        public string CheckRadioButton()
        {
            if (FirstRadioButton)
            {
                TheAnswer = "I'm hosting as part of a business";
            }
            else TheAnswer = "I'm hosting as a private individual";
            return TheAnswer;
        }
        public void DetermineAddress()
        {
            SearchCityService search = new SearchCityService();
            cityInformation = search.GetWeatherByCoords((float)CurrLoc.Latitude, (float)CurrLoc.Longitude);
            CityName = cityInformation.name;
            MessageBox.Show(cityInformation.name);
        }



        public int DecreasGivenNumber(int value)
        {
            if (value != 0)
                value--;
            else MessageBox.Show("Nağayrırsan ?", "Easter egg", MessageBoxButton.OK, MessageBoxImage.Question);

            return value;
        }

        public void DragAndPutPicture(string path)
        {
            Collection.Add(new ImageOfHome() { ImagePath = path });
        }
        public string Error => throw new NotImplementedException();


        public string this[string columnName]
        {
            get
            {
                var validator = new AddPublicationValidation();
                var result = validator.Validate(this);
                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains(columnName));
                    if (error != null)
                        return error.ErrorMessage;
                }
                return string.Empty;
            }
        }
    }
}
