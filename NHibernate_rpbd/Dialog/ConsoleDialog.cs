using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;

namespace NHibernate_rpbd.Dialog
{
    class ConsoleDialog
    {
        private int inputNum;
        private string inputStr;

        private int GetUserOption()
        {
            inputStr = Console.ReadLine();
            Int32.TryParse(inputStr, out var num);
            return num;
        }

        public void MainDialog()
        {
            //infinite run cycle
            while (true)
            {
                Console.WriteLine("Enter one of the options.");
                Console.WriteLine("1) Work with equipment.");
                Console.WriteLine("2) Work with the journal.");
                Console.WriteLine("9) Wipe all data.");
                Console.WriteLine("0) Exit.");
                inputNum = GetUserOption();

                switch (inputNum)
                {
                    case 1: EquipDialog();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 2: JournalDialog();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 9: WipeData();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 0: return;
                    default: Console.WriteLine("Enter correct option, please.");
                        continue;
                }
                Console.Clear();
            }
        }

        private void WipeData()
        {
            try
            {
                FluentNHibernate.Cfg.Fluently.Configure()
                    .Database(
                        FluentNHibernate.Cfg.Db.PostgreSQLConfiguration.Standard
                            .ConnectionString(c => c.Host("127.0.0.1")
                                .Port(5432)
                                .Database("RPBD_1")
                                .Username("postgres")
                                .Password("password"))
                    )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>()
                        .Conventions.Add(FluentNHibernate.Conventions.Helpers.Table.Is(x => x.TableName.ToLower())))
                    .ExposeConfiguration(
                        cfg => new NHibernate.Tool.hbm2ddl.SchemaExport(cfg).Execute(false, true, true))
                    .BuildConfiguration().BuildSessionFactory();

                Console.WriteLine("Data wiped successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("smth went wrong");
            }

        }

        private void EquipDialog()
        {
            while (true)
            {
                Console.WriteLine("Enter one of the options.");
                Console.WriteLine("1) Print all equipment.");
                Console.WriteLine("2) Add equipment.");
                Console.WriteLine("3) Alter equipment.");
                Console.WriteLine("4) Delete equipment.");
                Console.WriteLine("5) Search by inventory type.");
                Console.WriteLine("6) Search by size.");
                Console.WriteLine("9) Get sorted equipment.");
                Console.WriteLine("0) Go back.");

                inputNum = GetUserOption();

                switch (inputNum)
                {
                    case 1:
                        PrintEquip();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 2:
                        AddEquip();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 3:
                        AlterEquip();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 4:
                        DeleteEquip();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 5:
                        SearchEquipByInvent();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 6:
                        SearchEquipBySize();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 9: GetSortedEquipment();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                    case 0: return;
                    default:
                        Console.WriteLine("Enter correct option, please.");
                        continue;

                }

                Console.Clear();
            }
        }

        private void PrintEquip()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Console.WriteLine("Id\tInventory type\tSize");
                    var equipList = session.QueryOver<Complete_info>().List();
                    foreach (var equipPiece in equipList)
                    {
                        Console.WriteLine(equipPiece.Id.ToString() + "\t" + equipPiece.Invent.Invent_type + "\t\t" + equipPiece.Size.Size);
                    }
                }
            }
        }

        private void AddEquip()
        {
            //string "Key (size)=(1337) already exists."ж
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Enter the inventory type.");
                        inputStr = Console.ReadLine();
                        var query = session.CreateQuery("from Invent_info as invent where invent.Invent_type = \'" + inputStr + '\'');
                        var invent = (Invent_info)query.UniqueResult();
                        if (invent == null)
                        {
                            Console.WriteLine("Adding new inventory type.");
                            invent = new Invent_info {Invent_type = inputStr};
                            session.SaveOrUpdate(invent);
                        }

                        Console.WriteLine("Enter the size.");
                        inputNum = GetUserOption();
                        var size = (Size_info)session.CreateQuery("from Size_info as size where size.Size = " + inputNum).UniqueResult();
                        if (size == null)
                        {
                            Console.WriteLine("Adding new size.");
                            size = new Size_info {Size = inputNum};
                            session.SaveOrUpdate(invent);
                        }

                        var completeKit = new Complete_info {Invent = invent, Size = size};
                        session.SaveOrUpdate(invent);
                        session.SaveOrUpdate(size);
                        session.SaveOrUpdate(completeKit);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        //e.InnerException.Message.Contains("key");
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void AlterEquip()
        {
            PrintEquip();
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Enter the kit id to alter.");
                        inputNum = GetUserOption();
                        var query = session.CreateQuery("from Complete_info as kit where kit.Id = " + inputNum);
                        var kit = (Complete_info) query.UniqueResult();

                        if (kit == null)
                        {
                            Console.WriteLine("No such inventory kit.");
                            return;
                        }

                        Console.WriteLine("Enter the inventory type.");
                        inputStr = Console.ReadLine();
                        query = session.CreateQuery("from Invent_info as invent where invent.Invent_type = \'" + inputStr + '\'');
                        var invent = (Invent_info)query.UniqueResult();
                        if (invent == null)
                        {
                            Console.WriteLine("Adding new inventory type.");
                            invent = new Invent_info { Invent_type = inputStr };
                            session.SaveOrUpdate(invent);
                        }

                        Console.WriteLine("Enter the size.");
                        inputNum = GetUserOption();
                        var size = (Size_info)session.CreateQuery("from Size_info as size where size.Size = " + inputNum).UniqueResult();
                        if (size == null)
                        {
                            Console.WriteLine("Adding new size.");
                            size = new Size_info { Size = inputNum };
                            session.SaveOrUpdate(invent);
                        }

                        kit.Invent = invent;
                        kit.Size = size;
                        session.SaveOrUpdate(invent);
                        session.SaveOrUpdate(size);
                        session.SaveOrUpdate(kit);
                        transaction.Commit();

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        //e.InnerException.Message.Contains("key");
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void DeleteEquip()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Enter the kit id to delete.");
                        inputNum = GetUserOption();
                        var query = session.CreateQuery("from Complete_info as kit where kit.Id = " + inputNum);
                        var kit = (Complete_info) query.UniqueResult();

                        if (kit == null)
                        {
                            Console.WriteLine("No such inventory kit.");
                            return;
                        }

                        session.Delete(kit);
                        transaction.Commit();
                        Console.WriteLine("Kit with id {0} was deleted successfully.", inputNum);

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        //e.InnerException.Message.Contains("key");
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void SearchEquipByInvent()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Enter the inventory type to search by.");
                        inputStr = Console.ReadLine();
                        var query = session.CreateQuery("from Complete_info as kit where kit.Invent.Invent_type = \'"+ inputStr + "\'");
                        var kits = query.List<Complete_info>();

                        if (kits.IsEmpty())
                        {
                            Console.WriteLine("No such inventory kits.");
                            return;
                        }
                        Console.WriteLine("Id\tInventory type\tSize");
                        foreach (var kit in kits)
                        {
                            Console.WriteLine(kit.Id.ToString() + "\t" + kit.Invent.Invent_type + "\t\t" + kit.Size.Size);
                        }
                        //transaction.Commit();
                        

                    }
                    catch (Exception e)
                    {
                        //transaction.Rollback();
                        //e.InnerException.Message.Contains("key");
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void SearchEquipBySize()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        int minSize, maxSize;
                        Console.WriteLine("Enter the lower size boundary.");
                        minSize = GetUserOption();
                        Console.WriteLine("Enter the upper size boundary.");
                        maxSize = GetUserOption();
                        if (maxSize < minSize)
                        {
                            Console.WriteLine("Bad input. Try again.");
                            return;
                        }

                        var query = session.CreateQuery("from Complete_info as kit where kit.Size.Size between " +
                                                        minSize +
                                                        " and " + maxSize);
                        var kits = query.List<Complete_info>();

                        if (kits.IsEmpty())
                        {
                            Console.WriteLine("No such inventory kits.");
                            return;
                        }
                        Console.WriteLine("Id\tInventory type\tSize");
                        foreach (var kit in kits)
                        {
                            Console.Write(
                                kit.Id.ToString() + "\t" + kit.Invent.Invent_type + "\t\t" + kit.Size.Size);
                            foreach (var journalPosition in kit.CompleteKitRented)
                            {
                                if (journalPosition.Date_check_in == null)
                                    Console.Write("(Not available)");
                            }

                            Console.WriteLine();
                        }
                        //transaction.Commit();


                    }
                    catch (Exception e)
                    {
                        //transaction.Rollback();
                        //e.InnerException.Message.Contains("key");
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void GetSortedEquipment()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var query = session.CreateSQLQuery(
                            "SELECT DISTINCT invent_info.invent_type, size_info.size, tmp.date_taken FROM journal " +
                            "INNER JOIN complete_info ON journal.complete_kit_id = complete_info.id " +
                            "INNER JOIN invent_info ON complete_info.invent_id = invent_info.id " +
                            "INNER JOIN size_info ON complete_info.size_id = size_info.id " +
                            "INNER JOIN(SELECT complete_info.id, SUM(CASE WHEN date_check_in is null " +
                                "THEN current_date ELSE date_check_in	END - date_check_out) AS date_taken FROM journal " +
                            "INNER JOIN complete_info ON journal.complete_kit_id = complete_info.id GROUP BY complete_info.id) AS tmp" +
                            " ON tmp.id = journal.complete_kit_id");

                        var res = query.List();
                        Console.WriteLine("Inventory type\tSize\tDate taken");
                        foreach (IList pos in res)
                        {
                            Console.WriteLine(String.Format("{0}\t\t{1}\t{2}", pos[0], pos[1], pos[2]));
                        }
                    }
                    catch (Exception e)
                    {
                        //transaction.Rollback();
                        //e.InnerException.Message.Contains("key");
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void JournalDialog()
        {
            while (true)
            {
                Console.WriteLine("Enter one of the options.");
                Console.WriteLine("1) Print journal.");
                Console.WriteLine("2) Add to journal.");
                Console.WriteLine("3) Alter position in journal.");
                Console.WriteLine("4) Delete position in journal.");
                Console.WriteLine("0) Go back.");

                inputNum = GetUserOption();

                switch (inputNum)
                {
                    case 1:
                        PrintJournal();
                        break;
                    case 2:
                        AddJournal();
                        break;
                    case 3:
                        AlterJournal();
                        break;
                    case 4:
                        DeleteJournal();
                        break;
                    case 0: return;
                    default:
                        Console.WriteLine("Enter correct option, please.");
                        continue;

                }

                Console.Clear();
            }
        }

        private void PrintJournal()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Id\tInventory type\t\tSize\tDate of issue\tDate of turning in");
                        var query = session.CreateQuery("from Journal as journal");
                        var journal = query.List<Journal>();
                        foreach (var position in journal)
                            if (position.Date_check_in != null)
                                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", position.Id,
                                    position.Complete_kit.Invent.Invent_type, position.Complete_kit.Size.Size,
                                    position.Date_check_out.ToShortDateString(), ((DateTime)position.Date_check_in).ToShortDateString());
                            else
                                Console.WriteLine("{0}\t{1}\t\t\t{2}\t{3}\tNot yet", position.Id,
                                    position.Complete_kit.Invent.Invent_type, position.Complete_kit.Size.Size,
                                    position.Date_check_out.Date.ToShortDateString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void AddJournal()
        {
            PrintEquip();
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Enter the inventory kit id.");
                        inputNum = GetUserOption();
                        var query = session.CreateQuery("from Complete_info as kit where kit.id = " + inputNum);
                        var kit = (Complete_info)query.UniqueResult();
                        if (kit == null)
                        {
                            Console.WriteLine("No such inventory kit.");
                            return;
                        }

                        query = session.CreateQuery("from Journal as journal where journal.Complete_kit = " + inputNum);
                        var journalPositions = query.List<Journal>();
                        
                        foreach (var journalPosition in journalPositions)
                        {
                            if (journalPosition.Date_check_in == null)
                            {
                                Console.WriteLine("Sorry, this kit is not available yet.");
                                return;
                            }
                        }

                        Console.WriteLine("Enter a date (dd/mm/yyyy format): ");
                        var position = new Journal();
                        position.Complete_kit = kit;
                        DateTime userDateTime;
                        while (!DateTime.TryParseExact(Console.ReadLine(), "dd/mm/yyyy", null,
                            System.Globalization.DateTimeStyles.None, out userDateTime))
                        {
                            Console.WriteLine("You have entered an incorrect date.");
                        }

                        position.Date_check_out = userDateTime;
                        position.Date_check_in = null;

                        Console.WriteLine("Enter a date (mm/dd/yyyy format or null): ");
                        inputStr = Console.ReadLine();
                        if(inputStr != "null")
                        {
                            while (!DateTime.TryParseExact(inputStr, "dd/mm/yyyy", null,
                                System.Globalization.DateTimeStyles.None, out userDateTime))
                                {
                                Console.WriteLine("You have entered an incorrect date.");
                                inputStr = Console.ReadLine();
                            }
                        }
                        session.SaveOrUpdate(position);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void AlterJournal()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        PrintEquip();
                        Console.WriteLine("Enter a journal position id that you want to edit.");
                        inputNum = GetUserOption();

                        var position = (Journal)session.CreateQuery("from Journal as journal where journal.Id = " + inputNum).UniqueResult();
                        if (position == null)
                        {
                            Console.WriteLine("No such position in the journal.");
                            return;
                        }
                        Console.WriteLine("Enter the inventory kit id.");
                        inputNum = GetUserOption();
                        var query = session.CreateQuery("from Complete_info as kit where kit.id = " + inputNum);
                        var kit = (Complete_info)query.UniqueResult();
                        if (kit == null)
                        {
                            Console.WriteLine("No such inventory kit.");
                            return;
                        }

                        Console.WriteLine("Enter a date (mm/dd/yyyy format): ");
                        position.Complete_kit = kit;
                        DateTime userDateTime;
                        while (!DateTime.TryParse(Console.ReadLine(), out userDateTime))
                        {
                            Console.WriteLine("You have entered an incorrect value.");
                        }

                        position.Date_check_out = userDateTime;
                        position.Date_check_in = null;

                        Console.WriteLine("Enter a date (mm/dd/yyyy format or null): ");
                        inputStr = Console.ReadLine();
                        if (inputStr != "null")
                        {
                            while (!DateTime.TryParse(inputStr, out userDateTime))
                            {
                                Console.WriteLine("You have entered an incorrect value.");
                                inputStr = Console.ReadLine();
                            }
                        }
                        session.SaveOrUpdate(position);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }

        private void DeleteJournal()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Enter a journal position id that you want to delete.");
                        inputNum = GetUserOption();

                        var position = (Journal)session.CreateQuery("from Journal as journal where journal.Id = " + inputNum).UniqueResult();
                        if (position == null)
                        {
                            Console.WriteLine("No such position in the journal.");
                            return;
                        }
                        
                        session.Delete(position);
                        transaction.Commit();
                        Console.WriteLine("Position with id {0} was deleted successfully.", inputNum);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine("smth went wrong");
                    }
                }
            }
        }
    }
}