using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using System;
using System.Linq;
using System.Windows;

namespace SlimGIS.Samples.TrackAndEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void map_Loaded(object sender, RoutedEventArgs e)
        {
            map.MapUnit = GeoUnit.Meter;
            map.UseOpenStreetMapAsBaseMap();
            map.ZoomToFullBound();
            map.Behaviors.TrackBehavior.TrackingCompleted += (s, ex) => EditFeaturesButton.IsEnabled = true;

            TrackComboBox.ItemsSource = Enum.GetValues(typeof(TrackMode));
        }

        private void EditOptionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            map.Behaviors.EditBehavior.Refresh();
        }

        private void EditFeaturesButton_Click(object sender, RoutedEventArgs e)
        {
            if (map.Behaviors.EditBehavior.Features.Count > 0) StopEditing();
            else if (map.Behaviors.TrackBehavior.Features.Count > 0)
            {
                StartEditing();
                map.Behaviors.TrackBehavior.TrackMode = TrackMode.None;
                TrackComboBox.SelectedItem = TrackMode.None;
            }

            bool isEditingFeatures = map.Behaviors.EditBehavior.Features.Count > 0;
            EditFeaturesButton.Content = isEditingFeatures ? "Stop Editing" : "Start Editing";
            DeleteInEditingFeaturesButton.IsEnabled = isEditingFeatures;
        }

        private void DeleteInEditingFeaturesButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteInEditingFeatures();
            if (map.Behaviors.EditBehavior.Features.Count == 0)
            {
                DeleteInEditingFeaturesButton.IsEnabled = false;
                EditFeaturesButton.Content = "Start Editing";
            }
        }

        private void StartEditing()
        {
            foreach (Feature feature in map.Behaviors.TrackBehavior.Features)
            {
                map.Behaviors.EditBehavior.Features.Add(feature);
            }
            map.Behaviors.TrackBehavior.Features.Clear();
            map.Behaviors.TrackBehavior.Refresh();
            map.Behaviors.EditBehavior.Refresh();
        }

        private void StopEditing()
        {
            foreach (Feature feature in map.Behaviors.EditBehavior.Features)
            {
                map.Behaviors.TrackBehavior.Features.Add(feature);
            }
            map.Behaviors.EditBehavior.Features.Clear();
            map.Behaviors.EditBehavior.InEditingFeatureIds.Clear();
            map.Behaviors.TrackBehavior.Refresh();
            map.Behaviors.EditBehavior.Refresh();
        }

        private void DeleteInEditingFeatures()
        {
            foreach (string id in map.Behaviors.EditBehavior.InEditingFeatureIds)
            {
                Feature deletedFeature = map.Behaviors.EditBehavior.Features.FirstOrDefault(f => f.Id == id);
                if (deletedFeature != null)
                {
                    map.Behaviors.EditBehavior.Features.Remove(deletedFeature);
                    map.Behaviors.EditBehavior.Refresh();
                }
            }
            map.Behaviors.EditBehavior.InEditingFeatureIds.Clear();
        }
    }
}
