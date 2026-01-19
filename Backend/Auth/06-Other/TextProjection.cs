
using Auth.Other;

namespace Auth.Projections;

public class TextSecurityCheckProjection {
    AccessType AccessType {get; set;}
    string OwnerId {get; set;}
}