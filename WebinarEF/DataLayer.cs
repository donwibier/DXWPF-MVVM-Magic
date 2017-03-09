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
			List<TrackViewModel> results = new List<TrackViewModel>();
			using (var ctx = new ChinookContext())
			{
				var albumLookupData = (from album in ctx.Album
									   select new LookupItem { Key = album.AlbumId, Value = album.Title }).ToArray();
				var mediaLookupData = (from media in ctx.MediaType
									   select new LookupItem() { Key = media.MediaTypeId, Value = media.Name }).ToArray();
				var genreLookupData = (from genre in ctx.Genre
									   select new LookupItem { Key = genre.GenreId, Value = genre.Name }).ToArray();

				var tracks = from track in ctx.Track
							 select track;

				foreach (var track in tracks)
				{
					results.Add(TrackViewModel.Create(track.TrackId,
										 track.Name,
										 track.AlbumId,
										 track.MediaTypeId,
										 track.GenreId,
										 track.Composer,
										 track.Milliseconds,
										 track.Bytes,
										 albumLookupData,
										 mediaLookupData,
										 genreLookupData));
				}				
			}
			return results;
		}

		//public static TrackViewModel InsertTrack(TrackViewModel trackViewModel)
		//{

		//}
	}
}
