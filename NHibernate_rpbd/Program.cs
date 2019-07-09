using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using NHibernate_rpbd.Dialog;

namespace NHibernate_rpbd
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter a date (mm/dd/yyyy format): ");
            //DateTime? userDateTime;
            //while (!Nullable<DateTime>.TryParse(Console.ReadLine(), out userDateTime))
            //{
            //    Console.WriteLine("You have entered an incorrect value.");
            //}
            ConsoleDialog diag = new ConsoleDialog();
            diag.MainDialog();
            //using (var session = NHibernateHelper.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        var lala = new Test { Name = "lala" };

            //        session.SaveOrUpdate(lala);

            //        var size = new Size_info { Size = 1337 };
            //        var invent = new Invent_info { Invent_type = "1337" };
            //        var kit = new Complete_info { Invent = invent, Size = size };
            //        //session.SaveOrUpdate(size);
            //        //session.SaveOrUpdate(kit);

            //        var journal = new Journal { Complete_kit = kit, Date_check_in = null, Date_check_out = new System.DateTime(2018, 5, 15) };
            //        session.SaveOrUpdate(journal);
            //        try
            //        {
            //            var hqlQuery = session.CreateQuery("from Journal as jr join jr.Complete_kit as kit join kit.Invent as inv where inv.Invent_type = '1337'").List();
            //            //hqlQuery[0].
            //            var res = hqlQuery.ToString();
            //            Console.WriteLine(res);
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e);
            //        }
            //        transaction.Commit();
            //    }
            //}
        }

        
    }
}
