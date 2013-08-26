using OpenGrooves.Core;
using OpenGrooves.Web.Models;
using StructureMap;
using System;
using System.Collections.Generic;

namespace OpenGrooves.Web.Proxies
{
    [PluginFamily("DataRepository")]
    public interface IRepository : IDisposable
    {
        IEnumerable<FeedItemModel> GetFeedByUser(Guid userId, int max = 30);
        IEnumerable<FeedItemModel> GetFeedByBand(Guid bandId, int max = 30);
        FeedItemModel GetFeedItem(Guid feedItemId);

        IEnumerable<UserModel> FindUsersStartingWith(string query);
        IEnumerable<BandModel> FindBandsStartingWith(string query);
        IEnumerable<EventModel> FindEventsStartingWith(string query);

        IEnumerable<BandModel> SearchBands(string query, int max = 20);

        IEnumerable<EventModel> GetEventsByUser(Guid userId, int max = 20);
        IEnumerable<BandEventRelation> GetEventsByBand(Guid bandId, int max = 20, bool upcomingOnly = true);
        IEnumerable<EventModel> GetEventsByLocation(Coordinate coord, int radius, int max = 20, SearchSort? sort = null);
        IEnumerable<EventModel> GetUpcomingEventsByUser(Guid userId, int max = 20);
        
        IEnumerable<BandModel> GetBandsByGenre(string genre, int max = 20);
        IEnumerable<BandModel> GetNewestBands(int max = 20);
        IEnumerable<BandModel> GetBandsByLetter(string s, int max = 20);
        IEnumerable<BandModel> GetBandsByLocation(Coordinate coord, int radius, int max = 20, Guid? excludeByUserId = null, SearchSort? sort = null);

        IEnumerable<ImageModel> GetRecentImages(int max = 20);
        IEnumerable<ImageModel> GetRecentBandImages(Guid userId, int max = 10, bool onlyUncategorized = false);
        
        IEnumerable<MemberTypeRelation> GetMemberTypes();
        
        IEnumerable<ImageModel> GetImagesByUser(string userName, bool onlyUncategorized = true);
        IEnumerable<ImageModel> GetImagesByUser(Guid userId, bool onlyUncategorized = true);
        IEnumerable<ImageModel> GetImagesByBand(string bandUrl, bool onlyUncategorized = false);
        IEnumerable<ImageModel> GetImagesByBand(Guid bandId, bool onlyUncategorized = false);
        IEnumerable<ImageModel> GetImagesByBatch(Guid batchId, int max = 10);
        IEnumerable<GalleryModel> GetGalleriesByUser(Guid userId);
        IEnumerable<GalleryModel> GetGalleriesByBand(Guid bandId);
        IEnumerable<GalleryModel> GetBandGalleriesByUser(Guid userId);

        IEnumerable<AudioModel> GetAudioByBand(Guid bandId, int max = 20);

        MessageModel GetMessage(Guid messageId);
        void MarkMessageRead(Guid messageId, Guid userId);
        IEnumerable<MessageModel> GetUserSentMessages(Guid userId, int max = 20);
        IEnumerable<MessageRecipientRelation> GetUserReceivedMessages(Guid userId);
        
        IEnumerable<UserBandRelation> GetBandRelationsByUser(Guid userId, int relationTypeId, int max = 10);
        IEnumerable<BandEventRelation> GetBandRelationsByEvent(Guid eventId);
        IEnumerable<UserBandRelation> GetUserRelationsByBand(Guid bandId, int relationTypeId, int max = 10);

        IEnumerable<GenreModel> GetGenres();
        GenreModel GetGenre(string genre);

        UserBandRelation GetUserBandRelation(Guid relationId);
        UserBandRelation GetUserBandRelation(Guid userId, Guid bandId);
        BandEventRelation GetBandEventRelation(Guid relationId);
        BandEventRelation GetBandEventRelation(Guid bandId, Guid eventId);

        BandModel GetBand(Guid bandId, bool loadExtended = true);
        BandModel GetBand(string name, bool isUrl = true, bool loadExtended = true);
        Guid? GetBandId(string bandUrl);
        EventModel GetEvent(string name, bool isUrl = true, bool loadExtended = true);
        Guid? GetEventId(string eventUrl);
        UserModel GetUser(Guid userId);
        Guid? GetUserId(string username);
        UserModel GetUser(string username);
        ImageModel GetImage(Guid imageId);
        GalleryModel GetGallery(string galleryName);
        IEnumerable<Setting> GetUserSettings(params Guid[] userIds);
        Location GetUserLocation(Guid userId);

        int GetTotalFans(Guid bandId);

        string CreateBand(BandModel band, Guid userId);
        string CreateEvent(EventModel ev, Guid userId);
        string CreateGallery(string name, string url, Guid userId, Guid? bandId = null);

        void DeleteBand(Guid bandId);
        void DeleteEvent(Guid eventId);
        void DeleteImage(Guid imageId);
        void DeleteImageGallery(Guid imageId);
        void DeleteGallery(Guid galleryId);
        void DeleteFeedItem(Guid feedItemId);
        Result DeleteUserBandRelation(Guid relationId);
        Result DeleteUserBandRelation(Guid bandId, Guid userId);
        Result DeleteBandEventRelation(Guid relationId);
        Result DeleteBandEventRelation(Guid bandId, Guid eventId);
        void DeleteMessage(Guid messageId, Guid userId);
        void DeleteAudio(Guid audioId);

        void UpdateUser(UserModel user);
        void UpdateBand(BandModel band);
        void UpdateBandGenres(Guid bandId, IEnumerable<Guid> genres);
        void UpdateEvent(EventModel ev);
        string UpdateGallery(GalleryModel gallery);
        void UpdateImageGallery(Guid imageId, Guid galleryId);

        IEnumerable<UserModel> SendMessages(Guid fromUserId, IEnumerable<string> recipients, string content, Guid? toBandId = null);

        void AddFeedItem(string data, int feedTypeId, Guid bandId, Guid? userId = null, Guid? eventId = null, Guid? batchId = null);

        void SetUserBandRelation(Guid relationId, int relationTypeId, bool isActive = true);
        void SetUserBandRelation(Guid bandId, Guid userId, int relationTypeId, bool isActive = true);
        void SetBandEventRelation(Guid relationId, bool isActive = true);
        void SetBandEventRelation(Guid bandId, Guid eventId, bool isActive = true);
        void SetMemberType(Guid relationId, Guid memberTypeId);
        void SetUserAvatar(string filename, Guid userId);
        void SetBandAvatar(string filename, Guid bandId);
        void SetEventAvatar(string filename, Guid eventId);

        void SetBandEventLineup(Guid relationId, DateTime? showTime = null, int? order = null);
        
        void AddBandImage(Guid imageId, Guid bandId);
        void AddImage(ImageModel img, Guid userId);

        void AddAudio(AudioModel audio);

        bool UserIsMemberOfBand(Guid userId, Guid bandId, int relationTypeId = 1);
    }

    public class Result
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
