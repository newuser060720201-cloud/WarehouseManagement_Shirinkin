using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WarehouseManagement_Shirinkin.WPF.ViewModels
{
    public static class FieldValidation
    {
        public static void Reset(params Control[] controls)
        {
            var normal = (Brush)Application.Current.Resources["BorderBrush"];
            foreach (var c in controls)
            {
                c.BorderBrush = normal;
                c.BorderThickness = new Thickness(1);
            }
        }

        public static bool Fail(Control control, string message)
        {
            control.BorderBrush = (Brush)Application.Current.Resources["DeleteBrush"];
            control.BorderThickness = new Thickness(2);
            MessageBox.Show(message, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
            return false;
        }
    }
}