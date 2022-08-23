using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;

namespace AgileConfig.BlazorUI.Services
{
    public class NavigationService : IDisposable
    {
        private const int MIN_HISTORY_SIZE = 256;
        private const int ADDITIONAL_HISTORY_SIZE = 64;
        private readonly NavigationManager _navigationManager;
        private readonly List<string> _history;

        public NavigationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _history = new List<string>(MIN_HISTORY_SIZE + ADDITIONAL_HISTORY_SIZE)
            {
                _navigationManager.Uri
            };
            _navigationManager.LocationChanged += OnLocationChanged;
        }

        /// <summary>
        /// Navigates to the specified url.
        /// </summary>
        /// <param name="url">The destination url (relative or absolute).</param>
        public void NavigateTo(string url) => _navigationManager.NavigateTo(url);

        /// <summary>
        /// Returns true if it is possible to navigate to the previous url.
        /// </summary>
        public bool CanNavigateBack => _history.Count >= 2;

        /// <summary>
        /// Navigates to the previous url if possible or does nothing if it is not.
        /// </summary>
        public void NavigateBack()
        {
            if (!CanNavigateBack)
            {
                return;
            }

            string backPageUrl = _history[^2];
            _history.RemoveRange(_history.Count - 2, 2);
            _navigationManager.NavigateTo(backPageUrl);
        }

        // .. All other navigation methods.

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            EnsureSize();
            _history.Add(e.Location);
        }

        private void EnsureSize()
        {
            if (_history.Count < MIN_HISTORY_SIZE + ADDITIONAL_HISTORY_SIZE)
            {
                return;
            }

            _history.RemoveRange(0, _history.Count - MIN_HISTORY_SIZE);
        }

        public void Dispose() => _navigationManager.LocationChanged -= OnLocationChanged;
    }
}
