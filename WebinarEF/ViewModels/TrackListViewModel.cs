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
        //public virtual TrackList Tracks { get; set; }
        public IEnumerable<TrackInfo> Tracks
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
            //Tracks = new TrackList();
            Messenger.Default.Register<RequerySuggestedMessage>(this, OnRequerySuggested);
        }
        public static TrackListViewModel Create()
        {
            return ViewModelSource.Create(() => new TrackListViewModel());
        }


        void OnRequerySuggested(RequerySuggestedMessage message)
        {
            LoadTracks();
        }

        [ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
        protected virtual IDocumentManagerService DocumentManagerService { get { return null; } }

        [ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
        protected virtual IDispatcherService DispatcherService { get { return null; } }


        public void EditTrack(/*object*/TrackInfo track/*Object*/)
        {
            //var track = trackObject as TrackInfo;
            //var document = DocumentManagerService.CreateDocument("TrackView", TrackViewModel.Create(track));
            //document.Show();
            var document = DocumentManagerService.FindDocumentByIdOrCreate(track.TrackId,
                svc => svc.CreateDocument("TrackView", track.TrackId, this));
            document.Id = track.TrackId;
            document.Show();
        }

        public Task LoadTracks()
        {
            IsLoading = true;
            return Task.Factory.StartNew((state) =>
            {
                using (var ctx = new ChinookModel())
                {
                    // Apply the DTO pattern
                    var result = from track in ctx.Track
                                    .Include(x => x.Album)
                                    .Include(x => x.Genre)
                                    .Include(x => x.MediaType)
                                 select new TrackInfo()
                                 {
                                     TrackId = track.TrackId,
                                     Name = track.Name,
                                     AlbumId = track.AlbumId,
                                     AlbumTitle = track.AlbumId.HasValue ? track.Album.Title : "",
                                     MediaTypeId = track.MediaTypeId,
                                     MediaType = track.MediaType.Name,
                                     GenreId = track.GenreId,
                                     Genre = track.GenreId.HasValue ? track.Genre.Name : "",
                                     Composer = track.Composer,
                                     Milliseconds = track.Milliseconds,
                                     Bytes = track.Bytes
                                 };
                    Tracks = new ObservableCollection<TrackInfo>(result.ToList());
                }
                 ((IDispatcherService)state).BeginInvoke(() =>
                 {
                     this.RaisePropertyChanged(x => x.Tracks); // Notify the UI
                     IsLoading = false;
                 });

            }, DispatcherService);
        }


    }
}
    
