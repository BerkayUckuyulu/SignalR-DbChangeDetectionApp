using System;
using DCDServer.Hubs;
using DCDServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TableDependency.SqlClient;

namespace DCDServer.Subscription
{
    public interface IDatabaseSubscription
    {
        void Configure(string tableName);
    }



    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class, new()
    {

        readonly IConfiguration configuration;
        readonly IHubContext<SalesHub> _hubcontext;

        SqlTableDependency<T> _tableDependency;

        public DatabaseSubscription(IConfiguration configuration, IHubContext<SalesHub> hubcontext)
        {
            this.configuration = configuration;
            _hubcontext = hubcontext;
        }


        public void Configure(string tableName)
        {
            _tableDependency = new SqlTableDependency<T>(configuration.GetConnectionString("SqlServer"), tableName);
            _tableDependency.OnChanged += async (o, e) =>
            {
                DcddbContext context = new();

                 var datas = context.Employees.GroupJoin(context.Sales, e => e.Id, s => s.EmployeeId, (e, s) => new { employee = e, sales = s })
                .Select(join1 => new
                {
                    name = join1.employee.Name,
                    type= "line",
                    data = join1.sales.Select(s=>s.Price).ToList()
                }).ToList();


                await _hubcontext.Clients.All.SendAsync("receiveMessage", datas);
            };
            _tableDependency.OnError += (o, e) => { };

            _tableDependency.Start();

        }

        ~DatabaseSubscription()
        {
            _tableDependency.Stop();
        }
    }
}

