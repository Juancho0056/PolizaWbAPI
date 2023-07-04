using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace PolizaWebAPI.Infrastructure.Identity;
[CollectionName("roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{

}