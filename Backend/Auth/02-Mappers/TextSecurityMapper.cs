using System.Diagnostics;
using Auth.Grpc;
using Auth.Other;

namespace Auth.Mappers;

public class TextSecurityMapper {
    public Other.AccessType ToLocalAccessType(Grpc.AccessType grpcAccessType) {
        return grpcAccessType switch {
            Grpc.AccessType.Personal => Other.AccessType.Personal,
            Grpc.AccessType.OnlyFriends => Other.AccessType.OnlyFriends,
            Grpc.AccessType.ByReferencePublic => Other.AccessType.ByReferencePublic,
            Grpc.AccessType.ByReferenceAuthorized => Other.AccessType.ByReferenceAuthorized,
            _ => throw new ArgumentException("Unknown access type")
        };
    }

    public Grpc.AccessType ToGrpcAccessType(Other.AccessType localAccessType) {
        return localAccessType switch {
            Other.AccessType.Personal => Grpc.AccessType.Personal,
            Other.AccessType.OnlyFriends => Grpc.AccessType.OnlyFriends,
            Other.AccessType.ByReferencePublic => Grpc.AccessType.ByReferencePublic,
            Other.AccessType.ByReferenceAuthorized => Grpc.AccessType.ByReferenceAuthorized,
            _ => throw new ArgumentException("Unknown access type")
        };
    }

    public TextSecurityProjection ToTextSecurityProjection(TextLightWeightModel text) {
        return new TextSecurityProjection {
            AccessType = ToLocalAccessType(text.SecuritySettings.AccessType),
            OwnerId = text.OwnerId
        };
    }

    public PasswordHash ToPasswordHash(string hash) {
        return new PasswordHash {
            Hash = hash
        };
    }
}