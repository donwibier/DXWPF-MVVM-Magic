using System;
using System.Linq;
using System.Data.Entity;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.ComponentModel;
using WebinarEF.Models;
using System.ComponentModel.DataAnnotations;

namespace WebinarEF.ViewModels
{
    [POCOViewModel]
    public class TrackViewModel: IEditableObject, IDocumentContent, ISupportParameter
    {
        private TrackInfo track;

        #region ISupportParameter Implementation

        public virtual object Parameter { get; set; }

        #endregion

        protected void OnParameterChanged()
        {
            int id = (int)Parameter;
            using (var ctx = new ChinookModel())
            {
                var result = (from track in ctx.Track
                                .Include(x => x.Album)
                                .Include(x => x.Genre)
                                .Include(x => x.MediaType)
                              where track.TrackId == id
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
                              }).FirstOrDefault();
                Load(result);
            }
        }

        //protected TrackViewModel()
        //{
        //    // for test purposes only !!
        //    Track = new TrackList()[15];
        //}
        //protected TrackViewModel() : this(new TrackList()[15]) { }
        protected TrackViewModel()
        {
        }
        //protected TrackViewModel(TrackInfo track)
        //{
        //    if (track == null)
        //        throw new ArgumentNullException("track", "track is null.");
        //    Load(track);
        //}
        public static TrackViewModel Create()
        {
            return ViewModelSource.Create(() => new TrackViewModel());
        }
        //public static TrackViewModel Create(TrackInfo track)
        //{
        //    return ViewModelSource.Create(() => new TrackViewModel(track));
        //}
        public bool CanResetName()
        {
            return track != null && !String.IsNullOrEmpty(Name);
        }
        public void ResetName()
        {
            if (track != null)
            {
                if (MessageBoxService.ShowMessage("Are you sure you want to reset the Name value?",
                                    "Question",
                                    MessageButton.YesNo,
                                    MessageIcon.Question,
                                    MessageResult.No) == MessageResult.Yes)
                    Name = "";
            }
        }

        [ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
        protected virtual IMessageBoxService MessageBoxService { get { return null; } }
        protected virtual ICurrentWindowService CurrentWindowService { get { return null; } }

        private void Load(TrackInfo track)
        {
            this.track = track;
            this.TrackId = track.TrackId;
            this.Name = track.Name;
            this.AlbumId = track.AlbumId;
            this.AlbumTitle = track.AlbumTitle;
            this.MediaTypeId = track.MediaTypeId;
            this.MediaType = track.MediaType;
            this.GenreId = track.GenreId;
            this.Genre = track.Genre;
            this.Composer = track.Composer;
            this.Milliseconds = track.Milliseconds;
            this.Bytes = track.Bytes;
            this.hasChanges = false;
        }

        private bool hasChanges;
        public void OnChange()
        {
            hasChanges = true;
            this.RaiseCanExecuteChanged(x => x.Save());
        }

        #region IEditableObject implementation
        void IEditableObject.BeginEdit()
        {

        }

        void IEditableObject.EndEdit()
        {
            //if (!string.Equals(Name, track.Name))
            //    track.Name = Name;
            //if (!string.Equals(Composer, track.Composer))
            //    track.Composer = Composer;
            //if (TrackId != track.TrackId)
            //    track.TrackId = TrackId;
            using (var ctx = new ChinookModel())
            {
                var track = (from t in ctx.Track
                             where t.TrackId == this.track.TrackId
                             select t).FirstOrDefault();
                                    
                if (track != null)
                {
                    track.TrackId = this.TrackId;
                    track.Name = this.Name;
                    track.AlbumId = this.AlbumId;
                    //track.AlbumTitle = this.AlbumTitle;
                    track.MediaTypeId = this.MediaTypeId;
                    //track.MediaType = this.MediaType;
                    track.GenreId = this.GenreId;
                    //track.Genre = this.Genre;
                    track.Composer = this.Composer;
                    track.Milliseconds = this.Milliseconds;
                    track.Bytes = this.Bytes;

                    ctx.SaveChanges();
                    
                    hasChanges = false;
                    this.RaiseCanExecuteChanged(x => x.Save());
                    Messenger.Default.Send(new RequerySuggestedMessage());
                    
                }
                CurrentWindowService.Close();
            }
        }

        void IEditableObject.CancelEdit()
        {
            Load(this.track);
            CurrentWindowService.Close();
        }

        #endregion
        public bool CanSave()
        {
            return (track != null) && hasChanges;
        }
        public void Save() { ((IEditableObject)this).EndEdit(); }
        public void Revert() { ((IEditableObject)this).CancelEdit(); }


        [Editable(false)]
        public int TrackId { get; set; }
        public virtual string Name { get; set; }
        public virtual int? AlbumId { get; set; }
        [Editable(false)]
        public virtual string AlbumTitle { get; set; }
        public virtual int MediaTypeId { get; set; }
        [Editable(false)]
        public virtual string MediaType { get; set; }
        public virtual int? GenreId { get; set; }
        [Editable(false)]
        public virtual string Genre { get; set; }
        public virtual string Composer { get; set; }
        public virtual int Milliseconds { get; set; }
        public virtual int? Bytes { get; set; }



        #region IDocumentContent implementation
        IDocumentOwner IDocumentContent.DocumentOwner { get; set; }

        object IDocumentContent.Title
        {
            get { return (track != null) ? track.Name : "New Track"; }
        }

        void IDocumentContent.OnClose(CancelEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void IDocumentContent.OnDestroy()
        {
            //throw new NotImplementedException();
        }

        #endregion

       

    }
}