using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Game_store.IntegrationTests
{
    public class GamesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GamesIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsSeededGame()
        {
            var client = _factory.CreateClient();

            var resp = await client.GetAsync("/api/games");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await resp.Content.ReadFromJsonAsync<RootResponse>();
            body.Should().NotBeNull();
            body.items.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Get_ReturnsGameById()
        {
            var client = _factory.CreateClient();

            // first list to obtain an id
            var resp = await client.GetAsync("/api/games");
            resp.EnsureSuccessStatusCode();
            var body = await resp.Content.ReadFromJsonAsync<RootResponse>();
            var id = body.items[0].Id;

            var single = await client.GetAsync($"/api/games/{id}");
            single.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private class RootResponse
        {
            public GameDto[] items { get; set; } = new GameDto[0];
            public int total { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }

        private class GameDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}
