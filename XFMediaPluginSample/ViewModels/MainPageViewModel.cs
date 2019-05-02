using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Plugin.Media;
using Xamarin.Forms;

namespace XFMediaPluginSample.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ICommand TakePhotoCommand { get; }

        private ImageSource _imgSource;
        public ImageSource ImgSource
        {
            get => _imgSource;
            set => SetValue(ref _imgSource, value);
        }

        public MainPageViewModel()
        {
            TakePhotoCommand = new Command(async () =>
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = false,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear
                });

                if(file == null)
                {
                    return; 
                }

                ImgSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            });
        }

        private void SetValue<T>(ref T initial, T value, [CallerMemberName]string propertyName = null)
        {
            if(!value.Equals(initial))
            {
                initial = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
