using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebinarEF.Models;
using WebinarEF.ViewModels;

namespace WebinarEF
{
	public class LookupItem
	{
		public int Key { get; set; }
		public string Value { get; set; }
	}
	public static class DataLayer
	{

		public static IEnumerable<TrackViewModel> GetTrackViewModelList()
		{
			using (var ctx = new ChinookContext())
			{
				var albumLookupData = (from album in ctx.Album
									   select new LookupItem { Key = album.AlbumId, Value = album.Title }).ToList();
				var mediaLookupData = (from media in ctx.MediaType
									   select new LookupItem() { Key = media.MediaTypeId, Value = media.Name }).ToList();
				var genreLookupData = (from genre in ctx.Genre
									   select new LookupItem { Key = genre.GenreId, Value = genre.Name }).ToList();

				foreach (var track in ctx.Track)
					yield return TrackViewModel.Create(track.TrackId,
										 track.Name,
										 track.AlbumId,
										 track.MediaTypeId,
										 track.GenreId,
										 track.Composer,
										 track.Milliseconds,
										 track.Bytes,
										 albumLookupData,
										 mediaLookupData,
										 genreLookupData);
			}
		}

		//public static TrackViewModel InsertTrack(TrackViewModel trackViewModel)
		//{

		//}
	}
}
