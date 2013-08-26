using AutoMapper;
using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Core.Helpers;
using OpenGrooves.Data;
using OpenGrooves.Services.Mapping;
using OpenGrooves.Web.Comparers;
using OpenGrooves.Web.Models;
using StructureMap;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Linq;

namespace OpenGrooves.Web.Proxies
{
    [Pluggable("DataRepository")]
    public class EFDataRepository : IRepository
    {
        // TODO: this shouldnt be in this layer. setting location should be in biz layer, when built
        [SetterProperty]
        public ILocationService location { get; set; }

        #region Private Methods
        private User GetCurrentUserModel(OpenGroovesEntities ctx, Guid loggedUserGuid)
        {
            var user = ctx.Users.Single(u => u.UserId == loggedUserGuid);

            return user;
        }

        private Result Result(bool valid = true, string message = "The operation completed without errors")
        {
            return new Result { IsValid = valid, Message = message };
        }

        private Location UpdateLocation(LocationAwareModel m, string city, string state, string zip, string address = null)
        {
            Location loc = null;

            if (m.Zip != (zip ?? "") || m.State != state || m.City != city || m.Address != address)
            {
                var search = Core.Helpers.LocationHelper.CityStateOrZip(city, state, zip);
                loc = location.GetLocation(search);
            }

            return loc;
        }

        public string GetUniqueUrl(string url, Guid guid)
        {
            return String.Format("{1}-{0}", url, guid);
        }

        // TODO: optimizet this? remove uneeded? ajax?
        
        #endregion

        #region Static Constructor (Automapper)
        static EFDataRepository()
        {
            Mapper.CreateMap<Genre, GenreModel>();
            Mapper.CreateMap<GenreModel, Genre>();
            Mapper.CreateMap<MemberType, MemberTypeRelation>();
            Mapper.CreateMap<FeedItem, FeedItemModel>();
            Mapper.CreateMap<BandsEvent, BandEventRelation>();
            Mapper.CreateMap<Image, ImageModel>();
            Mapper.CreateMap<Audio, AudioModel>();
            Mapper.CreateMap<AudioModel, Audio>();
            Mapper.CreateMap<Gallery, GalleryModel>();
            Mapper.CreateMap<UsersBand, UserBandRelation>();
            Mapper.CreateMap<UserSetting, Setting>();
            Mapper.CreateMap<Venue, VenueModel>();
            Mapper.CreateMap<MessageRecipient, MessageRecipientRelation>();
            
            Mapper.CreateMap<Message, MessageModel>()
                .ForMember(d => d.Recipients, o => o.MapFrom(s => s.MessageRecipients));

            Mapper.CreateMap<Event, EventModel>()
                .ForMember(d => d.Bands, o => o.MapFrom(s => s.BandsEvents.OrderBy(e => e.Event.Date)));

            Mapper.CreateMap<EventModel, Event>()
                .ForMember(d => d.UrlName, o => o.Ignore())
                .ForMember(d => d.AvatarUrl, o => o.Ignore())
                .ForMember(d => d.BandsEvents, o => o.Ignore())
                .ForMember(d => d.Users, o => o.Ignore())
                .ForMember(d => d.FeedItems, o => o.Ignore())
                .ForMember(d => d.Latitude, o => o.Ignore())
                .ForMember(d => d.Longitude, o => o.Ignore());

            Mapper.CreateMap<User, UserModel>()
                .ForMember(d => d.AllBands, o => o.Ignore());

            Mapper.CreateMap<UserModel, User>()
                .ForMember(d => d.UsersBands, o => o.Ignore())
                .ForMember(d => d.MessagesReceived, o => o.Ignore())
                .ForMember(d => d.MessagesSent, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.Events, o => o.Ignore())
                .ForMember(d => d.AvatarUrl, o => o.Ignore())
                .ForMember(d => d.Venues, o => o.Ignore())
                .ForMember(d => d.Galleries, o => o.Ignore())
                .ForMember(d => d.Settings, o => o.Ignore())
                .ForMember(d => d.Latitude, o => o.Ignore())
                .ForMember(d => d.Longitude, o => o.Ignore());

            Mapper.CreateMap<Band, BandModel>()
                .ForMember(d => d.AllEvents, o => o.MapFrom(s => s.BandsEvents.OrderBy(e => e.Event.Date)))
                .ForMember(d => d.ActiveEvents, o => o.MapFrom(s => s.BandsEvents.Where(e => e.IsActive).OrderBy(e => e.Event.Date)))
                .ForMember(d => d.PendingEvents, o => o.MapFrom(s => s.BandsEvents.Where(e => !e.IsActive).OrderBy(e => e.Event.Date)))
                .ForMember(d => d.AllUsers, o => o.Ignore())
                .ForMember(d => d.Fans, o => o.Ignore())
                .ForMember(d => d.PendingMemberRequests, o => o.Ignore())
                .ForMember(d => d.ActiveMembers, o => o.Ignore());

            Mapper.CreateMap<BandModel, Band>()
                .ForMember(d => d.Galleries, o => o.Ignore())
                .ForMember(d => d.BandsEvents, o => o.Ignore())
                .ForMember(d => d.UsersBands, o => o.Ignore())
                .ForMember(d => d.Genres, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.AvatarUrl, o => o.Ignore())
                .ForMember(d => d.Date, o => o.Ignore())
                .ForMember(d => d.Latitude, o => o.Ignore())
                .ForMember(d => d.Longitude, o => o.Ignore());
        } 
        #endregion

        public void Dispose() { }

        #region Feed
        public void AddFeedItem(string data, int feedTypeId, Guid bandId, Guid? userId = null, Guid? eventId = null, Guid? batchId = null)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var f = new FeedItem
                {
                    Content = data,
                    BandId = bandId,
                    Date = DateTime.Now,
                    FeedItemTypeId = feedTypeId,
                    FeedItemId = Guid.NewGuid(),
                    UserId = userId,
                    EventId = eventId,
                    BatchId = batchId
                };

                ctx.AddToFeedItems(f);
                ctx.SaveChanges();
            }
        }

        public void DeleteFeedItem(Guid feedItemId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var feedItem = (from a in ctx.FeedItems
                                where a.FeedItemId == feedItemId
                                select a).SingleOrDefault();

                if (feedItem != null)
                {
                    ctx.FeedItems.DeleteObject(feedItem);
                    ctx.SaveChanges();
                }
            }
        }

        public IEnumerable<FeedItemModel> GetFeedByUser(Guid userId, int max = 30)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var feed = ((from b in ctx.UsersBands
                             from f in b.Band.FeedItems
                             where b.UserId == userId && (b.RelationTypeId == 1 || b.RelationTypeId == 4) // member or fan
                             orderby f.Date descending
                             select f) as ObjectQuery<FeedItem>).Include("Band").Include("User").Include("Event");

                return Mapper.Map<IEnumerable<FeedItem>, IEnumerable<FeedItemModel>>(feed.Take(max * 2).ToList().Distinct(new FeedComparer()).Take(max));
            }
        }

        public IEnumerable<FeedItemModel> GetFeedByBand(Guid bandId, int max = 30)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var feed = (from f in ctx.FeedItems.Include("Band").Include("User").Include("Event")
                            where f.BandId == bandId
                            orderby f.Date descending
                            select f).Take(max).ToList().Distinct(new FeedComparer());

                return Mapper.Map<IEnumerable<FeedItem>, IEnumerable<FeedItemModel>>(feed);
            }
        }

        public FeedItemModel GetFeedItem(Guid feedItemId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var feed = ctx.FeedItems.SingleOrDefault(f => f.FeedItemId == feedItemId);

                return Mapper.Map<FeedItem, FeedItemModel>(feed);
            }
        }
        #endregion

        #region Events
        public IEnumerable<EventModel> GetUpcomingEventsByUser(Guid userId, int max = 20)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var events = ((from u in ctx.UsersBands
                               from e in u.Band.BandsEvents
                               where e.Event.Date >= DateTime.Now && u.IsActive && e.IsActive && u.UserId == userId
                               orderby e.Event.Date ascending
                               select e.Event) as ObjectQuery<Event>).Include("BandsEvents.Band").Take(max);

                return Mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events.Distinct());
            }
        }

        public IEnumerable<EventModel> GetEventsByUser(Guid userId, int max = 20)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var events = ctx.Users.Include("Events").SingleOrDefault(u => u.UserId == userId).Events.Take(max);
                return Mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events);
            }
        }

        public IEnumerable<BandEventRelation> GetEventsByBand(Guid bandId, int max = 20, bool upcomingOnly = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var events = ctx.BandsEvents.Include("Event.BandsEvents.Band").Include("Band").Where(r => r.BandId == bandId).Take(max);

                if (upcomingOnly)
                {
                    events = events.Where(e => e.Event.Date >= DateTime.Now);
                }

                return Mapper.Map<IEnumerable<BandsEvent>, IEnumerable<BandEventRelation>>(events);
            }
        }

        public string CreateEvent(EventModel ev, Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var user = ctx.Users.Single(u => u.UserId == userId);

                var efEvent = new Event
                {
                    EventId = Guid.NewGuid(),
                    Name = ev.Name,
                    Searchable = EntityNameHelper.StripUselessWords(ev.Name),
                    UrlName = ev.UrlName,
                    Date = DateTime.Now.Date,
                    Description = ev.Description,
                };

                efEvent.Users.Add(user);

                ctx.Events.AddObject(efEvent);
                ctx.SaveChanges();

                return ev.UrlName;
            }
        }

        public Guid? GetEventId(string eventUrl)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var ev = ctx.Events.SingleOrDefault(e => e.UrlName == eventUrl);

                if (ev != null)
                {
                    return ev.EventId;
                }

                return null;
            }
        }
        #endregion

        #region Images
        public IEnumerable<ImageModel> GetImagesByUser(string userName, bool onlyUncategorized = false)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var user = ctx.Users.SingleOrDefault(u => u.UserName == userName);
                return GetImagesByUser(user.UserId);
            }
        }

        public IEnumerable<ImageModel> GetImagesByUser(Guid userId, bool onlyUncategorized = false)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                IEnumerable<Image> images;

                if (onlyUncategorized)
                {
                    images = ctx.Images
                        .Include("User")
                        .Where(u => u.UserId == userId && u.GalleryId == null);
                }
                else
                {
                    images = ctx.Images
                        .Include("User")
                        .Include("Gallery")
                        .Where(u => u.UserId == userId);
                }

                return Mapper.Map<IEnumerable<Image>, IEnumerable<ImageModel>>(images);
            }
        }

        public IEnumerable<ImageModel> GetImagesByBand(string bandUrl, bool onlyUncategorized = false)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var band = ctx.Bands.SingleOrDefault(b => b.UrlName == bandUrl);
                return GetImagesByBand(band.BandId, onlyUncategorized);
            }
        }

        public IEnumerable<ImageModel> GetImagesByBand(Guid bandId, bool onlyUncategorized = false)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var band = ctx.Bands.Include("Images.User").Single(b => b.BandId == bandId);
                IEnumerable<Image> images;

                if (onlyUncategorized)
                {
                    images = band.Images.ToList().Where(i => i.GalleryId == null);
                }
                else
                {
                    images = band.Images;
                }

                //var images = ctx.Bands.Include("Images.User").Single(b => b.BandId == bandId).Images;
                return Mapper.Map<IEnumerable<Image>, IEnumerable<ImageModel>>(images);
            }
        }

        public string UpdateGallery(GalleryModel gallery)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                ctx.ContextOptions.LazyLoadingEnabled = true;
                var currGal = ctx.Galleries.SingleOrDefault(g => g.GalleryId == gallery.GalleryId);
                var galleryBand = ctx.Bands.SingleOrDefault(b => b.BandId == gallery.BandId);

                if (gallery.BandId != null && galleryBand != null)
                {
                    currGal.Images.ToList().ForEach(i =>
                    {
                        if (!i.Bands.Any(b => b.BandId == galleryBand.BandId))
                        {
                            i.Bands.Add(galleryBand);
                        }
                    });
                }
                else
                {
                    currGal.Images.ToList().ForEach(i =>
                    {
                        i.Bands.Clear();
                    });
                }

                var url = GetUniqueUrl(gallery.UrlName, gallery.GalleryId);

                currGal.Name = gallery.Name;
                currGal.UrlName = url;
                currGal.Description = gallery.Description;
                currGal.BandId = gallery.BandId;

                ctx.SaveChanges();

                return url;
            }
        }

        public IEnumerable<ImageModel> GetRecentImages(int max = 10)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var images = ((from b in ctx.Bands
                               from i in b.Images
                               orderby i.Date descending
                               select i).Take(max) as ObjectQuery<Image>).Include("Bands").Include("Gallery");

                return Mapper.Map<IEnumerable<Image>, IEnumerable<ImageModel>>(images);
            }
        }

        public IEnumerable<ImageModel> GetRecentBandImages(Guid userId, int max = 10, bool onlyUncategorized = false)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var images = ((from b in ctx.UsersBands
                               from i in b.Band.Images
                               where b.UserId == userId && (b.RelationTypeId == 1 || b.RelationTypeId == 4) && (onlyUncategorized ? i.GalleryId == null : true)
                               orderby i.Date descending
                               select i).Take(max) as ObjectQuery<Image>).Include("Bands").Include("Gallery");

                return Mapper.Map<IEnumerable<Image>, IEnumerable<ImageModel>>(images);
            }
        }

        public IEnumerable<GalleryModel> GetGalleriesByUser(Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var galleries = ctx.Galleries
                    .Include("Images")
                    .Include("Band")
                    .Where(g => g.UserId == userId);

                return Mapper.Map<IEnumerable<Gallery>, IEnumerable<GalleryModel>>(galleries);
            }
        }

        public IEnumerable<GalleryModel> GetGalleriesByBand(Guid bandId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var galleries = ctx.Galleries
                    .Include("Images")
                    .Where(g => g.BandId == bandId);

                return Mapper.Map<IEnumerable<Gallery>, IEnumerable<GalleryModel>>(galleries);
            }
        }

        public IEnumerable<GalleryModel> GetBandGalleriesByUser(Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var galleries = ((from ub in ctx.UsersBands
                                  join g in ctx.Galleries on ub.BandId equals g.BandId
                                  where ub.UserId == userId
                                  select g) as ObjectQuery<Gallery>).Include("Band").Include("Images");

                return Mapper.Map<IEnumerable<Gallery>, IEnumerable<GalleryModel>>(galleries);
            }
        }

        public void UpdateImageGallery(Guid imageId, Guid galleryId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                ctx.ContextOptions.LazyLoadingEnabled = true;
                var image = ctx.Images.SingleOrDefault(i => i.ImageId == imageId);
                var band = (from b in ctx.Bands
                            join g in ctx.Galleries on b.BandId equals g.BandId
                            where g.GalleryId == galleryId
                            select b).SingleOrDefault();

                image.GalleryId = galleryId;

                if (band != null && !image.Bands.Contains(band))
                {
                    image.Bands.Add(band);
                }

                ctx.SaveChanges();
            }
        }


        public void DeleteImageGallery(Guid imageId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var image = ctx.Images.Include("Bands").SingleOrDefault(i => i.ImageId == imageId);
                image.GalleryId = null;
                image.Bands.Clear();
                ctx.SaveChanges();
            }
        }

        public void DeleteGallery(Guid galleryId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var gallery = ctx.Galleries.Include("Images").SingleOrDefault(g => g.GalleryId == galleryId);

                ctx.Galleries.DeleteObject(gallery);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<ImageModel> GetImagesByBatch(Guid batchId, int max = 10)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var images = ctx.Images.Where(i => i.BatchID == batchId).Take(max);

                return Mapper.Map<IEnumerable<Image>, IEnumerable<ImageModel>>(images);
            }
        }

        public void AddImage(ImageModel img, Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                ctx.Images.AddObject(new Image
                {
                    ImageId = img.ImageId,
                    Url = img.Url,
                    Caption = img.Caption,
                    Date = DateTime.Now,
                    UserId = userId,
                    GalleryId = img.GalleryId,
                    BatchID = img.BatchId
                });

                ctx.SaveChanges();
            }
        }


        public void AddBandImage(Guid imageId, Guid bandId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var band = ctx.Bands.SingleOrDefault(b => b.BandId == bandId);
                var image = ctx.Images.SingleOrDefault(i => i.ImageId == imageId);

                if (band != null && image != null)
                {
                    band.Images.Add(image);
                    ctx.SaveChanges();
                }
            }
        }


        public void DeleteImage(Guid imageId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var image = ctx.Images
                    .Include("Bands")
                    .Single(i => i.ImageId == imageId);

                ctx.DeleteObject(image);

                ctx.SaveChanges();
            }
        }


        public ImageModel GetImage(Guid imageId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var image = ctx.Images
                    .Include("Bands")
                    .Single(i => i.ImageId == imageId);

                return Mapper.Map<Image, ImageModel>(image);
            }
        }


        public void SetUserAvatar(string filename, Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var user = ctx.Users.Single(u => u.UserId == userId);
                user.AvatarUrl = filename;
                ctx.SaveChanges();
            }
        }


        public void SetBandAvatar(string filename, Guid bandId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var band = ctx.Bands.Single(b => b.BandId == bandId);
                band.AvatarUrl = filename;
                ctx.SaveChanges();
            }
        }

        public void SetEventAvatar(string filename, Guid eventId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var ev = ctx.Events.Single(e => e.EventId == eventId);
                ev.AvatarUrl = filename;
                ctx.SaveChanges();
            }
        }

        public string CreateGallery(string name, string url, Guid userId, Guid? bandId = null)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var guid = Guid.NewGuid();
                var urlName = GetUniqueUrl(url, guid);

                var gallery = new Gallery
                {
                    GalleryId = guid,
                    Name = name,
                    UrlName = urlName,
                    UserId = userId,
                    BandId = bandId,
                    Date = DateTime.Now
                };

                ctx.AddToGalleries(gallery);
                ctx.SaveChanges();

                return urlName;
            }
        }

        public GalleryModel GetGallery(string galleryName)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var gallery = ctx.Galleries.Include("Images.User").Include("Band").Single(g => g.UrlName == galleryName);

                return Mapper.Map<Gallery, GalleryModel>(gallery);
            }
        }
        #endregion

        #region Audio
        public IEnumerable<AudioModel> GetAudioByBand(Guid bandId, int max = 20)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var audio = ctx.Audios.Include("Band").Where(a => a.BandID == bandId).Take(max);

                return Mapper.Map<IEnumerable<Audio>, IEnumerable<AudioModel>>(audio);
            }
        }

        public void AddAudio(AudioModel audio)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var efAudio = Mapper.Map<AudioModel, Audio>(audio);

                ctx.AddToAudios(efAudio);

                ctx.SaveChanges();
            }
        }

        public void DeleteAudio(Guid audioId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var audio = ctx.Audios.FirstOrDefault(a => a.AudioID == audioId);

                if (audio != null)
                {
                    ctx.Audios.DeleteObject(audio);
                }

                ctx.SaveChanges();
            }
        }
        #endregion

        #region User-Band Relations
        public IEnumerable<UserBandRelation> GetBandRelationsByUser(Guid userId, int relationType, int max = 10)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relations = ctx.UsersBands
                    .Include("Band.BandsEvents.Event")
                    .Include("Band.Genres")
                    .Where(r => r.UserId == userId && r.RelationTypeId == relationType)
                    .Take(max)
                    .OrderBy(r => r.Band.Name);

                return Mapper.Map<IEnumerable<UsersBand>, IEnumerable<UserBandRelation>>(relations);
            }
        }

        public IEnumerable<UserBandRelation> GetUserRelationsByBand(Guid bandId, int relationTypeId, int max = 10)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var users = ((from r in ctx.UsersBands
                              where r.BandId == bandId && r.RelationTypeId == relationTypeId
                              orderby r.User.UserName
                              select r) as ObjectQuery<UsersBand>).Include("User").Include("MemberType").Take(max);

                return Mapper.Map<IEnumerable<UsersBand>, IEnumerable<UserBandRelation>>(users);
            }
        }

        public UserBandRelation GetUserBandRelation(Guid relationId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.UsersBands.Include("User").Include("Band.UsersBands").Where(u => u.RelationId == relationId).SingleOrDefault();
                return Mapper.Map<UsersBand, UserBandRelation>(relation);
            }
        }

        public UserBandRelation GetUserBandRelation(Guid userId, Guid bandId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.UsersBands.Include("User").Include("Band").Where(u => u.UserId == userId && u.BandId == bandId).SingleOrDefault();
                return Mapper.Map<UsersBand, UserBandRelation>(relation);
            }
        }

        public void SetUserBandRelation(Guid relationId, int relationTypeId, bool isActive = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                // TODO: RAFACTOR
                var relation = ctx.UsersBands.SingleOrDefault(r => r.RelationId == relationId);
                SetUserBandRelation(relation.BandId, relation.UserId, relationTypeId, isActive);
            }
        }

        public void SetUserBandRelation(Guid bandId, Guid userId, int relationTypeId, bool isActive = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.UsersBands.Where(r => r.UserId == userId && r.BandId == bandId).FirstOrDefault();

                // if a relation already exists, update it
                if (relation != null)
                {
                    relation.RelationTypeId = relationTypeId;
                    relation.IsActive = isActive;
                }
                // if not, create one
                else
                {
                    ctx.UsersBands.AddObject(new UsersBand { RelationId = Guid.NewGuid(), UserId = userId, BandId = bandId, RelationTypeId = relationTypeId, IsActive = isActive, Date = DateTime.Now }); // 3 = friend
                }

                ctx.SaveChanges();
            }
        }

        public Result DeleteUserBandRelation(Guid relationId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.UsersBands
                    .Include("Band.FeedItems")
                    .Include("Band.UsersBands")
                    .Include("User")
                    .Where(r => r.RelationId == relationId)
                    .ToList()
                    .SingleOrDefault(); // hate doing this...

                if (relation != null)
                {
                    // if last member, delete the band
                    var numUsers = relation.Band.UsersBands.Count();
                    var numMembers = relation.Band.UsersBands.Where(r => r.IsActive && r.RelationTypeId == 2).Count();
                    if (numUsers == 1 && numMembers == 1)
                    {
                        DeleteBand(relation.BandId);
                    }
                    else
                    {
                        ctx.UsersBands.DeleteObject(relation);
                        ctx.SaveChanges();
                    }
                }

                return Result();
            }
        }

        public Result DeleteUserBandRelation(Guid bandId, Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.UsersBands.SingleOrDefault(r => r.BandId == bandId && r.UserId == userId);
                return DeleteUserBandRelation(relation.RelationId);
            }
        }

        public void SetBandEventLineup(Guid relationId, DateTime? showTime = null, int? order = null)
        {
            if (showTime == null && order == null) throw new ArgumentException("showTime and order cannot both be null.");

            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.BandsEvents.SingleOrDefault(r => r.RelationId == relationId);
                relation.ShowTime = showTime;
                relation.Order = order;
                ctx.SaveChanges();
            }
        }

        public bool UserIsMemberOfBand(Guid userId, Guid bandId, int relationTypeId = 1)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                return ctx.UsersBands.Any(b => b.BandId == bandId && b.UserId == userId && b.RelationTypeId == relationTypeId);
            }
        }
        #endregion

        #region Band-Event Relations
        public BandEventRelation GetBandEventRelation(Guid relationId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.BandsEvents.Include("Band").Include("Event").Where(b => b.RelationId == relationId).SingleOrDefault();
                return Mapper.Map<BandsEvent, BandEventRelation>(relation);
            }
        }

        public BandEventRelation GetBandEventRelation(Guid bandId, Guid eventId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.BandsEvents.Include("Band.UsersBands").Include("Event").Where(b => b.EventId == eventId && b.BandId == bandId).SingleOrDefault();
                return Mapper.Map<BandsEvent, BandEventRelation>(relation);
            }
        }

        public Result DeleteBandEventRelation(Guid relationId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.BandsEvents.Where(r => r.RelationId == relationId).Single();

                if (relation != null)
                {
                    ctx.BandsEvents.DeleteObject(relation);
                    ctx.SaveChanges();
                }

                return Result();
            }
        }

        public Result DeleteBandEventRelation(Guid bandId, Guid eventId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.BandsEvents.SingleOrDefault(r => r.BandId == bandId && r.EventId == eventId);
                return DeleteBandEventRelation(relation.BandId, relation.EventId);
            }
        }

        public void SetBandEventRelation(Guid relationId, bool isActive = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                // TODO: RAFACTOR
                var relation = ctx.BandsEvents.SingleOrDefault(r => r.RelationId == relationId);
                SetBandEventRelation(relation.BandId, relation.EventId, isActive);
            }
        }

        public void SetBandEventRelation(Guid bandId, Guid eventId, bool isActive = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.BandsEvents.Include("Event").Where(r => r.BandId == bandId && r.EventId == eventId).FirstOrDefault();

                // if a relation already exists, update it
                if (relation != null)
                {
                    relation.IsActive = isActive;
                    relation.ShowTime = relation.Event.Date;
                }
                // if not, create one
                else
                {
                    var eventDate = ctx.Events.Where(e => e.EventId == eventId).SingleOrDefault().Date;
                    ctx.BandsEvents.AddObject(new BandsEvent { RelationId = Guid.NewGuid(), EventId = eventId, BandId = bandId, IsActive = isActive, ShowTime = eventDate });
                }

                ctx.SaveChanges();
            }
        }
        #endregion

        #region Messages
        public IEnumerable<MessageModel> GetUserSentMessages(Guid userId, int max = 20)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var messages = ctx.Messages
                    .Include("Sender")
                    .Include("MessageRecipients.User")
                    .Where(m => m.SenderUserId == userId)
                    .Take(max);

                return Mapper.Map<IEnumerable<Message>, IEnumerable<MessageModel>>(messages);
            }
        }

        public IEnumerable<MessageRecipientRelation> GetUserReceivedMessages(Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var messages = from m in ctx.MessageRecipients.Include("Message.Sender").Include("User")
                               where m.UserId == userId
                               select m;

                return Mapper.Map<IEnumerable<MessageRecipient>, IEnumerable<MessageRecipientRelation>>(messages);
            }
        }

        public MessageModel GetMessage(Guid messageId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var message = ctx.Messages.Include("MessageRecipients.User").Include("Sender").SingleOrDefault(m => m.MessageId == messageId);
                return Mapper.Map<Message, MessageModel>(message);
            }
        }

        public void MarkMessageRead(Guid messageId, Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var mr = ctx.MessageRecipients.SingleOrDefault(m => m.MessageId == messageId && m.UserId == userId);

                if (mr != null)
                {
                    mr.IsRead = true;
                    ctx.SaveChanges();
                }
            }
        }


        public IEnumerable<UserModel> SendMessages(Guid fromUserId, IEnumerable<string> recipients, string content, Guid? toBandId = null)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var recips = ((from u in ctx.Users
                               join r in recipients on u.LoweredUserName equals r
                               select u) as ObjectQuery<User>).Include("Settings").ToList();

                var sender = ctx.Users.SingleOrDefault(u => u.UserId == fromUserId);

                var message = new Message
                {
                    MessageId = Guid.NewGuid(),
                    SenderUserId = fromUserId,
                    Content = content,
                    Date = DateTime.Now
                };

                recips.ForEach(u => message.MessageRecipients.Add(new MessageRecipient { UserId = u.UserId, MessageId = message.MessageId, IsRead = false }));

                ctx.Messages.AddObject(message);

                ctx.SaveChanges();

                return Mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(recips);
            }
        }

        public void DeleteMessage(Guid messageId, Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var message = ctx.MessageRecipients.SingleOrDefault(m => m.MessageId == messageId && m.UserId == userId);
                ctx.MessageRecipients.DeleteObject(message);
                ctx.SaveChanges();
            }
        }
        #endregion

        #region Bands

        public Guid? GetBandId(string bandUrl)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var band = ctx.Bands.SingleOrDefault(b => b.UrlName == bandUrl);

                if (band != null)
                {
                    return band.BandId;
                }

                return null;
            }
        }

        public BandModel GetBand(Guid bandId, bool loadExtended = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var name = ctx.Bands.SingleOrDefault(b => b.BandId == bandId);
                return GetBand(name.UrlName, loadExtended: loadExtended);
            }
        }

        public BandModel GetBand(string name, bool isUrl = true, bool loadExtended = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                Band band = null;

                if (loadExtended)
                {
                    band = (from b in ctx.Bands
                               .Include("Genres")
                               .Include("BandsEvents.Event")
                           where isUrl ? b.UrlName == name : b.Name == name
                           select b).SingleOrDefault();

                    if (band != null)
                    {
                        band.BandsEvents = band.BandsEvents.Where(e => e.Event.Date >= DateTime.Now) as EntityCollection<BandsEvent>;
                    }
                }
                else
                {
                    band = ctx.Bands.SingleOrDefault(b => isUrl ? b.UrlName == name : b.Name == name);
                }

                return Mapper.Map<Band, BandModel>(band);
            }
        }

        public IEnumerable<BandModel> GetBandsByGenre(string genre, int max = 20)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var bands = from b in ctx.Bands
                                .Include("BandsEvents.Event")
                                .Include("Genres")
                            where b.Genres.Where(g => g.Name == genre).Any()
                            select b;

                return Mapper.Map<IEnumerable<Band>, IEnumerable<BandModel>>(bands.Take(max));
            }
        }

        public IEnumerable<BandModel> GetNewestBands(int max = 20)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var bands = ctx.Bands.Include("Genres").Include("BandsEvents.Event").OrderByDescending(b => b.Date).Take(max);
                return Mapper.Map<IEnumerable<Band>, IEnumerable<BandModel>>(bands);
            }
        }

        public void UpdateBand(BandModel band)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var currBand = ctx.Bands.Single(b => b.BandId == band.BandId);

                // get new location if needed
                Location loc = null;
                if (currBand.Zip != (band.Zip ?? "") || currBand.State != band.State || currBand.City != band.City)
                {
                    var address = Core.Helpers.LocationHelper.CityStateOrZip(band.City, band.State, band.Zip);
                    loc = location.GetLocation(address);
                }

                // map data
                Mapper.Map(band, currBand);
                currBand.Searchable = EntityNameHelper.StripUselessWords(band.Name);

                // if location is not null, update entity directly, after mapping, since they're equal during mapping
                if (loc != null)
                {
                    currBand.Zip = loc.Zip;
                    currBand.City = loc.City;
                    currBand.State = loc.State;
                    currBand.Latitude = loc.Coordinate.Latitude;
                    currBand.Longitude = loc.Coordinate.Longitude;
                }

                ctx.SaveChanges();
            }
        }

        public string CreateBand(BandModel band, Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var efBand = new Band
                {
                    UrlName = band.UrlName,
                    Name = band.Name,
                    Searchable = EntityNameHelper.StripUselessWords(band.Name),
                    BandId = band.BandId,
                    Date = DateTime.Now
                };

                ctx.Users.Single(u => u.UserId == userId).UsersBands.Add(new UsersBand { Band = efBand, RelationTypeId = 1, RelationId = Guid.NewGuid(), Date = DateTime.Now, IsActive = true });
                ctx.SaveChanges();

                return band.UrlName;
            }
        }

        public void DeleteBand(Guid bandId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var band = ctx.Bands
                    .Include("UsersBands")
                    .Include("FeedItems")
                    .Include("BandsEvents")
                    .Include("Genres")
                    .Include("Galleries")
                    .Include("Images")
                    .Single(b => b.BandId == bandId);

                ctx.DeleteObject(band);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<BandModel> GetBandsByLetter(string s, int max = 20)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var bands = ctx.Bands
                    .Include("UsersBands")
                    .Include("BandsEvents.Event")
                    .Include("Genres")
                    .Where(b => b.Searchable.StartsWith(s))
                    .Take(max);

                return Mapper.Map<IEnumerable<Band>, IEnumerable<BandModel>>(bands);
            }
        }

        public int GetTotalFans(Guid bandId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                return ctx.UsersBands.Count(b => b.BandId == bandId && b.RelationTypeId == 4);
            }
        }
        #endregion

        #region Users
        public UserModel GetUser(Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                return GetUser(ctx, userId);
            }
        }

        public UserModel GetUser(string username)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var userId = ctx.Users.Single(u => u.UserName == username).UserId;
                return GetUser(ctx, userId);
            }
        }

        private UserModel GetUser(OpenGroovesEntities ctx, Guid userId)
        {
            using (ctx)
            {
                IQueryable<User> users;

                users = from u in ctx.Users
                        where u.UserId == userId
                        select u;

                return Mapper.Map<User, UserModel>(users.Single());
            }
        }

        public void UpdateUser(UserModel user)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var currUser = ctx.Users.Include("UsersBands.Band").Single(u => u.UserId == user.UserId);

                // WTF is this?
                //UpdateLocation(user, currUser.City, currUser.State, currUser.Zip);

                Location loc = null;
                if (currUser.Zip != (user.Zip ?? String.Empty) || currUser.State != user.State || currUser.City != user.City)
                {
                    var address = LocationHelper.CityStateOrZip(user.City, user.State, user.Zip);
                    loc = location.GetLocation(address);
                }

                Mapper.Map(user, currUser);

                if (loc != null)
                {
                    currUser.Zip = loc.Zip;
                    currUser.City = loc.City;
                    currUser.State = loc.State;
                    currUser.Latitude = loc.Coordinate.Latitude;
                    currUser.Longitude = loc.Coordinate.Longitude;

                    // if a user has bands with no address info, set this as the default
                    var bandsWithNoLocation = currUser.UsersBands.Where(u => u.Band.City == null && u.Band.State == null).Select(u => u.Band);

                    foreach (var b in bandsWithNoLocation)
                    {
                        b.City = loc.City;
                        b.State = loc.State;
                        b.Zip = loc.Zip;
                        b.Latitude = loc.Coordinate.Latitude;
                        b.Longitude = loc.Coordinate.Longitude;
                    }
                }

                ctx.SaveChanges();
            }
        }

        public Guid? GetUserId(string username)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var user = ctx.Users.SingleOrDefault(u => u.UserName == username);

                if (user != null)
                {
                    return user.UserId;
                }

                return null;
            }
        }
        #endregion

        #region Events
        public EventModel GetEvent(string name, bool isUrl = true, bool loadExtended = true)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                Event ev;

                if (loadExtended)
                {
                    ev = ctx.Events
                        .Include("Users")
                        .Include("BandsEvents.Band.BandsEvents.Event")
                        .Include("BandsEvents.Band.Genres")
                        .SingleOrDefault(e => isUrl ? e.UrlName == name : e.Name == name);
                }
                else
                {
                    ev = ctx.Events.Include("Users").SingleOrDefault(e => isUrl ? e.UrlName == name : e.Name == name);
                }

                return Mapper.Map<Event, EventModel>(ev);
            }
        }

        public IEnumerable<BandEventRelation> GetBandRelationsByEvent(Guid eventId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var events = ctx.BandsEvents.Include("Band.BandsEvents.Event").Include("Band.Genres").Include("Event").Where(e => e.EventId == eventId);

                return Mapper.Map<IEnumerable<BandsEvent>, IEnumerable<BandEventRelation>>(events);
            }
        }

        public void DeleteEvent(Guid eventId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var ev = ctx.Events
                    .Include("FeedItems")
                    .Include("BandsEvents")
                    .Include("Users")
                    .Single(e => e.EventId == eventId);

                ctx.DeleteObject(ev);
                ctx.SaveChanges();
            }
        }

        public void UpdateEvent(EventModel ev)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var currEvent = ctx.Events.SingleOrDefault(e => e.EventId == ev.EventId);

                Location loc = null;
                if (currEvent.Zip != (ev.Zip ?? "") || currEvent.State != ev.State || currEvent.City != ev.City || currEvent.Address != ev.Address)
                {
                    var address = Core.Helpers.LocationHelper.CityStateOrZip(ev.City, ev.State, ev.Zip);
                    loc = location.GetLocation(ev.Address + " " + address);
                }

                Mapper.Map(ev, currEvent);
                currEvent.Searchable = EntityNameHelper.StripUselessWords(ev.Name);

                if (loc != null)
                {
                    currEvent.Zip = loc.Zip;
                    currEvent.City = loc.City;
                    currEvent.State = loc.State;
                    currEvent.Latitude = loc.Coordinate.Latitude;
                    currEvent.Longitude = loc.Coordinate.Longitude;
                }

                ctx.SaveChanges();
            }
        }
        #endregion

        #region Location/Browse
        public Location GetUserLocation(Guid userId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var user = ctx.Users.SingleOrDefault(u => u.UserId == userId);

                if (user != null && user.Latitude != 0 && user.Longitude != 0)
                {
                    return new Location
                    {
                        Zip = user.Zip,
                        State = user.State,
                        City = user.City,
                        Coordinate = new Coordinate
                        {
                            Latitude = (float)user.Latitude,
                            Longitude = (float)user.Longitude
                        }
                    };
                }

                return default(Location);
            }
        }

        public IEnumerable<BandModel> GetBandsByLocation(Coordinate coord, int radius, int max = 20, Guid? excludeByUserId = null, SearchSort? sort = null)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var bands = ctx.Bands
                    .Where(b => b.Latitude != null && b.Longitude != null)
                    .Select(b => new { Band = b, Radius = (b.Longitude == coord.Longitude && b.Latitude == coord.Latitude) ? 0 : (3959 * SqlFunctions.Acos(SqlFunctions.Cos(SqlFunctions.Radians(coord.Latitude)) * SqlFunctions.Cos(SqlFunctions.Radians(b.Latitude)) * SqlFunctions.Cos(SqlFunctions.Radians(b.Longitude) - SqlFunctions.Radians(coord.Longitude)) + SqlFunctions.Sin(SqlFunctions.Radians(coord.Latitude)) * SqlFunctions.Sin(SqlFunctions.Radians(b.Latitude)))) })
                    .Where(b => b.Radius <= radius);

                if (sort == SearchSort.Name) bands = bands.OrderBy(b => b.Band.Name);
                else if (sort == SearchSort.Newest) bands = bands.OrderByDescending(b => b.Band.Date);
                else bands = bands.OrderBy(b => b.Radius);

                if (excludeByUserId != null)
                {
                    var userId = (Guid)excludeByUserId;
                    var userBands = ctx.UsersBands.Where(u => u.UserId == userId);

                    bands = bands.Where(b => !userBands.Select(u => u.BandId).Contains(b.Band.BandId));
                }

                return Mapper.Map<IEnumerable<Band>, IEnumerable<BandModel>>((bands.Take(max).Select(e => e.Band) as ObjectQuery<Band>).Include("BandsEvents.Event").Include("Genres"));
            }
        }

        public IEnumerable<EventModel> GetEventsByLocation(Coordinate coord, int radius, int max = 20, SearchSort? sort = null)
        {
            if (coord == null) throw new ArgumentNullException("coord");

            using (var ctx = new OpenGroovesEntities())
            {
                var events = ctx.Events
                    .Where(b => b.Latitude != null && b.Longitude != null && b.Date >= DateTime.Now)
                    .Select(b => new { Event = b, Radius = (b.Longitude == coord.Longitude && b.Latitude == coord.Latitude) ? 0 : (3959 * SqlFunctions.Acos(SqlFunctions.Cos(SqlFunctions.Radians(coord.Latitude)) * SqlFunctions.Cos(SqlFunctions.Radians(b.Latitude)) * SqlFunctions.Cos(SqlFunctions.Radians(b.Longitude) - SqlFunctions.Radians(coord.Longitude)) + SqlFunctions.Sin(SqlFunctions.Radians(coord.Latitude)) * SqlFunctions.Sin(SqlFunctions.Radians(b.Latitude)))) })
                    .Where(b => b.Radius <= radius);

                if (sort == SearchSort.Name) events = events.OrderBy(b => b.Event.Name);
                else if (sort == SearchSort.Newest) events = events.OrderByDescending(b => b.Event.Date);
                else events = events.OrderBy(b => b.Radius);

                return Mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>((events.Take(max).Select(e => e.Event) as ObjectQuery<Event>).Include("BandsEvents.Band"));
            }
        }

        public IEnumerable<UserModel> FindUsersStartingWith(string query)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var users = ctx.Users
                    .Where(u => u.UserName.StartsWith(query) || u.RealName.StartsWith(query))
                    .OrderBy(u => u.UserName)
                    .Take(30);

                return Mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(users);
            }
        }

        public IEnumerable<BandModel> FindBandsStartingWith(string query)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var bands = ctx.Bands
                    .Where(b => b.Name.StartsWith(query) || b.Searchable.StartsWith(query))
                    .OrderBy(b => b.Name)
                    .Take(30);

                return Mapper.Map<IEnumerable<Band>, IEnumerable<BandModel>>(bands);
            }
        }

        public IEnumerable<EventModel> FindEventsStartingWith(string query)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var events = ctx.Events
                    .Where(e => e.Name.StartsWith(query) && e.Date >= DateTime.Now)
                    .OrderBy(e => e.Name)
                    .Take(30);

                return Mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events);
            }
        }

        public IEnumerable<BandModel> SearchBands(string query, int max = 50)
        {
            var bands = new List<BandModel>();

            using (var ctx = new OpenGroovesEntities())
            {
                var connStr = (ctx.Connection as System.Data.EntityClient.EntityConnection).StoreConnection.ConnectionString;

                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.CommandText = "spSearchBands";
                        cmd.Parameters.Add(new SqlParameter("@Query", query));
                        cmd.Parameters.Add(new SqlParameter("@Max", max));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bands.Add(new BandModel
                                {
                                    Name = reader.GetValue<string>("Name"),
                                    BandId = reader.GetValue<Guid>("BandId"),
                                    UrlName = reader.GetValue<string>("UrlName"),
                                    Description = reader.GetValue<string>("Description"),
                                    AvatarUrl = reader.GetValue<string>("AvatarUrl"),
                                    City = reader.GetValue<string>("City"),
                                    State = reader.GetValue<string>("State")
                                });

                            }

                            return bands;
                        }
                    }
                }
            }
        }
        #endregion

        #region Genres
        public IEnumerable<GenreModel> GetGenres()
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var genres = from g in ctx.Genres
                             orderby g.Name ascending
                             select g;

                return Mapper.Map<IEnumerable<Genre>, IEnumerable<GenreModel>>(genres);
            }
        }

        public GenreModel GetGenre(string genre)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var gen = ctx.Genres.Include("Bands").SingleOrDefault(g => g.Name == genre);
                return Mapper.Map<Genre, GenreModel>(gen);
            }
        }

        public void UpdateBandGenres(Guid bandId, IEnumerable<Guid> genres)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var newGenres = from g in ctx.Genres
                                join t in genres on g.GenreId equals t
                                select g;

                //var bandGenres = ctx.Genres.Include("Bands").Where(g => g.Bands.Any(b => b.BandId == bandId));
                var band = ctx.Bands.Include("Genres").SingleOrDefault(b => b.BandId == bandId);

                band.Genres.Clear();
                newGenres.ToList().ForEach(g => band.Genres.Add(g));

                ctx.SaveChanges();
            }
        }
        #endregion

        #region Settings
        public IEnumerable<Setting> GetUserSettings(params Guid[] userIds)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var settings = from s in ctx.UserSettings
                               join u in userIds on s.UserId equals u
                               select s;

                return Mapper.Map<IEnumerable<UserSetting>, IEnumerable<Setting>>(settings);
            }
        }
        #endregion

        #region Member Types
        public void SetMemberType(Guid relationId, Guid memberTypeId)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var relation = ctx.UsersBands.Single(r => r.RelationId == relationId);
                relation.MemberTypeId = memberTypeId;

                ctx.SaveChanges();
            }
        }

        public IEnumerable<MemberTypeRelation> GetMemberTypes()
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var memberTypes = ctx.MemberTypes.OrderBy(m => m.Name);
                return Mapper.Map<IEnumerable<MemberType>, IEnumerable<MemberTypeRelation>>(memberTypes);
            }
        } 
        #endregion
    }
}