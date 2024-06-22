using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    public class Device : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual required string HwId { get; set; }
        public virtual required string DisplayName { get; set; }
        public virtual required string Os { get; set; }

        public ICollection<DeviceOwner> Owners { get; } = [];
        public ICollection<DeviceConnection> Connections1 { get; } = [];
        public ICollection<DeviceConnection> Connections2 { get; } = [];
        public ICollection<TransientCertificate> TransientRefererCertificates { get; } = [];
        public ICollection<TransientCertificate> TransientClientCertificates { get; } = [];
        public ICollection<PeerContract> PeerContracts { get; } = [];

        public DateTime? DeletedOn { get; set; }
    }

    public class  DeviceOwner : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DeviceId { get; set; }
        public required Device Device { get; set; }

        public int OwnerId { get; set; }
        public required UserAccount Owner { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

    public class DeviceConnection : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DeviceId1 { get; set; }
        public int DeviceId2 { get; set; }

        //Connected Devices
        public Device Device1 { get; set; } = null!;
        public Device Device2 { get; set; } = null!;

        //Extras
        public int AssignedById {  get; set; }
        public UserAccount AssignedBy { get; set; } = null!;

        public DateTime? DeletedOn { get; set; }
    }

    public class TransientCertificate : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int RefererId { get; set; }
        public required Device Referer { get; set; }
        public required int ClientId { get; set; }
        public required Device Client { get; set; }
        //Matches PeerContract.Token on both peers
        public required Guid SharedPeerToken { get; set; }
        public required DateTime Issued { get; set; }
        public required DateTime Expiration { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

    public class PeerContract : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int AuthorizerId { get; set; }
        public required UserAccount Authorizer { get; set; }
        public required int PeerId { get; set; }
        public required Device Peer { get; set; }
        public required Guid Token { get; set; }
        public required DateTime Issued { get; set; }
        public required DateTime Expiration { get; set; }
        public required bool AllowWrite { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

}
