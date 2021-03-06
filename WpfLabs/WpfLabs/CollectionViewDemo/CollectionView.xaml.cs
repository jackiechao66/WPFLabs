﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace WpfLabs.CollectionViewDemo
{
    /// <summary>
    /// CollectionView.xaml 的交互逻辑
    /// </summary>
    public partial class CollectionView : INotifyPropertyChanged
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public ObservableCollection<ItemModel> ItemModels { get; set; }

        private ICollectionView _displayItemModels;

        public ICollectionView DisplayItemModels
        {
            get => _displayItemModels;
            set
            {
                _displayItemModels = value;
                OnPropertyChanged();
            }
        }

        public string Key { get; set; }

        private long _elapsedMilliseconds;

        public long ElapsedMilliseconds
        {
            get => _elapsedMilliseconds;
            set
            {
                _elapsedMilliseconds = value;
                OnPropertyChanged();
            }
        }

        public CollectionView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            _stopwatch.Restart();

            Refresh();

            _stopwatch.Stop();
            ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            _stopwatch.Restart();

            var newItem = ItemModel.GetNewItem();
            ItemModels.Insert(0, newItem);

            _stopwatch.Stop();
            ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        }

        private void Sub_OnClick(object sender, RoutedEventArgs e)
        {
            _stopwatch.Restart();

            if (ItemModels.Any())
            {
                ItemModels.RemoveAt(0);
            }

            _stopwatch.Stop();
            ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        }

        private void Up_OnClick(object sender, RoutedEventArgs e)
        {
            _stopwatch.Restart();

            if (DisplayItemModels != null)
            {
                if (DisplayItemModels.SortDescriptions.Any())
                {
                    DisplayItemModels.SortDescriptions.Clear();
                }

                DisplayItemModels.SortDescriptions.Add(new SortDescription(nameof(ItemModel.Name),
                    ListSortDirection.Ascending));
            }

            _stopwatch.Stop();
            ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        }

        private void Down_OnClick(object sender, RoutedEventArgs e)
        {
            _stopwatch.Restart();

            if (DisplayItemModels != null)
            {
                if (DisplayItemModels.SortDescriptions.Any())
                {
                    DisplayItemModels.SortDescriptions.Clear();
                }

                DisplayItemModels.SortDescriptions.Add(new SortDescription(nameof(ItemModel.Name),
                    ListSortDirection.Descending));
            }

            _stopwatch.Stop();
            ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        }

        private void Refresh()
        {
            if (!string.IsNullOrEmpty(Key))
            {
                DisplayItemModels.Filter = o => ((ItemModel)o).Name.Contains(Key);
            }
            else
            {
                DisplayItemModels = CollectionViewSource.GetDefaultView(ItemModels);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CollectionView_OnLoaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
