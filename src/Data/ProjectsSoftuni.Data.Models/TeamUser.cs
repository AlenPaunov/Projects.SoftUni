namespace ProjectsSoftuni.Data.Models
{
    using ProjectsSoftuni.Data.Common.Models;

    public class TeamUser
    {
        public string TeamId { get; set; }

        public virtual Team Team { get; set; }

        public string UserId { get; set; }

        public virtual ProjectsSoftuniUser User { get; set; }

        public int TeamUserStatusId { get; set; }

        public virtual TeamUserStatus TeamUserStatus { get; set; }
    }
}
