using Microsoft.Azure.Mobile.Server;

namespace bTankService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Nickname { get; set; }

        public string Time { get; set; }
    }
}