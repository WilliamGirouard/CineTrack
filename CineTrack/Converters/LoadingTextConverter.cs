using System.Globalization;
using System.Windows.Data;

namespace CineTrack.Converters
{
    /// Retourne "Chargement..." si IsLoading=true, sinon le ConverterParameter (texte normal).
    /// Usage : Converter={StaticResource LoadingTextConverter}, ConverterParameter=S'inscrire
    
    public class LoadingTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is true ? "Chargement..." : (parameter?.ToString() ?? string.Empty);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
