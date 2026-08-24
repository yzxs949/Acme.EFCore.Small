using Acme.EFCore.Small.Enums;
using Acme.EFCore.Small.Extensions;
using Acme.EFCore.Small.Querys;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Acme.EFCore.Small.Tests
{
    /// <summary>
    /// 查询扩展（LinqExtension / PageListExtension）完整覆盖测试，对应主项目 Querys 与 Extensions 功能
    /// </summary>
    public class QueryExtensionTests
    {
        private static List<Product> Seed()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Category = "Fruit", Price = 10, Tags = "red,sweet" },
                new Product { Id = 2, Name = "Banana", Category = "Fruit", Price = 20, Tags = "yellow" },
                new Product { Id = 3, Name = "Carrot", Category = "Veg", Price = 30, Tags = "orange" },
                new Product { Id = 4, Name = "Date", Category = "Fruit", Price = 40, Tags = "brown" },
            };
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string? Tags { get; set; }
        }

        [Fact]
        public void WhereIf_applies_filter_only_when_true_on_IEnumerable()
        {
            var list = Seed();
            var result = list.WhereIf(true, p => p.Price > 15).ToList();
            var unchanged = list.WhereIf(false, p => p.Price > 15).ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal(4, unchanged.Count);
        }

        [Fact]
        public void WhereIf_IQueryable_applies_filter_only_when_true()
        {
            var query = Seed().AsQueryable();
            var result = query.WhereIf(true, p => p.Category == "Fruit").ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(4, query.WhereIf(false, p => p.Category == "Fruit").Count());
        }

        [Theory]
        [InlineData("Category", Symbol.Equal, "Fruit", 3)]
        [InlineData("Category", Symbol.NotEqual, "Fruit", 1)]
        [InlineData("Name", Symbol.Contains, "an", 2)]        // Banana, Date
        [InlineData("Name", Symbol.NotContains, "an", 2)]     // Apple, Carrot
        [InlineData("Name", Symbol.StartsWith, "A", 1)]       // Apple
        [InlineData("Name", Symbol.EndsWith, "t", 2)]         // Carrot, Date
        [InlineData("Price", Symbol.GreaterThan, "15", 3)]    // 20,30,40
        [InlineData("Price", Symbol.GreaterThanOrEqual, "20", 3)]
        [InlineData("Price", Symbol.LessThan, "30", 2)]       // 10,20
        [InlineData("Price", Symbol.LessThanOrEqual, "30", 3)]
        [InlineData("Category", Symbol.In, "Fruit,Veg", 4)]
        [InlineData("Category", Symbol.NotIn, "Fruit,Veg", 0)]
        public void AddConditions_supports_all_Symbol_operators(string field, Symbol symbol, string value, int expectedCount)
        {
            var conditions = new List<Condition>
            {
                new Condition(field, value, symbol)
            };
            var query = Seed().AsQueryable();

            var result = query.AddConditions(conditions).ToList();

            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public void AddConditions_AND_combines_multiple_conditions()
        {
            var conditions = new List<Condition>
            {
                new Condition("Category", "Fruit", Symbol.Equal),
                new Condition("Price", "20", Symbol.GreaterThanOrEqual),
            };

            var result = Seed().AsQueryable().AddConditions(conditions).ToList();

            Assert.Equal(2, result.Count); // Banana(20), Date(40)
        }

        [Fact]
        public void AddConditionsIf_applies_only_when_flag_true()
        {
            var conditions = new List<Condition> { new Condition("Category", "Veg", Symbol.Equal) };

            var applied = Seed().AsQueryable().AddConditionsIf(true, conditions).ToList();
            var skipped = Seed().AsQueryable().AddConditionsIf(false, conditions).ToList();

            Assert.Single(applied);
            Assert.Equal(4, skipped.Count);
        }

        [Fact]
        public void AddConditionsContains_keywords_OR_search_across_fields()
        {
            var keywords = new Keywords(new[] { "Name", "Category" }, "a");
            var result = Seed().AsQueryable().AddConditionsContains(keywords).ToList();

            // 任一字段包含 "a"：Apple, Banana, Carrot, Date(都含a或Category Fruit含a?) -> 全部 4 条
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void AddConditionsContains_with_isAdd_false_skips_filter()
        {
            var keywords = new Keywords(new[] { "Name" }, "Apple");
            var result = Seed().AsQueryable().AddConditionsContains(false, keywords).ToList();

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void AddSorting_orders_ascending_and_descending()
        {
            var asc = Seed().AsQueryable().AddSorting(new Sorting("Price", SortingType.ASC)).ToList();
            var desc = Seed().AsQueryable().AddSorting(new Sorting("Price", SortingType.DESC)).ToList();

            Assert.Equal(10m, asc.First().Price);
            Assert.Equal(40m, desc.First().Price);
        }

        [Fact]
        public void AddSorting_null_falls_back_to_Id_ascending()
        {
            var result = Seed().AsQueryable().AddSorting(null).ToList();
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public void AddSortingIf_applies_only_when_flag_true()
        {
            var sorted = Seed().AsQueryable().AddSortingIf(true, new Sorting("Price", SortingType.DESC)).ToList();
            var unsorted = Seed().AsQueryable().AddSortingIf(false, new Sorting("Price", SortingType.DESC)).ToList();

            Assert.Equal(40m, sorted.First().Price);
            Assert.Equal(10m, unsorted.First().Price);
        }

        [Fact]
        public void GetKey_returns_distinct_values_from_IQueryable()
        {
            var cities = Seed().AsQueryable().GetKey(p => p.Category).ToList();
            Assert.Equal(2, cities.Count); // Fruit, Veg
        }

        [Fact]
        public void GetKeyList_returns_distinct_values_from_IEnumerable()
        {
            var prices = Seed().GetKeyList(p => p.Price).ToList();
            Assert.Equal(4, prices.Count);
        }

        [Fact]
        public void List_ToPageList_wraps_items_with_total()
        {
            var items = Seed();
            var page = items.ToPageList(total: 100);

            Assert.Equal(100, page.Total);
            Assert.Equal(4, page.Items.Count);
        }
    }
}
