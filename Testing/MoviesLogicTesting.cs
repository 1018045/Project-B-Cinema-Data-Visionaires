namespace Testing;

[TestClass]
public class TestMoviesLogic
{
    [TestMethod]
    public void PromoteMovieTestAdd()
    {
        MoviesLogic ml = new(new LogicManager());
        ml.PromoteMovie(new MovieModel(13000, "x", 10, 16, "x", new List<string>{}, "x"), 0);
        Assert.IsInstanceOfType<MovieModel>(ml.PromotedMovies[0]);
    }

    public void PromoteMovieTestSetAsEmpty()
    {
        MoviesLogic ml = new(new LogicManager());
        ml.PromoteMovie(new MovieModel(13000, "x", 10, 16, "x", new List<string>{}, "x"), 0);
        ml.RemovePromotion(0);
        Assert.IsNull(ml.PromotedMovies[0]);
    }

    public void PromoteMovieTestAddOutsideBoundaries()
    {
        
    }
}