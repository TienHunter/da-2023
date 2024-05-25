using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Constants;
using ComputerManagement.Data;
using ComputerManagerment.Repos.Implement;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Queue
{
    public class SubcribeCommandOption : SubcriberBase
    {
        private readonly AppDbContext _dbContext;
        public SubcribeCommandOption(AppDbContext dbContext)
        {
            _queueName = QueueKey.COMMAND_OPTION;
            _dbContext = dbContext;
        }

        public override void DeQueue(object model, BasicDeliverEventArgs ea)
        {
            
            var newTask = new Task(async () =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    // Deserialize JSON string to object
                    var obj = JsonConvert.DeserializeObject<MessageQueue>(message);
                    var commandOption = JsonConvert.DeserializeObject<CommandOption>(obj.Message);
                    switch (obj.ActionType)
                    {
                        case QueueKey.UPSERT_COMMAND_OPTON_DOWLOAD_SOFTWARE:
                            this.UpserttCommandOption(commandOption);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // log error
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }

            });
            newTask.Start();

            //newTask.ContinueWith((completedTask) =>
            //{
            //    _channel.BasicAck(ea.DeliveryTag, false);
            //});
        }

        private async Task UpserttCommandOption(CommandOption commandOption)
        {
            var commandOptionRepo = new CommandOptionRepo(_dbContext);
            var commandExist = await commandOptionRepo.GetQueryable().Where(cm => cm.SourceId == commandOption.SourceId && cm.DesId == commandOption.DesId && cm.CommandKey == commandOption.CommandKey).FirstOrDefaultAsync();
            if(commandExist != null)
            {
                // update
                commandExist.CommandValue = commandOption.CommandValue;
                commandExist.UpdatedAt = DateTime.Now;

                await commandOptionRepo.UpdateAsync(commandExist);
                //_dbContext.CommandOption.Update(commandExist);
                //await _dbContext.SaveChangesAsync();
            }else
            {
                commandOption.Id = Guid.NewGuid();
                commandOption.CreatedAt  = DateTime.Now;
                commandOption.UpdatedAt = DateTime.Now;

                await commandOptionRepo.AddAsync(commandOption);

                //_dbContext.CommandOption.Add(commandOption);
                //await _dbContext.SaveChangesAsync();
            }
        }
    }
}
