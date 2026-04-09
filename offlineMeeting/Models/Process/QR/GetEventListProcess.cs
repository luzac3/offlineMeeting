using offlineMeeting.Models.Entity.QR;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.QR
{
    public class GetEventListProcess
    {
        private readonly IPleasanterRepository _repository;

        public GetEventListProcess(IPleasanterRepository pleasanterRepository)
        {
            _repository = pleasanterRepository;
        }

        public List<EventListEntity> Get(long siteId)
        {
            List<TablesEntity> records = _repository.Select(siteId, null, true);

            return records
                .Where(r => !string.IsNullOrEmpty(r.ClassA))
                .Select(r => new EventListEntity(
                    eventId: r.ReferenceId,
                    eventName: r.ClassA!
                ))
                .ToList();
        }
    }
}
