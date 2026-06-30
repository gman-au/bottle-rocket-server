using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MailgunConnector)))
            {
                BsonClassMap.RegisterClassMap<MailgunConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(MailgunSendEmailExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<MailgunSendEmailExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(MailgunSendEmailWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<MailgunSendEmailWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}