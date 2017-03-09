using System;
using System.Linq;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebinarEF.Models;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace WebinarEF.ViewModels
{
    [POCOViewModel]
    public class TrackListViewModel
	{
        public virtual ObservableCollection<TrackViewModel> Tracks
        {
            get;
            /* We only want to set this through the ViewModel code */
            protected set;
        }

        public virtual bool IsLoading
        {
            get;
            protected set;
        }

        protected TrackListViewModel()
        {
        }

        public static TrackListViewModel Create()
        {
            return ViewModelSource.Create(() => new TrackListViewModel());
        }

        [ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
        public virtual IDialogService DialogService { get { return null; } }
        [ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
        protected virtual IDispatcherService DispatcherService { get { return null; } }


        public void EditTrack(TrackViewModel track)
        {
            var trackClone = track.Clone();
            if (DialogService.ShowDialog(
                MessageButton.OKCancel, "Edit Track", "TrackView", trackClone) == MessageResult.OK)
            {
                track.LoadFrom(trackClone);
                DataLayer.PersistTrack(track);
            }
		}

		public Task LoadTracks()
		{
			IsLoading = true;

			return Task.Factory.StartNew((state) =>
			{
				Tracks = new ObservableCollection<TrackViewModel>(DataLayer.GetTrackViewModelList());
				IsLoading = false;
			}, DispatcherService);
		}		
	}
}
    
