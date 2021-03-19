using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperHomework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DapperHomework.Controllers
{
    [ApiController]
    [Route("[controller]/{action}")]
    public class DapperSampleController : ControllerBase
    {
        private readonly ILogger<DapperSampleController> _logger;
        private readonly IConfiguration _configuration;

        public DapperSampleController(ILogger<DapperSampleController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = nameof(DapperInsert))]
        public IActionResult DapperInsert()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open) //db'nin durumuna bakıp connection yok ise bu kısımda açıyoruz 
                    db.Open();

                //Db tarafında yeni NewHomeworkTable adında bir tablo oluşturdum. Bu tabloya veri kaydedebilmek için bir sql cümleciği yazmamız gerekiyor. Dapper Execute yöntemi
                //ile insert into kullanıp name, surname,phone,address ve age bilgisini kaydediyorum. Parametre isimleri sql cümlesindeki parametrelerle aynı olması gerekiyor.
                sql = @"INSERT INTO dbo.NewHomeworkTable (Name, Surname, Phone, Address, Age)
                                    Values (@Name, @SurName, @Phone, @Address, @Age);";


                var affected = db.Execute(sql, new
                {
                    Name = "Merve",
                    Surname = "Mor",
                    Phone = "5555555555",
                    Address = "Mecidiyeköy",
                    Age = 24,
                });
            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperInsertToMany))]
        public IActionResult DapperInsertToMany()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();

                sql = @"INSERT INTO dbo.NewHomeworkTable (Name, Surname, Phone, Address, Age)
                                    Values (@Name, @Surname, @Phone, @Address, @Age);";
                
                //Normal insert into dan tek farkı tek bir değer değil de birden fazla değer kaydetmiş oluyoruz, object türünden 10 elemanlı bir array oluşturup 
                //verileri sırasıyla db'ye kaydediyoruz. 
                object[] objList = new object[10];
                for (var i = 0; i < 10; i++)
                {
                    objList[i] = new
                    {
                        Name = "Merve-" + i,
                        SurName = "Mor",
                        Phone = "5555555555",
                        Address = "Mecdiyeköy",
                        Age = 24 + i,
                    };
                }

                var affected = db.Execute(sql, objList);
            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperUpdate))]
        public IActionResult DapperUpdate()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();

                //Update işlemini yaparken yazdığımız sql cümleciğinde set kullanıyoruz. Burada NewHomework tablosunda girilecek olan id numarasına göre yeni addressi set et 
                //demiş oluyoruz. 
                sql = "Update dbo.NewHomeworkTable Set Address = @AddressNew Where Id = @IdReal";
                var paramsArray = new[]
                {
                    new {IdReal = 2, AddressNew = "Zeytinburnu"},
                    new {IdReal = 3, AddressNew = "Sarıyer"},
                    new {IdReal = 4, AddressNew = "Zeytinburnu"},
                    new {IdReal = 5, AddressNew = "Sarıyer"},
                    new {IdReal = 6, AddressNew = "Zeytinburnu"},
                    new {IdReal = 7, AddressNew = "Sarıyer"},
                    new {IdReal = 8, AddressNew = "Zeytinburnu"},
                    new {IdReal = 9, AddressNew = "Sarıyer"},
                    new {IdReal = 10, AddressNew = "Zeytinburnu"},
                    new {IdReal = 11, AddressNew = "Sarıyer"},
                };

                var affected = db.Execute(sql, paramsArray);
            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperDelete))]
        public IActionResult DapperDelete()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();

                //Silme işleminde yine ilgili tablo olan NewHomeworkTable seçip verilecek id'ye göre silme işlemi yapıyoruz.
                sql = "Delete From dbo.NewHomeworkTable Where Id = @Id";
                var affected = db.Execute(sql, new[] {

                    new {Id = 1}
                });
            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperSelect))]
        public IActionResult DapperSelect()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();

                sql = "Select * From [Sales].[CreditCard];";
                var creditCard = db.Query(sql); //Query() metodu veritabanından retrieve data işlemini yani veri almamızı sağlayan ve model mapping'i yaparak modellerimizi dolduran metoddur.
            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperTransaction))]
        public IActionResult DapperTransaction()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();

                //birden fazla transaction kullanılabilir.
                using (var transaction = db.BeginTransaction()) //transaction işlemi başlatılmalı 
                {
                    sql = @"INSERT INTO dbo.NewHomeworkTable (Name, Surname, Phone, Address, Age)
                                        Values (@Name, @Surname, @Phone, @Address, @Age);";

                    var affected = db.Execute(sql, new
                    {
                        Name = "Merve",
                        SurName = "Mor",
                        Phone = "5555555555",
                        Address = "Şişli",
                        Age = 24,
                    }, transaction); // bu sefer bu kısımda transaction göndermek gerekiyor 

                    SalesCurrency salesCurrency = new SalesCurrency()
                    {
                        CurrencyCode = "NEW",
                        Name = "NewUnit",
                        ModifiedDate = DateTime.Now
                    };

                    sql = @"Insert into [Sales].[Currency] (CurrencyCode, Name, ModifiedDate)
                                Values(@CurrencyCode, @Name, @ModifiedDate)";

                    db.Execute(sql, salesCurrency, transaction);

                    transaction.Commit(); //commit etmezsek işlemler görünmez 
                }
            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperOneToMany))]
        public IActionResult DapperOneToMany()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();

                /*OneToMany ilişkiden kastımız tablonun birinde birincil anahtar ile diğer tabloda tekrarlı bir alan arasında kurulan ilişki türü, örnek olarak okuldaki danışman bir
                 * hocayı ve öğrencileri düşünebiliriz. Her öğrencinin bir danışman hocası olabilirken, öğretmenlerin birden fazla öğrencisi olabilir. Bu yapıyı yakalayabilmek için
                 * AdventureWorks veritabanımdan baktım, one to many ilişki kurabileceğim tablolalara, ContactType tablosundaki ContactTypeID ile BusinessEntityContact tablosundaki
                 * ContactTypeID'leri bir sql cümlesi yapıp, QueryMultiple metoduna bu sql cümlesini verip, ekstra olarak bu metoda ContactTypeID bilgisini vererek birleştirmiş oldum. */

                sql = @"Select * From [Person].[ContactType] Where ContactTypeID = @ContactTypeID;
                        Select * From [Person].[BusinessEntityContact] Where ContactTypeID = @ContactTypeID;";

                var multipleQuery = db.QueryMultiple(sql, new { ContactTypeID = 11 });

                var contactType = multipleQuery.Read<ContactType>();

                var businessEntityContact = multipleQuery.Read<BusinessEntityContact>().ToList();

            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperOneToOne))]
        public IActionResult DapperOneToOne()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();
                /*One to one ilişki için örnek olarak bilet ve müşteri tablosu örnek verilebilir. Her müşterinin bir bileti olabilir ve her biletin tek sahibi olabilir. Bu yapıyı 
                 * yakalayabilmek için AdvantureWorks veritabanımdan bu sefer tablolara baktım. CreditCard tablosu içerisindeki CreditCardID bilgisi ile PersonCreditCard
                 * tablosundaki CreditCardID bilgisini birleştirmiş oldum. QueryMultiple metoduna sql cümleciğimi verdim, ekstra olarak bir de CreditCardID bilgisini verdim.*/

                sql = @"Select * From [Sales].[CreditCard] Where CreditCardID = @CreditCardID;
                        Select * From [Sales].[PersonCreditCard] Where CreditCardID = @CreditCardID;";

                var multipleQuery = db.QueryMultiple(sql, new { CreditCardID = 11 });

                var creditCard = multipleQuery.Read<CreditCard>();

                var personCreditCard = multipleQuery.Read<PersonCreditCard>().ToList();

            }
            return Ok();
        }

        [HttpGet(Name = nameof(DapperMultipleQueryMapping))]
        public IActionResult DapperMultipleQueryMapping()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sql = "";
                if (db.State != ConnectionState.Open)
                    db.Open();

                /* Multiple Query olması durumunda birbirinden farklı iki ya da 3 tabloyu tek connection ile çalıştırabiliyoruz. Rastgele iki tablo seçip sql cümlesine verdim. 
                 * Yine QueryMultiple metoduna sql cümlesini verip, ekstradan objeleri gönderip birleştirmiş oldum.
                 */

                sql = @"Select * From [Production].[Location] Where LocationID = @LocationID;
                        Select * From [Sales].[Customer] Where CustomerID = @CustomerID;";

                var multipleQuery = db.QueryMultiple(sql, new { LocationID = 1, CustomerID = 11 });

                var locations = multipleQuery.Read<Location>();

                var customers = multipleQuery.Read<Customer>().ToList();

            }
            return Ok();
        }
    }
}
