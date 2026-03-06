using FluentAssertions;
using Inventory.IntTest.DTOs;
using Inventory.IntTest.Factories;
using Joseco.DDD.Core.Results;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Inventory.IntTest
{
    public class ItemControllerIntTest
    {

        [Fact]
        [Trait("Category", "Integration")]

        public async void ItemController_CreateAValidItem()
        {
            var guid = Guid.NewGuid();
            var response =
               await new HttpClientBuilder()
               .WithRequestBody(new
               {
                   id = guid,
                   itemName = "Test Item"
               }, "/api/Item")
               .Send();


            response.Should()
                .NotBeNull()
                .And.Subject.Should()
                .BeOfType<HttpResponseMessage>();

            var jsonResponse = await response.Content.ReadFromJsonAsync<ItemCreateResponseDto>();

            jsonResponse
                .Should().NotBeNull();

            jsonResponse.Value.Should().Be(guid);

        }
        [Fact]
        [Trait("Category", "Integration")]

        public async void ItemController_CreateDuplicateItem()
        {
            var guid = Guid.NewGuid();
            var item1 = ItemFactory.createItemCommand(guid, "Test Item 1");
            var item2 = ItemFactory.createItemCommand(guid, "Test Item 2");

            var response1 =
                await new HttpClientBuilder()
                .WithRequestBody(item1, "/api/Item")
                .Send();
            var response2 =
                await new HttpClientBuilder()
                .WithRequestBody(item2, "/api/Item")
                .Send();

            response1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response2.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);

        }
    }
}