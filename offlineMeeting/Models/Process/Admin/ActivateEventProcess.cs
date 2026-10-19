using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Share;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.APIBridge.Contracts;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;
using System.Net;

namespace offlineMeeting.Models.Process.Admin
{
    public class ActivateEventProcess
    {
        private readonly IWebHostEnvironment _env;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _pleasanterRepository;
        private readonly long _eventId;

        public ActivateEventProcess(
            IWebHostEnvironment env,
            IPleasanterApiBridge bridge,
            IPleasanterRepository pleasanterRepository,
            long eventId
        )
        {
            _env = env;
            _bridge = bridge;
            _pleasanterRepository = pleasanterRepository;
            _eventId = eventId;
        }

        public async Task<string> Activate()
        {
            string sessionId = Guid.NewGuid().ToString("N");

            foreach (ImageTableDefinition definition in GetImageTableDefinitions())
            {
                List<TablesEntity> results = _pleasanterRepository.Select(definition.SiteId, definition.Where, true);

                foreach (TablesEntity result in results)
                {
                    List<string>? images = definition.GetImages(result);
                    if (images == null)
                    {
                        continue;
                    }

                    for (int index = 0; index < images.Count; index++)
                    {
                        string base64 = images[index];
                        if (string.IsNullOrEmpty(base64))
                        {
                            continue;
                        }

                        try
                        {
                            EventImageResolver.SaveImage(_env, sessionId, definition.SiteId, result.ReferenceId, index, base64);
                        }
                        catch
                        {
                            // fixme 不審なBase64が握りつぶされてるので修正
                        }
                    }
                }
            }

            Dictionary<string, object> updateData = new Dictionary<string, object>
            {
                { "ClassB", sessionId },
                { "CheckB", true }
            };

            PleasanterApiResponse response = await _bridge.Update(
                _eventId,
                updateData
            );

            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                // 更新に失敗した場合、展開済みフォルダを削除してロールバック
                EventImageResolver.DeleteSessionFolder(_env, sessionId);
                throw new Exception("イベントの有効化に失敗しました");
            }

            return sessionId;
        }

        private List<ImageTableDefinition> GetImageTableDefinitions()
        {
            TableIdProperty tableIdProperty = new();
            string eventIdStr = _eventId.ToString();

            return new List<ImageTableDefinition>
            {
                new ImageTableDefinition(
                    siteId: tableIdProperty.SakeSiteId,
                    where: new Dictionary<string, object> { { "ClassH", new string[] { eventIdStr } } },
                    getImages: result => result.DescriptionG
                ),
                new ImageTableDefinition(
                    siteId: tableIdProperty.QuizSiteId,
                    where: new Dictionary<string, object> { { "ClassA", eventIdStr } },
                    getImages: result => result.DescriptionC
                ),
                new ImageTableDefinition(
                    siteId: tableIdProperty.ExtraSiteId,
                    where: new Dictionary<string, object> { { "ClassD", new string[] { eventIdStr } } },
                    getImages: result => result.Body
                ),
                new ImageTableDefinition(
                    siteId: tableIdProperty.EventFoodSiteId,
                    where: new Dictionary<string, object> { { "ClassD", new string[] { eventIdStr } } },
                    getImages: result => result.Body
                ),
                new ImageTableDefinition(
                    siteId: tableIdProperty.RawFoodSiteId,
                    where: new Dictionary<string, object> { { "ClassD", new string[] { eventIdStr } } },
                    getImages: result => result.Body
                ),
            };
        }

        private class ImageTableDefinition
        {
            public long SiteId { get; }
            public Dictionary<string, object> Where { get; }
            public Func<TablesEntity, List<string>?> GetImages { get; }

            public ImageTableDefinition(
                long siteId,
                Dictionary<string, object> where,
                Func<TablesEntity, List<string>?> getImages
            )
            {
                SiteId = siteId;
                Where = where;
                GetImages = getImages;
            }
        }
    }
}
