using System;
using System.Linq;
using System.Data.Entity;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebinarEF.ViewModels
{
    [POCOViewModel]
    public class TrackViewModel: IEditableObject, IDocumentContent
    {
        protected TrackViewModel()
        {
        }

		protected TrackViewModel(int trackId, string name, int? albumId, int? mediaTypeId, int? genreId,
											string composer, int milliSeconds, int? bytes,
											IEnumerable<LookupItem> albumLookupData,
											IEnumerable<LookupItem> mediaLookupData,
											IEnumerable<LookupItem> genreLookupData)
		{
			this.TrackId = trackId;
			this.Name = name;
			this.AlbumId = albumId;
			this.MediaTypeId = mediaTypeId;
			this.GenreId = genreId;
			this.Composer = composer;
			this.Milliseconds = milliSeconds;
			this.Bytes = bytes;
			
			//calculated fields here

			//lookup data here
			this.AlbumLookupData = albumLookupData;
			this.MediaLookupData = mediaLookupData;
			this.GenreLookupData = genreLookupData;
		}

        public static TrackViewModel Create()
        {
            return ViewModelSource.Create(() => new TrackViewModel());
        }

		public static TrackViewModel Create(int trackId, string name, int? albumId, int? mediaTypeId, int? genreId, 
											string composer, int milliSeconds, int? bytes,
											IEnumerable<LookupItem> albumLookupData,
											IEnumerable<LookupItem> mediaLookupData,
											IEnumerable<LookupItem> genreLookupData)
		{
			return ViewModelSource.Create(() => new TrackViewModel(trackId, name, albumId, mediaTypeId, genreId, composer, milliSeconds, bytes,
				albumLookupData, mediaLookupData, genreLookupData));
		}
        //public static TrackViewModel Create(TrackInfo track)
        //{
        //    return ViewModelSource.Create(() => new TrackViewModel(track));
        //}
        public bool CanResetName()
        {
            return !String.IsNullOrEmpty(Name);
        }
		public void ResetName()
		{
			if (MessageBoxService.ShowMessage("Are you sure you want to reset the Name value?",
								"Question",
								MessageButton.YesNo,
								MessageIcon.Question,
								MessageResult.No) == MessageResult.Yes)
				Name = "";
		}

        [ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
        protected virtual IMessageBoxService MessageBoxService { get { return null; } }
        protected virtual ICurrentWindowService CurrentWindowService { get { return null; } }

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
     //       using (var ctx = new ChinookModel())
     //       {
     //           var track = (from t in ctx.Track
     //                        where t.TrackId == this.track.TrackId
     //                        select t).FirstOrDefault();
                                    
     //           if (track != null)
     //           {
     //               track.TrackId = this.TrackId;
     //               track.Name = this.Name;
     //               track.AlbumId = this.AlbumId;
     //               //track.AlbumTitle = this.AlbumTitle;
     //               track.MediaTypeId = this.MediaTypeId;
     //               //track.MediaType = this.MediaType;
     //               track.GenreId = this.GenreId;
     //               //track.Genre = this.Genre;
     //               track.Composer = this.Composer;
     //               track.Milliseconds = this.Milliseconds;
     //               track.Bytes = this.Bytes;

     //               ctx.SaveChanges();
                    
     //               hasChanges = false;
					//this.RaiseCanExecuteChanged(x => x.Save());                    
     //           }
     //           CurrentWindowService.Close();
     //       }
        }

        void IEditableObject.CancelEdit()
        {
            //Load(this.track);

            CurrentWindowService.Close();
        }

        #endregion
        public bool CanSave()
        {
            return hasChanges;
        }
        public void Save() { ((IEditableObject)this).EndEdit(); }
        public void Revert() { ((IEditableObject)this).CancelEdit(); }


        [Editable(false)]
        public int TrackId { get; set; }
        public virtual string Name { get; set; }
        public virtual int? AlbumId { get; set; }
        public virtual int? MediaTypeId { get; set; }
        public virtual int? GenreId { get; set; }
        public virtual string Composer { get; set; }
        public virtual int Milliseconds { get; set; }
        public virtual int? Bytes { get; set; }

		public virtual IEnumerable<LookupItem> AlbumLookupData { get; protected set; }
		public virtual IEnumerable<LookupItem> MediaLookupData { get; protected set; }
		public virtual IEnumerable<LookupItem> GenreLookupData { get; protected set; }


		#region IDocumentContent implementation
		IDocumentOwner IDocumentContent.DocumentOwner { get; set; }

        object IDocumentContent.Title
        {
            get { return Name; }
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