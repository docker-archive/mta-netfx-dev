using AutoMapper;
using NATS.Client;
using SignUp.Messaging;
using SignUp.Messaging.Messages.Events;
using SignUp.Model;
using System;
using System.Linq;
using System.Threading;

namespace SignUp.MessageHandlers.SaveProspect
{
    class Program
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);

        private const string QUEUE_GROUP = "save-handler";

        static void Main(string[] args)
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Entities.Prospect, Model.Prospect>()
                   .ForMember(dest => dest.Country, opt => opt.Ignore())
                   .ForMember(dest => dest.Role, opt => opt.Ignore());
            });

            Console.WriteLine($"Connecting to message queue url: {Messaging.Config.MessageQueueUrl}");
            using (var connection = MessageQueue.CreateConnection())
            {
                var subscription = connection.SubscribeAsync(ProspectSignedUpEvent.MessageSubject, QUEUE_GROUP);
                subscription.MessageHandler += SaveProspect;
                subscription.Start();
                Console.WriteLine($"Listening on subject: {ProspectSignedUpEvent.MessageSubject}, queue: {QUEUE_GROUP}");

                _ResetEvent.WaitOne();
                connection.Close();
            }
        }

        private static void SaveProspect(object sender, MsgHandlerEventArgs e)
        {
            Console.WriteLine($"Received message, subject: {e.Message.Subject}");
            var eventMessage = MessageHelper.FromData<ProspectSignedUpEvent>(e.Message.Data);

            Console.WriteLine($"Saving new prospect, signed up at: {eventMessage.SignedUpAt}; event ID: {eventMessage.CorrelationId}");
            try
            {
                var prospect = Mapper.Map<Model.Prospect>(eventMessage.Prospect);
                using (var context = new SignUpDbEntities())
                {
                    //reload child objects:
                    prospect.Country = context.Countries.First(x => x.CountryCode == eventMessage.Prospect.Country.CountryCode);
                    prospect.Role = context.Roles.First(x => x.RoleCode == eventMessage.Prospect.Role.RoleCode);
                    
                    context.AddToProspects(prospect);
                    context.SaveChanges();
                }
                Console.WriteLine($"Prospect saved. Prospect ID: {prospect.ProspectId}; event ID: {eventMessage.CorrelationId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save FAILED! Event ID: {eventMessage.CorrelationId}, ex: {ex}");
            }
        }
    }
}